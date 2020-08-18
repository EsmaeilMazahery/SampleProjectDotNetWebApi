using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ESkimo.ServiceLayer.Services
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
