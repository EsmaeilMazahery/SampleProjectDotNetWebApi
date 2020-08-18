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
    public interface IAreaPriceRepository : IGenericRepository<AreaPrice>
    {
        Task<AreaPriceListViewModel> GetAllAsync(AreaPriceListViewModel model);
        Task<AreaPriceServiceModel> GetAsync(int areaId, int periodTypeId);
        Task DeleteAsync(int areaId, int periodTypeId);
        AreaPrice Insert(AreaPriceServiceModel model);
        void Update(AreaPriceServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class AreaPriceRepository : GenericRepository<AreaPrice>, IAreaPriceRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<AreaPrice>> _areaPrices;
        public AreaPriceRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _areaPrices = new Lazy<DbSet<AreaPrice>>(() => _uow.Set<AreaPrice>());
        }

        public async Task DeleteAsync(int areaId, int periodTypeId)
        {
            await DeleteAsync(d => d.areaId == areaId && d.periodTypeId== periodTypeId);
        }

        public async Task<AreaPriceListViewModel> GetAllAsync(AreaPriceListViewModel model)
        {
            var query = _areaPrices.Value.AsQueryable();

            if (model.areaId.HasValue)
                query = query.Where(w => w.areaId == model.areaId.Value);

            if (model.periodTypeId.HasValue)
                query = query.Where(w => w.periodTypeId == model.periodTypeId.Value);

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<AreaPrice> records = await query.Select(s => new
            {
                s.amountSend,
                s.areaId,
                s.periodTypeId,
                s.periodType,
                s.area
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new AreaPrice
            {
                areaId = s.areaId,
                area = s.area,
                periodType = s.periodType,
                periodTypeId = s.periodTypeId,
                amountSend = s.amountSend
            }).ToList());

            AreaPriceListViewModel userListViewModel = new AreaPriceListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<AreaPriceServiceModel> GetAsync(int areaId,int periodTypeId)
        {
            return _areaPrices.Value.Where(w => w.areaId == areaId && w.periodTypeId== periodTypeId).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new AreaPriceServiceModel
                    {
                        amountSend=s.amountSend,
                        periodTypeId=s.periodTypeId,
                        areaId=s.areaId
                    }).FirstOrDefault()
                );
        }

        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public AreaPrice Insert(AreaPriceServiceModel model)
        {
            AreaPrice area = new AreaPrice()
            {
                amountSend=model.amountSend,
                areaId=model.areaId,
                periodTypeId=model.periodTypeId
            };

            ChangeState(area, EntityState.Added);

            return area;
        }

        public void Update(AreaPriceServiceModel model)
        {
            AreaPrice area = _areaPrices.Value.FirstOrDefault(f => f.areaId == model.areaId && f.periodTypeId==model.periodTypeId);
            if (area == null)
                throw new NotFoundException();

            area.amountSend = model.amountSend;

            ChangeState(area, EntityState.Modified);
        }
    }

}




