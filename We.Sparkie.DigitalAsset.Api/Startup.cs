using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using We.Sparkie.DigitalAsset.Api.Repository;
using We.Sparkie.DigitalAsset.Api.Services;

namespace We.Sparkie.DigitalAsset.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration["MONGO_DB_CONNECTION_STRING"];

            services.AddScoped<IMongoDatabase>(x =>
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                settings.GuidRepresentation = GuidRepresentation.Standard;
                var client = new MongoClient(settings);
                return client.GetDatabase("Sparkie");
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICloudStorage, AzureStorage>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = this.GetType().Assembly.FullName, Version = "v1" }); });
            services.AddControllers();

            OnServiceRegistration?.Invoke(services);
        }

        public static Action<IServiceCollection> OnServiceRegistration { get; set; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Asset API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
