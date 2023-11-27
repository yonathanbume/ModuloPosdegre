using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Base
{
    public interface IRepository<T> where T : class
    {
        Task<bool> Any(Guid id);
        Task<bool> Any(string id);
        Task<int> Count();
        Task<T> First();
        Task<T> Last();
        Task<T> Get(Guid id);
        Task<T> Get(string id);
        Task<T> Get(params object[] keyValues);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task AddRange(IEnumerable<T> entity);
        Task Delete(T entity);
        Task DeleteById(Guid id);
        Task DeleteById(string id);
        Task DeleteById(params object[] keyValues);
        Task DeleteRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);
        Task Update(T entity);
        Task UpdateRange(IEnumerable<T> entities);
    }
}