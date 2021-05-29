using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Mongo2Go;
using MongoDB.Driver;
using We.Sparkie.DigitalAsset.Api;
using We.Sparkie.DigitalAsset.Api.Services;

namespace We.Sparkie.DigitalAsset.Tests
{
    public class TestBase : IDisposable
    {
        private MongoDbRunner _runner;
        protected TestServer TestServer { get; private set; }
        protected HttpClient Client { get; private set; }
        protected IMongoDatabase Database { get; private set; }
        protected CloudStorageStub CloudStorageStub { get; private set; }

        public TestBase()
        {
            CloudStorageStub = new CloudStorageStub();
            _runner = MongoDbRunner.StartForDebugging();
            var client = new MongoClient(_runner.ConnectionString);
            Database = client.GetDatabase("Sparkie");

            Startup.OnServiceRegistration = services =>
            {
                services.AddScoped<IMongoDatabase>((_) => Database);
                services.AddScoped<ICloudStorage>((_) => CloudStorageStub);
            };

            TestServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = TestServer.CreateClient();
        }

        public void Dispose()
        {
            TestServer?.Dispose();
            Client?.Dispose();
            _runner?.Dispose();
        }
    }
}