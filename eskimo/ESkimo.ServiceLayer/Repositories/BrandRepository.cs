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
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<BrandListViewModel> GetAllAsync(BrandListViewModel model);
        Task<BrandServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Brand Insert(BrandServiceModel model);
        void Update(BrandServiceModel model);

        Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null);


        Task<BrandListViewModel> GetAllMemberAsync();
        Task<List<Brand>> GetPriceTableAsync();
        Task<List<Brand>> GetWholeSaleAsync();

    }

    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Brand>> _brands;
        public BrandRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _brands = new Lazy<DbSet<Brand>>(() => _uow.Set<Brand>());
        }


        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.brandId == id);
        }

        public async Task<BrandListViewModel> GetAllMemberAsync()
        {
            var query = _brands.Value.AsQueryable();

            query = query.Where(w => w.products.Any() && w.enable);

            // select
            List<Brand> records = await query.Select(s => new
            {
                s.brandId,
                s.name,
                s.image
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Brand
            {
                name = s.name,
                brandId = s.brandId,
                image = s.image
            }).ToList());

            BrandListViewModel userListViewModel = new BrandListViewModel()
            {
                list = records
            };

            return userListViewModel;
        }

        public async Task<List<Brand>> GetPriceTableAsync()
        {
            var query = _brands.Value.AsQueryable();

            query = query.Where(w => w.products.Any(a=>a.productPrices.Any()) && w.enable).Include(i => i.products);

            // select
            List<Brand> records = await query.Select(s => new
            {
                s.brandId,
                s.name,
                s.image,
                products = s.products.Where(w=>w.productPrices.Any()).Select(p => new
                {
                    p.name,
                    p.amountBase,
                    p.imageAddress,
                    productPrices = p.productPrices.Select(pp => new
                    {
                        pp.amount,
                        pp.minCountSell,
                        pp.productPriceId
                    }).OrderBy(o => o.minCountSell)
                })
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Brand
            {
                name = s.name,
                brandId = s.brandId,
                image = s.image,
                products = s.products.Select(p => new Product()
                {
                    name = p.name,
                    amountBase = p.amountBase,
                    imageAddress = p.imageAddress,
                    productPrices = p.productPrices.Select(pp => new ProductPrice()
                    {
                        amount = pp.amount,
                        minCountSell = pp.minCountSell,
                        productPriceId = pp.productPriceId
                    }).ToList()
                }).ToList()
            }).ToList());

            return records;
        }

        public async Task<List<Brand>> GetWholeSaleAsync()
        {
            var query = _brands.Value.AsQueryable();

            query = query.Where(w => w.products.Any(a=>a.productPriceWholesales.Any()) && w.enable).Include(i => i.products);

            // select
            List<Brand> records = await query.Select(s => new
            {
                s.brandId,
                s.name,
                s.image,
                products = s.products.Where(w=>w.productPriceWholesales.Any()).Select(p => new
                {
                    p.name,
                    p.amountBase,
                    p.imageAddress,
                    productPriceWholesales = p.productPriceWholesales.Select(pp => new
                    {
                        pp.amount,
                        pp.minCountSell,
                        pp.productPriceWholesaleId
                    }).OrderBy(o => o.minCountSell)
                })
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Brand
            {
                name = s.name,
                brandId = s.brandId,
                image = s.image,
                products = s.products.Select(p => new Product()
                {
                    name = p.name,
                    amountBase = p.amountBase,
                    imageAddress = p.imageAddress,
                    productPriceWholesales = p.productPriceWholesales.Select(pp => new ProductPriceWholesale()
                    {
                        amount = pp.amount,
                        minCountSell = pp.minCountSell,
                        productPriceWholesaleId = pp.productPriceWholesaleId
                    }).ToList()
                }).ToList()
            }).ToList());

            return records;
        }





        public async Task<BrandListViewModel> GetAllAsync(BrandListViewModel model)
        {
            var query = _brands.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Brand> records = await query.Select(s => new
            {
                s.brandId,
                s.name,
                s.image,
                s.enable
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Brand
            {
                name = s.name,
                brandId = s.brandId,
                image = s.image,
                enable = s.enable
            }).ToList());

            BrandListViewModel userListViewModel = new BrandListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<BrandServiceModel> GetAsync(int id)
        {
            return _brands.Value.Where(w => w.brandId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new BrandServiceModel
                    {
                        brandId = s.brandId,
                        name = s.name,
                        image = s.image
                    }).FirstOrDefault()
                );
        }

        public async Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var query = _brands.Value.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            // rows count
            int allRows = query.Count();

            // paging
            if (rowsPerPage.HasValue && page.HasValue)
                query = query.Paging(rowsPerPage.Value, page.Value);

            var list = await query.Select(s => new
            {
                id = s.brandId,
                value = s.name
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

        public Brand Insert(BrandServiceModel model)
        {
            Brand brand = new Brand()
            {
                name = model.name,
                image = model.image,
                enable = model.enable
            };

            ChangeState(brand, EntityState.Added);

            return brand;
        }

        public void Update(BrandServiceModel model)
        {
            Brand brand = _brands.Value.FirstOrDefault(f => f.brandId == model.brandId);
            if (brand == null)
                throw new NotFoundException();

            brand.name = model.name;
            brand.image = model.image;
            brand.enable = model.enable;

            ChangeState(brand, EntityState.Modified);
        }
    }

}




