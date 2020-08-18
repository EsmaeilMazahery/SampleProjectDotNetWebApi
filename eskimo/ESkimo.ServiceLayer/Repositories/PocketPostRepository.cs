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
    public interface IPocketPostRepository : IGenericRepository<PocketPost>
    {
        Task<PocketPostListViewModel> GetAllAsync(PocketPostListViewModel model);
        Task<PocketPostServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        PocketPost Insert(PocketPostServiceModel model);
    }

    public class PocketPostRepository : GenericRepository<PocketPost>, IPocketPostRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<PocketPost>> _pocketPosts;
        public PocketPostRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _pocketPosts = new Lazy<DbSet<PocketPost>>(() => _uow.Set<PocketPost>());
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteAsync(d => d.pocketPostId == id);
        }

        public async Task<PocketPostListViewModel> GetAllAsync(PocketPostListViewModel model)
        {
            var query = _pocketPosts.Value.AsQueryable();

            if (model.startSendDateTime.HasValue)
                query = query.Where(w => w.sendDateTime > model.startSendDateTime);

            if (model.endSendDateTime.HasValue)
                query = query.Where(w => w.sendDateTime < model.endSendDateTime);

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<PocketPost> records = await query.Select(s => new
            {
                s.pocketPostId,
                s.amount,
                s.registerDateTime,
                s.sendDateTime,
                s.count,
                s.userId,
                s.user,
                s.userSender,
                s.userSenderId
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new PocketPost
            {
                pocketPostId = s.pocketPostId,
                amount = s.amount,
                user = s.user,
                userId = s.userId,
                registerDateTime = s.registerDateTime,
                sendDateTime = s.sendDateTime,
                count = s.count,
                userSender = s.userSender,
                userSenderId = s.userSenderId
            }).ToList());

            PocketPostListViewModel userListViewModel = new PocketPostListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<PocketPostServiceModel> GetAsync(int id)
        {
            return _pocketPosts.Value.Where(w => w.pocketPostId == id)
                .Include(i=>i.factors)
                .Include(i=>i.user)
                .Include(i=>i.userSender)
                .ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new PocketPostServiceModel
                    {
                        pocketPostId = s.pocketPostId,
                        amount = s.amount,
                        count = s.count,
                        sendDateTime = s.sendDateTime,
                        registerDateTime = s.registerDateTime,
                        factors = s.factors,
                        user = s.user,
                        userId = s.userId,
                        userSenderId = s.userSenderId,
                        userSender = s.userSender,
                    }).FirstOrDefault()
                );
        }

        public PocketPost Insert(PocketPostServiceModel model)
        {
            PocketPost pocketPost = new PocketPost()
            {
                registerDateTime = DateTime.Now,
                userSenderId = model.userSenderId,
                userId = model.userId,
                amount = model.amount,
                count = model.selectedFactors.Count(),
                sendDateTime = model.sendDateTime,
                description = model.description,
            };

            ChangeState(pocketPost, EntityState.Added);

            return pocketPost;
        }

    }
}




