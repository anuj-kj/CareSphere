using CareSphere.Data.Core.DataContexts;
using System;
using CareSphere.Data.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(TContext context, IServiceProvider serviceProvider)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IRepository<T, TContext> Repository<T>() where T : class
    {
        if (!_repositories.ContainsKey(typeof(T)))
        {
            var repository = _serviceProvider.GetRequiredService<IRepository<T, TContext>>();
            _repositories.Add(typeof(T), repository);
        }
        return (IRepository<T, TContext>)_repositories[typeof(T)];
    }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

    public void Rollback()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Unchanged;
        }
    }

    public void Dispose() => _context.Dispose();
}


public class CareSphereUnitOfWork : UnitOfWork<CareSphereDbContext>, ICareSphereUnitOfWork
{
    public CareSphereUnitOfWork(CareSphereDbContext context, IServiceProvider serviceProvider)
        : base(context, serviceProvider) { }
}

public class OrderUnitOfWork : UnitOfWork<OrderDbContext>, IOrderUnitOfWork
{
    public OrderUnitOfWork(OrderDbContext context, IServiceProvider serviceProvider)
        : base(context, serviceProvider) { }
}

