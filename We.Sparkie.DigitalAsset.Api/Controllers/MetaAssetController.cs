using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using We.Sparkie.DigitalAsset.Api.Entities;
using We.Sparkie.DigitalAsset.Api.Repository;

namespace We.Sparkie.DigitalAsset.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("admin")]
    public class MetaAssetController : EntityController<Asset>
    {
        public MetaAssetController(IRepository<Asset> repository) : base(repository) { }
    }
}