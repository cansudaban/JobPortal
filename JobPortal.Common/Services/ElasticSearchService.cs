

using Microsoft.Extensions.Configuration;
using Nest;

namespace JobPortal.Common.Services
{
    public class ElasticSearchService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticSearchService(IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:Uri"]))
                           .DefaultIndex("jobs");

            _elasticClient = new ElasticClient(settings);
        }

        public IElasticClient Client => _elasticClient;
    }
}
