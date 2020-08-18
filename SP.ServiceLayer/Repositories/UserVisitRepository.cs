using System;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IUserVisitRepository : IGenericRepository<UserVisit>
    {

    }

    public class UserVisitRepository : GenericRepository<UserVisit>, IUserVisitRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<UserVisit>> _userVisits;
        public UserVisitRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _userVisits = new Lazy<DbSet<UserVisit>>(() => _uow.Set<UserVisit>());
        }
        
    }
}
