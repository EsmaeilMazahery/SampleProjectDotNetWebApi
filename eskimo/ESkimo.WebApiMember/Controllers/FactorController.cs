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
    [Route("api/factor")]
    public class FactorController : BaseApiController
    {
        Lazy<IFactorRepository> _factorRepository;
        Lazy<IFactorItemRepository> _factorItemRepository;
        Lazy<IProductRepository> _productRepository;
        Lazy<IDiscountCodeRepository> _discountCodeRepository;
        Lazy<IDiscountFactorRepository> _discountFactorRepository;
        Lazy<IPeriodTypeRepository> _periodTypeRepository;
        Lazy<IMemberRepository> _memberRepository;
        Lazy<IPaymentRepository> _paymentRepository;
        Lazy<IMemberLocationRepository> _memberLocationRepository;
        Lazy<IMemberOrderPeriodRepository> _memberOrderPeriodRepository;
        Lazy<ISettingRepository> _settingRepository;

        public FactorController(IServiceProvider serviceProvider, IUnitOfWork uow,
            Lazy<IFactorRepository> FactorRepository,
            Lazy<IFactorItemRepository> FactorItemRepository,
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
            _factorItemRepository = FactorItemRepository;
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
        public async Task<IActionResult> Index(FactorListViewModel model)
        {
            model = await _factorRepository.Value.GetAllAsync(model, memberId.Value);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                var model = await _factorRepository.Value.GetAsync(Id, memberId.Value);
                return Ok(model);
            }
            catch
            {
                return BadRequest("خطایی در دریافت اطلاعات رخ داد");
            }
          
        }

        [HttpPost, Route("basket"), AllowAnonymous]
        public async Task<IActionResult> Basket([FromBody]BasketViewModel model)
        {
            try
            {

            ProductListViewModel productListViewModel = new ProductListViewModel()
            {
                products = model.products.Select(s => s.productId).ToList()
            };

            productListViewModel = await _productRepository.Value.GetAllMemberAsync(productListViewModel);

            model.list = productListViewModel.list;



            if (!string.IsNullOrEmpty(model.discountCode) && memberId.HasValue)
            {
                var member = await _memberRepository.Value.GetAsync(memberId.Value);

                var discount = _discountCodeRepository.Value.AsQueryable().Where(w => w.code == model.discountCode).FirstOrDefault();
                if (discount == null || !discount.enable)
                    model.messageDiscountCode = "کد تخفیف اشتباه است";
                else if (discount.startDate > DateTime.Now || discount.endDate < DateTime.Now)
                    model.messageDiscountCode = "کد تخفیف منقضی شده است";
                else if (discount.memberId.HasValue && discount.memberId != memberId.Value)
                    model.messageDiscountCode = "کد تخفیف اشتباه است";
                else if (discount.factors.Count(c => c.memberId == memberId.Value) >= discount.countUse)
                    model.messageDiscountCode = "تعداد مجاز استفاده از این کد تخفیف تمام شده است";
                else if (discount.maxRegisterDate.HasValue && discount.maxRegisterDate.Value < (DateTime.Now - member.registerDate).TotalDays)
                    model.messageDiscountCode = "کد تخفیف اشتباه است";
                else
                    model.discountCodeData = discount;
            }

            decimal sumFactor = model.products
                    .Where(f => productListViewModel.list.Where(ff => ff.productId == f.productId).Any())
                    .Select(m => m.count * 
                    
                    productListViewModel.list.Where(f => f.productId == m.productId).FirstOrDefault()
                    .productPrices.Where(pp => pp.minCountSell <= m.count).OrderBy(o => o.amount).FirstOrDefault()?.amount?? productListViewModel.list.Where(f => f.productId == m.productId).FirstOrDefault().amountBase

                    ).Sum(s => s);

            var periodType = _periodTypeRepository.Value.AsQueryable().Where(w => w.periodTypeId == model.periodTypeId || model.periodTypeId == null).FirstOrDefault();

            if (model.discountCodeData == null || !model.discountCodeData.activeAlone)
            {
                var discountFactor = _discountFactorRepository.Value.AsQueryable().Where(w => w.startDate > DateTime.Now && w.endDate < DateTime.Now && w.enable && w.minPrice < sumFactor).FirstOrDefault();
                if (discountFactor != null)
                {
                    decimal amountDiscountFactor = 0;
                    if (discountFactor.percent.HasValue)
                        amountDiscountFactor = discountFactor.percent.Value * sumFactor / 100;
                    else if (discountFactor.discount.HasValue)
                        amountDiscountFactor = discountFactor.discount.Value;
                    model.amountDiscountFactor = discountFactor.maxDiscount.HasValue && amountDiscountFactor > discountFactor.maxDiscount.Value ? discountFactor.maxDiscount.Value : amountDiscountFactor;
                }


                if (periodType != null)
                {
                    decimal amountDiscountPeriodType = periodType.percentDiscount * sumFactor / 100;
                    model.amountDiscountPeriod = periodType.maxDiscount.HasValue && amountDiscountPeriodType > periodType.maxDiscount.Value ? periodType.maxDiscount.Value : amountDiscountPeriodType;
                }
            }

            if (memberId.HasValue)
            {
                model.memberLocation = _memberLocationRepository.Value.AsQueryable().Where(w => w.memberId == memberId.Value && (model.memberLocationId == null || w.memberLocationId == model.memberLocationId.Value))
                    .Select(s => new MemberLocation()
                    {
                        memberLocationId = s.memberLocationId,
                        address = s.address,
                        name = s.name,
                        area = new Area()
                        {
                            prices = s.area.prices.Where(w => (w.periodTypeId == null || w.periodTypeId == periodType.periodTypeId)).OrderBy(o => o.amountSend).ToList()
                        }
                    })
                    .FirstOrDefault();
            }

            return Ok(model);

            }
            catch(Exception ex)
            {

                return BadRequest();
            }
        }

        [HttpPost, Route("pay"), Authorize]
        public async Task<IActionResult> pay([FromBody]BasketViewModel model)
        {
            try
            {
                Factor factor = new Factor()
                {
                    memberId = memberId.Value,
                    dateTime = DateTime.Now,
                    status=model.paymentType==PaymentType.Factor ?  FactorStatus.RegFactor : FactorStatus.Payment
                };

                ProductListViewModel productListViewModel = new ProductListViewModel()
                {
                    products = model.products.Select(s => s.productId).ToList()
                };

                productListViewModel = await _productRepository.Value.GetAllMemberAsync(productListViewModel);

                model.list = productListViewModel.list;

                if (!string.IsNullOrEmpty(model.discountCode) && memberId.HasValue)
                {
                    var member = await _memberRepository.Value.GetAsync(memberId.Value);

                    var discount = _discountCodeRepository.Value.AsQueryable().Where(w => w.code == model.discountCode).FirstOrDefault();
                    if (discount == null || !discount.enable)
                        model.messageDiscountCode = "کد تخفیف اشتباه است";
                    else if (discount.startDate > DateTime.Now || discount.endDate < DateTime.Now)
                        model.messageDiscountCode = "کد تخفیف منقضی شده است";
                    else if (discount.memberId.HasValue && discount.memberId != memberId.Value)
                        model.messageDiscountCode = "کد تخفیف اشتباه است";
                    else if (discount.factors.Count(c => c.memberId == memberId.Value) >= discount.countUse)
                        model.messageDiscountCode = "تعداد مجاز استفاده از این کد تخفیف تمام شده است";
                    else if (discount.maxRegisterDate.HasValue && discount.maxRegisterDate.Value < (DateTime.Now - member.registerDate).TotalDays)
                        model.messageDiscountCode = "کد تخفیف اشتباه است";
                    else
                        model.discountCodeData = discount;
                }

              //  decimal sumFactor = model.products.Where(f => productListViewModel.list.Where(ff => ff.productId == f.productId).Any()).Select(m => m.count * productListViewModel.list.Where(f => f.productId == m.productId).FirstOrDefault().productPrices.Where(pp => pp.minCountSell <= m.count).OrderBy(o => o.amount).FirstOrDefault().amount).Sum(s => s);


                decimal sumFactor = model.products
                    .Where(f => productListViewModel.list.Where(ff => ff.productId == f.productId).Any())
                    .Select(m => m.count *

                   ( productListViewModel.list.Where(f => f.productId == m.productId).FirstOrDefault()
                    .productPrices.Where(pp => pp.minCountSell <= m.count).OrderBy(o => o.amount).FirstOrDefault()?.amount ?? productListViewModel.list.Where(f => f.productId == m.productId).FirstOrDefault().amountBase)

                    ).Sum(s => s);

                var periodType = _periodTypeRepository.Value.AsQueryable().Where(w => w.periodTypeId == model.periodTypeId || model.periodTypeId == null).FirstOrDefault();

                if (model.discountCodeData == null || !model.discountCodeData.activeAlone)
                {
                    var discountFactor = _discountFactorRepository.Value.AsQueryable().Where(w => w.startDate > DateTime.Now && w.endDate < DateTime.Now && w.enable && w.minPrice < sumFactor).FirstOrDefault();
                    if (discountFactor != null)
                    {
                        decimal amountDiscountFactor = 0;
                        if (discountFactor.percent.HasValue)
                            amountDiscountFactor = discountFactor.percent.Value * sumFactor / 100;
                        else if (discountFactor.discount.HasValue)
                            amountDiscountFactor = discountFactor.discount.Value;
                        model.amountDiscountFactor = discountFactor.maxDiscount.HasValue && amountDiscountFactor > discountFactor.maxDiscount.Value ? discountFactor.maxDiscount.Value : amountDiscountFactor;

                        factor.discountFactorId = discountFactor.discountFactorId;
                        factor.discountOfFactor = model.amountDiscountFactor;
                    }

                    if (periodType != null)
                    {
                        decimal amountDiscountPeriodType = periodType.percentDiscount * sumFactor / 100;
                        model.amountDiscountPeriod = periodType.maxDiscount.HasValue && amountDiscountPeriodType > periodType.maxDiscount.Value ? periodType.maxDiscount.Value : amountDiscountPeriodType;

                        factor.discountOfPeriod = model.amountDiscountPeriod;
                        factor.periodTypeId = periodType.periodTypeId;
                    }
                }

                if (memberId.HasValue)
                {
                    model.memberLocation = _memberLocationRepository.Value.AsQueryable().Where(w => w.memberId == memberId.Value && (model.memberLocationId == null || w.memberLocationId == model.memberLocationId.Value))
                        .Select(s => new MemberLocation()
                        {
                            memberLocationId = s.memberLocationId,
                            address = s.address,
                            name = s.name,
                            area = new Area()
                            {
                                prices = s.area.prices.Where(w => (w.periodTypeId == null || w.periodTypeId == periodType.periodTypeId)).OrderBy(o => o.amountSend).ToList()
                            }
                        })
                        .FirstOrDefault();

                    factor.memberLocationId = model.memberLocation.memberLocationId;
                }

                decimal discountCodeAmount = 0;
                if (model.discountCodeData != null)
                {
                    if (model.discountCodeData.percent.HasValue)
                        discountCodeAmount = model.discountCodeData.percent.Value * sumFactor / 100;
                    else if (model.discountCodeData.discount.HasValue)
                        discountCodeAmount = model.discountCodeData.discount.Value;
                    discountCodeAmount = model.discountCodeData.maxDiscount.HasValue && discountCodeAmount > model.discountCodeData.maxDiscount.Value ? model.discountCodeData.maxDiscount.Value : discountCodeAmount;

                    factor.discountCodeId = model.discountCodeData.discountCodeId;
                    factor.discountCodeName = model.discountCodeData.name;
                    factor.discountOfCode = discountCodeAmount;
                }

                var amountSend = model.memberLocation != null && model.memberLocation.area.prices.Count() > 0 ? model.memberLocation.area.prices[0].amountSend : 0;
                decimal payAmount = sumFactor - model.amountDiscountFactor - discountCodeAmount - model.amountDiscountPeriod + amountSend;// + (sumFactor * 9 / 100);

                factor.amountSend = amountSend;
                factor.amount = payAmount;
                factor.tax = 0;// (sumFactor * 9 / 100);

                Payment payment = new Payment()
                {
                    amount = payAmount,
                    dateTime = DateTime.Now,
                    factor = factor,
                    memberId = memberId.Value,
                    //memberOrderPeriod = memberOrderPeriod
                    paymentType = model.paymentType
                };
                factor.payment = payment;

                if (periodType != null && (periodType.day > 0 || periodType.month > 0))
                {
                    MemberOrderPeriod memberOrderPeriod = new MemberOrderPeriod()
                    {
                        //بعدا تغییر کند
                        payType = Infrastructure.Enumerations.PayType.Factor,
                        periodTypeId = periodType.periodTypeId,
                        memberId = memberId.Value
                    };

                    memberOrderPeriod.periodTypeId = periodType.periodTypeId;
                    factor.memberOrderPeriod = memberOrderPeriod;

                    _memberOrderPeriodRepository.Value.ChangeState(memberOrderPeriod, EntityState.Added);
                }

                foreach (var product in model.products)
                {
                    var pItem = productListViewModel.list.Where(ff => ff.productId == product.productId).FirstOrDefault();
                    var ppItem = pItem.productPrices.Where(pp => pp.minCountSell <= product.count).OrderBy(o => o.amount).FirstOrDefault();

                    FactorItem factorItem = new FactorItem()
                    {
                        count = product.count,
                        amount = (ppItem==null? pItem.amountBase: ppItem.amount) * product.count,
                        amountBasePerItem = pItem.amountBase,
                        amountPerItem = (ppItem == null ? pItem.amountBase : ppItem.amount),
                        factor = factor,
                        name = pItem.name,
                        productPriceId =  ppItem?.productPriceId
                    };

                    _factorItemRepository.Value.ChangeState(factorItem, EntityState.Added);
                }

                _factorRepository.Value.ChangeState(factor, EntityState.Added);
                _paymentRepository.Value.ChangeState(payment, EntityState.Added);

                await _uow.SaveAsync();

                if (model.paymentType == PaymentType.Factor)
                {
                    try
                    {
                        string MerchantID = await _settingRepository.Value.read(SettingType.ZarinPalMerchent);

                        if (string.IsNullOrEmpty(MerchantID))
                        {
                            return Ok(new { result = false, message = "مشکلی در پرداخت آنلاین به وجود آمد با پشتیبانی تماس بگیرید" });
                        }
                        var paymentZ = new Zarinpal.Payment(MerchantID, (int)payAmount);

                        string address = await _settingRepository.Value.read(SettingType.CustomerSite);

                        String CallbackURL = address + "/VerficationPage/" + payment.paymentId;
                        string Description = await _settingRepository.Value.read(SettingType.DescriptionPayment);

                        var result = await paymentZ.PaymentRequest(Description, CallbackURL);

                        if (result.Status == 100)
                        {
                            return Ok(new { result = true, message = "", url = result.Link });
                        }
                        else
                        {
                            return Ok(new { result = false, message = "مشکلی در پرداخت آنلاین به وجود آمد با پشتیبانی تماس بگیرید - کد خطا : " + result.Status });
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.Value.Insert(LogType.Api_Payment, LogLevel.ERROR, "Factor-pay-ZarinPal", ex.ToString());
                        return Ok(new { result = false, factor.factorId, message = "عملیات با خطا مواجه شد" });
                    }
                }
                else
                {
                    return Ok(new { result = true, message = "", factor.factorId });
                }
            }
            catch (Exception ex)
            {
                logger.Value.Insert(LogType.Api_Factor_Pay, LogLevel.ERROR, "Factor-pay", ex.ToString());
                return Ok(new { result = false, message = "عملیات با خطا مواجه شد" });
            }
        }

        [HttpGet, Route("{Id}/payOnline"), Authorize]
        public async Task<IActionResult> payOnline(int Id)
        {
            var factor = _factorRepository.Value.AsQueryable().Include(i => i.payment)
                .Where(w => w.factorId == Id).FirstOrDefault();

            if (!factor.payment.success)
            {
                string MerchantID = await _settingRepository.Value.read(SettingType.ZarinPalMerchent);

                if (string.IsNullOrEmpty(MerchantID))
                {
                    return Ok(new { result = false, message = "مشکلی در پرداخت آنلاین به وجود آمد با پشتیبانی تماس بگیرید" });
                }
                var paymentZ = new Zarinpal.Payment(MerchantID, (int)factor.amount);

                string address = await _settingRepository.Value.read(SettingType.CustomerSite);

                String CallbackURL = address + "/VerficationPage/" + factor.payment.paymentId;
                string Description = await _settingRepository.Value.read(SettingType.DescriptionPayment);

                var result = await paymentZ.PaymentRequest(Description, CallbackURL);

                if (result.Status == 100)
                {
                    return Ok(new { result = true, message = "", url = result.Link });
                }
                else
                {
                    return Ok(new { result = false, message = "مشکلی در پرداخت آنلاین به وجود آمد با پشتیبانی تماس بگیرید - کد خطا : " + result.Status });
                }
            }
            else
            {
                return Ok(new { result = false, message = "پرداخت شده است" });
            }
        }

        [HttpGet, Route("{Id}/payPerson"), Authorize]
        public async Task<IActionResult> payPerson(int Id)
        {
            var factor = _factorRepository.Value.AsQueryable().Include(i => i.payment)
                .Where(w => w.factorId == Id && w.memberId==memberId.Value).FirstOrDefault();

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

        [HttpGet, Route("VerficationPage/{PaymentId}"), AllowAnonymous]
        public async Task<IActionResult> VerficationPage(int PaymentId, string Status, string authority)
        {
            var payment = _paymentRepository.Value.AsQueryable().Include(i=>i.factor)
                .Where(w => w.paymentId == PaymentId).FirstOrDefault();
            if (payment != null)
            {
                string MerchantID = await _settingRepository.Value.read(SettingType.ZarinPalMerchent);

                if (string.IsNullOrEmpty(MerchantID))
                {
                    return Ok(new { result = false, factorId = payment.factorId, message = "مشکلی در پرداخت آنلاین به وجود آمد با پشتیبانی تماس بگیرید" });
                }
                var paymentZ = new Zarinpal.Payment(MerchantID, (int)payment.amount);

                var result = await paymentZ.Verification(authority);

                if (result.Status == 100)
                {
                    payment.trackingCode = result.RefId.ToString();
                    payment.success = true;
                    if (payment.paymentType == PaymentType.Person)
                        payment.paymentType = PaymentType.Factor;
                    _paymentRepository.Value.ChangeState(payment, EntityState.Modified);
                    _factorRepository.Value.ChangeState(payment.factor, EntityState.Modified);
                    await _uow.SaveAsync();
                    return Ok(new { result = true, payment.factorId, message = "" });
                }
                else
                {
                    return Ok(new { result = false, payment.factorId, message = "عملیات پرداخت با خطا مواجه شد" });
                }
            }
            else
            {
                return Ok(new { result = false, message = "درخواست نامعتبر است" });
            }
        }

    }
}