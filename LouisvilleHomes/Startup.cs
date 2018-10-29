using LouisvilleHomes.Data;
using LouisvilleHomes.Models.Alexa;
using LouisvilleHomes.Models.Caching;
using LouisvilleHomes.Models.GoogleMaps;
using LouisvilleHomes.Models.PVA;
using LouisvilleHomes.Utilities;
using LouisvilleHomes.Utilities.Caching;
using LouisvilleHomes.Utilities.GoogleMaps;
using LouisvilleHomes.Utilities.PVA;
using LouisvilleHomes.Utilities.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace LouisvilleHomes
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var louisvilleDemographicsConnection = Configuration.GetConnectionString("LouisvilleDemographics");
            services.AddDbContext<LouisvilleDemographicsContext>(options => options.UseSqlServer(louisvilleDemographicsConnection));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Louisville Homes API", Version = "v1" });
            });

            var alexaSettings = new AlexaSettings();
            Configuration.Bind("AlexaSettings", alexaSettings);
            services.AddSingleton(alexaSettings);

            var pvaSettings = new PvaSettings();
            Configuration.Bind("PVASettings", pvaSettings);
            services.AddSingleton(pvaSettings);

            var sheriffSettings = new SheriffSettings();
            Configuration.Bind("SheriffSettings", sheriffSettings);
            services.AddSingleton(sheriffSettings);

            var cacheSettings = new CacheSettings();
            Configuration.Bind("CacheSettings", cacheSettings);
            services.AddSingleton(cacheSettings);

            var googleMapsSettings = new GoogleMapsSettings();
            Configuration.Bind("GoogleMapsSettings", googleMapsSettings);
            services.AddSingleton(googleMapsSettings);

            switch (cacheSettings.Type)
            {
                case CacheType.None:
                    services.AddSingleton<IDistributedCache>(new NoCacheDistributedCache());
                    break;
                case CacheType.InMemory:
                    services.AddDistributedMemoryCache();
                    break;
                //case CacheType.Redis:
                //    services.AddDistributedRedisCache(option =>
                //    {
                //        option.Configuration = cacheSettings.Redis;
                //        option.InstanceName = $"{_configuration["ApplicationName"]}:{_hostingEnvironment.EnvironmentName}:";
                //    });
                //    break;
            }
            services.AddSingleton<ICacheAdapter, CacheAdapter>();

            services.AddScoped<ISkillRequestHandler, SkillRequestHandler>();

            services.AddScoped<IAddressParser, AddressParser>();

            services.AddScoped<IPvaAddressLookup, PvaAddressLookup>();

            services.AddScoped<ITagRepository, TagRepository>();

            services.AddScoped<IZipCodeRepository, ZipCodeRepository>();

            services.AddHttpClient<IDeviceApiHttpClient, DeviceApiHttpClient>();

            services.AddSingleton<IMapImageUrlGenerator, MapImageUrlGenerator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(env.IsDevelopment() ? "v1/swagger.json" : "swagger/v1/swagger.json", "Louisville Homes API V1");
            });
        }
    }
}
