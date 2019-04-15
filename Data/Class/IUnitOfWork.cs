using Data.Interfaces;

namespace Data.Class
{
    public interface IUnitOfWork
    {
        Repository<TEntity> GetClient<TEntity>(string nameInstance) where TEntity : class;
        bool IndexExists(string _indexName);
        IRepository<TEntity> StartClient<TEntity>(string nameInstance, string _index) where TEntity : class;
    }
}