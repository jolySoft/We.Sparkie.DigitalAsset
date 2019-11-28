using System.IO;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using We.Sparkie.DigitalAsset.Api.Entities;

namespace We.Sparkie.DigitalAsset.Tests.Steps
{
    [Binding]
    public class DigitalAssetSteps
    {
        private MemoryStream _memoryStream;
        private Asset _asset;


        [Given(@"I have this wav file")]
        public void GivenIHaveThisWavFile()
        {
            var bytes = File.ReadAllBytes("Data\\Ochre - Project Caelus - 06 Crowd of Stars.wav");
            _memoryStream = new MemoryStream(bytes);
        }

        [When(@"I upload it")]
        public void WhenIUploadIt()
        {
            _asset = new Asset();
            _asset.Upload("Ochre - Project Caelus - 06 Crowd of Stars.wav", _memoryStream);
        }

        [Then(@"I get the following details")]
        public void ThenIGetTheFollowingDetails(Table table)
        {
            var expected = table.CreateInstance<Asset>();

            expected.Should().BeEquivalentTo(_asset);
        }

    }
}