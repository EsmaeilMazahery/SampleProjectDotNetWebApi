using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESkimo.ServiceLayer.Services
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserListViewModel> GetAllAsync(UserListViewModel model);
        Task<UserServiceModel> GetAsync(int id);
        Task<UserServiceModel> GetAsync(string username);
        Task DeleteAsync(int id);
        User Insert(UserServiceModel model);
        void Update(UserServiceModel model);

        Task<FilterPaggingViewModel> GetFilterAsync(string name, int[] regions, int? page = null, int? rowsPerPage = null);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<User>> _users;
        public UserRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _users = new Lazy<DbSet<User>>(() => _uow.Set<User>());
        }

        public Task DeleteAsync(int id)
        {
            return DeleteAsync(d => d.userId == id);
        }

        public async Task<UserListViewModel> GetAllAsync(UserListViewModel model)
        {
            var query = _users.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            if (!string.IsNullOrEmpty(model.family))
                query = query.Where(w => w.family.Contains(model.family));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<User> records = await query.Select(s => new
            {
                s.username,
                s.email,
                s.enable,
                s.family,
                s.image,
                s.mobile,
                s.name,
                s.registerDate,
                s.userId
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new User
            {
                userId = s.userId,
                username = s.username,
                registerDate = s.registerDate,
                name = s.name,
                mobile = s.mobile,
                image = s.image,
                family = s.family,
                enable = s.enable,
                email = s.email
            }).ToList());

            UserListViewModel userListViewModel = new UserListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<UserServiceModel> GetAsync(int id)
        {
            return _users.Value.Where(w => w.userId == id).Include(i => i.roles).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new UserServiceModel
                    {
                        userId = s.userId,
                        username = s.username,
                        registerDate = s.registerDate,
                        name = s.name,
                        mobile = s.mobile,
                        image = s.image,
                        family = s.family,
                        enable = s.enable,
                        email = s.email,
                        selectedRoles = s.roles.Select(r => r.roleId).ToList()
                    }).FirstOrDefault()
                );
        }

        public Task<UserServiceModel> GetAsync(string username)
        {
            return _users.Value.Where(w => w.username == username).Include(i => i.roles).ToListAsync()
                  .ContinueWith(
                      list => list.Result.Select(s => new UserServiceModel
                      {
                          userId = s.userId,
                          username = s.username,
                          registerDate = s.registerDate,
                          name = s.name,
                          mobile = s.mobile,
                          image = s.image,
                          family = s.family,
                          enable = s.enable,
                          email = s.email,
                          password=s.password,
                          selectedRoles = s.roles.Select(r => r.roleId).ToList()
                      }).FirstOrDefault()
                  );
        }

        public async Task<FilterPaggingViewModel> GetFilterAsync(string name, int[] regions, int? page = null, int? rowsPerPage = null)
        {
            var query = _users.Value.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            // rows count
            int allRows = query.Count();

            // paging
            if (rowsPerPage.HasValue && page.HasValue)
                query = query.Paging(rowsPerPage.Value, page.Value);

            var list = await query.Select(s => new
            {
                id = s.userId,
                value = s.name+" "+s.family,
            }).ToListAsync().ContinueWith(l => l.Result.Select(s => new FilterViewModel()
            {
                id = s.id,
                value = s.value
            }).ToList());

            return new FilterPaggingViewModel()
            {
                allRows = allRows,
                list = list
            };
        }

        public User Insert(UserServiceModel model)
        {

            if (model.selectedRoles == null)
                model.selectedRoles = new List<Infrastructure.Enumerations.RolesKey>();

            User user = new User()
            {
                username = model.username,
                email = model.email,
                enable = model.enable,
                mobile = model.mobile,
                image = model.image,
                name = model.name,
                family = model.family,
                password = IdentityCryptography.HashPassword(model.password),
                registerDate = DateTime.Now,
            };

            user.roles = new List<Rel_RoleUser>();

            model.selectedRoles.ForEach(roleId =>
            {
                user.roles.Add(new Rel_RoleUser()
                {
                    user = user,
                    roleId = roleId
                });
            });

            ChangeState(user, EntityState.Added);

            return user;
        }

        public void Update(UserServiceModel model)
        {
            User user = _users.Value.Include(i => i.roles).FirstOrDefault(f => f.userId == model.userId);
            if (user == null)
                throw new NotFoundException();

            user.username = model.username;
            user.email = model.email;
            user.enable = model.enable;
            user.family = model.family;
            if (!string.IsNullOrEmpty(model.image))
                user.image = model.image;
            user.mobile = model.mobile;
            user.name = model.name;
            if (!string.IsNullOrEmpty(model.password))
                user.password = IdentityCryptography.HashPassword(model.password);

            user.roles.Clear();
            user.roles = new List<Rel_RoleUser>();

            model.selectedRoles.ForEach(roleId =>
            {
                user.roles.Add(new Rel_RoleUser()
                {
                    user = user,
                    roleId = roleId
                });
            });

            ChangeState(user, EntityState.Modified);
        }
    }

}




