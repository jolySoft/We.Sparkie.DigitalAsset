using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using Microsoft.Extensions.Configuration;
using We.Sparkie.DigitalAsset.Api.Entities;

namespace We.Sparkie.DigitalAsset.Api.Services
{
    public class AzureStorage : ICloudStorage
    {
        private string _connectionString;
        private CloudFileClient _client;

        public AzureStorage(IConfiguration configuration)
        {
            _connectionString = configuration["AZURE_FILE_CONNECTION_STRING"];
            var storageAccount = CloudStorageAccount.Parse(_connectionString);

            _client = storageAccount.CreateCloudFileClient();
        }

        public async Task<Guid> Upload(Asset asset)
        {
            var location = Guid.NewGuid();

            var share = _client.GetShareReference("audio");
            await share.CreateIfNotExistsAsync();
            var dir = share.GetRootDirectoryReference();
            var file = dir.GetFileReference(location.ToString());
            await file.UploadFromStreamAsync(asset.Stream);
            file.Properties.ContentType = asset.ContentType;
            await file.SetPropertiesAsync();

            return location;
        }

        public async Task<MemoryStream> Download(Guid location)
        {
            var stream = new MemoryStream();
            var share = _client.GetShareReference("audio");
            var dir = share.GetRootDirectoryReference();
            var file = dir.GetFileReference(location.ToString());
            await file.DownloadToStreamAsync(stream);
            return stream;
        }
    }
}