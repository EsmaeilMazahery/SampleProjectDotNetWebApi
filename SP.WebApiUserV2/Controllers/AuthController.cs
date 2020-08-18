using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Extensions;
using SP.ServiceLayer.Services;
using SP.WebApiUser.Extension;
using SP.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SP.WebApiUser.Controllers
{
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : BaseApiController
    {
        Lazy<IUserRepository> _userRepository;
        Lazy<IRoleRepository> _roleRepository;
        public AuthController(IUnitOfWork uow, Lazy<IUserRepository> UserRepository, Lazy<IRoleRepository> RoleRepository) : base(uow)
        {
            _userRepository = UserRepository;
            _roleRepository = RoleRepository;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var user = await _userRepository.Value.GetAsync(model.username);

            if (user != null && IdentityCryptography.VerifyHashedPassword(user.password, model.password))
            {
                if (!user.enable)
                    return Unauthorized();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {

                        new Claim(ClaimTypes.MobilePhone, user.mobile),
                         new Claim(ClaimTypes.Name, user.userId.ToString()),
                         new Claim(ClaimTypes.Role, string.Join(",",user.selectedRoles))
                    };

                var tokeOptions = new JwtSecurityToken(null, null,
                    claims: claims,
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new
                {
                    Token = tokenString,
                    TokenExpirationTime = ((DateTimeOffset)DateTime.Now.AddHours(12)).ToUnixTimeSeconds(),
                    Id = user.userId
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet, Route("roles"), Authorize]
        public async Task<IActionResult> Roles()
        {
            try
            {
                var accessRoles = HttpContext.User.Claims
                 .Where(w => w.Type == ClaimTypes.Role).FirstOrDefault().Value.Split(",")
                 .Select(s =>
                 {
                     Enum.TryParse(s, out RolesKey myStatus);
                     return myStatus;
                 });

                return Ok(Enum.GetValues(typeof(RolesKey)).Cast<RolesKey>().ToList().Select(s => new
                {
                    roleId = s,
                    access = accessRoles.Contains(s),
                    name = s.GetDescriptionOrDefault()
                }));
            }
            catch
            {
                return Unauthorized();
            }
        }




    }
}