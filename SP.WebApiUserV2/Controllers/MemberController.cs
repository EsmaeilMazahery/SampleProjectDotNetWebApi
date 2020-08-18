using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using SP.DomainLayer.ViewModel;
using SP.ServiceLayer.Services;
using SP.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SP.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/member")]
    public class MemberController : BaseApiController
    {

        Lazy<IMemberRepository> _memberRepository;

        public MemberController(IUnitOfWork uow, Lazy<IMemberRepository> MemberRepository) : base(uow)
        {
            _memberRepository = MemberRepository;
        }

        [HttpGet, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Index(MemberListViewModel model)
        {
            model = await _memberRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize(Roles = "1")]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _memberRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Insert([FromBody]InsertMemberViewModel model)
        {
            try
            {
                var member = _memberRepository.Value.Insert(new MemberServiceModel
                {
                    email = model.email,
                    enable = model.enable,
                    family = model.family,
                    mobile = model.mobile,
                    name = model.name,
                    password = model.password,
                    registerDate = DateTime.Now,
                    username = model.username
                });
                await _uow.SaveAsync();

                return Ok(new { member.memberId });

            }
            catch(Exception ex)
            {
                return BadRequest();
            }          
        }

        [HttpPatch, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Update([FromBody] UpdateMemberViewModel model)
        {
            _memberRepository.Value.Update(new MemberServiceModel
            {
                memberId= model.memberId,
                email = model.email,
                enable = model.enable,
                family = model.family,
                mobile = model.mobile,
                name = model.name,
                password = model.password,
                registerDate = DateTime.Now,
                username = model.username,
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _memberRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize(Roles = "1")]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _memberRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }

        //[HttpGet, Route("filterPagging"), Authorize]
        //public async Task<IActionResult> Filter(ResellerFilterListViewModel model)
        //{
        //    var tuple = await _resellerRepository.Value.GetFilterAsync(model);
        //    if (tuple.Item2 == "")
        //        return Ok(tuple);
        //    else
        //        return NotFound(tuple.Item2);
        //}


    }
}