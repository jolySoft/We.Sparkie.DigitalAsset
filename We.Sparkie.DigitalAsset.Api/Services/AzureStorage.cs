using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using We.Sparkie.DigitalAsset.Api.Entities;

namespace We.Sparkie.DigitalAsset.Api.Services
{
    public class AzureStorage : ICloudStorage
    {
        private string _accountName;
        private string _key;
        private CloudBlobClient _client;

        public AzureStorage(IConfiguration configuration)
        {
            _accountName = configuration["AZURE_BLOB_ACCOUNT_NAME"];
            _key = configuration["AZURE_BLOB_KEY"];
            var creds = new StorageCredentials(_accountName, _key);
            _client = new CloudBlobClient(new Uri("https://sparkieassets.blob.core.windows.net"), creds);
        }

        public async Task<Guid> Upload(Asset asset)
        {
            var location = Guid.NewGuid();
            var container = _client.GetContainerReference("audio");
            var blob = container.GetBlockBlobReference(location.ToString());
            blob.Metadata["ContentType"] = asset.ContentType;
            await blob.UploadFromStreamAsync(asset.Stream);
            
            return location;
        }

        public async Task<MemoryStream> Download(Guid location)
        {
            var stream = new MemoryStream();
            var container = _client.GetContainerReference("audio");
            var blob = container.GetBlockBlobReference(location.ToString());
            await blob.DownloadToStreamAsync(stream);
            return stream;
        }
    }
}