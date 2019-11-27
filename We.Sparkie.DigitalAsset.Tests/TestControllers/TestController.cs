using We.Sparkie.DigitalAsset.Api.Controllers;
using We.Sparkie.DigitalAsset.Api.Repository;
using We.Sparkie.DigitalAsset.Tests.TestEntities;

namespace We.Sparkie.DigitalAsset.Tests.TestControllers
{
    public class TestController : EntityController<TestEntity>
    {
        public TestController(Repository<TestEntity> repository) : base(repository)
        {
        }
    }
}