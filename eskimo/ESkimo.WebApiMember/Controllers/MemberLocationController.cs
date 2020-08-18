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
    [Route("api/memberLocation")]
    public class MemberLocationController : BaseApiController
    {

        Lazy<IMemberLocationRepository> _memberLocationRepository;

        public MemberLocationController(IServiceProvider serviceProvider, IUnitOfWork uow, Lazy<IMemberLocationRepository> MemberLocationRepository) : base(serviceProvider, uow)
        {
            _memberLocationRepository = MemberLocationRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(MemberLocationListViewModel model)
        {
            model = await _memberLocationRepository.Value.GetAllAsync(model,memberId.Value);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _memberLocationRepository.Value.GetAsync(Id,memberId.Value);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertMemberLocationViewModel model)
        {
            try
            {
                var area = _memberLocationRepository.Value.Insert(new MemberLocationServiceModel
                {
                    address=model.address,
                    areaId=model.areaId,
                    location=model.location,
                    memberId=memberId.Value,
                    name=model.name,
                    phone=model.phone,
                    postalCode=model.postalCode,
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
        public async Task<IActionResult> Update([FromBody]UpdateMemberLocationViewModel model)
        {
            _memberLocationRepository.Value.Update(new MemberLocationServiceModel
            {
                memberLocationId=model.memberLocationId,
                address = model.address,
                areaId = model.areaId,
                location = model.location,
                memberId = memberId.Value,
                name = model.name,
                phone = model.phone,
                postalCode = model.postalCode,
            },memberId.Value);
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _memberLocationRepository.Value.DeleteAsync(Id,memberId.Value);
            return NoContent();
        }

        //[HttpGet, Route("filter"), Authorize]
        //public async Task<IActionResult> Filter(string name = null, int? page = null, int? rowsPerPage = null)
        //{
        //    var result = await _memberLocationRepository.Value.GetFilterAsync(name, page, rowsPerPage);
        //    return Ok(new { result.list, result.allRows });
        //}
    }
}