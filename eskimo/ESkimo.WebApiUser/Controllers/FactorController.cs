using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace ESkimo.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/factor")]
    public class FactorController : BaseApiController
    {
        Lazy<IFactorRepository> _factorRepository;
        Lazy<IPaymentRepository> _paymentRepository;

        public FactorController(IUnitOfWork uow,
            Lazy<IPaymentRepository> PaymentRepository,
            Lazy<IFactorRepository> FactorRepository) : base(uow)
        {
            _factorRepository = FactorRepository;
            _paymentRepository = PaymentRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(FactorListViewModel model)
        {
            model = await _factorRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _factorRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpGet, Route("{Id}/payPerson"), Authorize]
        public async Task<IActionResult> payPerson(int Id)
        {
            var factor = _factorRepository.Value.AsQueryable().Include(i => i.payment)
                .Where(w => w.factorId == Id).FirstOrDefault();

            if (!factor.payment.success)
            {
                factor.payment.paymentType = PaymentType.Person;
                factor.status = FactorStatus.Payment;
                _paymentRepository.Value.ChangeState(factor.payment, EntityState.Modified);
                _factorRepository.Value.ChangeState(factor, EntityState.Modified);
                await _uow.SaveAsync();

                return NoContent();
            }
            else
            {
                return Ok("پرداخت شده است");
            }
        }

        [HttpGet, Route("{Id}/status/{status}"), Authorize]
        public async Task<IActionResult> changeStatus(int Id, int status)
        {
            var factor = _factorRepository.Value.AsQueryable().Where(w => w.factorId == Id).FirstOrDefault();

            if (((factor.status == FactorStatus.Payment || factor.status == FactorStatus.Receive) && status == 3) || 
                (factor.status == FactorStatus.Sent && status == 4))
            {
                factor.status = 
                    status == 3 ? FactorStatus.Sent : 
                    status == 4 ? FactorStatus.Receive : factor.status;
                _factorRepository.Value.ChangeState(factor, EntityState.Modified);
                await _uow.SaveAsync();

                return NoContent();
            }
            else
            {
                return Ok("امکان تغییر وضعیت در این مرحله وجود ندارد");
            }
        }
    }
}