using Relations.API.Data;
using Relations.API.Entities;

namespace Relations.API.Repositories
{
    public class RelationsRepository : IRelationsRepository
    {
        private readonly IRelationsContext _context;

        public RelationsRepository(IRelationsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetUserFriends(string userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:FRIENDS_WITH]->(friend:User)")
                .Where((User user) => user.Id == userId)
                .Return(friend => friend.As<User>())
                .ResultsAsync;
        }

        public async Task<IEnumerable<User>> GetBlockedUsers(string userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:BLOCKED]->(blocked:User)")
                .Where((User user) => user.Id == userId)
                .Return(blocked => blocked.As<User>())
                .ResultsAsync;
        }

        public async Task<IEnumerable<User>> GetSentFriendRequests(string userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:REQUESTED_FRIENDSHIP_WITH]->(potenital_friend:User)")
                .Where((User user) => user.Id == userId)
                .Return(potenital_friend => potenital_friend.As<User>())
                .ResultsAsync;
        }

        public async Task<IEnumerable<User>> GetReceivedFriendRequests(string userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:REQUESTED_FRIENDSHIP_WITH]->(potential_friend:User)")
                .Where((User potential_friend) => potential_friend.Id == userId)
                .Return(user => user.As<User>())
                .ResultsAsync;
        }
    }
}
