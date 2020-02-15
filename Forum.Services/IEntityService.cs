using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum.Services
{
    public interface IEntityService<TEntity, TRequestEntity>
    {
        Task<IEnumerable<TEntity>> GetEntitiesAsync();

        Task<TEntity> GetEntityAsync(Guid entityId);

        Task<TEntity> CreateEntityAsync(TRequestEntity entityRequest);

        Task<TEntity> UpdateEntityAsync(Guid entityId, TRequestEntity entityRequest);

        Task<bool> DeleteEntityAsync(Guid entityId);
    }
}
