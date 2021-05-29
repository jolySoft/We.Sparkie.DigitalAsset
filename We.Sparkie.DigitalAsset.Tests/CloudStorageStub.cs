using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using We.Sparkie.DigitalAsset.Api.Entities;
using We.Sparkie.DigitalAsset.Api.Services;

namespace We.Sparkie.DigitalAsset.Tests
{
    public class CloudStorageStub : ICloudStorage
    {
        public Asset Asset { get; set; }
        public Guid Location { get; private set; }

        public Task<Guid> Upload(Asset asset)
        {
            Asset = asset;
            Location = Guid.NewGuid();
            return Task.FromResult(Location);
        }

        public Task<MemoryStream> Download(Guid location)
        {
            return Task.FromResult(Asset.Stream);
        }
    }
}