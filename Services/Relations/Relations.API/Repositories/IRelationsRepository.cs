using Relations.API.Entities;

namespace Relations.API.Repositories
{
    public interface IRelationsRepository
    {
        Task<IEnumerable<User>> GetUserFriends(string userId);
    }
}
