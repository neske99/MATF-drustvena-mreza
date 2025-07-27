using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Post.Domain.Common;

namespace Post.Application.Contracts
{
    public interface IAsyncRepository<T> where T : Aggregate
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        Task<T> AddAsync(T item);

        Task UpdateAsync(T item);

        Task DeleteAsync(T item);

    }
}