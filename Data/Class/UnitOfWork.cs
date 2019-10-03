using Nest;
using System;
using System.Collections.Generic;
using SystemHelper;
using Infra.Class;
using Microsoft.Extensions.Options;
using System.Linq;
using Infra.Entidades;
using Data.ContextExtension;

namespace Data.Class
{
    public class UnitOfWork : IUnitOfWork
    {
        private Uri ElasticSearchURL { get; set; }
        private ConnectionSettings ConnectionSettings { get; set; }
        private Dictionary<string, object> ClientFactory { get; set; }
        private Configuration _configuration { get; set; }

        public UnitOfWork(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
            this.ElasticSearchURL = new Uri(_configuration.ElasticSearchURL);
            this.ConnectionSettings = new ConnectionSettings(ElasticSearchURL);
            ClientFactory = new Dictionary<string, object>();
        }

        public Infra.Interfaces.IRepository<TEntity> StartClient<TEntity>(string nameInstance, string _index) where TEntity : class
        {
            if (this.ClientFactory.ContainsKey(nameInstance))
                return this.GetClient<TEntity>(nameInstance);

            this.ConnectionSettings.DefaultIndex(_index);
            var client = new ElasticClient(this.ConnectionSettings);
            var repository = new Repository<TEntity>(client);
            this.ClientFactory.Add(nameInstance, repository);

            return repository;
        }

        public Infra.Interfaces.IRepository<TEntity> GetClient<TEntity>(string nameInstance) where TEntity:class
        {
            this.ClientFactory.TryGetValue(nameInstance, out object repository);

            return (repository as Repository<TEntity>);
        }

        public IEnumerable<Index> ListarIndices()
        {
            var client = new ElasticClient(this.ConnectionSettings);
            var result = client.Indices.GetAlias();

            if (!result.IsValid)
                throw result.OriginalException;

            var indices = result.Indices.Keys;
            var indicesName = indices.Select(a => a.Name).Where(a => !a.StartsWith("."))
                .Select(a => new Index {
                    Name = a
                });

            return indicesName;
        }

        public IEnumerable<Cabecalho> Colunas(string indexName)
        {
            var client = new ElasticClient(this.ConnectionSettings);
            var result = client.Search<dynamic>(a =>

                a.Index(indexName)
                .Size(1)
                .Query(q => q.MatchAll())
            );

            if (!result.IsValid)
                throw result.OriginalException;

            if (result.Documents.Count < 1)
                return new List<Cabecalho> { new Cabecalho() };

            var indicesName = (result.Documents.FirstOrDefault() as Dictionary<string, object>).Select(a => new Cabecalho
            {
                Descricao = a.Key
            });

            return indicesName;
        }

        public long TotalDocuments(string indexName)
        {
            var client = new ElasticClient(this.ConnectionSettings);

            var request = client.Count<Dictionary<string, string>>(c => c.Index(indexName));

            if (!request.IsValid)
                throw request.OriginalException;

            return request.Count;
        }

        public IList<Dictionary<string, string>> MatchAll(string indexName, bool columnGroup = false, IList<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, int from = 0, int size = 1000)
        {
            var client = new ElasticClient(this.ConnectionSettings);

            if (selectFilter == null)
                selectFilter = this.Colunas(indexName).Select(a => a.Descricao).ToList();

            var selectFilterArray = selectFilter.ToArray();

            client.Indices.UpdateSettings(indexName, a => a.IndexSettings(z => z.Setting("index.max_result_window", int.MaxValue)));

            var result = client.Search<Dictionary<string, string>>(a =>
            a.Index(indexName)
                .Source(s =>
                    s.Includes(i =>
                        i.Fields(selectFilterArray)))
                .From(from)
                .Size(size)
                .Query(query => query.FilterMatch(filterFilter))
            );

            if (!result.IsValid)
                throw result.OriginalException;

            if (result.Documents.Count < 1)
                return new List<Dictionary<string, string>> { new Dictionary<string, string>() };

            var resultDictionary = result.Documents.ToList();

            if (columnGroup)
            {
                resultDictionary = resultDictionary.Select(a => a.Values).Select(a => a.FirstOrDefault()).GroupBy(z => z).Select(a => new Dictionary<string, string>() {
                    { a.Key, a.Count().ToString() }
                }).ToList();
            }

            return resultDictionary;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: descartar estado gerenciado (objetos gerenciados).
                    if (this.ClientFactory != null)
                        this.ClientFactory = null;

                    if (this.ConnectionSettings != null)
                        this.ConnectionSettings = null;

                    if (this.ElasticSearchURL != null)
                        this.ElasticSearchURL = null;

                    if (this._configuration != null)
                        this._configuration = null;
                }

                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~UnitOfWork()
        // {
        //   // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
