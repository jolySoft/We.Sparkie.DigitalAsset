using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using We.Sparkie.DigitalAsset.Api.Entities;
using We.Sparkie.DigitalAsset.Api.Repository;

namespace We.Sparkie.DigitalAsset.Tests.Steps
{
    [Binding]
    public class DigitalAssetSteps
    {
        private MemoryStream _memoryStream;
        private Asset _asset;
        private ICloudStorage _storage;
        private IRepository<Asset> _repository;
        private Guid _location;

        public DigitalAssetSteps()
        {
            
            _storage = A.Fake<ICloudStorage>();
            _location = new Guid();
            A.CallTo(() => _storage.Upload(_asset)).Returns(Task.FromResult(_location));

            _repository = A.Fake<IRepository<Asset>>();
            A.CallTo(() => _repository.Insert(_asset)).Returns(Task.CompletedTask);
        }

        [Given(@"I have this wav file")]
        public void GivenIHaveThisWavFile()
        {
            var bytes = File.ReadAllBytes("Data\\Ochre - Project Caelus - 06 Crowd of Stars.wav");
            _memoryStream = new MemoryStream(bytes);
        }

        [When(@"I upload it")]
        public async Task WhenIUploadIt()
        {
            _asset = new Asset();
            await _asset.Upload("Ochre - Project Caelus - 06 Crowd of Stars.wav", _memoryStream, _storage, _repository);
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
    }
}