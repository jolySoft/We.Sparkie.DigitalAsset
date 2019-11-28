using System.IO;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    internal interface IEncodingStrategy
    {
        void PopulateAssetMetadata(Asset asset, MemoryStream audioStream);
    }
}