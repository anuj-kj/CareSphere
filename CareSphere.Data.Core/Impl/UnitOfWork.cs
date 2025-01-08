using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareSphere.Data.Core.DataContexts;
using CareSphere.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CareSphere.Data.Core.Impl
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(TContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repository = _serviceProvider.GetService<IRepository<T>>() ?? new Repository<T>(_context);
                _repositories.Add(type, repository);
            }
            return (IRepository<T>)_repositories[type];
        }
        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            var type = typeof(TRepository);
            if (!_repositories.ContainsKey(type))
            {
                var repository = _serviceProvider.GetRequiredService<TRepository>();
                _repositories.Add(type, repository);
            }
            return (TRepository)_repositories[type];
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Rollback()
        {
            _context.ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public void Dispose() => _context.Dispose();
    }



}
