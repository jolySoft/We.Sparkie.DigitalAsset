using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using MongoDB.Driver;
using We.Sparkie.DigitalAsset.Api.Entities;
using Xunit;

namespace We.Sparkie.DigitalAsset.Tests
{
    public class AssetTests : TestBase
    {
        private long _streamLength;
        private readonly string _uploadUrl;
        private readonly string _downloadUrl;

        public AssetTests()
        {
            _uploadUrl = "api/asset/upload";
            _downloadUrl = "api/asset/download";
        }

        [Fact]
        public async Task Can_upload_file()
        {
            var content = await BuildFormData();

            var response = await Client.PostAsync(_uploadUrl, content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            CloudStorageStub.Asset.Stream.Length.Should().Be(_streamLength);
        }

        [Fact]
        public async Task Uploaded_file_should_have_data_mined()
        {
            var content = await BuildFormData();

            var response = await Client.PostAsync(_uploadUrl, content);

            var stringContent = await response.Content.ReadAsStringAsync();
            var id = Newtonsoft.Json.JsonConvert.DeserializeObject<Guid>(stringContent);

            var query = Builders<Asset>.Filter.Eq(e => e.Id, id);
            var queryResult = await Database.GetCollection<Asset>("Asset").FindAsync(query);
            var asset = await queryResult.FirstOrDefaultAsync();

            asset.BitDepth.Should().Be(24);
            asset.ContentType.Should().Be("audio/wav");
            asset.Name.Should().Be("24_44k_PerfectTest.wav");
            asset.Location.Should().Be(CloudStorageStub.Location);
            asset.SampleRate.Should().Be(44100);
            asset.Size.Should().Be(3458286M);
        }

        [Fact]
        public async Task Can_download_file()
        {
            await BuildAsset();

            var queryString = new Dictionary<string, string>
            {
                {"id", CloudStorageStub.Asset.Id.ToString()}
            };

            var pathAndQueryString = QueryHelpers.AddQueryString(_downloadUrl, queryString);
            var response = await Client.GetAsync(pathAndQueryString);
            var content = response.Content.ReadAsStreamAsync();

            content.Result.Length.Should().Be(_streamLength);
        }

        private async Task BuildAsset()
        {
            CloudStorageStub.Asset = new Asset
            {
                Id = Guid.NewGuid(),
                BitDepth = 24,
                ContentType = "audio/wav",
                Location = Guid.NewGuid(),
                Name = "24_44k_PerfectTest.wav",
                SampleRate = 44100,
                Size = 3458286M,
                Stream = await BuildMemoryStream()
            };
            _streamLength = CloudStorageStub.Asset.Stream.Length;
            await Database.GetCollection<Asset>("Asset").InsertOneAsync(CloudStorageStub.Asset);
        }

        private async Task<MultipartFormDataContent> BuildFormData()
        {
            byte[] byteArray;
            var memoryStream = await BuildMemoryStream();
            byteArray = memoryStream.ToArray();
            _streamLength = memoryStream.Length;


            var content = new ByteArrayContent(byteArray);

            var multipartContent = new MultipartFormDataContent {{content, "file", "24_44k_PerfectTest.wav"}};
            return multipartContent;
        }

        private async Task<MemoryStream> BuildMemoryStream()
        {
            var assembly = GetType().Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                .First(s => s.EndsWith("24_44k_PerfectTest.wav", StringComparison.CurrentCultureIgnoreCase));
            byte[] byteArray;
            var memoryStream = new MemoryStream();

            await assembly.GetManifestResourceStream(resourceName).CopyToAsync(memoryStream);
            return memoryStream;
        }
    }
}