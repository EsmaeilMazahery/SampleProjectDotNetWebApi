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
    public interface IMemberAskRepository : IGenericRepository<MemberAsk>
    {
        Task<MemberAskListViewModel> GetAllAsync(MemberAskListViewModel model);
        Task<MemberAskServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        MemberAsk Insert(MemberAskServiceModel model);
    }

    public class MemberAskRepository : GenericRepository<MemberAsk>, IMemberAskRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<MemberAsk>> _memberAsks;
        public MemberAskRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _memberAsks = new Lazy<DbSet<MemberAsk>>(() => _uow.Set<MemberAsk>());
        }

        public Task DeleteAsync(int id)
        {
            return DeleteAsync(d => d.memberAskId == id);
        }

        public async Task<MemberAskListViewModel> GetAllAsync(MemberAskListViewModel model)
        {
            var query = _memberAsks.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            if (!string.IsNullOrEmpty(model.family))
                query = query.Where(w => w.family.Contains(model.family));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<MemberAsk> records = await query.Select(s => new
            {
                s.memberAskId,
                s.email,
                s.type,
                s.registerDate,
                s.name,
                s.family,
                s.mobile,
                s.read
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new MemberAsk
            {
                memberAskId = s.memberAskId,
                email = s.email,
                family = s.family,
                name = s.name,
                registerDate = s.registerDate,
                type = s.type,
                mobile = s.mobile,
                read=s.read
            }).ToList());

            MemberAskListViewModel memberAskListViewModel = new MemberAskListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return memberAskListViewModel;
        }

        public Task<MemberAskServiceModel> GetAsync(int id)
        {
            return _memberAsks.Value.Where(w => w.memberAskId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new MemberAskServiceModel
                    {
                        memberAskId = s.memberAskId,
                        mobile = s.mobile,
                        type = s.type,
                        registerDate = s.registerDate,
                        name = s.name,
                        family = s.family,
                        email = s.email,
                        description = s.description,
                        read = s.read
                    }).FirstOrDefault()
                );
        }

        public MemberAsk Insert(MemberAskServiceModel model)
        {
            MemberAsk member = new MemberAsk()
            {
                memberAskId = model.memberAskId,
                mobile = model.mobile,
                type = model.type,
                registerDate = model.registerDate,
                name = model.name,
                family = model.family,
                email = model.email,
                description = model.description,
                read = model.read
            };

            ChangeState(member, EntityState.Added);

            return member;
        }
    }
}




