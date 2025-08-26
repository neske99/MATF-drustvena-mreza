using Relations.Common.Data;
using Relations.Common.Entities;

namespace Relations.Common.Repositories
{
    public class RelationsRepository : IRelationsRepository
    {
        private readonly IRelationsContext _context;

        public RelationsRepository(IRelationsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string> GetRelation(int sourceUserId, int targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();

            var relation = await _context.DatabaseClient.Cypher
                .Match("(a:User)-[r]->(b:User)")
                .Where((User a) => a.Id == sourceUserId)
                .AndWhere((User b) => b.Id == targetUserId)
                .Return(r => r.Type())
                .ResultsAsync;

            // Return the first found relationship type, or empty string if none
            return relation.FirstOrDefault("NONE");
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


        public async Task<Boolean> DeleteUser(int userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                 .Match("(user:User)-[r]-()")
                    .Where((User user) => user.Id == userId)
                    .Delete("r, user")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<IEnumerable<User>> GetUserFriends(int userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:FRIEND_WITH]->(friend:User)")
                .Where((User user) => user.Id == userId)
                .Return(friend => friend.As<User>())
                .ResultsAsync;
        }

        public async Task<IEnumerable<User>> GetBlockedUsers(int userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:BLOCKED]->(blocked:User)")
                .Where((User user) => user.Id == userId)
                .Return(blocked => blocked.As<User>())
                .ResultsAsync;
        }

        public async Task<bool> RemoveFriendship(int sourceUserId, int targetUserId)
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
        public async Task<bool> BlockUser(int sourceUserId, int targetUserId)
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
                    .Create("(targetUser)-[:BLOCKED_BY]->(sourceUser)")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<bool> UnblockUser(int sourceUserId, int targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r:BLOCKED]->(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(targetUser:User)-[r:BLOCKED_BY]->(sourceUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<IEnumerable<User>> GetSentFriendRequests(int userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            return await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:REQUESTED_FRIENDSHIP_WITH]->(potenital_friend:User)")
                .Where((User user) => user.Id == userId)
                .Return(potenital_friend => potenital_friend.As<User>())
                .ResultsAsync;
        }

        public async Task<bool> SendFriendRequest(int sourceUserId, int targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)", "(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Merge("(sourceUser)-[:REQUESTED_FRIENDSHIP_WITH]->(targetUser)")
                    .Merge("(targetUser)-[:RECEIVED_FRIENDSHIP_REQUEST_FROM]->(sourceUser)")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<bool> UnsendFriendRequest(int sourceUserId, int targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r:REQUESTED_FRIENDSHIP_WITH]->(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(targetUser)-[r:RECEIVED_FRIENDSHIP_REQUEST_FROM]->(sourceUser)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetReceivedFriendRequests(int userId)
        {
            await _context.DatabaseClient.ConnectAsync();
            var result = await _context.DatabaseClient.Cypher
                .OptionalMatch("(user:User)-[:REQUESTED_FRIENDSHIP_WITH]->(potential_friend:User)")
                .Where((User potential_friend) => potential_friend.Id == userId)
                .Return(user => user.As<User>())
                .ResultsAsync;

            return result.Where(u => u != null).ToList();
        }

        public async Task<bool> AcceptFriendRequest(int sourceUserId, int targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r]-(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Merge("(sourceUser)-[:FRIEND_WITH]->(targetUser)")
                    .Merge("(sourceUser)<-[:FRIEND_WITH]-(targetUser)")
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
        }

        public async Task<bool> DeclineFriendRequest(int sourceUserId, int targetUserId)
        {
            await _context.DatabaseClient.ConnectAsync();
            await _context.DatabaseClient.Cypher
                    .Match("(sourceUser:User)-[r]-(targetUser:User)")
                    .Where((User sourceUser) => sourceUser.Id == sourceUserId)
                    .AndWhere((User targetUser) => targetUser.Id == targetUserId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

            return true;
        }


    }
}
