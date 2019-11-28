using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using We.Sparkie.DigitalAsset.Api.Repository;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    public class Asset : Entity
    {
        private readonly Dictionary<string, IEncodingStrategy> _encodingStrategies;

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
        public Guid Location { get; set; }
        public string Genre { get; set; }


        public async Task Upload(string name, MemoryStream audioStream, ICloudStorage storage, IRepository<Asset> repository)
        {
            Name = name;
            var typeFromExtension = ExtractTypeFromExtension(name);
            _encodingStrategies[typeFromExtension].PopulateAssetMetadata(this, audioStream);

            Location = await storage.Upload(this);
            await repository.Insert(this);
        }

        private string ExtractTypeFromExtension(string name)
        {
            var extension = Path.GetExtension(name);
            return extension.Substring(1).ToLower();
        }
    }
}