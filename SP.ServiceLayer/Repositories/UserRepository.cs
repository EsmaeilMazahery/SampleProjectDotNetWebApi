
using System;
using System.Collections.Generic;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.DomainLayer.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IQueryable<User> Read(ListUserModel model);
        IQueryable<User> Read(string username);
        User Read(int userId);
        User Register(RegisterUserViewModel model);
        void Edit(EditUserViewModel model);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<User>> _admins;
        public UserRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _admins = new Lazy<DbSet<User>>(() => _uow.Set<User>());
        }

        public void Edit(EditUserViewModel model)
        {
            throw new NotImplementedException();
        }
        

        public IQueryable<User> Read(string username)
        {
            return _admins.Value.Where(w => w.mobile == username || w.email == username);
        }

        public User Read(int adminId)
        {
            return _admins.Value.Where(w => w.userId == adminId).FirstOrDefault();
        }

        public IQueryable<User> Read(ListUserModel model)
        {
            throw new NotImplementedException();
        }

        public User Register(RegisterUserViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
