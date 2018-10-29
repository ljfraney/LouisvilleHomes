using LouisvilleHomes.Models;
using LouisvilleHomes.Utilities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LouisvilleHomes.Utilities
{
    public class AddressParser : IAddressParser
    {
        private readonly ITagRepository _tagRepository;

        public AddressParser(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        private static readonly Dictionary<string, string> NumberedStreets = new[]
        {
            new { Key = "FIRST", Value = "1ST" },
            new { Key = "SECOND", Value = "2ND" },
            new { Key = "THIRD", Value = "3RD" },
            new { Key = "FOURTH", Value = "4TH" },
            new { Key = "FIFTH", Value = "5TH" },
            new { Key = "SIXTH", Value = "6TH" },
            new { Key = "SEVENTH", Value = "7TH" },
            new { Key = "EIGHTH", Value = "8TH" },
            new { Key = "NINTH", Value = "9TH" },

            new { Key = "TENTH", Value = "10TH" },
            new { Key = "ELEVENTH", Value = "11TH" },
            new { Key = "TWELFTH", Value = "12TH" },
            new { Key = "THIRTEENTH", Value = "13TH" },
            new { Key = "FOURTEENTH", Value = "14TH" },
            new { Key = "FIFTEENTH", Value = "15TH" },
            new { Key = "SIXTEENTH", Value = "16TH" },
            new { Key = "SEVENTEENTH", Value = "17TH" },
            new { Key = "EIGHTEENTH", Value = "18TH" },
            new { Key = "NINETEENTH", Value = "19TH" },

            new { Key = "TWENTIETH", Value = "20TH" },
            new { Key = "TWENTYFIRST", Value = "21ST" },
            new { Key = "TWENTYSECOND", Value = "22ND" },
            new { Key = "TWENTYTHIRD", Value = "23RD" },
            new { Key = "TWENTYFOURTH", Value = "24TH" },
            new { Key = "TWENTYFIFTH", Value = "25TH" },
            new { Key = "TWENTYSIXTH", Value = "26TH" },
            new { Key = "TWENTYSEVENTH", Value = "27TH" },
            new { Key = "TWENTYEIGHTH", Value = "28TH" },
            new { Key = "TWENTYNINTH", Value = "29TH" },
            new { Key = "TWENTY-FIRST", Value = "21ST" },
            new { Key = "TWENTY-SECOND", Value = "22ND" },
            new { Key = "TWENTY-THIRD", Value = "23RD" },
            new { Key = "TWENTY-FOURTH", Value = "24TH" },
            new { Key = "TWENTY-FIFTH", Value = "25TH" },
            new { Key = "TWENTY-SIXTH", Value = "26TH" },
            new { Key = "TWENTY-SEVENTH", Value = "27TH" },
            new { Key = "TWENTY-EIGHTH", Value = "28TH" },
            new { Key = "TWENTY-NINTH", Value = "29TH" },
            new { Key = "TWENTY FIRST", Value = "21ST" },
            new { Key = "TWENTY SECOND", Value = "22ND" },
            new { Key = "TWENTY THIRD", Value = "23RD" },
            new { Key = "TWENTY FOURTH", Value = "24TH" },
            new { Key = "TWENTY FIFTH", Value = "25TH" },
            new { Key = "TWENTY SIXTH", Value = "26TH" },
            new { Key = "TWENTY SEVENTH", Value = "27TH" },
            new { Key = "TWENTY EIGHTH", Value = "28TH" },
            new { Key = "TWENTY NINTH", Value = "29TH" },

            new { Key = "THIRTIETH", Value = "30TH" },
            new { Key = "THIRTYFIRST", Value = "31ST" },
            new { Key = "THIRTYSECOND", Value = "32ND" },
            new { Key = "THIRTYTHIRD", Value = "33RD" },
            new { Key = "THIRTYFOURTH", Value = "34TH" },
            new { Key = "THIRTYFIFTH", Value = "35TH" },
            new { Key = "THIRTYSIXTH", Value = "36TH" },
            new { Key = "THIRTYSEVENTH", Value = "37TH" },
            new { Key = "THIRTYEIGHTH", Value = "38TH" },
            new { Key = "THIRTYNINTH", Value = "39TH" },
            new { Key = "THIRTY-FIRST", Value = "31ST" },
            new { Key = "THIRTY-SECOND", Value = "32ND" },
            new { Key = "THIRTY-THIRD", Value = "33RD" },
            new { Key = "THIRTY-FOURTH", Value = "34TH" },
            new { Key = "THIRTY-FIFTH", Value = "35TH" },
            new { Key = "THIRTY-SIXTH", Value = "36TH" },
            new { Key = "THIRTY-SEVENTH", Value = "37TH" },
            new { Key = "THIRTY-EIGHTH", Value = "38TH" },
            new { Key = "THIRTY-NINTH", Value = "39TH" },
            new { Key = "THIRTY FIRST", Value = "31ST" },
            new { Key = "THIRTY SECOND", Value = "32ND" },
            new { Key = "THIRTY THIRD", Value = "33RD" },
            new { Key = "THIRTY FOURTH", Value = "34TH" },
            new { Key = "THIRTY FIFTH", Value = "35TH" },
            new { Key = "THIRTY SIXTH", Value = "36TH" },
            new { Key = "THIRTY SEVENTH", Value = "37TH" },
            new { Key = "THIRTY EIGHTH", Value = "38TH" },
            new { Key = "THIRTY NINTH", Value = "39TH" },

            new { Key = "FORTIETH", Value = "40TH" },
            new { Key = "FORTYFIRST", Value = "41ST" },
            new { Key = "FORTYSECOND", Value = "42ND" },
            new { Key = "FORTYTHIRD", Value = "43RD" },
            new { Key = "FORTYFOURTH", Value = "44TH" },
            new { Key = "FORTYFIFTH", Value = "45TH" },
            new { Key = "FORTYSIXTH", Value = "46TH" },
            new { Key = "FORTYSEVENTH", Value = "47TH" },
            new { Key = "FORTYEIGHTH", Value = "48TH" },
            new { Key = "FORTYNINTH", Value = "49TH" },
            new { Key = "FORTY-FIRST", Value = "41ST" },
            new { Key = "FORTY-SECOND", Value = "42ND" },
            new { Key = "FORTY-THIRD", Value = "43RD" },
            new { Key = "FORTY-FOURTH", Value = "44TH" },
            new { Key = "FORTY-FIFTH", Value = "45TH" },
            new { Key = "FORTY-SIXTH", Value = "46TH" },
            new { Key = "FORTY-SEVENTH", Value = "47TH" },
            new { Key = "FORTY-EIGHTH", Value = "48TH" },
            new { Key = "FORTY-NINTH", Value = "49TH" },
            new { Key = "FORTY FIRST", Value = "41ST" },
            new { Key = "FORTY SECOND", Value = "42ND" },
            new { Key = "FORTY THIRD", Value = "43RD" },
            new { Key = "FORTY FOURTH", Value = "44TH" },
            new { Key = "FORTY FIFTH", Value = "45TH" },
            new { Key = "FORTY SIXTH", Value = "46TH" },
            new { Key = "FORTY SEVENTH", Value = "47TH" },
            new { Key = "FORTY EIGHTH", Value = "48TH" },
            new { Key = "FORTY NINTH", Value = "49TH" },

            new { Key = "FIFTIETH", Value = "50TH" },
            new { Key = "FIFTYFIRST", Value = "51ST" },
            new { Key = "FIFTYSECOND", Value = "52ND" },
            new { Key = "FIFTYTHIRD", Value = "53RD" },
            new { Key = "FIFTYFOURTH", Value = "54TH" },
            new { Key = "FIFTYFIFTH", Value = "55TH" },
            new { Key = "FIFTYSIXTH", Value = "56TH" },
            new { Key = "FIFTYSEVENTH", Value = "57TH" },
            new { Key = "FIFTYEIGHTH", Value = "58TH" },
            new { Key = "FIFTYNINTH", Value = "59TH" },
            new { Key = "FIFTY-FIRST", Value = "51ST" },
            new { Key = "FIFTY-SECOND", Value = "52ND" },
            new { Key = "FIFTY-THIRD", Value = "53RD" },
            new { Key = "FIFTY-FOURTH", Value = "54TH" },
            new { Key = "FIFTY-FIFTH", Value = "55TH" },
            new { Key = "FIFTY-SIXTH", Value = "56TH" },
            new { Key = "FIFTY-SEVENTH", Value = "57TH" },
            new { Key = "FIFTY-EIGHTH", Value = "58TH" },
            new { Key = "FIFTY-NINTH", Value = "59TH" },
            new { Key = "FIFTY FIRST", Value = "51ST" },
            new { Key = "FIFTY SECOND", Value = "52ND" },
            new { Key = "FIFTY THIRD", Value = "53RD" },
            new { Key = "FIFTY FOURTH", Value = "54TH" },
            new { Key = "FIFTY FIFTH", Value = "55TH" },
            new { Key = "FIFTY SIXTH", Value = "56TH" },
            new { Key = "FIFTY SEVENTH", Value = "57TH" },
            new { Key = "FIFTY EIGHTH", Value = "58TH" },
            new { Key = "FIFTY NINTH", Value = "59TH" },

            new { Key = "SIXTIETH", Value = "60TH" },
            new { Key = "SIXTYFIRST", Value = "61ST" },
            new { Key = "SIXTYSECOND", Value = "62ND" },
            new { Key = "SIXTYTHIRD", Value = "63RD" },
            new { Key = "SIXTYFOURTH", Value = "64TH" },
            new { Key = "SIXTYFIFTH", Value = "65TH" },
            new { Key = "SIXTYSIXTH", Value = "66TH" },
            new { Key = "SIXTYSEVENTH", Value = "67TH" },
            new { Key = "SIXTYEIGHTH", Value = "68TH" },
            new { Key = "SIXTYNINTH", Value = "69TH" },
            new { Key = "SIXTY-FIRST", Value = "61ST" },
            new { Key = "SIXTY-SECOND", Value = "62ND" },
            new { Key = "SIXTY-THIRD", Value = "63RD" },
            new { Key = "SIXTY-FOURTH", Value = "64TH" },
            new { Key = "SIXTY-FIFTH", Value = "65TH" },
            new { Key = "SIXTY-SIXTH", Value = "66TH" },
            new { Key = "SIXTY-SEVENTH", Value = "67TH" },
            new { Key = "SIXTY-EIGHTH", Value = "68TH" },
            new { Key = "SIXTY-NINTH", Value = "69TH" },
            new { Key = "SIXTY FIRST", Value = "61ST" },
            new { Key = "SIXTY SECOND", Value = "62ND" },
            new { Key = "SIXTY THIRD", Value = "63RD" },
            new { Key = "SIXTY FOURTH", Value = "64TH" },
            new { Key = "SIXTY FIFTH", Value = "65TH" },
            new { Key = "SIXTY SIXTH", Value = "66TH" },
            new { Key = "SIXTY SEVENTH", Value = "67TH" },
            new { Key = "SIXTY EIGHTH", Value = "68TH" },
            new { Key = "SIXTY NINTH", Value = "69TH" },

            new { Key = "SEVENTIETH", Value = "70TH" },
            new { Key = "SEVENTYFIRST", Value = "71ST" },
            new { Key = "SEVENTYSECOND", Value = "72ND" },
            new { Key = "SEVENTYTHIRD", Value = "73RD" },
            new { Key = "SEVENTYFOURTH", Value = "74TH" },
            new { Key = "SEVENTYFIFTH", Value = "75TH" },
            new { Key = "SEVENTYSIXTH", Value = "76TH" },
            new { Key = "SEVENTYSEVENTH", Value = "77TH" },
            new { Key = "SEVENTYEIGHTH", Value = "78TH" },
            new { Key = "SEVENTYNINTH", Value = "79TH" },
            new { Key = "SEVENTY-FIRST", Value = "71ST" },
            new { Key = "SEVENTY-SECOND", Value = "72ND" },
            new { Key = "SEVENTY-THIRD", Value = "73RD" },
            new { Key = "SEVENTY-FOURTH", Value = "74TH" },
            new { Key = "SEVENTY-FIFTH", Value = "75TH" },
            new { Key = "SEVENTY-SIXTH", Value = "76TH" },
            new { Key = "SEVENTY-SEVENTH", Value = "77TH" },
            new { Key = "SEVENTY-EIGHTH", Value = "78TH" },
            new { Key = "SEVENTY-NINTH", Value = "79TH" },
            new { Key = "SEVENTY FIRST", Value = "71ST" },
            new { Key = "SEVENTY SECOND", Value = "72ND" },
            new { Key = "SEVENTY THIRD", Value = "73RD" },
            new { Key = "SEVENTY FOURTH", Value = "74TH" },
            new { Key = "SEVENTY FIFTH", Value = "75TH" },
            new { Key = "SEVENTY SIXTH", Value = "76TH" },
            new { Key = "SEVENTY SEVENTH", Value = "77TH" },
            new { Key = "SEVENTY EIGHTH", Value = "78TH" },
            new { Key = "SEVENTY NINTH", Value = "79TH" },

            new { Key = "EIGHTIETH", Value = "70TH" },
            new { Key = "EIGHTYFIRST", Value = "81ST" },
            new { Key = "EIGHTYSECOND", Value = "82ND" },
            new { Key = "EIGHTYTHIRD", Value = "83RD" },
            new { Key = "EIGHTYFOURTH", Value = "84TH" },
            new { Key = "EIGHTYFIFTH", Value = "85TH" },
            new { Key = "EIGHTYSIXTH", Value = "86TH" },
            new { Key = "EIGHTYSEVENTH", Value = "87TH" },
            new { Key = "EIGHTYEIGHTH", Value = "88TH" },
            new { Key = "EIGHTYNINTH", Value = "89TH" },
            new { Key = "EIGHTY-FIRST", Value = "81ST" },
            new { Key = "EIGHTY-SECOND", Value = "82ND" },
            new { Key = "EIGHTY-THIRD", Value = "83RD" },
            new { Key = "EIGHTY-FOURTH", Value = "84TH" },
            new { Key = "EIGHTY-FIFTH", Value = "85TH" },
            new { Key = "EIGHTY-SIXTH", Value = "86TH" },
            new { Key = "EIGHTY-SEVENTH", Value = "87TH" },
            new { Key = "EIGHTY-EIGHTH", Value = "88TH" },
            new { Key = "EIGHTY-NINTH", Value = "89TH" },
            new { Key = "EIGHTY FIRST", Value = "81ST" },
            new { Key = "EIGHTY SECOND", Value = "82ND" },
            new { Key = "EIGHTY THIRD", Value = "83RD" },
            new { Key = "EIGHTY FOURTH", Value = "84TH" },
            new { Key = "EIGHTY FIFTH", Value = "85TH" },
            new { Key = "EIGHTY SIXTH", Value = "86TH" },
            new { Key = "EIGHTY SEVENTH", Value = "87TH" },
            new { Key = "EIGHTY EIGHTH", Value = "88TH" },
            new { Key = "EIGHTY NINTH", Value = "89TH" },

            new { Key = "NINETIETH", Value = "90TH" },
            new { Key = "NINETYFIRST", Value = "91ST" },
            new { Key = "NINETYSECOND", Value = "92ND" },
            new { Key = "NINETYTHIRD", Value = "93RD" },
            new { Key = "NINETYFOURTH", Value = "94TH" },
            new { Key = "NINETYFIFTH", Value = "95TH" },
            new { Key = "NINETYSIXTH", Value = "96TH" },
            new { Key = "NINETYSEVENTH", Value = "97TH" },
            new { Key = "NINETYEIGHTH", Value = "98TH" },
            new { Key = "NINETYNINTH", Value = "99TH" },
            new { Key = "NINETY-FIRST", Value = "91ST" },
            new { Key = "NINETY-SECOND", Value = "92ND" },
            new { Key = "NINETY-THIRD", Value = "93RD" },
            new { Key = "NINETY-FOURTH", Value = "94TH" },
            new { Key = "NINETY-FIFTH", Value = "95TH" },
            new { Key = "NINETY-SIXTH", Value = "96TH" },
            new { Key = "NINETY-SEVENTH", Value = "97TH" },
            new { Key = "NINETY-EIGHTH", Value = "98TH" },
            new { Key = "NINETY-NINTH", Value = "99TH" },
            new { Key = "NINETY FIRST", Value = "91ST" },
            new { Key = "NINETY SECOND", Value = "92ND" },
            new { Key = "NINETY THIRD", Value = "93RD" },
            new { Key = "NINETY FOURTH", Value = "94TH" },
            new { Key = "NINETY FIFTH", Value = "95TH" },
            new { Key = "NINETY SIXTH", Value = "96TH" },
            new { Key = "NINETY SEVENTH", Value = "97TH" },
            new { Key = "NINETY EIGHTH", Value = "98TH" },
            new { Key = "NINETY NINTH", Value = "99TH" }
        }.ToDictionary(l => l.Key, l => l.Value);

        public async Task<IEnumerable<AddressResult>> Parse(string address1)
        {
            var addressResults = new List<AddressResult>();

            //The following is the list of C2 Secondary Unit Designators (http://pe.usps.gov/text/pub28/28apc_003.htm). These generally preceed an apartment number.
            var sudLookup = new[] { "APARTMENT", "APT", "BUILDING", "BLDG", "DEPARTMENT", "DEPT", "FLOOR", "FL", "HANGER", "HNGR", "KEY", "LOT", "PIER", "ROOM", "RM", "SLIP", "SPACE", "SPC", "STOP", "SUITE", "STE", "TRAILER", "TRLR", "UNIT" };

            //Make the entire string upper case so that we don't have to worry about case sensetivity going forward.
            address1 = address1.ToUpper();

            //Replace the # sign with APT, which is our default SUD.
            address1 = address1.Replace("#", " APT ");

            //If the address ends in a 5 digit string, and it isn't preceeded by a SUD, assume it is the zip and remove it.
            var zipRemoved = false;
            if (address1.Length >= 5)
            {
                var zipRegEx = new Regex(@"^\d{5}$");
                var lastFiveChars = address1.Substring(address1.Length - 5, 5);
                if (zipRegEx.IsMatch(lastFiveChars))
                {
                    var remainingAddress = address1.TrimEnd(lastFiveChars.ToCharArray()).Trim();
                    if (!sudLookup.Any(sud => remainingAddress.EndsWith(sud)))
                    {
                        zipRemoved = true;
                        address1 = remainingAddress;
                    }
                }
            }

            //If the address ends in a 9 digit zip code formatted as 99999-9999, remove the zip.
            if (!zipRemoved && address1.Length >= 10)
            {
                var zipRegEx = new Regex(@"^\d{5}-\d{4}$");
                var lastTenChars = address1.Substring(address1.Length - 10, 10);
                if (zipRegEx.IsMatch(lastTenChars))
                    address1 = address1.TrimEnd(lastTenChars.ToCharArray());
            }

            //Replace 1/2 with H before special characters are stripped out.
            address1 = address1.Replace("1/2", "H");

            //If the number contains .5, replace it with H before special characters are stripped out.
            var pointFiveRegEx = new Regex(@"^\d+\.5");
            var numberPart = address1.Substring(0, address1.IndexOf(" ", StringComparison.Ordinal));
            if (pointFiveRegEx.IsMatch(numberPart))
                address1 = pointFiveRegEx.Replace(address1, numberPart.TrimEnd(".5".ToCharArray()) + "H", 1);

            //Remove any non alpha-numeric characters
            var chrsToIgnore = new[] { '~', '`', '!', '@', '$', '%', '^', '&', '*', '(', ')', '_', '=', '+', '{', '}', '[', ']', '|', '\\', '/', '<', '>', ':', ';', '"', '\'', ',', '.', '?' };
            address1 = chrsToIgnore.Aggregate(address1, (current, chr) => current.Replace(chr.ToString(), ""));

            //Replace any instance of two spaces with a single space.
            while (address1.Contains("  "))
                address1 = address1.Replace("  ", " ");

            //Commas have been removed. Now remove "Louisville KY" since this algorithm only parses Louisville addresses. We can't remove "Louisville" because there exists a street with that name.
            address1 = address1.Replace("LOUISVILLE KY", "");

            //Trim leading and trailing spaces and split the address into its parts.
            var addressParts = address1.Trim().Split(' ').ToList();

            //Return an empty result list if the address has no parts.
            if (addressParts.Count == 0)
                return new List<AddressResult>();

            //If a SUD is found, assume that the following addressPart is an apartment number. Skip the first addresspart since it is already determined to be the number.
            //At this point, it is possible that we have multiple Secondary Unit Designators, especially if they included both a SUD and a # sign. We will want to consider
            //the possibility that that is both accurate or inaccurate. Create a new version of the address where any extra SUDs have been removed and pass that back through 
            //this function. Then continue on with the original input.
            var sudIndexes = new List<int>();
            var distinctAddressInputs = new List<string>();
            var sudCount = 0;
            for (var i = 0; i < addressParts.Count; i++)
            {
                if (sudLookup.Contains(addressParts[i]))
                {
                    sudIndexes.Add(i);
                    sudCount++;
                }
            }
            if (sudCount > 1)
            {
                foreach (var sudIndex in sudIndexes)
                {
                    var sudRemoved = addressParts.Where((t, i) => i != sudIndex).Aggregate("", (current, t) => current + (t + " "));
                    //Don't send this through if we have already passed the exact string (i.e. there were two of the same SUD, 123 Main St Apt Apt 103)
                    if (distinctAddressInputs.Contains(sudRemoved))
                        continue;

                    distinctAddressInputs.Add(sudRemoved);
                    var sudRemovedParsings = await Parse(sudRemoved);
                    addressResults.AddRange(sudRemovedParsings);
                }
            }

            var firstPartNumber = "";
            var firstPartHalfHouse = "";
            //Break apart the numeric and non-numeric portions of the first part of the address in case the half house is contained in this part of the string.
            foreach (var chr in addressParts[0])
            {
                var numericRegEx = new Regex("[0-9]");
                if (numericRegEx.IsMatch(chr.ToString()))
                    firstPartNumber += chr;
                else
                    firstPartHalfHouse += chr;
            }

            //If the first part of the address contains numbers, assign them as the "number" part of the address. Otherwise, return null. All addresses must have a number.
            int iNumber;
            if (!int.TryParse(firstPartNumber, out iNumber))
                return new List<AddressResult>();

            //Hard-code a list of possible half houses. Find anything that appeared as part of the number, or the first address part after the number that resembles a half house
            //and add it to a list of half houses with a the index of the half house as it appears in addressParts.
            var halfHouseLookup = new[] { "A", "B", "C", "D", "F", "H", "M", "R", "U" };
            var halfHouses = new Dictionary<int, string>();
            if (halfHouseLookup.Any(hh => firstPartHalfHouse == hh))
            {
                //Since half house was found with no space between it and the number, it is still at the end of addressParts[0]. It needs to be at addressParts[1].
                addressParts[0] = addressParts[0].TrimEnd(firstPartHalfHouse.ToCharArray());
                addressParts.Insert(1, firstPartHalfHouse);
                halfHouses.Add(1, firstPartHalfHouse);
            }
            else if (halfHouseLookup.Any(hh => addressParts.Count > 1 && addressParts[1] == hh))
                halfHouses.Add(1, addressParts[1]);

            //Add empty string as a possible half house.
            halfHouses.Add(-1, "");

            var apts = new Dictionary<int, string>();
            var sudFound = false;
            for (var i = 1; i < addressParts.Count; i++)
            {
                if (sudFound)
                {
                    apts.Add(i, addressParts[i]);
                    sudFound = false;
                }
                else if (sudLookup.Contains(addressParts[i]))
                    sudFound = true;
            }
            //Add empty apartment as a possible apartment.
            apts.Add(-1, "");

            //Get a list of all possible tags (street suffixes).
            var tagLookup = await _tagRepository.GetCommonAbbreviations();

            var directions = new Dictionary<int, string>();
            for (var i = 1; i < addressParts.Count; i++)
            {
                if (addressParts[i] == "N" || addressParts[i] == "NORTH" || addressParts[i] == "NO")
                    directions.Add(i, "N");
                if (addressParts[i] == "S" || addressParts[i] == "SOUTH" || addressParts[i] == "SO")
                    directions.Add(i, "S");
                if (addressParts[i] == "E" || addressParts[i] == "EAST")
                    directions.Add(i, "E");
                if (addressParts[i] == "W" || addressParts[i] == "WEST")
                    directions.Add(i, "W");
            }
            //Add empty string as a possible direction.
            directions.Add(-1, "");

            var tags = new Dictionary<int, string>();
            for (var i = 1; i < addressParts.Count; i++)
            {
                //Always add the USPSStandardAbbreviation, even if the tag found is a CommonAbbreviation.
                var commonTagMatches = tagLookup.Where(t => string.Equals(t.CommonAbbreviation, addressParts[i], StringComparison.CurrentCultureIgnoreCase)).ToList();
                if (commonTagMatches.Any())
                {
                    tags.Add(i, commonTagMatches.First().StandardAbbreviation);
                    continue;
                }
                var uspsTagMatches = tagLookup.Where(t => string.Equals(t.StandardAbbreviation, addressParts[i], StringComparison.CurrentCultureIgnoreCase)).ToList();
                if (uspsTagMatches.Any())
                    tags.Add(i, uspsTagMatches.First().StandardAbbreviation);
            }
            //Add empty string as a possible tag.
            tags.Add(-1, "");

            //Loop through every combination of half house, direction, tag, and apartment.
            foreach (var halfHouse in halfHouses)
            {
                foreach (var direction in directions)
                {
                    foreach (var tag in tags)
                    {
                        foreach (var apt in apts)
                        {
                            var address = new AddressResult
                            {
                                Number = iNumber,
                                HalfHouse = halfHouse.Value,
                                Direction = direction.Value,
                                Street = "",
                                Tag = tag.Value,
                                Apartment = apt.Value,
                                Score = 1
                            };

                            //If the address part isn't already designated as a half house, direction, tag, or apartment, add it to the street. If there is an apartment
                            //designated, ignore the previous string since it will be the Secondary Unit Designator.
                            for (var i = 1; i < addressParts.Count; i++)
                            {
                                if (halfHouse.Key != i && direction.Key != i && tag.Key != i && apt.Key != i && apt.Key != i + 1)
                                    address.Street += (i > 1 ? " " : "") + addressParts[i];

                                //The score was initialized as 1. 1 is the lowest initial score. Anything less than one indicates an address part that was found out of
                                //place. If this verions contains a half house, increment the score.
                                if (halfHouse.Key == i)
                                    address.Score++;
                                if (direction.Key == i)
                                {
                                    //Only give a point for the direction if it falls in an expected order.
                                    if (direction.Key == 1) //After number
                                        address.Score++;
                                    else if (direction.Key == 2 && halfHouse.Key == 1) //After half house
                                        address.Score++;
                                    else if (direction.Key + 1 == addressParts.Count) //At the end.
                                        address.Score++;
                                    else if (direction.Key + 3 == addressParts.Count && apt.Key + 1 == addressParts.Count) //Third from the end if apartment is at end.
                                        address.Score++;
                                    else //Subtract a point for a direction that is out of place.
                                        address.Score--;
                                }
                                if (tag.Key == i)
                                {
                                    //Only give points for the tag if it falls in an expected order.
                                    //Tags get a bonus point to give an address with a tag a higher score than one without. For example "123 South St" parsed as 123 S "ST"
                                    //would get 1 point for having a direction in the right place. 123 "South" St would get two points for having a tag in the right place.
                                    if (tag.Key + 1 == addressParts.Count) //At the end.
                                        address.Score += 2;
                                    else if (tag.Key + 2 == addressParts.Count && direction.Key + 1 == addressParts.Count) //Second from the end if direction is at the end.
                                        address.Score += 2;
                                    else if (tag.Key + 3 == addressParts.Count && apt.Key + 1 == addressParts.Count) //Third from the end if apartment is at the end.
                                        address.Score += 2;
                                    else if (tag.Key + 4 == addressParts.Count && apt.Key + 1 == addressParts.Count && direction.Key + 3 == addressParts.Count) //Fourth from the end if apartment is at the end and direction is third from end.
                                        address.Score += 2;
                                    else //Subtract a point for a tag that is out of place.
                                        address.Score--;
                                }
                                if (apt.Key == i + 1)
                                {
                                    //Only give a point for the apartment if it falls in an expected order.
                                    if (apt.Key == addressParts.Count - 1)
                                        address.Score++; //Apartment is at the end.
                                    else //Subtract a point for an apartment that is out of place.
                                        address.Score--;
                                }
                            }

                            //There is a possiblity that there is a leading space. Trim it off.
                            address.Street = address.Street.Trim();

                            //Replace numbered street names that are spelled out with their non-spelled out counterpart. (i.e. Third -> 3RD)
                            if (NumberedStreets.ContainsKey(address.Street))
                                address.Street = NumberedStreets[address.Street];

                            if (!string.IsNullOrWhiteSpace(address.Street))
                                addressResults.Add(address);
                        }
                    }
                }
            }

            //When we reach here, we could have duplicate results if there was more than one Secondary Unit Designator present. For now, I
            //am just returning a distinct list. We might decide that we need to get a distinct list of addresses with their highest score.
            //In my testing, the duplicate address parsings had the same score.
            return addressResults.Distinct();
        }

        public async Task<bool> AreEqual(string firstAddress, string secondAddress)
        {
            var firstAddressParsings = (await Parse(firstAddress)).ToList();
            if (!firstAddressParsings.Any())
                return false;

            var secondAddressParsings = (await Parse(secondAddress)).ToList();
            if (!secondAddressParsings.Any())
                return false;

            var maxFirstAddressScore = firstAddressParsings.Max(p => p.Score);
            var bestFirstAddressParsing = firstAddressParsings.First(p => p.Score == maxFirstAddressScore);

            var maxSecondAddressScore = secondAddressParsings.Max(p => p.Score);
            var bestSecondAddressParsing = secondAddressParsings.First(p => p.Score == maxSecondAddressScore);

            return bestFirstAddressParsing.Number == bestSecondAddressParsing.Number
                && bestFirstAddressParsing.HalfHouse == bestSecondAddressParsing.HalfHouse
                && bestFirstAddressParsing.Direction == bestSecondAddressParsing.Direction
                && bestFirstAddressParsing.Street == bestSecondAddressParsing.Street
                && bestFirstAddressParsing.Tag == bestSecondAddressParsing.Tag
                && bestFirstAddressParsing.Apartment == bestSecondAddressParsing.Apartment;
        }
    }
}
