using Nest;
using System;
using System.Collections.Generic;
using SystemHelper;
using Infra.Class;
using Microsoft.Extensions.Options;

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

        public bool IndexExists (string _indexName)
        {
            var client = new ElasticClient(this.ConnectionSettings);
            var request = new IndexExistsRequest(_indexName);
            return client.IndexExists(request).Exists;
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
    }
}
