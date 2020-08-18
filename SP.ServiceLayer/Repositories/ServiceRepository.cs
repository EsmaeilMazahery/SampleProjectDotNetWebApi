using System;
using System.Collections.Generic;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.DomainLayer.ViewModel;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        /// <summary>
        /// گرفتن لیست سرویس ها
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IQueryable<Service> Read(int userId, int? count);

        EditServiceViewModel Read(int serviceId);

        Service Register(RegisterServiceViewModel model);

        void RegComplate(int serviceId);

        void Edit(EditServiceViewModel model);
        bool checkServiceNameNotTaken(int userId, string name, int? serviceId);
    }

    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Service>> _services;
        public ServiceRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _services = new Lazy<DbSet<Service>>(() => _uow.Set<Service>());
        }

        public void Edit(EditServiceViewModel model)
        {
            Service service = _services.Value.AsQueryable()
                .Where(w => w.serviceId == model.serviceId)
                .FirstOrDefault();

            service.unitOfWork = model.unitOfWork;
            service.logo = model.logo;
            service.description = model.description;
            service.name = model.name;
            service.nameWorkPlace = model.nameWorkPlace;
            service.majorText = model.majorText;
            service.majorImg = model.majorImg;
            service.website = model.website;

            //if (model.cataloges != null && model.cataloges.Count > 0)
            //{
            //    var addedCataloges = model.cataloges.Where(w => w.mediaId == 0).Select(s => { s.serviceCatalogeId = model.serviceId; return s; }).ToList();
            //    var deletedCataloges = service.cataloges.Where(w => !model.cataloges.Any(a => a.mediaId == w.mediaId)).ToList();
            //    deletedCataloges.ForEach(x => _uow.Entry(x).State = EntityState.Deleted);
            //    addedCataloges.ForEach(x => _uow.Entry(x).State = EntityState.Added);
            //}

            //if (model.portfolios != null && model.portfolios.Count > 0)
            //{
            //    var addedPortfolios = model.portfolios.Where(w => w.mediaId == 0).Select(s => { s.servicePortfolioId = model.serviceId; return s; }).ToList();
            //    var deletedPortfolios = service.portfolios.Where(w => !model.portfolios.Any(a => a.mediaId == w.mediaId)).ToList();
            //    deletedPortfolios.ForEach(x => _uow.Entry(x).State = EntityState.Deleted);
            //    addedPortfolios.ForEach(x => _uow.Entry(x).State = EntityState.Added);
            //}

            //ChangeState(service, EntityState.Modified);
            //if (model.locations != null && model.locations.Count > 0)
            //    TryUpdateManyToMany(service.locationsServices, model.locations
            //        .Select(x => new Rel_LocationsServices
            //        {
            //            locationId = x,
            //            serviceId = model.serviceId
            //        }), x => x.locationId);
        }

        public IQueryable<Service> Read(int userId, int? count)
        {
            var query = _services.Value.Where(w => w.memberId == userId);
            if (count.HasValue)
                return query.Take(count.Value);
            else
                return query;
        }

        public EditServiceViewModel Read(int serviceId)
        {
            return _services.Value.AsQueryable().Where(w => w.serviceId == serviceId).Select(s => new EditServiceViewModel()
            {
                description = s.description,
                enable = s.enable,
                name = s.name,
                serviceId = s.serviceId,
                unitOfWork = s.unitOfWork,
                logo = s.logo,
                nameWorkPlace = s.nameWorkPlace,
                majorImg = s.majorImg,
                majorText = s.majorText,
                website = s.website,
            }).FirstOrDefault();
        }

        public Service Register(RegisterServiceViewModel model)
        {
            Service service = new Service()
            {
                name = model.name,
                memberId = model.memberId,
                enable = true,
            };
            var eservice = _uow.AddEntity(service);

            return eservice.Entity;
        }

        public bool checkServiceNameNotTaken(int userId, string name, int? serviceId)
        {
            return _services.Value.Any(w => w.name == name && w.memberId == userId && (serviceId == null || w.serviceId != serviceId));
        }
    }
}
