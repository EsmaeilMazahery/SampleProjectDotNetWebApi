using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Extensions;
using SP.ServiceLayer.Services;
using SP.WebApiMember.Controllers;
using SP.WebApiMember.Extension;
using SP.WebApiMember.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace SP.WebApiMember.Controllers
{
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : BaseApiController
    {
        public AuthController(IUnitOfWork uow,
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

        //login
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var member = _memberRepository.Value.Read(model.username.TrimStart('0')).SingleOrDefault();

            if (member != null && IdentityCryptography.VerifyHashedPassword(member.password, model.password))
            {

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.MobilePhone, member.mobile),
                         new Claim(ClaimTypes.Name, member.memberId.ToString()),
                         new Claim(ClaimTypes.Expiration, DateTime.Now.AddMonths(12).ToString())
                    };

                var tokeOptions = new JwtSecurityToken(
                    null, null,
                    claims: claims,
                    expires: DateTime.Now.AddMonths(12),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new
                {
                    token = tokenString,
                    member.email,
                    member.mobile,
                    member.image,
                    member.name,
                    member.family
                });
            }
            else
            {
                return Unauthorized();
            }
        }


        //register
        [HttpGet, Route("checkMobileTaken")]
        public IActionResult checkMobileTaken(string mobile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(_memberRepository.Value.checkMobileTaken(mobile.TrimStart('0')));
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

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }



        [HttpPost, Route("changeTokenFireBase"), Authorize]
        public IActionResult ChangeToken([FromBody]ChangeTokenModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var member = _memberRepository.Value.AsQueryable().Where(w=>w.memberId == memberId.Value).FirstOrDefault();

            if (member != null)
            {
                member.tokenFireBase = model.token;
                _uow.Save();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet, Route("checkEmailTaken")]
        public IActionResult checkEmailTaken(string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(_memberRepository.Value.checkEmailTaken(email));
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

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }


        [HttpPost, Authorize, Route("changeImage")]
        public IActionResult changeImage([FromBody]ChangeImageModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            _memberRepository.Value.changeImage(memberId.Value, model.address);

            return Ok();
        }

        [HttpGet, Authorize, Route("profile")]
        public IActionResult profile()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var member = _memberRepository.Value.Read(memberId.Value);
                    ProfileModel profile = new ProfileModel()
                    {
                        email = member.email,
                        family = member.family,
                        name = member.name,
                        image = member.image,
                        mobile = member.mobile
                    };
                    return Ok(profile);
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
