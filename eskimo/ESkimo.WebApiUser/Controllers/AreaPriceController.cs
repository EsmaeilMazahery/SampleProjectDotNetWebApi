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
using NetTopologySuite.Geometries;

namespace ESkimo.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/areaPrice")]
    public class AreaPriceController : BaseApiController
    {

        Lazy<IAreaPriceRepository> _areaPriceRepository;

        public AreaPriceController(IUnitOfWork uow, Lazy<IAreaPriceRepository> AreaPriceRepository) : base(uow)
        {
            _areaPriceRepository = AreaPriceRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(AreaPriceListViewModel model)
        {
            model = await _areaPriceRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{areaId}/{periodTypeId}"), Authorize]
        public async Task<IActionResult> Get(int areaId, int periodTypeId)
        {
            var model = await _areaPriceRepository.Value.GetAsync(areaId,periodTypeId);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertAreaPriceViewModel model)
        {
            try
            {
                var areaPrice = _areaPriceRepository.Value.Insert(new AreaPriceServiceModel
                {
                    periodTypeId=model.periodTypeId,
                    areaId=model.areaId,
                    amountSend=model.amountSend
                });
                await _uow.SaveAsync();

                return NoContent();

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateAreaPriceViewModel model)
        {
            _areaPriceRepository.Value.Update(new AreaPriceServiceModel
            {
                amountSend=model.amountSend,
                areaId=model.areaId,
                periodTypeId=model.periodTypeId
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{areaId}/{periodTypeId}"), Authorize]
        public async Task<IActionResult> Delete(int areaId, int periodTypeId)
        {
            await _areaPriceRepository.Value.DeleteAsync(areaId, periodTypeId);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _areaPriceRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }
    }
}