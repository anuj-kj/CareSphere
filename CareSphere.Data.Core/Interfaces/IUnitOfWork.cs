using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace CareSphere.Data.Core.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IRepository<T, TContext> Repository<T>() where T : class;
        Task<int> CommitAsync();
        void Rollback();
    }

    public interface ICareSphereUnitOfWork : IUnitOfWork<CareSphereDbContext> { }

    public interface IOrderUnitOfWork : IUnitOfWork<OrderDbContext> { }



}
