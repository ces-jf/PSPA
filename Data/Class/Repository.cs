using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Elasticsearch.Net;

namespace Data.Class
{
    public class Repository<TEntity> : Interfaces.IRepository<TEntity> where TEntity: class
    {
        private ElasticClient Client { get; set; }

        public Repository(ElasticClient _client)
        {
            this.Client = _client;
        }

        public TEntity GetByID(string _id)
        {
            var searchResponse = this.Client.Search<TEntity>(s => s.From(0)

            .Size(10)
            .Query(a => a
                .Match(z => z
                    .Field("_id")
                    .Query(_id)
                    )
                )
            );

            if (!searchResponse.IsValid)
                throw searchResponse.OriginalException;

            return searchResponse.Documents.FirstOrDefault();
        }

        public async Task<TEntity> GetByIDAsync(string _id)
        {
            var searchResponse = await this.Client.SearchAsync<TEntity>(s => s.From(0)
            
            .Size(10)
            .Query(a => a
                .Match(z => z
                    .Field("_id")
                    .Query(_id)
                    )
                )            
            );

            if (!searchResponse.IsValid)
                throw searchResponse.OriginalException;

            return searchResponse.Documents.FirstOrDefault();
        }

        public void Insert(TEntity entity)
        {
            this.Client.IndexDocument(entity);
        }

        public async void InsertAsync(TEntity entity)
        {
            await this.Client.IndexDocumentAsync(entity);
        }

        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            var response = this.Client.IndexMany(entities);
        }
    }
}
