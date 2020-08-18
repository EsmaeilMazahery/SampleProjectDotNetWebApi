using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/discountCode")]
    public class DiscountCodeController : BaseApiController
    {

        Lazy<IDiscountCodeRepository> _discountCodeRepository;

        public DiscountCodeController(IUnitOfWork uow, Lazy<IDiscountCodeRepository> DiscountCodeRepository) : base(uow)
        {
            _discountCodeRepository = DiscountCodeRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(DiscountCodeListViewModel model)
        {
            model = await _discountCodeRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _discountCodeRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertDiscountCodeViewModel model)
        {
            try
            {
                var discountCode = _discountCodeRepository.Value.Insert(new DiscountCodeServiceModel
                {
                    name = model.name,
                    activeAlone = model.activeAlone,
                    code = model.code,
                    countUse = model.countUse,
                    discount = model.discount,
                    enable = model.enable,
                    endDate = model.endDate,
                    maxDiscount = model.maxDiscount,
                    maxRegisterDate = model.maxRegisterDate,
                    minPrice = model.minPrice,
                    percent = model.percent,
                    startDate = model.startDate,
                    selectedBrands = model.selectedBrands.ToList(),
                    selectedCategories = model.selectedCategories.ToList(),
                });
                await _uow.SaveAsync();

                return Ok(new { discountCode.discountCodeId });

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateDiscountCodeViewModel model)
        {
            _discountCodeRepository.Value.Update(new DiscountCodeServiceModel
            {
                discountCodeId = model.discountCodeId,
                name = model.name,
                activeAlone = model.activeAlone,
                code = model.code,
                countUse = model.countUse,
                discount = model.discount,
                enable = model.enable,
                endDate = model.endDate,
                maxDiscount = model.maxDiscount,
                maxRegisterDate = model.maxRegisterDate,
                minPrice = model.minPrice,
                percent = model.percent,
                startDate = model.startDate,
                selectedBrands = model.selectedBrands.ToList(),
                selectedCategories = model.selectedCategories.ToList(),
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _discountCodeRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _discountCodeRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }

    }
}