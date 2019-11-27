using System;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    public class Asset : Entity
    {
        public string Name { get; set; }
        public decimal Size { get; set; }
        public int BitDepth { get; set; }
        public int SampleRate { get; set; }
        public Uri Location { get; set; }
    }
}