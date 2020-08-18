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
    [Route("api/area")]
    public class AreaController : BaseApiController
    {
        Lazy<IAreaRepository> _areaRepository;

        public AreaController(IUnitOfWork uow, Lazy<IAreaRepository> AreaRepository) : base(uow)
        {
            _areaRepository = AreaRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(AreaListViewModel model)
        {
            model = await _areaRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _areaRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertAreaViewModel model)
        {
            try
            {
                var area = _areaRepository.Value.Insert(new AreaServiceModel
                {
                    name = model.name,
                    address = model.address,
                    location = model.location,
                    sendDaies = string.Join(',', model.sendDaies.Split(',').Distinct()),
                    zoom = model.location?.zoom??0,
                });
                await _uow.SaveAsync();

                return Ok(new { area.areaId });

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateAreaViewModel model)
        {
            _areaRepository.Value.Update(new AreaServiceModel
            {
                areaId = model.areaId,
                name = model.name,
                address = model.address,
                location = model.location,
                sendDaies = string.Join(',',model.sendDaies.Split(',').Distinct()),
                zoom = model.location?.zoom ?? 0,
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _areaRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _areaRepository.Value.GetFilterAsync(name, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }
    }
}