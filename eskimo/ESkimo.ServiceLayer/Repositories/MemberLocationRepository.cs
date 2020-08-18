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
    public interface IMemberLocationRepository : IGenericRepository<MemberLocation>
    {
        Task<MemberLocationListViewModel> GetAllAsync(MemberLocationListViewModel model, int? memberId = null);
        Task<MemberLocationServiceModel> GetAsync(int id, int? memberId = null);
        Task DeleteAsync(int id, int? memberId = null);
        MemberLocation Insert(MemberLocationServiceModel model, int? memberId = null);
        void Update(MemberLocationServiceModel model, int? memberId = null);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name, int? memberId = null);
    }

    public class MemberLocationRepository : GenericRepository<MemberLocation>, IMemberLocationRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<MemberLocation>> _memberLocations;
        public MemberLocationRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _memberLocations = new Lazy<DbSet<MemberLocation>>(() => _uow.Set<MemberLocation>());
        }

        public async Task DeleteAsync(int id, int? memberId = null)
        {
            await DeleteAsync(d => d.memberLocationId == id && (memberId == null || memberId == d.memberId));
        }

        public async Task<MemberLocationListViewModel> GetAllAsync(MemberLocationListViewModel model, int? memberId = null)
        {
            var query = _memberLocations.Value.AsQueryable().Include(i=>i.area).Where(w => (memberId == null || memberId == w.memberId));

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            int allRows = 0;
            if (model.rowsPerPage>0)
            {
                // sort
                query = query.OrderByPropertyName(model.sort, model.sortDirection);

                // rows count
                allRows = query.Count();

                // paging
                query = query.Paging(model.rowsPerPage, model.page);
            }
            
            // select
            List<MemberLocation> records = await query.Select(s => new
            {
                s.address,
                s.memberLocationId,
                s.name,
                s.phone,
                s.postalCode,
                s.area
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new MemberLocation
            {
                name = s.name,
                memberLocationId = s.memberLocationId,
                address = s.address,
                area = s.area,
                postalCode=s.postalCode,
                phone=s.phone
            }).ToList());

            MemberLocationListViewModel userListViewModel = new MemberLocationListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<MemberLocationServiceModel> GetAsync(int id, int? memberId = null)
        {
            return _memberLocations.Value.Where(w => w.memberLocationId == id && (memberId == null || memberId == w.memberId)).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new MemberLocationServiceModel
                    {
                        memberLocationId = s.memberLocationId,
                        address = s.address,
                        name = s.name,
                        phone=s.phone,
                        postalCode=s.postalCode,
                        areaId = s.areaId,
                        location = new LocationViewModel()
                        {
                            lat = s.location.X,
                            lng = s.location.Y,
                            zoom = s.zoom
                        },
                    }).FirstOrDefault()
                );
        }

        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name, int? memberId = null)
        {
            throw new NotImplementedException();
        }

        public MemberLocation Insert(MemberLocationServiceModel model, int? memberId = null)
        {
            MemberLocation memberLocation = new MemberLocation()
            {
                address = model.address,
                name = model.name,
                phone = model.phone,
                postalCode = model.postalCode,
                location = new Point(model.location.lat, model.location.lng) { SRID = 4326 },
                zoom = model.location.zoom,
                areaId = model.areaId,
                memberId = model.memberId,
            };

            ChangeState(memberLocation, EntityState.Added);

            return memberLocation;
        }

        public void Update(MemberLocationServiceModel model, int? memberId = null)
        {
            MemberLocation memberLocation = _memberLocations.Value.FirstOrDefault(f => f.memberLocationId == model.memberLocationId && (memberId == null || memberId == f.memberId));
            if (memberLocation == null)
                throw new NotFoundException();

            memberLocation.address = model.address;
            memberLocation.name = model.name;
            memberLocation.phone = model.phone;
            memberLocation.postalCode = model.postalCode;
            memberLocation.location = new Point(model.location.lat, model.location.lng) { SRID = 4326 };
            memberLocation.zoom = model.location.zoom;
            memberLocation.areaId = model.areaId;

            ChangeState(memberLocation, EntityState.Modified);
        }
    }

}




