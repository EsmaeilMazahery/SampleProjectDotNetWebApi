
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace ESkimo.ServiceLayer.Services
{
    public interface IGenericRepository<T> where T : class
    {
        int SaveChanges();
        int SaveChangesWithoutValidation();
        IQueryable<T> AsQueryable();
        IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate);
        T First();
        T First(Expression<Func<T, bool>> predicate);
        T FirstOrDefault();
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        List<T> ToList();
        List<T> ToList(Expression<Func<T, bool>> predicate);
        int Delete(Expression<Func<T, bool>> predicate);
        int Update(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression);
        void ChangeState(T Entity, EntityState State);
        bool Any();
        bool Any(Expression<Func<T, bool>> predicate);
        int Count();
        int Count(Expression<Func<T, bool>> predicate);
        IQueryable<T> Including(params Expression<Func<T, object>>[] includeProperties);
        T Attach(T Entity);
        IOrderedQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IOrderedQueryable<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
        int? MaxNullableInt(Expression<Func<T, int?>> predicate);
        int? MinNullableInt(Expression<Func<T, int?>> predicate);
        void TryUpdateManyToMany<TRel, TKey>(IEnumerable<TRel> currentItems, IEnumerable<TRel> newItems, Func<TRel, TKey> getKey) where TRel : class;
    }

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        IUnitOfWork _uow;
        DbSet<T> _entity;
        public GenericRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _entity = _uow.Set<T>();
        }
        

        public int SaveChanges()
        {
            return _uow.Save();
        }

        public void TryUpdateManyToMany<TRel, TKey>(IEnumerable<TRel> currentItems, IEnumerable<TRel> newItems, Func<TRel, TKey> getKey) where TRel : class
        {
            
            _uow.Set<TRel>().RemoveRange(Except(currentItems, newItems, getKey));
            _uow.Set<TRel>().AddRange(Except(newItems, currentItems, getKey));
        }

        private IEnumerable<TRel> Except<TRel, TKey>(IEnumerable<TRel> items, IEnumerable<TRel> other, Func<TRel, TKey> getKeyFunc)
        {
            return items
                .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp })
                .Where(t => ReferenceEquals(null, t.temp) || t.temp.Equals(default(T)))
                .Select(t => t.t.item);
        }

        public int SaveChangesWithoutValidation()
        {
            try
            {
                _uow.getChangeTracker().AutoDetectChangesEnabled = false;
                return _uow.Save();
            }
            catch (Exception ex) { throw ex; }
            finally { _uow.getChangeTracker().AutoDetectChangesEnabled = true; }
        }

        public IQueryable<T> AsQueryable()
        {
            return _entity.AsQueryable<T>();
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, bool>> predicate)
        {
            return _entity.Where(predicate);
        }

        public T First()
        {
            return _entity.First();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return _entity.First(predicate);
        }

        public T FirstOrDefault()
        {
            return _entity.FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _entity.FirstOrDefault(predicate);
        }

        public List<T> ToList()
        {
            return _entity.ToList();
        }

        public List<T> ToList(Expression<Func<T, bool>> predicate)
        {
            return _entity.Where(predicate).ToList();
        }

        public Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            return _entity.Where(predicate).DeleteFromQueryAsync();
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            return _entity.Where(predicate).DeleteFromQuery();
        }

        public int Update(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression)
        {
            return _entity.Where(filterExpression).UpdateFromQuery(updateExpression);
        }

        public Task<int> UpdateAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression)
        {
            return _entity.Where(filterExpression).UpdateFromQueryAsync(updateExpression);
        }

        public void ChangeState(T Entity, EntityState State)
        {
            _uow.Entry(Entity).State = State;
        }

        public bool Any()
        {
            return _entity.Any();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _entity.Any(predicate);
        }

        public int Count()
        {
            return _entity.Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _entity.Count(predicate);
        }

        public IQueryable<T> Including(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entity.AsQueryable();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public T Attach(T Entity)
        {
            return _entity.Attach(Entity).Entity;
        }

        public IOrderedQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _entity.OrderBy(keySelector);
        }

        public IOrderedQueryable<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return _entity.OrderByDescending(keySelector);
        }

        public int? MaxNullableInt(Expression<Func<T, int?>> predicate)
        {
            return _entity.Max(predicate);
        }

        public int? MinNullableInt(Expression<Func<T, int?>> predicate)
        {
            return _entity.Min(predicate);
        }

    }
}
