using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using We.Sparkie.DigitalAsset.Api.Entities;

namespace We.Sparkie.DigitalAsset.Api.Services
{
    public class AzureStorage : ICloudStorage
    {
        private BlobServiceClient _serviceClient;
        private readonly string _connectionString;
        private BlobContainerClient _containerClient;

        public AzureStorage(IConfiguration configuration)
        {
            _connectionString = configuration["AZURE_BLOB_CONNECTION_STRING"];
            _serviceClient = new BlobServiceClient(_connectionString);
        }

        public async Task<Guid> Upload(Asset asset)
        {
            _containerClient = await _serviceClient.CreateBlobContainerAsync("Audio");
            var location = Guid.NewGuid();
            var blobClient = _containerClient.GetBlobClient(location.ToString());
            await blobClient.UploadAsync(asset.Stream);
            return location;
        }

        public Task<MemoryStream> Download(Guid location)
        {
            throw new NotImplementedException();
        }
    }
}