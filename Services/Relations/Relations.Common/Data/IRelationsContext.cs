using Neo4jClient;

namespace Relations.Common.Data
{
    public interface IRelationsContext
    {
        IGraphClient DatabaseClient { get;  }
    }
}
