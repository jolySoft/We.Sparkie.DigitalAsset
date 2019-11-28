using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using We.Sparkie.DigitalAsset.Api.Entities;

namespace We.Sparkie.DigitalAsset.Api.Repository
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<List<TEntity>> Get();
        Task<TEntity> Get(Guid id);
        Task Insert(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
        Task<bool> Delete(Guid id);
    }
}