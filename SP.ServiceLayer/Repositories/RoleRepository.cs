using System;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
    }

    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Role>> _roles;
        public RoleRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _roles = new Lazy<DbSet<Role>>(() => _uow.Set<Role>());
        }
    }
}
