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
    public interface IDiscountCodeRepository : IGenericRepository<DiscountCode>
    {
        Task<DiscountCodeListViewModel> GetAllAsync(DiscountCodeListViewModel model);
        Task<DiscountCodeServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        DiscountCode Insert(DiscountCodeServiceModel model);
        void Update(DiscountCodeServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class DiscountCodeRepository : GenericRepository<DiscountCode>, IDiscountCodeRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<DiscountCode>> _discountCodes;
        public DiscountCodeRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _discountCodes = new Lazy<DbSet<DiscountCode>>(() => _uow.Set<DiscountCode>());
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.discountCodeId == id);
        }

        public async Task<DiscountCodeListViewModel> GetAllAsync(DiscountCodeListViewModel model)
        {
            var query = _discountCodes.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<DiscountCode> records = await query.Select(s => new
            {
                s.discountCodeId,
                s.name,
                s.activeAlone,
                s.code,
                s.countUse,
                s.discount,
                s.enable,
                s.endDate,
                s.maxDiscount,
                s.maxRegisterDate,
                s.minPrice,
                s.percent,
                s.startDate,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new DiscountCode
            {
                discountCodeId = s.discountCodeId,
                name = s.name,
                startDate = s.startDate,
                percent = s.percent,
                minPrice = s.minPrice,
                maxRegisterDate = s.maxRegisterDate,
                maxDiscount = s.maxDiscount,
                endDate = s.endDate,
                enable = s.enable,
                discount = s.discount,
                countUse = s.countUse,
                activeAlone = s.activeAlone,
                code = s.code,
            }).ToList());

            DiscountCodeListViewModel userListViewModel = new DiscountCodeListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<DiscountCodeServiceModel> GetAsync(int id)
        {
            return _discountCodes.Value.Where(w => w.discountCodeId == id)
                .Include(i=>i.brands)
                .Include(i=>i.categories).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new DiscountCodeServiceModel
                    {
                        discountCodeId = s.discountCodeId,
                        name = s.name,
                        activeAlone = s.activeAlone,
                        endDate = s.endDate,
                        enable = s.enable,
                        code = s.code,
                        discount = s.discount,
                        countUse = s.countUse,
                        maxDiscount = s.maxDiscount,
                        maxRegisterDate = s.maxRegisterDate,
                        minPrice = s.minPrice,
                        percent = s.percent,
                        startDate = s.startDate,
                        selectedBrands=s.brands.Select(b=>b.brandId).ToList(),
                        selectedCategories=s.categories.Select(c=>c.categoryId).ToList()
                    }).FirstOrDefault()
                );
        }

        public async Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            var query = _discountCodes.Value.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            return await query.Select(s => new
            {
                id = s.discountCodeId,
                value = s.name
            }).ToListAsync().ContinueWith(list => list.Result.Select(s => new FilterViewModel()
            {
                id = s.id,
                value = s.value
            }).ToList());
        }

        public DiscountCode Insert(DiscountCodeServiceModel model)
        {
            DiscountCode discountCode = new DiscountCode()
            {
                name = model.name,
                startDate=model.startDate,
                percent=model.percent,
                minPrice=model.minPrice,
                maxRegisterDate=model.maxRegisterDate,
                maxDiscount=model.maxDiscount,
                countUse=model.countUse,
                discount=model.discount,
                code=model.code,
                enable=model.enable,
                activeAlone=model.activeAlone,
                endDate=model.endDate,

                categories= model.categories,
                brands =model.brands,
            };

            discountCode.categories = new List<Rel_DiscountCodeCategory>();

            model.selectedCategories.ForEach(categoryId =>
            {
                discountCode.categories.Add(new Rel_DiscountCodeCategory()
                {
                    discountCode = discountCode,
                    categoryId = categoryId
                });
            });

            discountCode.brands = new List<Rel_DiscountCodeBrand>();

            model.selectedBrands.ForEach(brandId =>
            {
                discountCode.brands.Add(new Rel_DiscountCodeBrand()
                {
                    discountCode = discountCode,
                    brandId = brandId
                });
            });

            ChangeState(discountCode, EntityState.Added);

            return discountCode;
        }

        public void Update(DiscountCodeServiceModel model)
        {
            DiscountCode discountCode = _discountCodes.Value
                .Include(i=>i.brands)
                .Include(i=>i.categories).FirstOrDefault(f => f.discountCodeId == model.discountCodeId);
            if (discountCode == null)
                throw new NotFoundException();

            discountCode.name = model.name;
            discountCode.startDate = model.startDate;
            discountCode.percent = model.percent;
            discountCode.minPrice = model.minPrice;
            discountCode.maxRegisterDate = model.maxRegisterDate;
            discountCode.maxDiscount = model.maxDiscount;
            discountCode.countUse = model.countUse;
            discountCode.discount = model.discount;
            discountCode.code = model.code;
            discountCode.enable = model.enable;
            discountCode.activeAlone = model.activeAlone;
            discountCode.endDate = model.endDate;


            discountCode.brands.Clear();
            discountCode.brands = new List<Rel_DiscountCodeBrand>();

            model.selectedBrands.ForEach(brandId =>
            {
                discountCode.brands.Add(new Rel_DiscountCodeBrand()
                {
                    discountCodeId = discountCode.discountCodeId,
                    brandId = brandId
                });
            });

            discountCode.categories.Clear();
            discountCode.categories = new List<Rel_DiscountCodeCategory>();

            model.selectedCategories.ForEach(categoryId =>
            {
                discountCode.categories.Add(new Rel_DiscountCodeCategory()
                {
                    discountCodeId = discountCode.discountCodeId,
                    categoryId = categoryId
                });
            });

            ChangeState(discountCode, EntityState.Modified);
        }
    }

}




