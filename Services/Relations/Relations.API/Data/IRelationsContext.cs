using Neo4jClient;

namespace Relations.API.Data
{
    public interface IRelationsContext
    {
        IGraphClient DatabaseClient { get;  }
    }
}
