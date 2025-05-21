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

        public async Task<IEnumerable<User>> GetUsers()
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)")
                .Return(user => user.As<User>())
                .ResultsAsync;
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                .Create("(user:User $newUser)")
                .WithParam("newUser", user)
                .ExecuteWithoutResultsAsync();

            return user;
        }
          

        public async Task<Boolean> DeleteUser(string userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                 .Match("(user:User)-[r]-()")
                    .Where((User user) => user.Id == userId)
                    .Delete("r, user")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<IEnumerable<User>> GetUserFriends(string userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:FRIEND_WITH]->(friend:User)")
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

        public async Task<bool> RemoveFriendship(string sourceUserId, string targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r:FRIEND_WITH]-(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
        }
        public async Task<bool> BlockUser(string sourceUserId, string targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r]-(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)", "(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Create("(sourceUser)-[:BLOCKED]->(targetUser)")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<bool> UnblockUser(string sourceUserId, string targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r:BLOCKED]->(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
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

        public async Task<bool> SendFriendRequest(string sourceUserId, string targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)", "(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Merge("(sourceUser)-[:REQUESTED_FRIENDSHIP_WITH]->(targetUser)")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<bool> UnsendFriendRequest(string sourceUserId, string targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r:REQUESTED_FRIENDSHIP_WITH]->(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
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

        public async Task<bool> AcceptFriendRequest(string sourceUserId, string targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)<-[r:REQUESTED_FRIENDSHIP_WITH]-(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Merge("(sourceUser)-[:FRIEND_WITH]->(targetUser)")
                    .Merge("(sourceUser)<-[:FRIEND_WITH]-(targetUser)")
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<bool> DeclineFriendRequest(string sourceUserId, string targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)<-[r:REQUESTED_FRIENDSHIP_WITH]-(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
        }


    }
}
