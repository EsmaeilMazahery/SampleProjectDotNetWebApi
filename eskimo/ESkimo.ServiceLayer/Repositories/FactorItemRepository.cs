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
    public interface IFactorItemRepository : IGenericRepository<FactorItem>
    {
        Task<FactorItemListViewModel> GetAllAsync(FactorItemListViewModel model);
        Task<FactorItemServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        FactorItem Insert(FactorItemServiceModel model);
        void Update(FactorItemServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class FactorItemRepository : GenericRepository<FactorItem>, IFactorItemRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<FactorItem>> _factorItems;
        public FactorItemRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _factorItems = new Lazy<DbSet<FactorItem>>(() => _uow.Set<FactorItem>());
        }

        
        public async Task DeleteAsync(int id)
        {
           await DeleteAsync(d => d.factorItemId == id);
        }

        public async Task<FactorItemListViewModel> GetAllAsync(FactorItemListViewModel model)
        {
            var query = _factorItems.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<FactorItem> records = await query.Select(s => new
            {
                s.factorItemId,
                s.name,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new FactorItem
            {
               name=s.name,
               factorItemId=s.factorItemId,
            }).ToList());

            FactorItemListViewModel userListViewModel = new FactorItemListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<FactorItemServiceModel> GetAsync(int id)
        {
            return _factorItems.Value.Where(w => w.factorItemId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new FactorItemServiceModel
                    {
                        factorItemId=s.factorItemId,
                        name=s.name,
                    }).FirstOrDefault()
                );
        }



        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public FactorItem Insert(FactorItemServiceModel model)
        {
            FactorItem factorItem = new FactorItem()
            {
                name=model.name,
            };
            
            ChangeState(factorItem, EntityState.Added);

            return factorItem;
        }

        public void Update(FactorItemServiceModel model)
        {
            FactorItem factorItem = _factorItems.Value.FirstOrDefault(f => f.factorItemId == model.factorItemId);
            if (factorItem == null)
                throw new NotFoundException();

            factorItem.name = model.name;
            
            ChangeState(factorItem, EntityState.Modified);
        }
    }

}




