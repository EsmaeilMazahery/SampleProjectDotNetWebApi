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
    public interface IBlogPostRepository : IGenericRepository<BlogPost>
    {
        Task<BlogPostListViewModel> GetAllAsync(BlogPostListViewModel model);
        Task<BlogPostListViewModel> GetAllMemberAsync(BlogPostListViewModel model);

        Task<BlogPostServiceModel> GetAsync(int id);
        Task<BlogPostServiceModel> GetAsync(string url);
        Task DeleteAsync(int id);
        BlogPost Insert(BlogPostServiceModel model);
        void Update(BlogPostServiceModel model);

        Task<FilterPaggingViewModel> GetFilterAsync(string name = null, int? page = null, int? rowsPerPage = null);
    }

    public class BlogPostRepository : GenericRepository<BlogPost>, IBlogPostRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<BlogPost>> _blogPosts;
        public BlogPostRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _blogPosts = new Lazy<DbSet<BlogPost>>(() => _uow.Set<BlogPost>());
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.blogPostId == id);
        }

        public async Task<BlogPostListViewModel> GetAllMemberAsync()
        {
            var query = _blogPosts.Value.AsQueryable();

            query = query.Where(w => w.enable && w.publishDate < DateTime.Now);

            // select
            List<BlogPost> records = await query.Select(s => new
            {
                s.blogPostId,
                s.title,
                s.image,
                s.blogCategory,
                s.publishDate,
                s.url
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new BlogPost
            {
                blogPostId = s.blogPostId,
                title = s.title,
                image = s.image,
                blogCategory = s.blogCategory,
                publishDate = s.publishDate,
                url = s.url,
            }).ToList());

            BlogPostListViewModel blogPostListViewModel = new BlogPostListViewModel()
            {
                list = records
            };

            return blogPostListViewModel;
        }


        public async Task<BlogPostListViewModel> GetAllMemberAsync(BlogPostListViewModel model)
        {
            var query = _blogPosts.Value.AsQueryable();

            query = query.Where(w => w.enable && w.publishDate < DateTime.Now);

            if (!string.IsNullOrEmpty(model.title))
                query = query.Where(w => w.title.Contains(model.title));

            if (model.categoryId.HasValue)
                query = query.Where(w => w.blogCategoryId == model.categoryId);

            // sort
            query = query.OrderBy(_=>_.publishDate);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<BlogPost> records = await query.Select(s => new
            {
                s.blogPostId,
                s.title,
                s.image,
                s.blogCategory,
                s.publishDate,
                s.url,
                s.content,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new BlogPost
            {
                blogPostId = s.blogPostId,
                title = s.title,
                image = s.image,
                blogCategory = s.blogCategory,
                publishDate = s.publishDate,
                url = s.url,
                content= s.content.ReduceTextPost(string.IsNullOrEmpty(s.image) ? 350 : 200)
            }).ToList());

            BlogPostListViewModel blogPostListViewModel = new BlogPostListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return blogPostListViewModel;
        }


        public async Task<BlogPostListViewModel> GetAllAsync(BlogPostListViewModel model)
        {
            var query = _blogPosts.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.title))
                query = query.Where(w => w.title.Contains(model.title));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<BlogPost> records = await query.Select(s => new
            {
                s.blogPostId,
                s.title,
                s.image,
                s.blogCategory,
                s.publishDate,
                s.url
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new BlogPost
            {
                blogPostId = s.blogPostId,
                title = s.title,
                image = s.image,
                blogCategory = s.blogCategory,
                publishDate = s.publishDate,
                url = s.url,
            }).ToList());

            BlogPostListViewModel blogPostListViewModel = new BlogPostListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return blogPostListViewModel;
        }

        public Task<BlogPostServiceModel> GetAsync(int id)
        {
            return _blogPosts.Value.Where(w => w.blogPostId == id)
                .Include(i=>i.blogComments).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new BlogPostServiceModel
                    {
                        blogPostId = s.blogPostId,
                        title = s.title,
                        image = s.image,
                        blogCategory = s.blogCategory,
                        publishDate = s.publishDate,
                        url = s.url,
                        blogComments = s.blogComments,
                        content = s.content,
                        enableComment = s.enableComment,
                        blogCategoryId = s.blogCategoryId
                    }).FirstOrDefault()
                );
        }

        public Task<BlogPostServiceModel> GetAsync(string url)
        {
            return _blogPosts.Value.Where(w => w.url == url).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new BlogPostServiceModel
                    {
                        blogPostId = s.blogPostId,
                        title = s.title,
                        image = s.image,
                        blogCategory = s.blogCategory,
                        publishDate = s.publishDate,
                        url = s.url,
                        blogComments = s.blogComments,
                        content = s.content,
                        enableComment = s.enableComment,
                        blogCategoryId = s.blogCategoryId
                    }).FirstOrDefault()
                );
        }

        public async Task<FilterPaggingViewModel> GetFilterAsync(string title = null, int? page = null, int? rowsPerPage = null)
        {
            var query = _blogPosts.Value.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(w => w.title.Contains(title));

            // rows count
            int allRows = query.Count();

            // paging
            if (rowsPerPage.HasValue && page.HasValue)
                query = query.Paging(rowsPerPage.Value, page.Value);

            var list = await query.Select(s => new
            {
                id = s.blogPostId,
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

        public BlogPost Insert(BlogPostServiceModel model)
        {
            BlogPost blogPost = new BlogPost()
            {
                title = model.title,
                image = model.image,
                publishDate = model.publishDate,
                url = model.url,
                content = model.content,
                enableComment = model.enableComment,
                blogCategoryId = model.blogCategoryId,
                registerDateTime = DateTime.Now,
                enable = model.enable,
                userId = model.userId,
            };

            ChangeState(blogPost, EntityState.Added);

            return blogPost;
        }

        public void Update(BlogPostServiceModel model)
        {
            BlogPost blogPost = _blogPosts.Value.FirstOrDefault(f => f.blogPostId == model.blogPostId);
            if (blogPost == null)
                throw new NotFoundException();

            blogPost.title = model.title;
            blogPost.image = model.image;
            blogPost.publishDate = model.publishDate;
            blogPost.url = model.url;
            blogPost.content = model.content;
            blogPost.enableComment = model.enableComment;
            blogPost.blogCategoryId = model.blogCategoryId;
            blogPost.enable = model.enable;

            ChangeState(blogPost, EntityState.Modified);
        }
    }

}




