using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiMember.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESkimo.WebApiMember.Controllers
{
    [Produces("application/json")]
    [Route("api/memberOrderPeriod")]
    public class MemberOrderPeriodController : BaseApiController
    {
        Lazy<IFactorRepository> _factorRepository;
        Lazy<IProductRepository> _productRepository;
        Lazy<IDiscountCodeRepository> _discountCodeRepository;
        Lazy<IDiscountFactorRepository> _discountFactorRepository;
        Lazy<IPeriodTypeRepository> _periodTypeRepository;
        Lazy<IMemberRepository> _memberRepository;
        Lazy<IPaymentRepository> _paymentRepository;
        Lazy<IMemberLocationRepository> _memberLocationRepository;
        Lazy<IMemberOrderPeriodRepository> _memberOrderPeriodRepository;
        Lazy<ISettingRepository> _settingRepository;

        public MemberOrderPeriodController(IServiceProvider serviceProvider, IUnitOfWork uow,
            Lazy<IFactorRepository> FactorRepository,
            Lazy<IProductRepository> ProductRepository,
            Lazy<IDiscountCodeRepository> DiscountCodeRepository,
            Lazy<IDiscountFactorRepository> DiscountFactorRepository,
            Lazy<IMemberRepository> MemberRepository,
            Lazy<IPaymentRepository> PaymentRepository,
            Lazy<ISettingRepository> SettingRepository,
            Lazy<IMemberOrderPeriodRepository> MemberOrderPeriodRepository,
            Lazy<IMemberLocationRepository> MemberLocationRepository,
            Lazy<IPeriodTypeRepository> PeriodTypeRepository) : base(serviceProvider, uow)
        {
            _factorRepository = FactorRepository;
            _productRepository = ProductRepository;
            _discountCodeRepository = DiscountCodeRepository;
            _discountFactorRepository = DiscountFactorRepository;
            _periodTypeRepository = PeriodTypeRepository;
            _memberRepository = MemberRepository;
            _memberLocationRepository = MemberLocationRepository;
            _paymentRepository = PaymentRepository;
            _memberOrderPeriodRepository = MemberOrderPeriodRepository;
            _settingRepository = SettingRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(MemberOrderPeriodListViewModel model)
        {
            try
            {
                model = await _memberOrderPeriodRepository.Value.GetAllAsync(model, memberId.Value);
                return Ok(model);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _memberOrderPeriodRepository.Value.GetAsync(Id, memberId.Value);
            return Ok(model);
        }

    }
}