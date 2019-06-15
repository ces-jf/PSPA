using Infra.Interfaces;

namespace Infra.Class
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetClient<TEntity>(string nameInstance) where TEntity : class;
        bool IndexExists(string _indexName);
        IRepository<TEntity> StartClient<TEntity>(string nameInstance, string _index) where TEntity : class;
    }
}