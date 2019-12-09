using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using We.Sparkie.DigitalAsset.Api.Entities;
using We.Sparkie.DigitalAsset.Api.Repository;
using We.Sparkie.DigitalAsset.Api.Services;

namespace We.Sparkie.DigitalAsset.Api.Controllers
{
    [Route("api/asset")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private ICloudStorage _storage;
        private IRepository<Asset> _repository;

        public AssetController(ICloudStorage storage, IRepository<Asset> repository)
        {
            _storage = storage;
            _repository = repository;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);
            var asset = new Asset();
            await asset.Upload(file.FileName, stream, _storage, _repository);
            return Ok(asset.Id);
        }
    }
}