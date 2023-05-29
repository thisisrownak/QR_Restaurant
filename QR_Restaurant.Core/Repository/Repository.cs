using Microsoft.EntityFrameworkCore;
using QR_Restaurant.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.Core.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly QR_Context _context;
        public Repository(QR_Context context)
        {
            _context = context;
        }
        public void Delete(T entity)
        {
            var deleteEntity = _context.Remove(entity);
            deleteEntity.State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null ? _context.Set<T>().AsEnumerable() : _context.Set<T>().Where(filter).AsEnumerable();
        }

        public T Get(Expression<Func<T, bool>> filter = null)
        {
            return _context.Set<T>().SingleOrDefault(filter);
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public T GetById(long id)
        {
            return _context.Set<T>().Find(id);
        }
        public T GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Add(T entity)
        {
            var addEntity = _context.Entry(entity);
            addEntity.State = EntityState.Added;
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void BulkUpdate(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            _context.SaveChanges();
        }

        public void BulkDelete(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
            _context.SaveChanges();
        }

        public void BulkAdd(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

        public void DisposeTransaction()
        {
            _context.Database.CurrentTransaction.Dispose();
        }

    }
}
