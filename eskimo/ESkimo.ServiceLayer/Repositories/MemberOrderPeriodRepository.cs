using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESkimo.ServiceLayer.Services
{
    public interface IMemberOrderPeriodRepository : IGenericRepository<MemberOrderPeriod>
    {
        Task<MemberOrderPeriodListViewModel> GetAllAsync(MemberOrderPeriodListViewModel model, int? memberId = null);
        Task<MemberOrderPeriodServiceModel> GetAsync(int id, int? memberId = null);
        Task DeleteAsync(int id);
        MemberOrderPeriod Insert(MemberOrderPeriodServiceModel model);
        void Update(MemberOrderPeriodServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class MemberOrderPeriodRepository : GenericRepository<MemberOrderPeriod>, IMemberOrderPeriodRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<MemberOrderPeriod>> _memberOrderPeriods;
        public MemberOrderPeriodRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _memberOrderPeriods = new Lazy<DbSet<MemberOrderPeriod>>(() => _uow.Set<MemberOrderPeriod>());
        }


        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.memberOrderPeriodId == id);
        }

        public async Task<MemberOrderPeriodListViewModel> GetAllAsync(MemberOrderPeriodListViewModel model, int? memberId = null)
        {
            var query = _memberOrderPeriods.Value.AsQueryable().Where(w => w.memberId == memberId || memberId == null);

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<MemberOrderPeriod> records = await query.Select(s => new
            {
                s.memberOrderPeriodId,
                s.payType,
                s.periodType,
                s.periodTypeId,
                targetFactor = s.factors.FirstOrDefault()
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new MemberOrderPeriod
            {
                memberOrderPeriodId = s.memberOrderPeriodId,
                payType = s.payType,
                periodType = s.periodType,
                periodTypeId = s.periodTypeId,
                targetFactor = s.targetFactor,
            }).ToList());

            MemberOrderPeriodListViewModel userListViewModel = new MemberOrderPeriodListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<MemberOrderPeriodServiceModel> GetAsync(int id, int? memberId = null)
        {
            return _memberOrderPeriods.Value
                .Where(w => w.memberOrderPeriodId == id)
                .Where(w => w.memberId == memberId || memberId == null)
                .Include(w=>w.factors)
                .Include(w=>w.periodType)
                .ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new MemberOrderPeriodServiceModel
                    {
                        memberOrderPeriodId = s.memberOrderPeriodId,
                        payType = s.payType,
                        periodType = s.periodType,
                        periodTypeId = s.periodTypeId,
                        targetFactor = s.factors.FirstOrDefault(),
                        factors=s.factors,
                    }).FirstOrDefault()
                );
        }

        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public MemberOrderPeriod Insert(MemberOrderPeriodServiceModel model)
        {
            MemberOrderPeriod memberOrderPeriod = new MemberOrderPeriod()
            {
            };

            ChangeState(memberOrderPeriod, EntityState.Added);

            return memberOrderPeriod;
        }

        public void Update(MemberOrderPeriodServiceModel model)
        {
            MemberOrderPeriod memberOrderPeriod = _memberOrderPeriods.Value.FirstOrDefault(f => f.memberOrderPeriodId == model.memberOrderPeriodId);
            if (memberOrderPeriod == null)
                throw new NotFoundException();

            ChangeState(memberOrderPeriod, EntityState.Modified);
        }
    }

}




