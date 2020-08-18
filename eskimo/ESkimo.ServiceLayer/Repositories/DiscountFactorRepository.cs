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
    public interface IDiscountFactorRepository : IGenericRepository<DiscountFactor>
    {
        Task<DiscountFactorListViewModel> GetAllAsync(DiscountFactorListViewModel model);
        Task<DiscountFactorServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        DiscountFactor Insert(DiscountFactorServiceModel model);
        void Update(DiscountFactorServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class DiscountFactorRepository : GenericRepository<DiscountFactor>, IDiscountFactorRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<DiscountFactor>> _discountFactors;
        public DiscountFactorRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _discountFactors = new Lazy<DbSet<DiscountFactor>>(() => _uow.Set<DiscountFactor>());
        }


        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.discountFactorId == id);
        }

        public async Task<DiscountFactorListViewModel> GetAllAsync(DiscountFactorListViewModel model)
        {
            var query = _discountFactors.Value.AsQueryable();


            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<DiscountFactor> records = await query.Select(s => new
            {
                s.discountFactorId,
                s.discount,
                s.enable,
                s.endDate,
                s.maxDiscount,
                s.maxRegisterDate,
                s.minPrice,
                s.percent,
                s.startDate,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new DiscountFactor
            {
                discountFactorId = s.discountFactorId,
                startDate = s.startDate,
                percent = s.percent,
                minPrice = s.minPrice,
                maxRegisterDate = s.maxRegisterDate,
                maxDiscount = s.maxDiscount,
                endDate = s.endDate,
                enable = s.enable,
                discount = s.discount
            }).ToList());

            DiscountFactorListViewModel userListViewModel = new DiscountFactorListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<DiscountFactorServiceModel> GetAsync(int id)
        {
            return _discountFactors.Value.Where(w => w.discountFactorId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new DiscountFactorServiceModel
                    {
                        discountFactorId = s.discountFactorId,
                        discount = s.discount,
                        enable = s.enable,
                        endDate = s.endDate,
                        maxDiscount = s.maxDiscount,
                        maxRegisterDate = s.maxRegisterDate,
                        minPrice = s.minPrice,
                        percent = s.percent,
                        startDate = s.startDate
                    }).FirstOrDefault()
                );
        }



        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public DiscountFactor Insert(DiscountFactorServiceModel model)
        {
            DiscountFactor discountFactor = new DiscountFactor()
            {
                startDate = model.startDate,
                percent = model.percent,
                minPrice = model.minPrice,
                maxRegisterDate = model.maxRegisterDate,
                maxDiscount = model.maxDiscount,
                endDate = model.endDate,
                enable = model.enable,
                discount = model.discount
            };

            ChangeState(discountFactor, EntityState.Added);

            return discountFactor;
        }

        public void Update(DiscountFactorServiceModel model)
        {
            DiscountFactor discountFactor = _discountFactors.Value.FirstOrDefault(f => f.discountFactorId == model.discountFactorId);
            if (discountFactor == null)
                throw new NotFoundException();

            discountFactor.startDate = model.startDate;
            discountFactor.percent = model.percent;
            discountFactor.minPrice = model.minPrice;
            discountFactor.maxRegisterDate = model.maxRegisterDate;
            discountFactor.maxDiscount = model.maxDiscount;
            discountFactor.endDate = model.endDate;
            discountFactor.enable = model.enable;
            discountFactor.discount = model.discount;

            ChangeState(discountFactor, EntityState.Modified);
        }
    }

}




