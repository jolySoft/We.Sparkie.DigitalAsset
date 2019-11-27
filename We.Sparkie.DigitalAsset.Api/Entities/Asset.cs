using System;
using System.Collections.Generic;
using System.IO;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    public class Asset : Entity
    {
        private Dictionary<string, IEncodingStrategy> _encodingStrategies;

        public Asset()
        {
            _encodingStrategies = new Dictionary<string, IEncodingStrategy>
            {
                { "wav", new WavStrategy() }
            };
        }

        public string Name { get; set; }
        public decimal Size { get; set; }
        public int BitDepth { get; set; }
        public decimal SampleRate { get; set; }
        public Uri Location { get; set; }
        public string Genre { get; set; }


        public void Upload(string name, MemoryStream audioStream)
        {
            Name = name;
            var typeFromExtension = ExtractTypeFromExtension(name);
            _encodingStrategies[typeFromExtension].PopulateAssetMetadata(this, audioStream);
        }

        private string ExtractTypeFromExtension(string name)
        {
            return "wav";
        }
    }
}