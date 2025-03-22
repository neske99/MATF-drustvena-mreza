using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.Application.Contracts;
using Post.Domain.Common;
using Post.Infrastructure.Persistance;
using Post.Infrastructure.Persistance.EntityConfigurations;

namespace Post.Infrastructure.Repositories
{
    public class RepositoryBase<T>: IAsyncRepository<T> where T:Aggregate
    {
        protected PostContext postContext;
        public RepositoryBase(PostContext postContext)
        {
            this.postContext = postContext ?? throw new ArgumentNullException(nameof(postContext));
        }

        public async Task<T> AddAsync(T item)
        {
            postContext.Set<T>().Add(item);
            await postContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(T item)
        {
            postContext.Set<T>().Remove(item);
            await postContext.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await postContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await postContext.Set<T>().FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(T item)
        {
            postContext.Entry(item).State =EntityState.Modified;
            await postContext.SaveChangesAsync();
        }
    }
}