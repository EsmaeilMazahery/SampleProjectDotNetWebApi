using System;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IPropertiseRepository : IGenericRepository<Propertise>
    {
    }

    public class PropertiseRepository : GenericRepository<Propertise>, IPropertiseRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Propertise>> _propertises;
        public PropertiseRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _propertises = new Lazy<DbSet<Propertise>>(() => _uow.Set<Propertise>());
        }

    }
}
