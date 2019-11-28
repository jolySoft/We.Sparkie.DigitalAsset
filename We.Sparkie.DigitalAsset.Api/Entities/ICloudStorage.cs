using System;
using System.Threading.Tasks;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    public interface ICloudStorage
    {
        Task<Guid> Upload(Asset asset);
        Task<Asset> Download(Guid location);

    }
}