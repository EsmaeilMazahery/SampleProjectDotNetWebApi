
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace ESkimo.DataLayer.Context
{
    public interface IUnitOfWork
    {
        int Save();
        Task<int> SaveAsync();
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        ChangeTracker getChangeTracker();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> AddEntity<TEntity>(TEntity entity)where TEntity : class;

        void initdb();
    }
}
