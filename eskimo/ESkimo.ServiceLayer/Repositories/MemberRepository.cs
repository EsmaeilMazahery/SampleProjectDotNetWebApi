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
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<MemberListViewModel> GetAllAsync(MemberListViewModel model);
        Task<MemberServiceModel> GetAsync(int id);
        Task<MemberServiceModel> GetAsync(string membername);
        Task DeleteAsync(int id);
        Member Insert(MemberServiceModel model);
        void Update(MemberServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Member>> _members;
        public MemberRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _members = new Lazy<DbSet<Member>>(() => _uow.Set<Member>());
        }

        public Task DeleteAsync(int id)
        {
            return UpdateAsync(w => w.memberId == id, u => new Member() { Delete = true });
            // return DeleteAsync(d => d.memberId == id);
        }

        public async Task<MemberListViewModel> GetAllAsync(MemberListViewModel model)
        {
            var query = _members.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            if (!string.IsNullOrEmpty(model.family))
                query = query.Where(w => w.family.Contains(model.family));

            query = query.Where(w => !w.Delete);

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Member> records = await query.Select(s => new
            {
                s.email,
                s.enable,
                s.family,
                s.mobile,
                s.name,
                s.registerDate,
                s.memberId
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Member
            {
                memberId = s.memberId,
                registerDate = s.registerDate,
                name = s.name,
                mobile = s.mobile,
                family = s.family,
                enable = s.enable,
                email = s.email
            }).ToList());

            MemberListViewModel memberListViewModel = new MemberListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return memberListViewModel;
        }

        public Task<MemberServiceModel> GetAsync(int id)
        {
            return _members.Value.Where(w => w.memberId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new MemberServiceModel
                    {
                        memberId = s.memberId,
                        registerDate = s.registerDate,
                        name = s.name,
                        mobile = s.mobile,
                        family = s.family,
                        enable = s.enable,
                        email = s.email,
                        description = s.description,
                    }).FirstOrDefault()
                );
        }

        public Task<MemberServiceModel> GetAsync(string username)
        {
            return _members.Value.Where(w => w.email == username || w.mobile == username).ToListAsync()
                  .ContinueWith(
                      list => list.Result.Select(s => new MemberServiceModel
                      {
                          memberId = s.memberId,
                          registerDate = s.registerDate,
                          name = s.name,
                          mobile = s.mobile,
                          family = s.family,
                          enable = s.enable,
                          email = s.email,
                          password = s.password,
                          description = s.description,
                          verifyMobile = s.verifyMobile,
                          amount = s.amount,
                          sumFactors = s.sumFactors,
                          sumPayment = s.sumPayment
                      }).FirstOrDefault()
                  );
        }

        public async Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            var query = _members.Value.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            // select
            List<FilterViewModel> records = await query.Select(s => new
            {
                s.family,
                s.name,
                s.memberId
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new FilterViewModel
            {
                id = s.memberId,
                value = s.name + " " + s.family
            }).ToList());

            return records;
        }

        public Member Insert(MemberServiceModel model)
        {
            Member member = new Member()
            {
                email = model.email,
                enable = model.enable,
                mobile = model.mobile,
                name = model.name,
                family = model.family,
                password = IdentityCryptography.HashPassword(model.password),
                registerDate = DateTime.Now,
                description = model.description
            };

            if (!string.IsNullOrEmpty(model.address))
            {
                member.memberLocations = new List<MemberLocation>();

                var newLocation = new MemberLocation()
                {
                    name = member.name + " " + member.family,
                    member = member,
                    address = model.address,
                    areaId = model.areaId
                };
                if (model.location != null)
                {
                    newLocation.location = new Point(model.location.lat, model.location.lng) { SRID = 4326 };
                    newLocation.zoom = model.location.zoom;
                }

                member.memberLocations.Add(newLocation);
                _uow.Entry(newLocation).State = EntityState.Added;
            }

            ChangeState(member, EntityState.Added);

            return member;
        }

        public void Update(MemberServiceModel model)
        {
            Member member = _members.Value.FirstOrDefault(f => f.memberId == model.memberId);
            if (member == null)
                throw new NotFoundException();

            member.email = model.email;
            member.enable = model.enable;
            member.family = model.family;
            //member.mobile = model.mobile;
            member.name = model.name;
            member.description = model.description;

            if (!string.IsNullOrEmpty(model.password))
                member.password = IdentityCryptography.HashPassword(model.password);

            if (!string.IsNullOrEmpty(model.address))
            {
                var newLocation = new MemberLocation()
                {
                    name = member.name + " " + member.family,
                    member = member,
                    address = model.address,
                    areaId = model.areaId
                };

                if (model.location != null)
                {
                    newLocation.location = new Point(model.location.lat, model.location.lng) { SRID = 4326 };
                    newLocation.zoom = model.location.zoom;
                }

                member.memberLocations.Add(newLocation);
                _uow.Entry(newLocation).State = EntityState.Added;
            }

            ChangeState(member, EntityState.Modified);
        }
    }
}




