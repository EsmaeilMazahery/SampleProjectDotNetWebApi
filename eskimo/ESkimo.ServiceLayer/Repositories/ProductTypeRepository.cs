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
    public interface IProductTypeRepository : IGenericRepository<ProductType>
    {
        Task<ProductTypeListViewModel> GetAllAsync(ProductTypeListViewModel model);
        Task<ProductTypeServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        ProductType Insert(ProductTypeServiceModel model);
        void Update(ProductTypeServiceModel model);

        Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null);
    }

    public class ProductTypeRepository : GenericRepository<ProductType>, IProductTypeRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<ProductType>> _productTypes;
        public ProductTypeRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _productTypes = new Lazy<DbSet<ProductType>>(() => _uow.Set<ProductType>());
        }

        
        public async Task DeleteAsync(int id)
        {
           await DeleteAsync(d => d.productTypeId == id);
        }

        public async Task<ProductTypeListViewModel> GetAllAsync(ProductTypeListViewModel model)
        {
            var query = _productTypes.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<ProductType> records = await query.Select(s => new
            {
                s.productTypeId,
                s.name,
                s.enable
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new ProductType
            {
               name=s.name,
               productTypeId=s.productTypeId,
               enable=s.enable
            }).ToList());

            ProductTypeListViewModel userListViewModel = new ProductTypeListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<ProductTypeServiceModel> GetAsync(int id)
        {
            return _productTypes.Value.Where(w => w.productTypeId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new ProductTypeServiceModel
                    {
                        productTypeId=s.productTypeId,
                        name=s.name,
                        enable=s.enable
                    }).FirstOrDefault()
                );
        }



        public async Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var query = _productTypes.Value.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            // rows count
            int allRows = query.Count();

            // paging
            if (rowsPerPage.HasValue && page.HasValue)
                query = query.Paging(rowsPerPage.Value, page.Value);

            var list = await query.Select(s => new
            {
                id = s.productTypeId,
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

        public ProductType Insert(ProductTypeServiceModel model)
        {
            ProductType productType = new ProductType()
            {
                name=model.name,
                enable=model.enable
            };
            
            ChangeState(productType, EntityState.Added);

            return productType;
        }

        public void Update(ProductTypeServiceModel model)
        {
            ProductType productType = _productTypes.Value.FirstOrDefault(f => f.productTypeId == model.productTypeId);
            if (productType == null)
                throw new NotFoundException();

            productType.name = model.name;
            productType.enable = model.enable;

            ChangeState(productType, EntityState.Modified);
        }
    }

}




