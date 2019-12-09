using System;
using System.IO;
using System.Threading.Tasks;
using We.Sparkie.DigitalAsset.Api.Entities;

namespace We.Sparkie.DigitalAsset.Api.Services
{
    public interface ICloudStorage
    {
        Task<Guid> Upload(Asset asset);
        Task<MemoryStream> Download(Guid location);
    }
}