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
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<CategoryListViewModel> GetAllAsync(CategoryListViewModel model);
        Task<CategoryServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Category Insert(CategoryServiceModel model);
        void Update(CategoryServiceModel model);

        Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null);
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Category>> _categories;
        public CategoryRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _categories = new Lazy<DbSet<Category>>(() => _uow.Set<Category>());
        }

        public Task DeleteAsync(int id)
        {
            return DeleteAsync(d => d.categoryId == id);
        }

        public async Task<CategoryListViewModel> GetAllAsync(CategoryListViewModel model)
        {
            var query = _categories.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.name))
                query = query.Where(w => w.name.Contains(model.name));

            query = query.Include(i => i.parent);

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Category> records = await query.Select(s => new
            {
                s.categoryId,
                s.name,
                parent = s.parentId != null ? s.parent.name + (s.parent.parentId != null ? "-" + s.parent.parent.name + (s.parent.parent.parentId != null ? "-" + s.parent.parent.parent.name : "") : "") : "",
                s.parentId,
                s.enable
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Category
            {
                parent = new Category()
                {
                    name = s.parent
                },
                name = s.name,
                categoryId = s.categoryId,
                enable = s.enable
            }).ToList());

            CategoryListViewModel userListViewModel = new CategoryListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<CategoryServiceModel> GetAsync(int id)
        {
            return _categories.Value.Where(w => w.categoryId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new CategoryServiceModel
                    {
                        categoryId = s.categoryId,
                        name = s.name,
                        parent = s.parent,
                        parentId = s.parentId,
                        enable = s.enable
                    }).FirstOrDefault()
                );
        }

        public async Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var query = _categories.Value.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(w => w.name.Contains(name));

            // rows count
            int allRows = query.Count();

            // paging
            if (rowsPerPage.HasValue && page.HasValue)
                query = query.Paging(rowsPerPage.Value, page.Value);

            var list = await query.Select(s => new
            {
                id = s.categoryId,
                value = s.name,
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

        public Category Insert(CategoryServiceModel model)
        {

            Category category = new Category()
            {
                parentId = model.parentId,
                name = model.name,
                enable = model.enable,
            };

            ChangeState(category, EntityState.Added);

            return category;
        }

        public void Update(CategoryServiceModel model)
        {
            Category category = _categories.Value.FirstOrDefault(f => f.categoryId == model.categoryId);
            if (category == null)
                throw new NotFoundException();

            category.name = model.name;
            category.parentId = model.parentId;
            category.enable = model.enable;

            ChangeState(category, EntityState.Modified);
        }
    }

}




