using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Services
{
    public interface IEntityService<TEntity, TRequestEntity>
    {
        Task<IEnumerable<TEntity>> GetEntitiesAsync();

        Task<TEntity> GetEntityAsync(string entityId);

        Task<TEntity> CreateEntityAsync(TRequestEntity entityRequest);

        Task<TEntity> UpdateEntityAsync(string entityId, TRequestEntity entityRequest);

        Task DeleteEntityAsync(string entityId);
    }
}
