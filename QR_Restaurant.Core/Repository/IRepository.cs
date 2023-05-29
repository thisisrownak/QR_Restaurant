using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Core.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter = null);
        T GetById(int id);
        T GetById(long id);
        T GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void BulkAdd(IEnumerable<T> entities);
        void BulkUpdate(IEnumerable<T> entities);

        void Delete(T entity);
        void BulkDelete(IEnumerable<T> entities);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void DisposeTransaction();

    }
}
