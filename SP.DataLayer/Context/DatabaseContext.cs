
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using SP.DataLayer.Mappings;
using SP.DataLayer.Mappings.ViewMapping;
using SP.DomainLayer.DbViewModels;
using SP.DomainLayer.Models;
using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace SP.DataLayer.Context
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> Builder) : base(Builder)
        {

        }

        protected  override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Rel_RolesUsers> Rel_RolesAdmins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Notify> Notifies { get; set; }
        public DbSet<LogUser> LogUsers { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SmsLog> SmsLogs { get; set; }

        public DbQuery<SearchAllResult> SearchAllResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Query<SearchAllResult>().ToView("View_BlogPostCounts").Property(v => v.BlogName).HasColumnName("Name");

            //model View
            modelBuilder.ApplyConfiguration(new SearchAllResultConfiguration());

            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new MemberMapping());
          
            modelBuilder.ApplyConfiguration(new ServiceMapping());
            modelBuilder.ApplyConfiguration(new Rel_RolesUsersMapping());
        }

        public void initdb()
        {
            var _roles = new Lazy<DbSet<Role>>(() => this.Set<Role>());
            var _users = new Lazy<DbSet<User>>(() => this.Set<User>());

            Enum.GetValues(typeof(RolesKey)).Cast<RolesKey>().ToList().ForEach(r =>
            {
                if (!_roles.Value.Any(a => a.roleKey == r))
                {
                    var role = new Role()
                    {
                        roleKey = r,
                        name = r.GetName(),
                        description = r.GetDescriptionOrDefault(),
                    };

                    this.Entry(role).State = EntityState.Added;
                }
                else
                {
                    var role = _roles.Value.AsQueryable().Where(w => w.roleKey == r).FirstOrDefault();
                    role.name = r.GetName();
                    role.description = r.GetDescriptionOrDefault();

                    this.Entry(role).State = EntityState.Modified;
                }
            });

            var deletedRoles = _roles.Value.AsQueryable().Where(w => !Enum.GetValues(typeof(RolesKey)).Cast<RolesKey>().ToList().Any(a => a == w.roleKey)).ToList();

            deletedRoles.ForEach(d =>
            {
                this.Entry(d).State = EntityState.Deleted;
            });

            if (!_users.Value.Any(w => w.mobile == "9179338319"))
            {
                User user = new User()
                {
                    name = "اسماعیل",
                    family = "مظاهری",
                    password = IdentityCryptography.HashPassword("unknown"),
                    mobile = "9179338319",
                    email = "info@mazahery.me",
                    enable = true
                };
                user.rolesUsers = new List<Rel_RolesUsers>();

                Enum.GetValues(typeof(RolesKey)).Cast<RolesKey>().ToList().ForEach(r =>
                {
                    Rel_RolesUsers rel_RoleUser = new Rel_RolesUsers()
                    {
                        roleId = r,
                        user = user
                    };

                    this.Entry(rel_RoleUser).State = EntityState.Added;
                    user.rolesUsers.Add(rel_RoleUser);
                });

                this.Entry(user).State = EntityState.Added;
            }

            this.Save();
        }

        public int Save()
        {
            try
            {
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IDbContextTransaction transaction = null;

        public IDbContextTransaction BeginTransaction()
        {
            if (null == transaction)
            {
                this.transaction = Database.BeginTransaction();
            }
            return transaction;
        }

        public void CommitTransaction()
        {
            if (null == transaction)
                throw new Exception("Transaction Not Began");
            transaction.Commit();
            transaction = null;
        }

        public void RollbackTransaction()
        {
            if (null == transaction)
                throw new Exception("Transaction Not Began");
            transaction.Rollback();
            transaction = null;
        }

        public ChangeTracker getChangeTracker()
        {
            return ChangeTracker;
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {

            return base.Set<TEntity>();
        }

        public EntityEntry<TEntity> AddEntity<TEntity>(TEntity entity) where TEntity : class
        {
            return Add(entity);
        }
    }
}
