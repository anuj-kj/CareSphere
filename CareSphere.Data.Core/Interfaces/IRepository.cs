using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CareSphere.Data.Core.Interfaces
{
    public interface IRepository<T, TContext> where T : class where TContext : DbContext
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByGuidIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetByPropertyAsync(Expression<Func<T, bool>> predicate);
    }



}
