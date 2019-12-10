using System;
using System.IO;
using System.Net.Http.Headers;
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
            await file.CopyToAsync(stream);
            var asset = new Asset();
            await asset.Upload(file.FileName, stream, _storage, _repository);
            return Ok(asset.Id);
        }

        [Route("download")]
        [HttpGet]
        public async Task<FileStreamResult> Download(Guid id)
        {
            var asset = await Asset.Download(id, _storage, _repository);
            Response.ContentType = new MediaTypeHeaderValue(asset.ContentType).ToString();// Content type
            return new FileStreamResult(asset.Stream, asset.ContentType) { FileDownloadName = asset.Name };
        }
    }
}