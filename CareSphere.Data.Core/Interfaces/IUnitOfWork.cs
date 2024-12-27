using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSphere.Data.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        TRepository GetRepository<TRepository>() where TRepository : class;
 
         Task<int> CommitAsync();
        void Rollback();
    }

}
