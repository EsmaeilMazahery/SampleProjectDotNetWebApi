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
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<ProductListViewModel> GetAllAsync(ProductListViewModel model);
        Task<ProductServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Task EnableAsync(int id, bool enable);
        Product Insert(ProductServiceModel model);
        void Update(ProductServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);

        Task<ProductListViewModel> GetAllMemberAsync(ProductListViewModel model);
        Task<ProductListViewModel> GetAllMemberWholesaleAsync(ProductListViewModel model);
    }

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Product>> _products;
        public ProductRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _products = new Lazy<DbSet<Product>>(() => _uow.Set<Product>());
        }

        public Task DeleteAsync(int id)
        {
            return DeleteAsync(d => d.productId == id);
        }

        public Task EnableAsync(int id, bool enable)
        {
            return UpdateAsync(w => w.productId == id, u => new Product() { enable = enable });
        }

        public async Task<ProductListViewModel> GetAllAsync(ProductListViewModel model)
        {
            var query = _products.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            //include
            query = query.Include(i => i.brand).Include(i => i.category).Include(i => i.productType);

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Product> records = await query.Select(s => new
            {

                s.brandId,
                s.name,
                s.amountBase,
                s.productId,
                s.categoryId,
                s.imageAddress,
                s.enable,
                brand = s.brand.name,
                category = s.category.name,
                productType = s.productType.name,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Product
            {
                productId = s.productId,
                name = s.name,
                amountBase=s.amountBase,
                brand = new Brand() { name = s.brand },
                brandId = s.brandId,
                category = new Category() { name = s.category },
                categoryId = s.categoryId,
                imageAddress = s.imageAddress,
                enable = s.enable,
                productType = new ProductType() { name = s.productType }
            }).ToList());

            ProductListViewModel productListViewModel = new ProductListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return productListViewModel;
        }

        public async Task<ProductListViewModel> GetAllMemberAsync(ProductListViewModel model)
        {
            var query = _products.Value.AsQueryable();

            query = query.Include(i => i.productPrices).Where(w => w.enable && w.productPrices.Any(a => a.enable && a.count > 0));

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            if (model.brandId.HasValue)
                query = query.Where(w => w.brandId == model.brandId.Value);

            if (model.products != null)
                query = query.Where(w => model.products.Any(a => a == w.productId));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            int allRows = 0;
            if (model.rowsPerPage != 0)
            {
                // rows count
                allRows = query.Count();
                // paging
                //query = query.Paging(model.rowsPerPage, model.page);
            }

            //include
            query = query.Include(i => i.brand).Include(i => i.category).Include(i => i.productType);

            // select
            List<Product> records = await query.Select(s => new
            {
                s.brandId,
                s.name,
                s.productId,
                s.categoryId,
                s.imageAddress,
                s.enable,
                s.amountBase,
                brand = s.brand.name,
                category = s.category.name,
                productType = s.productType.name,
                productPrices = s.productPrices.Where(wp => wp.enable && wp.count > 0).ToList()
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Product
            {
                productId = s.productId,
                name = s.name,
                brand = new Brand() { name = s.brand },
                brandId = s.brandId,
                category = new Category() { name = s.category },
                categoryId = s.categoryId,
                imageAddress = s.imageAddress,
                enable = s.enable,
                amountBase=s.amountBase,
                productType = new ProductType() { name = s.productType },
                productPrices = s.productPrices.OrderBy(o => o.amount).ToList()
            }).ToList());

            ProductListViewModel productListViewModel = new ProductListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return productListViewModel;
        }

        public async Task<ProductListViewModel> GetAllMemberWholesaleAsync(ProductListViewModel model)
        {
            var query = _products.Value.AsQueryable();

            query = query.Include(i => i.productPriceWholesales).Where(w => w.enable && w.productPriceWholesales.Any(a => a.enable && a.count > 0));

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            if (model.brandId.HasValue)
                query = query.Where(w => w.brandId == model.brandId.Value);

            if (model.products != null)
                query = query.Where(w => model.products.Any(a => a == w.productId));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            int allRows = 0;
            if (model.rowsPerPage != 0)
            {
                // rows count
                allRows = query.Count();
                // paging
                //query = query.Paging(model.rowsPerPage, model.page);
            }

            //include
            query = query.Include(i => i.brand).Include(i => i.category).Include(i => i.productType);

            // select
            List<Product> records = await query.Select(s => new
            {
                s.brandId,
                s.name,
                s.productId,
                s.categoryId,
                s.imageAddress,
                s.enable,
                s.amountBase,
                brand = s.brand.name,
                category = s.category.name,
                productType = s.productType.name,
                productPriceWholesales = s.productPriceWholesales.Where(wp => wp.enable && wp.count > 0).ToList()
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Product
            {
                productId = s.productId,
                name = s.name,
                brand = new Brand() { name = s.brand },
                brandId = s.brandId,
                category = new Category() { name = s.category },
                categoryId = s.categoryId,
                imageAddress = s.imageAddress,
                enable = s.enable,
                amountBase = s.amountBase,
                productType = new ProductType() { name = s.productType },
                productPriceWholesales = s.productPriceWholesales.OrderBy(o => o.amount).ToList()
            }).ToList());

            ProductListViewModel productListViewModel = new ProductListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return productListViewModel;
        }



        public Task<ProductServiceModel> GetAsync(int id)
        {
            return _products.Value.Where(w => w.productId == id)
                .Include(i => i.productPrices)
                .Include(i => i.productPriceWholesales).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new ProductServiceModel
                    {
                        productId = s.productId,
                        name = s.name,
                        amountBase=s.amountBase,
                        attributes = s.attributes,
                        imageAddress = s.imageAddress,
                        enable = s.enable,
                        description = s.description,

                        productPrices = s.productPrices,
                        productPriceWholesales=s.productPriceWholesales,

                        productTypeId = s.productTypeId,
                        productType = s.productType,

                        categoryId = s.categoryId,
                        category = s.category,

                        brandId = s.brandId,
                        brand = s.brand,

                    }).FirstOrDefault()
                );
        }

        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Product Insert(ProductServiceModel model)
        {
            Product product = new Product()
            {
                name = model.name,
                description = model.description,
                enable = model.enable,
                imageAddress = model.imageAddress,
                attributes = model.attributes,

                brandId = model.brandId,
                categoryId = model.categoryId,
                productTypeId = model.productTypeId,
                amountBase=model.amountBase
            };

            product.productPrices = new List<ProductPrice>();

            model.productPrices.ToList().ForEach(pp =>
            {
                var newp = new ProductPrice()
                {
                    product = product,
                    amount = pp.amount,
                    count = pp.count,
                    enable = pp.enable,
                    minCountSell = pp.minCountSell,
                };
                product.productPrices.Add(newp);
                _uow.Entry(newp).State = EntityState.Added;
            });

            product.productPriceWholesales = new List<ProductPriceWholesale>();

            model.productPriceWholesales.ToList().ForEach(pp =>
            {
                var newp = new ProductPriceWholesale()
                {
                    product = product,
                    amount = pp.amount,
                    count = pp.count,
                    enable = pp.enable,
                    minCountSell = pp.minCountSell,
                };
                product.productPriceWholesales.Add(newp);
                _uow.Entry(newp).State = EntityState.Added;
            });

            ChangeState(product, EntityState.Added);

            return product;
        }

        public void Update(ProductServiceModel model)
        {
            Product product = _products.Value.Include(i => i.productPrices).Include(i => i.productPriceWholesales)
                .FirstOrDefault(f => f.productId == model.productId);
            if (product == null)
                throw new NotFoundException();

            product.brandId = model.brandId;
            product.name = model.name;
            product.attributes = model.attributes;
            product.categoryId = model.categoryId;
            product.description = model.description;
            product.enable = model.enable;
            product.imageAddress = model.imageAddress;
            product.productTypeId = model.productTypeId;
            product.amountBase = model.amountBase;

            product.productPrices
               .Where(w => !model.productPrices.Any(a => a.productPriceId == w.productPriceId))
               .ToList().ForEach(pp =>
               {
                   _uow.Entry(pp).State = EntityState.Deleted;
               });

            model.productPrices.Where(w => w.productPriceId == 0).ToList().ForEach(pp =>
                {
                    var newpp = new ProductPrice()
                    {
                        product = product,
                        amount = pp.amount,
                        count = pp.count,
                        enable = pp.enable,
                        minCountSell = pp.minCountSell,
                    };

                    product.productPrices.Add(newpp);
                    _uow.Entry(newpp).State = EntityState.Added;
                });

            model.productPrices.Where(w => w.productPriceId != 0).ToList().ForEach(pp =>
            {
                var ddf = product.productPrices.FirstOrDefault(f => f.productPriceId == pp.productPriceId);

                if (ddf != null)
                {
                    ddf.amount = pp.amount;
                    ddf.count = pp.count;
                    ddf.enable = pp.enable;
                    ddf.minCountSell = pp.minCountSell;

                    _uow.Entry(ddf).State = EntityState.Modified;
                }
            });



            product.productPriceWholesales
             .Where(w => !model.productPriceWholesales.Any(a => a.productPriceWholesaleId == w.productPriceWholesaleId))
             .ToList().ForEach(pp =>
             {
                 _uow.Entry(pp).State = EntityState.Deleted;
             });

            model.productPriceWholesales.Where(w => w.productPriceWholesaleId == 0).ToList().ForEach(pp =>
            {
                var newpp = new ProductPriceWholesale()
                {
                    product = product,
                    amount = pp.amount,
                    count = pp.count,
                    enable = pp.enable,
                    minCountSell = pp.minCountSell,
                };

                product.productPriceWholesales.Add(newpp);
                _uow.Entry(newpp).State = EntityState.Added;
            });

            model.productPriceWholesales.Where(w => w.productPriceWholesaleId != 0).ToList().ForEach(pp =>
            {
                var ddf = product.productPriceWholesales.FirstOrDefault(f => f.productPriceWholesaleId == pp.productPriceWholesaleId);

                if (ddf != null)
                {
                    ddf.amount = pp.amount;
                    ddf.count = pp.count;
                    ddf.enable = pp.enable;
                    ddf.minCountSell = pp.minCountSell;

                    _uow.Entry(ddf).State = EntityState.Modified;
                }
            });


            ChangeState(product, EntityState.Modified);
        }
    }
}




