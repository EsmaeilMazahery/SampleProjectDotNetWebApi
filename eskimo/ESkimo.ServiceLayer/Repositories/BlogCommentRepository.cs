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
    public interface IBlogCommentRepository : IGenericRepository<BlogComment>
    {
        Task<BlogCommentListViewModel> GetAllAsync(BlogCommentListViewModel model);
        Task<BlogCommentServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        BlogComment Insert(BlogCommentServiceModel model);
        void Update(BlogCommentServiceModel model);

        Task<BlogCommentListViewModel> GetAllMemberAsync(int blogPostId, int page);
    }

    public class BlogCommentRepository : GenericRepository<BlogComment>, IBlogCommentRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<BlogComment>> _blogComments;
        public BlogCommentRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _blogComments = new Lazy<DbSet<BlogComment>>(() => _uow.Set<BlogComment>());
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.blogCommentId == id);
        }

        public async Task<BlogCommentListViewModel> GetAllMemberAsync(int blogPostId, int page)
        {
            var query = _blogComments.Value.AsQueryable();

            query = query.Where(w => w.enable && w.blogPostId == blogPostId);

            // sort
            query = query.OrderByDescending(o => o.registerDate);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(10, page);

            // select
            List<BlogComment> records = await query.Select(s => new
            {
                s.blogCommentId,
                s.body,
                s.member,
                s.memberName,
                s.registerDate,
                s.user
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new BlogComment
            {
                blogCommentId = s.blogCommentId,
                body = s.body,
                member = s.member,
                memberName = s.memberName,
                registerDate = s.registerDate,
                user = s.user,
            }).ToList());

            BlogCommentListViewModel userListViewModel = new BlogCommentListViewModel()
            {
                list = records
            };

            return userListViewModel;
        }

        public async Task<BlogCommentListViewModel> GetAllAsync(BlogCommentListViewModel model)
        {
            var query = _blogComments.Value.AsQueryable();

            if (!string.IsNullOrEmpty(model.body))
                query = query.Where(w => w.body.Contains(model.body));

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<BlogComment> records = await query.Select(s => new
            {
                s.blogCommentId,
                s.body,
                s.member,
                s.memberName,
                s.registerDate,
                s.user,
                s.enable
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new BlogComment
            {
                blogCommentId = s.blogCommentId,
                body = s.body,
                member = s.member,
                memberName = s.memberName,
                registerDate = s.registerDate,
                user = s.user,
                enable = s.enable
            }).ToList());

            BlogCommentListViewModel userListViewModel = new BlogCommentListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<BlogCommentServiceModel> GetAsync(int id)
        {
            return _blogComments.Value.Where(w => w.blogCommentId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new BlogCommentServiceModel
                    {
                        blogCommentId = s.blogCommentId,
                        body = s.body,
                        member = s.member,
                        memberName = s.memberName,
                        registerDate = s.registerDate,
                        user = s.user,
                        enable = s.enable,
                        memberEmail = s.memberEmail,
                        memberMobile = s.memberMobile,
                        memberId = s.memberId,
                        blogPost = s.blogPost,
                        userId = s.userId
                    }).FirstOrDefault()
                );
        }

        public BlogComment Insert(BlogCommentServiceModel model)
        {
            BlogComment blogComment = new BlogComment()
            {
                body = model.body,
                registerDate = model.registerDate,
                memberEmail = model.memberEmail,
                memberMobile = model.memberMobile,
                memberName = model.memberName,
                memberId = model.memberId,
                userId = model.userId,
                enable = model.enable
            };

            ChangeState(blogComment, EntityState.Added);

            return blogComment;
        }

        public void Update(BlogCommentServiceModel model)
        {
            BlogComment blogComment = _blogComments.Value.FirstOrDefault(f => f.blogCommentId == model.blogCommentId);
            if (blogComment == null)
                throw new NotFoundException();

            blogComment.body = model.body;
            blogComment.registerDate = model.registerDate;
            blogComment.memberEmail = model.memberEmail;
            blogComment.memberMobile = model.memberMobile;
            blogComment.memberName = model.memberName;
            blogComment.memberId = model.memberId;
            blogComment.userId = model.userId;
            blogComment.enable = model.enable;

            ChangeState(blogComment, EntityState.Modified);
        }
    }

}




