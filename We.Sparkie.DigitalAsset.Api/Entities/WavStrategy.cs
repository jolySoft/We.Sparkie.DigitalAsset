using System.IO;
using NAudio.Wave;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    public class WavStrategy : IEncodingStrategy
    {
        public void PopulateAssetMetadata(Asset asset, MemoryStream audioStream)
        {
            audioStream.Position = 0;
            IWaveProvider provider = new WaveFileReader(audioStream);

            asset.BitDepth = provider.WaveFormat.BitsPerSample;
            asset.Size = audioStream.Length;
            asset.SampleRate = provider.WaveFormat.SampleRate;
            asset.Stream = audioStream;
        }
    }
}