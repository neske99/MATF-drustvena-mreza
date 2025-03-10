using Neo4jClient;

namespace Relations.API.Data
{
    public class RelationsContext : IRelationsContext
    {

        public RelationsContext()
        {
            DatabaseClient = new BoltGraphClient("neo4j://localhost:7687", "neo4j", "12345678");
        }

        public IGraphClient DatabaseClient { get;  }
    }
}
