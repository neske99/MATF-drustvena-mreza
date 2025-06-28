using Neo4jClient;

namespace Relations.API.Data
{
    public class RelationsContext : IRelationsContext
    {

        public RelationsContext(IConfiguration configuration)
        {

            DatabaseClient = new BoltGraphClient(configuration.GetValue<string>("Neo4jSettings:ConnectionString"),
                                                 configuration.GetValue<string>("Neo4jSettings:Username"),
                                                 configuration.GetValue<string>("Neo4jSettings:Password"));

        }

        public IGraphClient DatabaseClient { get;  }
    }
}
