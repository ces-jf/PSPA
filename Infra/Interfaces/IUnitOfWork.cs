using Infra.Entidades;
using Infra.Interfaces;
using System;
using System.Collections.Generic;

namespace Infra.Class
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetClient<TEntity>(string nameInstance) where TEntity : class;
        IRepository<TEntity> StartClient<TEntity>(string nameInstance, string _index) where TEntity : class;
        IEnumerable<Index> ListarIndices();
        IEnumerable<Cabecalho> Colunas(string indexName);
        long TotalDocuments(string indexName);
        IList<Dictionary<string, string>> MatchAll(string indexName, string columnGroup = null, IList<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, int from = 0, int size = 1000);
    }
}