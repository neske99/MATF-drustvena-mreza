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
    public class PostRepository:RepositoryBase<Post.Domain.Entities.Post>,IPostRepository
    {
        public PostRepository(PostContext postContext): base(postContext)
        {
            if (postContext is null)
            {
                throw new ArgumentNullException(nameof(postContext));
            }
        }

        public async Task<bool> AddCommentToPost(int postId, Comment comment)
        {
            var post =  await postContext.Posts.Where(p=>p.Id == postId).FirstOrDefaultAsync();

            if(post ==null)
                return false;
            
            post.Comments.Add(comment);
            var result = await postContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CreatePostForUser(int userId, Domain.Entities.Post post)
        {
            post.UserId =userId;
            await postContext.Posts.AddAsync(post);
            var result= await postContext.SaveChangesAsync();
            return result >0;
        }

        public async Task<IEnumerable<Domain.Entities.Post>> GetPostsForUser(int userId,List<int>friendIdList)
        {
            return await postContext.Posts.Include(p=>p.Comments).Where(p=> p.CreatedDate.AddMonths(3) > DateTime.Now && friendIdList.Contains(p.UserId)).ToListAsync();
        }

        public async Task<bool> ReplicateUser(User userToReplicate)
        {
            string toExecute = "SET IDENTITY_INSERT [dbo].[Users] ON " +
                $"INSERT INTO [dbo].[Users] (Id,Username,CreatedDate) VALUES ({userToReplicate.Id},'{userToReplicate.Username}','{userToReplicate.CreatedDate}')" +
                " SET IDENTITY_INSERT [dbo].[Users] OFF";
            var result= await postContext.Database.ExecuteSqlRawAsync(toExecute);

            //postContext.Users.Add(userToReplicate);
            //var result = await postContext.SaveChangesAsync()>0;

            return result>0;
        }
    }
}