
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Mappings;
using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace ESkimo.DataLayer.Context
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> Builder) : base(Builder)
        {
            
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaPrice> AreaPrices { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BlogCategory> blogCategories { get; set; }
        public DbSet<BlogComment> blogComments { get; set; }
        public DbSet<BlogPost> blogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<DiscountFactor> DiscountFactors { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<FactorItem> FactorItems { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberAsk> MemberAsks { get; set; }
        public DbSet<MemberLocation> MemberLocations { get; set; }
        public DbSet<MemberOrderPeriod> MemberOrderPeriods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PeriodType> PeriodTypes { get; set; }
        public DbSet<PocketPost> PocketPosts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<ProductPriceWholesale> ProductPriceWholesales { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Rel_CategoryBrand> Rel_CategoryBrand { get; set; }
        public DbSet<Rel_CategoryProductType> Rel_CategoryProductType { get; set; }
        public DbSet<Rel_DiscountCodeBrand> Rel_DiscountCodeBrand { get; set; }
        public DbSet<Rel_DiscountCodeCategory> Rel_DiscountCodeCategory { get; set; }
        public DbSet<Rel_RoleUser> Rel_RoleUser { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSessionUpdate> UserSessionUpdates { get; set; }
        public DbSet<UserSmsMessage> UserSmsMessages { get; set; }
        public DbSet<SmsLog> SmsLogs { get; set; }
        public DbSet<Log> Logs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.HasDefaultSchema("eskimo_USER");

            modelBuilder.ApplyConfiguration(new AreaMapping());
            modelBuilder.ApplyConfiguration(new AreaPriceMapping());
            modelBuilder.ApplyConfiguration(new BrandMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new CommentMapping());
            modelBuilder.ApplyConfiguration(new DiscountCodeMapping());
            modelBuilder.ApplyConfiguration(new DiscountFactorMapping());
            modelBuilder.ApplyConfiguration(new FactorMapping());
            modelBuilder.ApplyConfiguration(new FactorItemMapping());
            modelBuilder.ApplyConfiguration(new MemberMapping());
            modelBuilder.ApplyConfiguration(new MemberLocationMapping());
            modelBuilder.ApplyConfiguration(new MemberOrderPeriodMapping());
            modelBuilder.ApplyConfiguration(new PaymentMapping());
            modelBuilder.ApplyConfiguration(new PeriodTypeMapping());
            modelBuilder.ApplyConfiguration(new PocketPostMapping());
            modelBuilder.ApplyConfiguration(new ProductMapping());
            modelBuilder.ApplyConfiguration(new ProductPriceMapping());
            modelBuilder.ApplyConfiguration(new ProductTypeMapping());
            modelBuilder.ApplyConfiguration(new Rel_CategoryBrandMapping());
            modelBuilder.ApplyConfiguration(new Rel_CategoryProductTypeMapping());
            modelBuilder.ApplyConfiguration(new Rel_DiscountCodeBrandMapping());
            modelBuilder.ApplyConfiguration(new Rel_DiscountCodeCategoryMapping());
            modelBuilder.ApplyConfiguration(new Rel_RoleUserMapping());
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());

            modelBuilder.ApplyConfiguration(new BlogCategoryMapping());
            modelBuilder.ApplyConfiguration(new BlogCommentMapping());
            modelBuilder.ApplyConfiguration(new BlogPostMapping());
        }

        public void initdb()
        {
            this.Save();
            var _roles = new Lazy<DbSet<Role>>(() => this.Set<Role>());
            var _users = new Lazy<DbSet<User>>(() => this.Set<User>());
            var _brands = new Lazy<DbSet<Brand>>(() => this.Set<Brand>());
            var _categories = new Lazy<DbSet<Category>>(() => this.Set<Category>());

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
                    var role = Roles.AsQueryable().Where(w => w.roleKey == r).FirstOrDefault();
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

            if (!_users.Value.Any(w => w.username == "admin"))
            {
                User user = new User()
                {
                    username = "admin",
                    name = "اسماعیل",
                    family = "مظاهری",
                    password = IdentityCryptography.HashPassword("unknown"),
                    mobile = "9179338319",
                    email = "info@mazahery.me",
                    enable = true
                };
                user.roles = new List<Rel_RoleUser>();

                Enum.GetValues(typeof(RolesKey)).Cast<RolesKey>().ToList().ForEach(r =>
                {
                    Rel_RoleUser rel_RoleUser = new Rel_RoleUser()
                    {
                        roleId = r,
                        user = user
                    };

                    this.Entry(rel_RoleUser).State = EntityState.Added;
                    user.roles.Add(rel_RoleUser);
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

        public Task<int> SaveAsync()
        {
            try
            {
                return SaveChangesAsync();
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
