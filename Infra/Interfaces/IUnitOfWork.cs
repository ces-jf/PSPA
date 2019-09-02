using Infra.Entidades;
using Infra.Interfaces;
using System.Collections.Generic;

namespace Infra.Class
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetClient<TEntity>(string nameInstance) where TEntity : class;
        IRepository<TEntity> StartClient<TEntity>(string nameInstance, string _index) where TEntity : class;
        IEnumerable<Index> ListarIndices();
        IEnumerable<Cabecalho> Colunas(string indexName);
        IEnumerable<Dictionary<string, string>> MatchAll(string indexName);
    }
}