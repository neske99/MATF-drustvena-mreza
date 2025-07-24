using Microsoft.Extensions.Configuration;
using Neo4jClient;

namespace Relations.Common.Data
{
    public class RelationsContext : IRelationsContext
    {

        public RelationsContext(IConfiguration configuration)
        {
            // Use the extension method by adding the correct using directive above
            DatabaseClient = new BoltGraphClient(
                configuration.GetSection("Neo4jSettings")["ConnectionString"],
                configuration.GetSection("Neo4jSettings")["Username"],
                configuration.GetSection("Neo4jSettings")["Password"]
            );
        }

        public IGraphClient DatabaseClient { get; }
    }
}
