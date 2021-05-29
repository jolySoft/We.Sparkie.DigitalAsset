using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using We.Sparkie.DigitalAsset.Api.Entities;
using Xunit;

namespace We.Sparkie.DigitalAsset.Tests
{
    public class MetaDataTests : TestBase
    {
        [Fact]
        public async Task Can_get_metadata()
        {
            CloudStorageStub.Asset = new Asset
            {
                Id = Guid.NewGuid(),
                BitDepth = 24,
                ContentType = "audio/wav",
                Location = Guid.NewGuid(),
                Name = "24_44k_PerfectTest.wav",
                SampleRate = 44100,
                Size = 3458286M
            };
            await Database.GetCollection<Asset>("Asset").InsertOneAsync(CloudStorageStub.Asset);

            var queryString = new Dictionary<string, string>
            {
                {"id", CloudStorageStub.Asset.Id.ToString()}
            };

            var pathAndQueryString = QueryHelpers.AddQueryString($"api/metaasset/{CloudStorageStub.Asset.Id}", queryString);
            var response = await Client.GetAsync($"api/metaasset/{CloudStorageStub.Asset.Id}");
            var content = await response.Content.ReadAsStringAsync();
            var asset = JsonConvert.DeserializeObject<Asset>(content);

            asset.BitDepth.Should().Be(24);
            asset.ContentType.Should().Be("audio/wav");
            asset.Name.Should().Be("24_44k_PerfectTest.wav");
            asset.SampleRate.Should().Be(44100);
            asset.Size.Should().Be(3458286M);
        }
    }
}