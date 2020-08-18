using System;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IMemberVisitRepository : IGenericRepository<MemberVisit>
    {

    }

    public class MemberVisitRepository : GenericRepository<MemberVisit>, IMemberVisitRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<MemberVisit>> _memberVisits;
        public MemberVisitRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _memberVisits = new Lazy<DbSet<MemberVisit>>(() => _uow.Set<MemberVisit>());
        }

     
    }
}
