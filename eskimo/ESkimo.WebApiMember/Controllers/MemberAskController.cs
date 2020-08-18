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

namespace ESkimo.WebApiMember.Controllers
{
    [Produces("application/json")]
    [Route("api/memberAsk")]
    public class MemberAskController : BaseApiController
    {
        Lazy<IMemberAskRepository> _memberAskRepository;

        public MemberAskController(IServiceProvider serviceProvider, IUnitOfWork uow, Lazy<IMemberAskRepository> MemberAskRepository) : base(serviceProvider, uow)
        {
            _memberAskRepository = MemberAskRepository;
        }

        [HttpPost, Route(""), AllowAnonymous]
        public async Task<IActionResult> Insert([FromBody]InsertMemberAskViewModel model)
        {
            try
            {
                var memberAsk = _memberAskRepository.Value.Insert(new MemberAskServiceModel
                {
                    name = model.name,
                    family = model.family,
                    description = model.description,
                    email = model.email,
                    mobile = model.mobile,
                    registerDate = DateTime.Now,
                    type = model.type,
                });
                await _uow.SaveAsync();

                return Ok(new { memberAsk.memberAskId });

            }
            catch
            {
                return BadRequest();
            }
        }
    }
}