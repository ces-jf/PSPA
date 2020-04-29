using Infra.Entidades;
using Infra.Interfaces;
using System;
using System.Collections.Generic;
using Index = Infra.Entidades.Index;

namespace Infra.Class
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetClient<TEntity>(string nameInstance) where TEntity : class;
        IRepository<TEntity> StartClient<TEntity>(string nameInstance, string _index) where TEntity : class;
        IEnumerable<Index> ListarIndices();
        Index GetIndex(string index);
        IEnumerable<Header> Colunas(string indexName);
        long TotalDocuments(string indexName);
        IList<Dictionary<string, string>> MatchAll(string indexName, bool columnGroup = false, IList<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, int from = 0, int size = 1000);
    }
}