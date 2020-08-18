using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESkimo.ServiceLayer.Services
{
    public interface IFactorRepository : IGenericRepository<Factor>
    {
        Task<FactorListViewModel> GetAllAsync(FactorListViewModel model, int? memberId = null);
        Task<FactorServiceModel> GetAsync(int id, int? memberId = null);
        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
        Task DeleteAsync(int id);
    }

    public class FactorRepository : GenericRepository<Factor>, IFactorRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Factor>> _factors;
        public FactorRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _factors = new Lazy<DbSet<Factor>>(() => _uow.Set<Factor>());
        }

        public Task DeleteAsync(int id)
        {
            return UpdateAsync(w => w.factorId == id, u => new Factor() { Delete = true });
            // return DeleteAsync(d => d.memberId == id);
        }

        public async Task<FactorListViewModel> GetAllAsync(FactorListViewModel model, int? memberId = null)
        {
            var query = _factors.Value.AsQueryable();

            if (memberId.HasValue)
            {
                query = query.Where(w => memberId == w.memberId);
            }

            if (model.startDateTime.HasValue)
                query = query.Where(w => w.sendDateTime >= model.startDateTime);

            if (model.endDateTime.HasValue)
                query = query.Where(w => w.sendDateTime <= model.endDateTime);

            if (model.status.HasValue)
                query = query.Where(w => w.status == model.status);

            query = query.Where(w => !w.Delete);

            // sort
            //query = query.OrderByPropertyName(model.sort, model.sortDirection);
            query = query.OrderByDescending(o => o.dateTime);
            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Factor> records = await query.Include(i => i.payment).Select(s => new
            {
                s.factorId,
                member = new Member()
                {
                    name = s.member.name,
                    family = s.member.family
                },
                s.amount,
                s.dateTime,
                s.sendDateTime,
                s.sent,
                s.status,
                success = s.payment != null ? s.payment.success : false,
                paymentType = s.payment != null ? s.payment.paymentType : PaymentType.Factor,
                s.pocketPostId
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Factor
            {
                factorId = s.factorId,
                dateTime = s.dateTime,
                amount = s.amount,
                member = s.member,
                payment = new Payment()
                {
                    success = s.success,
                    paymentType = s.paymentType
                },
                sent = s.sent,
                status = s.status,
                sendDateTime = s.sendDateTime,
                pocketPostId = s.pocketPostId
            }).ToList());

            FactorListViewModel userListViewModel = new FactorListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<FactorServiceModel> GetAsync(int id, int? memberId = null)
        {
                return _factors.Value.Where(w => w.factorId == id && (memberId == null || memberId == w.memberId))
                    .Include(i => i.factorItems)
                    .Include(i => i.payment)
                    .Include(i => i.memberLocation)
                    .Include(i => i.member)
                    .ToListAsync()
                    .ContinueWith(
                        list => list.Result.Select(s => new FactorServiceModel
                        {
                            factorId = s.factorId,
                            paymentId = s.paymentId,
                            sendDateTime = s.sendDateTime,
                            periodType = s.periodType,
                            sent = s.sent,
                            status = s.status,
                            tax = s.tax,
                            amount = s.amount,
                            amountSend = s.amountSend,
                            dateTime = s.dateTime,
                            discountCode = s.discountCode,
                            discountCodeId = s.discountCodeId,
                            discountCodeName = s.discountCodeName,
                            discountFactor = s.discountFactor,
                            discountFactorId = s.discountFactorId,
                            discountOfCode = s.discountOfCode,
                            discountOfFactor = s.discountOfFactor,
                            discountOfPeriod = s.discountOfPeriod,
                            member = s.member,
                            memberId = s.memberId,
                            memberLocation = s.memberLocation,
                            memberLocationId = s.memberLocationId,
                            memberOrderPeriod = s.memberOrderPeriod,
                            memberOrderPeriodId = s.memberOrderPeriodId,
                            payment = s.payment,
                            periodTypeId = s.periodTypeId,
                            pocketPost = s.pocketPost,
                            pocketPostId = s.pocketPostId,
                            factorItems = s.factorItems,
                        }).FirstOrDefault()
                    );
        }

        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

    }
}




