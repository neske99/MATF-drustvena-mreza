using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.Application.Contracts;
using Post.Domain.Entities;
using Post.Infrastructure.Persistance;

namespace Post.Infrastructure.Repositories
{
    public class PostRepository : RepositoryBase<Post.Domain.Entities.Post>, IPostRepository
    {
        public PostRepository(PostContext postContext) : base(postContext)
        {
            if (postContext is null)
            {
                throw new ArgumentNullException(nameof(postContext));
            }
        }

        public async Task<bool> AddCommentToPost(int postId, Comment comment)
        {
            var post = await postContext.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return false;

            post.Comments.Add(comment);
            var result = await postContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> AddLikeToPost(int postId, Like like)
        {
            var post = await postContext.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return false;

            // Check if user already liked this post
            var existingLike = await postContext.Likes
                .Where(l => l.PostId == postId && l.UserId == like.UserId)
                .FirstOrDefaultAsync();

            if (existingLike != null)
                return false; // User already liked this post

            post.Likes.Add(like);
            var result = await postContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> RemoveLikeFromPost(int postId, int userId)
        {
            var like = await postContext.Likes
                .Where(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefaultAsync();

            if (like == null)
                return false; // Like doesn't exist

            postContext.Likes.Remove(like);
            var result = await postContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreatePostForUser(int userId, Domain.Entities.Post post)
        {
            post.UserId = userId;
            await postContext.Posts.AddAsync(post);
            var result = await postContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Domain.Entities.Post>> GetAllPostsAsync()
        {
            return await postContext.Posts.
            Include(p => p.User).
            Include(p => p.Comments).
            ThenInclude(c => c.User).
            Include(p => p.Likes).
            ThenInclude(l => l.User).
            ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Post>> GetPostsForUser(int userId, List<int> friendIdList)
        {
           
            var usersToInclude = new List<int>(friendIdList) { userId };
            
            var result = await postContext.Posts.
                Include(p => p.User).
                Include(p => p.Comments).
                ThenInclude(c => c.User).
                Include(p => p.Likes).
                ThenInclude(l => l.User).
                Where(p => p.CreatedDate.AddMonths(3) > DateTime.Now && usersToInclude.Contains(p.UserId)).
                OrderByDescending(p => p.CreatedDate).
                ToListAsync();

            return result;
        }

        public async Task<bool> ReplicateUser(User userToReplicate)
        {
            string toExecute = "SET IDENTITY_INSERT [dbo].[Users] ON " +
                $"INSERT INTO [dbo].[Users] (Id,Username,CreatedDate) VALUES ({userToReplicate.Id},'{userToReplicate.Username}','{userToReplicate.CreatedDate}')" +
                " SET IDENTITY_INSERT [dbo].[Users] OFF";
            var result = await postContext.Database.ExecuteSqlRawAsync(toExecute);

            return result > 0;
        }

        public async Task<List<User>> GetUsers()
        {
            return await postContext.Users.ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Post>> GetPostsCreatedByUser(int userId)
        {       
            return await postContext.Posts.
                Include(p => p.User).
                Include(p => p.Comments).
                ThenInclude(c => c.User).
                Include(p => p.Likes).
                ThenInclude(l => l.User).
                Where(p => p.CreatedDate.AddMonths(3) > DateTime.Now && p.UserId==userId).
                OrderByDescending(p => p.CreatedDate).
                ToListAsync();
        }

        public async Task<bool> DeletePost(int postId, int userId)
        {
            var post = await postContext.Posts
                .Where(p => p.Id == postId && p.UserId == userId)
                .FirstOrDefaultAsync();

            if (post == null)
                return false; // Post doesn't exist or user doesn't own it

            postContext.Posts.Remove(post);
            var result = await postContext.SaveChangesAsync();
            return result > 0;
        }
    }
}