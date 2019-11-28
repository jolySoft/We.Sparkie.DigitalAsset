using System.IO;
using NAudio.Wave;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    public class WavStrategy : IEncodingStrategy
    {
        
        public WavStrategy()
        {
        }

        public void PopulateAssetMetadata(Asset asset, MemoryStream audioStream)
        {
            
            IWaveProvider provider = new WaveFileReader(audioStream);

            asset.BitDepth = provider.WaveFormat.BitsPerSample;
            asset.Size = audioStream.Length;
            asset.SampleRate = provider.WaveFormat.SampleRate;
        }
    }
}