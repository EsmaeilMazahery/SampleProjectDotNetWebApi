using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace ESkimo.ServiceLayer.Services
{
    public interface IAreaRepository : IGenericRepository<Area>
    {
        Task<AreaListViewModel> GetAllAsync(AreaListViewModel model);
        Task<AreaListViewModel> GetAllMemberAsync();
        Task<AreaServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Area Insert(AreaServiceModel model);
        void Update(AreaServiceModel model);

        Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null);
    }

    public class AreaRepository : GenericRepository<Area>, IAreaRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Area>> _areas;
        public AreaRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _areas = new Lazy<DbSet<Area>>(() => _uow.Set<Area>());
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.areaId == id);
        }

        public async Task<AreaListViewModel> GetAllAsync(AreaListViewModel model)
        {
            var query = _areas.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Area> records = await query.Select(s => new
            {
                s.address,
                s.name,
                s.areaId,
                //  s.location,
                s.zoom,
                s.sendDaies,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Area
            {
                areaId = s.areaId,
                name = s.name,
                address = s.address,
                // location = s.location,
                zoom = s.zoom,
                sendDaies = s.sendDaies,
            }).ToList());

            AreaListViewModel userListViewModel = new AreaListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public async Task<AreaListViewModel> GetAllMemberAsync()
        {
            var query = _areas.Value.AsQueryable();

            // select
            List<Area> records = await query.Select(s => new
            {
                s.address,
                s.name,
                s.areaId,
                s.zoom,
                s.sendDaies,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Area
            {
                areaId = s.areaId,
                name = s.name,
                address = s.address,
                zoom = s.zoom,
                sendDaies = s.sendDaies,
            }).ToList());

            AreaListViewModel userListViewModel = new AreaListViewModel()
            {
                list = records
            };

            return userListViewModel;
        }

        public Task<AreaServiceModel> GetAsync(int id)
        {
            return _areas.Value.Where(w => w.areaId == id).Include(i => i.prices).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new AreaServiceModel
                    {
                        areaId = s.areaId,
                        address = s.address,
                        name = s.name,
                        sendDaies = s.sendDaies,
                        prices = s.prices,
                        location = s.location != null ? new LocationViewModel()
                        {
                            lat = s.location.X,
                            lng = s.location.Y,
                            zoom = s.zoom,
                        } : null
                    }).FirstOrDefault()
                );
        }

        public async Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var query = _areas.Value.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            // rows count
            int allRows = query.Count();

            // paging
            if (rowsPerPage.HasValue && page.HasValue)
                query = query.Paging(rowsPerPage.Value, page.Value);

            var list = await query.Select(s => new
            {
                id = s.areaId,
                value = s.name,
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

        public Area Insert(AreaServiceModel model)
        {
            Area area = new Area()
            {
                address = model.address,
                name = model.name,
                sendDaies = model.sendDaies,
            };

            if (model.location != null)
            {
                area.location = new Point(model.location.lat, model.location.lng) { SRID = 4326 };
                area.zoom = model.location.zoom;
            }

            ChangeState(area, EntityState.Added);

            return area;
        }

        public void Update(AreaServiceModel model)
        {
            Area area = _areas.Value.FirstOrDefault(f => f.areaId == model.areaId);
            if (area == null)
                throw new NotFoundException();

            area.name = model.name;
            area.address = model.address;

            area.sendDaies = model.sendDaies;

            if (model.location != null)
            {
                area.location = new Point(model.location.lat, model.location.lng) { SRID = 4326 };
                area.zoom = model.location.zoom;
            }
            else
            {
                area.location = null;
                area.zoom = 0;
            }

            ChangeState(area, EntityState.Modified);
        }
    }

}




