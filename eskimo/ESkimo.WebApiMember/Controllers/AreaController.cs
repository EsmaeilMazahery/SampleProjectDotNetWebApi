using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiMember.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace ESkimo.WebApiMember.Controllers
{
    [Produces("application/json")]
    [Route("api/area")]
    public class AreaController : BaseApiController
    {

        Lazy<IAreaRepository> _areaRepository;

        public AreaController(IServiceProvider serviceProvider,IUnitOfWork uow, Lazy<IAreaRepository> AreaRepository) : base(serviceProvider,uow)
        {
            _areaRepository = AreaRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index()
        {
            AreaListViewModel model = await _areaRepository.Value.GetAllMemberAsync();
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _areaRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _areaRepository.Value.GetFilterAsync(name, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }
    }
}