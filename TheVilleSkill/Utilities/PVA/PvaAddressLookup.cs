using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TheVilleSkill.Models.Addresses;
using TheVilleSkill.Models.PVA;
using TheVilleSkill.Utilities.Repositories;

namespace TheVilleSkill.Utilities.PVA
{
    public class PvaAddressLookup : IPvaAddressLookup
    {
        private readonly ILogger<PvaAddressLookup> _logger;
        private readonly IAddressRepository _addressRepository;
        private readonly PvaSettings _settings;
        private readonly IAddressParser _addressParser;        

        public PvaAddressLookup(ILogger<PvaAddressLookup> logger, 
            IAddressRepository addressRepository, 
            PvaSettings settings, 
            IAddressParser addressParser)
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _settings = settings;
            _addressParser = addressParser;
        }

        public async Task<PvaPropertyInfo> GetPropertyInfo(string address)
        {            
            address = Regex.Replace(address, " pky", " pkwy", RegexOptions.IgnoreCase);

            var propertyInfo = new PvaPropertyInfo { Address = new AddressModel() };

            var addressParsings = (await _addressParser.Parse(address)).ToList();
            if (!addressParsings.Any())
            {
                propertyInfo.Error = "Could not parse address.";
                return propertyInfo;
            }

            var maxScore = addressParsings.Max(p => p.Score);
            var bestParsing = addressParsings.First(p => p.Score == maxScore);

            propertyInfo.Address = bestParsing.Address;

            // See if the address exists in the database...
            var savedAddress = await _addressRepository.Get(bestParsing.Address.Number, bestParsing.Address.Direction, bestParsing.Address.Street, bestParsing.Address.Tag);

            // If found, and owner, assessed value, and acreage exist, return this instead of asking PVA. The AddressRepository deals with determining
            // whether these attributes are expired.
            if (savedAddress != null && !string.IsNullOrEmpty(savedAddress.Owner) && savedAddress.AssessedValue != null && savedAddress.Acreage != null)
            {
                propertyInfo.Address = savedAddress;
                return propertyInfo;
            }

            try
            {
                var webGet = new HtmlWeb();
                var document1 = webGet.Load(_settings.Url + string.Format(_settings.SearchUrl, HttpUtility.UrlEncode(bestParsing.Address.BaseAddressDisplay)));
                HtmlDocument document2 = null;

                // If the search returned only one result, the first web page that the PVA site returns will contain javascript redirect to another URL. Get the script tag.
                var redirectScriptTag = document1.DocumentNode.Descendants("script").Where(tag => tag.InnerHtml.Contains("window.location.href=")).ToList();

                var redirectUrl = "";

                if (webGet.ResponseUri.LocalPath.StartsWith("/property-search/property-details")) //  301 redirect
                {
                    document2 = document1;
                }
                else if (redirectScriptTag.Any())
                {
                    // Get the redirect script out of the script tag.
                    var redirectScript = redirectScriptTag.First().InnerHtml;

                    // Remove the javascript and the quotation marks. The remainder is the actual URL.
                    redirectUrl = redirectScript.Replace("window.location.href=", "").Replace("\"", "");
                }
                else if (!redirectScriptTag.Any() && !string.IsNullOrEmpty(bestParsing.Address.Tag))
                {
                    var resultsTable = document1.DocumentNode.Descendants("table").FirstOrDefault(tag => tag.Attributes["class"] != null && tag.Attributes["class"].Value.Contains("searchResultsTable"));

                    if (resultsTable != null)
                    {
                        var resultsRows = resultsTable.Descendants("tr").ToList();

                        // The first row is the table header. Skip this row.
                        foreach (var resultRow in resultsRows.Skip(1))
                        {
                            var resultRowLink = resultRow.ChildNodes.Where(tag => tag.Name == "td").ElementAt(1).ChildNodes.First(tag => tag.Name == "a");
                            if (resultRowLink == null)
                                continue;
                            if (!await _addressParser.AreEqual(resultRowLink.InnerHtml, address))
                                continue;
                            redirectUrl = resultRowLink.Attributes["href"].Value;
                            break;
                        }
                    }
                }

                if (document2 == null && string.IsNullOrEmpty(redirectUrl))
                {
                    _logger.LogError("Did not find expected redirect URL or was not automatically redirected: {address}", address);
                    propertyInfo.Error = "Did not find expected redirect URL.";
                    return propertyInfo;
                }

                // The redurectUrl is a relative path, so we have to add the rootUrl to it before making another request.
                document2 = document2 ?? webGet.Load(string.Concat(_settings.Url, redirectUrl.Replace("#038;", "&")));

                var returnedAddressTag = document2.DocumentNode.SelectSingleNode("// div[@id='primary']/div/h1");
                if (returnedAddressTag == null)
                {
                    _logger.LogError("Could not find <h1> tag containing address: {address}", address);
                    propertyInfo.Error = "Could not find <h1> tag containing address.";
                    return propertyInfo;
                }

                var returnedAddress = returnedAddressTag.InnerHtml;

                if (!await _addressParser.AreEqual(returnedAddress, address))
                {
                    _logger.LogInformation("PVA returned differenct address than searched: (original) {address} (returned) {returnedAddress}", address, returnedAddress);
                    propertyInfo.Error = "PVA returned different address than searched.";
                    return propertyInfo;
                }

                var ownerLabelDt = document2.DocumentNode.Descendants().FirstOrDefault(tag => tag.Name == "dt" && tag.InnerHtml == "Owner");

                if (ownerLabelDt == null)
                {
                    _logger.LogError("Could not find <dt> tag containing string \"Owner\": {address}", address);
                    propertyInfo.Error = "Could not find <dt> tag containing string \"Owner\".";
                    return propertyInfo;
                }

                var ownerNames = ownerLabelDt.NextSibling.NextSibling.InnerHtml.Trim();

                if (string.IsNullOrEmpty(ownerNames))
                {
                    _logger.LogInformation("Could not find owner in document or owner was empty: {address}", address);
                    propertyInfo.Error = "Could not find owner in document or owner was empty.";
                    return propertyInfo;
                }

                propertyInfo.Address.Owner = HttpUtility.HtmlDecode(ownerNames);

                var assessedValueLabelDt = document2.DocumentNode.Descendants().FirstOrDefault(tag => tag.Name == "dt" && tag.InnerHtml == "Assessed Value");

                if (assessedValueLabelDt == null)
                {
                    _logger.LogError("Could not find <dt> tag containing string \"Assessed Value\": {address}", address);
                }
                else
                {
                    if (int.TryParse(HttpUtility.HtmlDecode(assessedValueLabelDt.NextSibling.NextSibling.InnerHtml.Trim().Replace(",", "")), out var iAssessedValue))
                    {
                        propertyInfo.Address.AssessedValue = iAssessedValue;
                    }                    
                }

                var acreageLabelDt = document2.DocumentNode.Descendants().FirstOrDefault(tag => tag.Name == "dt" && tag.InnerHtml == "Acres");

                if (acreageLabelDt == null)
                {
                    _logger.LogError("Could not find <dt> tag containing string \"Acres\": {address}", address);
                }
                else
                {
                    if (double.TryParse(HttpUtility.HtmlDecode(acreageLabelDt.NextSibling.NextSibling.InnerHtml.Trim()), out var dAcres))
                    {
                        propertyInfo.Address.Acreage = dAcres;
                    }
                }
            }
            catch (Exception ex)
            {
                propertyInfo.Error = ex.ToString();
            }

            if (savedAddress == null)
            {
                savedAddress = new AddressModel
                {
                    Id = await _addressRepository.Add(bestParsing.Address.Number, bestParsing.Address.Direction, bestParsing.Address.Street, bestParsing.Address.Tag)
                };
            }

            await _addressRepository.AddAttributes(savedAddress.Id.Value, new List<AddressAttributeModel> {
                new AddressAttributeModel { Source = "PVA", Name = "owner", Value = propertyInfo.Address.Owner },
                new AddressAttributeModel { Source = "PVA", Name = "assessedvalue", Value = propertyInfo.Address.AssessedValue.ToString() },
                new AddressAttributeModel { Source = "PVA", Name = "acreage", Value = propertyInfo.Address.Acreage.ToString() }
            });

            return propertyInfo;
        }
    }
}