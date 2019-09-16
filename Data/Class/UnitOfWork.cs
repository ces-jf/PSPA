using Nest;
using System;
using System.Collections.Generic;
using SystemHelper;
using Infra.Class;
using Microsoft.Extensions.Options;
using System.Linq;
using Infra.Entidades;

namespace Data.Class
{
    public class UnitOfWork : IUnitOfWork
    {
        private Uri ElasticSearchURL { get; set; }
        private readonly ConnectionSettings ConnectionSettings;
        private readonly Dictionary<string, object> ClientFactory;
        private readonly Configuration _configuration;

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

        public IEnumerable<Dictionary<string, string>> MatchAll(string indexName, IEnumerable<string> selectFilter = null, int from = 0, int size = 1000)
        {
            var client = new ElasticClient(this.ConnectionSettings);

            if(selectFilter == null)
                selectFilter = this.Colunas(indexName).Select(a => a.Descricao);

            var selectFilterArray = selectFilter.ToArray();

            var result = client.Search<Dictionary<string, string>>(a =>
            a.Index(indexName)
                .Source(s => 
                    s.Includes(i => 
                        i.Fields(selectFilterArray)))
                .From(from)
                .Size(size)
                .Query(query => query.MatchAll())
            );

            if (!result.IsValid)
                throw result.OriginalException;

            if (result.Documents.Count < 1)
                return new List<Dictionary<string, string>> { new Dictionary<string, string>() };

            return result.Documents;
        }
    }
}
