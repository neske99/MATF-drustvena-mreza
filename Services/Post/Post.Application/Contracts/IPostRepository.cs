using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Domain.Entities;

namespace Post.Application.Contracts
{
    public interface IPostRepository : IAsyncRepository<Post.Domain.Entities.Post>
    {
        Task<IEnumerable<Post.Domain.Entities.Post>> GetPostsForUser(int userId, List<int> friendIdList);
        Task<bool> CreatePostForUser(int userId, Post.Domain.Entities.Post post);
        Task<bool> AddCommentToPost(int postId, Comment comment);
        Task<bool> AddLikeToPost(int postId, Like like);
        Task<bool> RemoveLikeFromPost(int postId, int userId);
        Task<bool> DeletePost(int postId, int userId);
        Task<bool> ReplicateUser(User userToReplicate);
        Task<IEnumerable<Post.Domain.Entities.Post>> GetAllPostsAsync();
        Task<List<User>> GetUsers();
        Task<IEnumerable<Post.Domain.Entities.Post>> GetPostsCreatedByUser(int userId);
        Task<bool> UpdateUserProfilePicture(int userId, string? profilePictureUrl);
    }
}