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
            IEnumerable<User> friends = await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[FRIENDS_WITH]-(friend:User)")
                .Where((User user) => user.Id == userId)
                .Return(friend => friend.As<User>())
                .ResultsAsync;
            return friends;


        }
    }
}
