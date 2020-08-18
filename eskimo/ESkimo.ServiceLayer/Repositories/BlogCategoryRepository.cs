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
    public interface IBlogCategoryRepository : IGenericRepository<BlogCategory>
    {
        Task<BlogCategoryListViewModel> GetAllAsync(BlogCategoryListViewModel model);
        Task<BlogCategoryServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        BlogCategory Insert(BlogCategoryServiceModel model);
        void Update(BlogCategoryServiceModel model);

        Task<FilterPaggingViewModel> GetFilterAsync(string title = null, int? page = null, int? rowsPerPage = null);
    }

    public class BlogCategoryRepository : GenericRepository<BlogCategory>, IBlogCategoryRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<BlogCategory>> _blogCategories;
        public BlogCategoryRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _blogCategories = new Lazy<DbSet<BlogCategory>>(() => _uow.Set<BlogCategory>());
        }


        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.blogCategoryId == id);
        }

        public async Task<BlogCategoryListViewModel> GetAllAsync(BlogCategoryListViewModel model)
        {
            var query = _blogCategories.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.title))
                query = query.Where(w => w.title.Contains(model.title));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            query = query.Include(i => i.parent);

            // select
            List<BlogCategory> records = await query.Select(s => new
            {
                s.blogCategoryId,
                s.title,
                parent = s.parentId != null ? s.parent.title + (s.parent.parentId != null ? "-" + s.parent.parent.title + (s.parent.parent.parentId != null ? "-" + s.parent.parent.parent.title : "") : "") : "",
                s.enable
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new BlogCategory
            {
                title = s.title,
                blogCategoryId = s.blogCategoryId,
                parent = new BlogCategory()
                {
                    title = s.parent
                },
                enable = s.enable
            }).ToList());

            BlogCategoryListViewModel blogCategoryListViewModel = new BlogCategoryListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return blogCategoryListViewModel;
        }

        public Task<BlogCategoryServiceModel> GetAsync(int id)
        {
            return _blogCategories.Value.Where(w => w.blogCategoryId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new BlogCategoryServiceModel
                    {
                        blogCategoryId = s.blogCategoryId,
                        title = s.title,
                        parentId = s.parentId,
                        parent = s.parent,
                        description = s.description,
                        enable = s.enable
                    }).FirstOrDefault()
                );
        }

        public async Task<FilterPaggingViewModel> GetFilterAsync(string title = null, int? page = null, int? rowsPerPage = null)
        {
            var query = _blogCategories.Value.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(w => w.title.Contains(title));

            // rows count
            int allRows = query.Count();

            // paging
            if (rowsPerPage.HasValue && page.HasValue)
                query = query.Paging(rowsPerPage.Value, page.Value);

            var list = await query.Select(s => new
            {
                id = s.blogCategoryId,
                value = s.title
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

        public BlogCategory Insert(BlogCategoryServiceModel model)
        {
            BlogCategory blogCategory = new BlogCategory()
            {
                title = model.title,
                parentId = model.parentId,
                description = model.description,
                enable = model.enable
            };

            ChangeState(blogCategory, EntityState.Added);

            return blogCategory;
        }

        public void Update(BlogCategoryServiceModel model)
        {
            BlogCategory blogCategory = _blogCategories.Value.FirstOrDefault(f => f.blogCategoryId == model.blogCategoryId);
            if (blogCategory == null)
                throw new NotFoundException();

            blogCategory.title = model.title;
            blogCategory.parentId = model.parentId;
            blogCategory.description = model.description;
            blogCategory.enable = model.enable;

            ChangeState(blogCategory, EntityState.Modified);
        }
    }
}