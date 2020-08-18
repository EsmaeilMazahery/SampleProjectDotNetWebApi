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
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<PaymentListViewModel> GetAllAsync(PaymentListViewModel model);
        Task<PaymentServiceModel> GetAsync(int id);
        Task DeleteAsync(int id);
        Payment Insert(PaymentServiceModel model);
        void Update(PaymentServiceModel model);

        Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name);
    }

    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Payment>> _payments;
        public PaymentRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _payments = new Lazy<DbSet<Payment>>(() => _uow.Set<Payment>());
        }

        
        public async Task DeleteAsync(int id)
        {
           await DeleteAsync(d => d.paymentId == id);
        }

        public async Task<PaymentListViewModel> GetAllAsync(PaymentListViewModel model)
        {
            var query = _payments.Value.AsQueryable();

            // sort
            query = query.OrderByPropertyName(model.sort, model.sortDirection);

            // rows count
            int allRows = query.Count();

            // paging
            query = query.Paging(model.rowsPerPage, model.page);

            // select
            List<Payment> records = await query.Select(s => new
            {
                s.paymentId,
            }).ToListAsync()
            .ContinueWith(list => list.Result.Select(s => new Payment
            {
               paymentId=s.paymentId,
            }).ToList());

            PaymentListViewModel userListViewModel = new PaymentListViewModel()
            {
                allRows = allRows,
                list = records
            };

            return userListViewModel;
        }

        public Task<PaymentServiceModel> GetAsync(int id)
        {
            return _payments.Value.Where(w => w.paymentId == id).ToListAsync()
                .ContinueWith(
                    list => list.Result.Select(s => new PaymentServiceModel
                    {
                        paymentId=s.paymentId,
                    }).FirstOrDefault()
                );
        }



        public Task<IEnumerable<FilterViewModel>> GetFilterAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Payment Insert(PaymentServiceModel model)
        {
            Payment payment = new Payment()
            {
            };
            
            ChangeState(payment, EntityState.Added);

            return payment;
        }

        public void Update(PaymentServiceModel model)
        {
            Payment payment = _payments.Value.FirstOrDefault(f => f.paymentId == model.paymentId);
            if (payment == null)
                throw new NotFoundException();

            
            ChangeState(payment, EntityState.Modified);
        }
    }

}




