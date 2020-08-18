using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Extensions;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiUser.Controllers
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

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(MemberListViewModel model)
        {
            model = await _memberRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _memberRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
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
                    description=model.description
                });
                await _uow.SaveAsync();

                return Ok(new { member.memberId });

            }
            catch
            {
                return BadRequest();
            }          
        }

        [HttpPut, Route(""), Authorize]
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
                description = model.description
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _memberRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _memberRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }
    }
}