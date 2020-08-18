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
    public interface IPeriodTypeRepository : IGenericRepository<PeriodType>
    {
        Task<PeriodTypeListViewModel> GetAllAsync(PeriodTypeListViewModel model);
        Task<PeriodTypeListViewModel> GetAllMemberAsync();
        Task<PeriodTypeServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        PeriodType Insert(PeriodTypeServiceModel model);
        void Update(PeriodTypeServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class PeriodTypeRepository : GenericRepository<PeriodType>, IPeriodTypeRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<PeriodType>> _periodTypes;
        public PeriodTypeRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _periodTypes = new Lazy<DbSet<PeriodType>>(() => _uow.Set<PeriodType>());
        }


        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.periodTypeId == id);
        }

        public async Task<PeriodTypeListViewModel> GetAllAsync(PeriodTypeListViewModel model)
        {
            var query = _periodTypes.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<PeriodType> records = await query.Select(s => new
            {
                s.periodTypeId,
                s.name,
                s.maxDiscount,
                s.month,
                s.day,
                s.percentDiscount,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new PeriodType
            {
                periodTypeId = s.periodTypeId,
                name = s.name,
                maxDiscount = s.maxDiscount,
                month = s.month,
                day = s.day,
                percentDiscount = s.percentDiscount
            }).ToList());

            PeriodTypeListViewModel userListViewModel = new PeriodTypeListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }
        public async Task<PeriodTypeListViewModel> GetAllMemberAsync()
        {
            var query = _periodTypes.Value.AsQueryable();

            query = query.Where(w => w.enable);

            // select
            List<PeriodType> records = await query.Select(s => new
            {
                s.periodTypeId,
                s.name,
                s.maxDiscount,
                s.month,
                s.day,
                s.percentDiscount,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new PeriodType
            {
                periodTypeId = s.periodTypeId,
                name = s.name,
                maxDiscount = s.maxDiscount,
                month = s.month,
                day = s.day,
                percentDiscount = s.percentDiscount
            }).ToList());

            PeriodTypeListViewModel userListViewModel = new PeriodTypeListViewModel()
            {
                list = records
            };

            return userListViewModel;
        }

        public Task<PeriodTypeServiceModel> GetAsync(int id)
        {
            return _periodTypes.Value.Where(w => w.periodTypeId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new PeriodTypeServiceModel
                    {
                        periodTypeId = s.periodTypeId,
                        name = s.name,
                        percentDiscount = s.percentDiscount,
                        day = s.day,
                        month = s.month,
                        maxDiscount = s.maxDiscount,
                    }).FirstOrDefault()
                );
        }



        public async Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            var query = _periodTypes.Value.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            return await query.Select(s => new
            {
                id = s.periodTypeId,
                value = s.name
            }).ToListAsync().ContinueWith(list => list.Result.Select(s => new FilterViewModel()
            {
                id = s.id,
                value = s.value
            }).ToList());
        }

        public PeriodType Insert(PeriodTypeServiceModel model)
        {
            PeriodType periodType = new PeriodType()
            {
                name = model.name,
                maxDiscount = model.maxDiscount,
                month = model.month,
                day = model.day,
                percentDiscount = model.percentDiscount,
            };

            ChangeState(periodType, EntityState.Added);

            return periodType;
        }

        public void Update(PeriodTypeServiceModel model)
        {
            PeriodType periodType = _periodTypes.Value.FirstOrDefault(f => f.periodTypeId == model.periodTypeId);
            if (periodType == null)
                throw new NotFoundException();

            periodType.name = model.name;
            periodType.percentDiscount = model.percentDiscount;
            periodType.month = model.month;
            periodType.maxDiscount = model.maxDiscount;
            periodType.day = model.day;

            ChangeState(periodType, EntityState.Modified);
        }
    }

}




