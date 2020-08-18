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
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<CommentListViewModel> GetAllAsync(CommentListViewModel model);
        Task<CommentServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Comment Insert(CommentServiceModel model);
        void Update(CommentServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Comment>> _comments;
        public CommentRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _comments = new Lazy<DbSet<Comment>>(() => _uow.Set<Comment>());
        }


        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.commentId == id);
        }

        public async Task<CommentListViewModel> GetAllAsync(CommentListViewModel model)
        {
            var query = _comments.Value.AsQueryable();

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Comment> records = await query.Select(s => new
            {
                s.commentId,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Comment()
            {
                commentId = s.commentId,
            }).ToList());

            CommentListViewModel commentListViewModel = new CommentListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return commentListViewModel;
        }

        public Task<CommentServiceModel> GetAsync(int id)
        {
            return _comments.Value.Where(w => w.commentId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new CommentServiceModel
                    {
                        commentId = s.commentId,

                    }).FirstOrDefault()
                );
        }



        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Comment Insert(CommentServiceModel model)
        {
            Comment comment = new Comment()
            {
                text = model.text
            };

            ChangeState(comment, EntityState.Added);

            return comment;
        }

        public void Update(CommentServiceModel model)
        {
            Comment comment = _comments.Value.FirstOrDefault(f => f.commentId == model.commentId);
            if (comment == null)
                throw new NotFoundException();

            comment.text = model.text;

            ChangeState(comment, EntityState.Modified);
        }
    }

}




