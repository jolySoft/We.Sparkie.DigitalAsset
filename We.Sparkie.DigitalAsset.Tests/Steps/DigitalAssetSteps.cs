using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using We.Sparkie.DigitalAsset.Api.Entities;
using We.Sparkie.DigitalAsset.Api.Repository;
using We.Sparkie.DigitalAsset.Api.Services;

namespace We.Sparkie.DigitalAsset.Tests.Steps
{
    [Binding]
    public class DigitalAssetSteps
    {
        private MemoryStream _memoryStream;
        private Asset _asset;
        private List<Asset> _assets;
        private ICloudStorage _storage;
        private IRepository<Asset> _repository;
        private Guid _location;
        private Guid _downloadId;

        public DigitalAssetSteps()
        {
            
            _storage = A.Fake<ICloudStorage>();

            _repository = A.Fake<IRepository<Asset>>();
        }

        [Given(@"I have this wav file")]
        public void GivenIHaveThisWavFile()
        {
            GetMemoryStream();

            _location = new Guid();
            A.CallTo(() => _storage.Upload(_asset)).Returns(Task.FromResult(_location));
            A.CallTo(() => _repository.Insert(_asset)).Returns(Task.CompletedTask);
        }

        private void GetMemoryStream()
        {
            var bytes = File.ReadAllBytes("Data\\24_44k_PerfectTest.wav");
            _memoryStream = new MemoryStream(bytes);
        }

        [Given(@"I have uploaded this asset")]
        public void GivenIHaveUploadThisAsset(Table table)
        {
            var asset = table.CreateInstance<Asset>();
            GetMemoryStream();
            _downloadId = asset.Id;
            A.CallTo(() => _repository.Get(asset.Id)).Returns(Task.FromResult(asset));
            A.CallTo(() => _storage.Download(asset.Location)).Returns(Task.FromResult(_memoryStream));
        }

        [Given(@"I have uploaded these assets")]
        public void GivenIHaveUploadTheseAssets(Table table)
        {
            var assets = table.CreateSet<Asset>().ToList();
            A.CallTo(() => _repository.Get()).Returns(Task.FromResult(assets));
        }

        [When(@"I upload it")]
        public async Task WhenIUploadIt()
        {
            _asset = new Asset();
            await _asset.Upload("Ochre - Project Caelus - 06 Crowd of Stars.wav", _memoryStream, _storage, _repository);
        }

        [When(@"I download an asset with this id (.*)")]
        public async Task WhenIDownloadAnAssetWithThisId(Guid id)
        {
            _asset = await Asset.Download(id, _storage, _repository);
        }

        [When(@"I list my assets")]
        public async Task WhenIListMyAssets()
        {
            _assets = await Asset.List(_repository);
        }

        [Then(@"I get the following details")]
        public void ThenIGetTheFollowingDetails(Table table)
        {
            var expected = table.CreateInstance<Asset>();

            expected.Should().BeEquivalentTo(_asset);
        }

        [Then(@"it is put in storage")]
        public void ThenItIsPutInStorage()
        {
            A.CallTo(() => _storage.Upload(_asset)).MustHaveHappened();
            _asset.Location.Should().Be(_location);
        }

        [Then(@"its metadata is stored")]
        public void ThenItsMetadataIsStored()
        {
            A.CallTo(() => _repository.Insert(_asset)).MustHaveHappened();
        }

        [Then(@"it is download from this location (.*)")]
        public void ThenItIsDownloadFromThisLocation(Guid expectedLocation)
        {
            _asset.Location.Should().Be(expectedLocation);
        }

        [Then(@"the audio data is populated")]
        public void ThenTheAudioDataIsPopulated()
        {
            _asset.Stream.Should().Be(_memoryStream);
        }

        [Then(@"I get this list")]
        public void ThenIGetThisList(Table table)
        {
            var expected = table.CreateSet<Asset>();

            _assets.Should().AllBeEquivalentTo(expected);
        }
    }
}