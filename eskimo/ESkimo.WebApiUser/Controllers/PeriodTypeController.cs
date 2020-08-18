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
    [Route("api/periodType")]
    public class PeriodTypeController : BaseApiController
    {

        Lazy<IPeriodTypeRepository> _periodTypeRepository;

        public PeriodTypeController(IUnitOfWork uow, Lazy<IPeriodTypeRepository> PeriodTypeRepository) : base(uow)
        {
            _periodTypeRepository = PeriodTypeRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(PeriodTypeListViewModel model)
        {
            model = await _periodTypeRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _periodTypeRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertPeriodTypeViewModel model)
        {
            try
            {
                if(model.day<=0 && model.month <= 0)
                {
                  if(  _periodTypeRepository.Value.AsQueryable().Where(w=>w.day<=0 && w.month<=0).Any())
                    return BadRequest("فقط یک بازه ارسال با روز و ماه صفر می توانید اضافه کنید");
                }

                var periodType = _periodTypeRepository.Value.Insert(new PeriodTypeServiceModel
                {
                    name = model.name,
                    day = model.day,
                    month = model.month,
                    percentDiscount = model.percentDiscount,
                    maxDiscount = model.maxDiscount,
                });
                await _uow.SaveAsync();

                return Ok(new { periodType.periodTypeId });

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdatePeriodTypeViewModel model)
        {
            if (model.day <= 0 && model.month <= 0)
            {
                if (_periodTypeRepository.Value.AsQueryable().Where(w => w.day <= 0 && w.month <= 0 && w.periodTypeId!=model.periodTypeId).Any())
                    return BadRequest("فقط یک بازه ارسال با روز و ماه صفر می توانید اضافه کنید");
            }

            _periodTypeRepository.Value.Update(new PeriodTypeServiceModel
            {
                periodTypeId = model.periodTypeId,
                name = model.name,
                day = model.day,
                month = model.month,
                percentDiscount = model.percentDiscount,
                maxDiscount = model.maxDiscount,
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _periodTypeRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _periodTypeRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }

    }
}