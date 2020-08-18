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
    [Route("api/discountFactor")]
    public class DiscountFactorController : BaseApiController
    {
        Lazy<IDiscountFactorRepository> _discountFactorRepository;

        public DiscountFactorController(IUnitOfWork uow, Lazy<IDiscountFactorRepository> DiscountFactorRepository) : base(uow)
        {
            _discountFactorRepository = DiscountFactorRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(DiscountFactorListViewModel model)
        {
            model = await _discountFactorRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _discountFactorRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertDiscountFactorViewModel model)
        {
            try
            {
                var discountFactor = _discountFactorRepository.Value.Insert(new DiscountFactorServiceModel
                {
                    discount = model.discount,
                    enable = model.enable,
                    endDate = model.endDate,
                    maxDiscount = model.maxDiscount,
                    maxRegisterDate = model.maxRegisterDate,
                    startDate = model.startDate,
                    minPrice = model.minPrice,
                    percent = model.percent,
                });
                await _uow.SaveAsync();

                return Ok(new { discountFactor.discountFactorId });

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateDiscountFactorViewModel model)
        {
            _discountFactorRepository.Value.Update(new DiscountFactorServiceModel
            {
                discountFactorId=model.discountFactorId,
                discount = model.discount,
                enable = model.enable,
                endDate = model.endDate,
                maxDiscount = model.maxDiscount,
                maxRegisterDate = model.maxRegisterDate,
                minPrice = model.minPrice,
                percent = model.percent,
                startDate = model.startDate,
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _discountFactorRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _discountFactorRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }

    }
}