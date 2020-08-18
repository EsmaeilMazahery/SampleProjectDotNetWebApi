using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.DomainLayer.ViewModel;
using SP.Infrastructure.Extensions;
using SP.ServiceLayer.Services;
using SP.WebApiMember.Controllers;
using SP.WebApiMember.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace SP.WebApiMember.Controllers
{
    [Produces("application/json")]
    public class MediaController : BaseApiController
    {
        public MediaController(IUnitOfWork uow,
        Lazy<IServiceRepository> ServiceRepository,
            Lazy<ILogUserRepository> LogUserRepository,
            Lazy<IMediaRepository> MediaRepository,
            Lazy<IMemberRepository> MemberRepository,
            Lazy<ISmsRepository> SmsRepository,
            Lazy<ICallRepository> CallRepository,
            IDistributedCache distributedCache
        ) : base(uow,
            ServiceRepository,
            LogUserRepository,
            MediaRepository,
            MemberRepository,
            SmsRepository,
            CallRepository,
            distributedCache)
        {
        }

        [HttpPost, Authorize, Route("api/media")]
        public IActionResult register([FromBody]Media model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var service = _mediaRepository.Value.Register(model);

                    _uow.Save();

                    return Ok(new { MediaId = service.mediaId });
                }
                else
                {
                    return BadRequest("err");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key row in object 'dbo.Services' with unique index"))
                    {

                    }
                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }
    }
}
