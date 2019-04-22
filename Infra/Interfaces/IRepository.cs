using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infra.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetByID(string _id);
        Task<TEntity> GetByIDAsync(string _id);
        void InsertAsync(TEntity entity);
        void Insert(TEntity entity);
        void BulkInsert(IEnumerable<TEntity> entities);
    }
}