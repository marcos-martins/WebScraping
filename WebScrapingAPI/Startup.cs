using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScrapingAPI.Abstract;
using WebScrapingAPI.Hub;
using WebScrapingAPI.Model;
using WebScrapingAPI.Service;

namespace WebScrapingAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            var config = new SeleniumConfigurations();
            Configuration.Bind("SeleniumConfigurations", config);
            services.AddSingleton(config);
            services.AddSingleton<ICnpjService, CnpjService>();

            services.AddCors(o => o.AddPolicy("cors", builder => {
                builder
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .WithOrigins("http://localhost:5000");
            }));
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("cors");
           
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Scraping");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HubClient>("/hub");
            });
        }
    }
}
