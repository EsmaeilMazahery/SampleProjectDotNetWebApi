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
    public interface IProductPriceRepository : IGenericRepository<ProductPrice>
    {
        Task<ProductPriceListViewModel> GetAllAsync(ProductPriceListViewModel model, int? memberId = null);
        Task<ProductPriceServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Task EnableAsync(int id, bool enable);
        ProductPrice Insert(ProductPriceServiceModel model);
        void Update(ProductPriceServiceModel model);
        void UpdatePrices(Dictionary<int,ProductPriceServiceModel> listModel);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class ProductPriceRepository : GenericRepository<ProductPrice>, IProductPriceRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<ProductPrice>> _productPrices;
        public ProductPriceRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _productPrices = new Lazy<DbSet<ProductPrice>>(() => _uow.Set<ProductPrice>());
        }

        public Task DeleteAsync(int id)
        {
            return DeleteAsync(d => d.productPriceId == id);
        }

        public Task EnableAsync(int id, bool enable)
        {
            return UpdateAsync(w => w.productPriceId == id, u => new ProductPrice() { enable = enable });
        }

        public async Task<ProductPriceListViewModel> GetAllAsync(ProductPriceListViewModel model,int? memberId=null)
        {
            var query = _productPrices.Value.AsQueryable();

            if (model.productId.HasValue)
                query = query.Where(w => w.productId == model.productId);

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            int allRows = 0;

            if (!model.excel)
            {
                // rows count
                allRows = query.Count();

                // paging
                query = query.Paging(model.rowsPerPage, model.page);
            }
           
            // select
            List<ProductPrice> records = await query.Select(s => new
            {
                s.enable,
                s.productId,
                s.productPriceId,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new ProductPrice
            {
                enable=s.enable,
                productId=s.productId,
                productPriceId=s.productPriceId,
            }).ToList());

            ProductPriceListViewModel productPriceListViewModel = new ProductPriceListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return productPriceListViewModel;
        }

        public Task<ProductPriceServiceModel> GetAsync(int id)
        {
            return _productPrices.Value.Where(w => w.productPriceId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new ProductPriceServiceModel
                    {
                        enable = s.enable,
                        product = s.product,
                        productId = s.productId,
                        productPriceId = s.productPriceId,
                    }).FirstOrDefault()
                );
        }
        
        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public ProductPrice Insert(ProductPriceServiceModel model)
        {
            ProductPrice productPrice = new ProductPrice()
            {
                enable = model.enable,
                productId = model.productId,
            };
            
            ChangeState(productPrice, EntityState.Added);

            return productPrice;
        }

        public void Update(ProductPriceServiceModel model)
        {
            ProductPrice productPrice = _productPrices.Value.FirstOrDefault(f => f.productPriceId == model.productPriceId);
            if (productPrice == null)
                throw new NotFoundException();

            productPrice.enable = model.enable;


            ChangeState(productPrice, EntityState.Modified);
        }

        public void UpdatePrices(Dictionary<int,ProductPriceServiceModel> listModel)
        {
            var productPrices = _productPrices.Value.AsQueryable().Where(w => listModel.Any(a => a.Key == w.productPriceId)).ToList();
            
            foreach(var productPrice in productPrices)
            {
                var model = listModel[productPrice.productPriceId];
                if (model == null)
                    continue;

                productPrice.enable = model.enable;

                ChangeState(productPrice, EntityState.Modified);
            }
        }
    }

}




