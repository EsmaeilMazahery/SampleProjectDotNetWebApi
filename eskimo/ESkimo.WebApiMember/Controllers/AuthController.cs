using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.Infrastructure.Extensions;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiMember.Extension;
using ESkimo.WebApiMember.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;

namespace ESkimo.WebApiMember.Controllers
{
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : BaseApiController
    {
        Lazy<IMemberRepository> _memberRepository;
        Lazy<ISmsRepository> _smsRepository;
        public AuthController(IServiceProvider serviceProvider,
            IUnitOfWork uow,
            Lazy<IMemberRepository> MemberRepository,
            Lazy<ISmsRepository> SmsRepository
            ) : base(serviceProvider, uow)
        {
            _memberRepository = MemberRepository;
            _smsRepository = SmsRepository;
        }

        [HttpPost, Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var member = await _memberRepository.Value.GetAsync(model.username.TrimStart("0"));

            if (member != null && IdentityCryptography.VerifyHashedPassword(member.password, model.password))
            {
                if (!member.enable)
                    return Unauthorized();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.MobilePhone, member.mobile.TrimStart("0")),
                         new Claim(ClaimTypes.Name, member.memberId.ToString()),
                    };

                var tokeOptions = new JwtSecurityToken(null, null,
                    claims: claims,
                    expires: DateTime.Now.AddDays(365),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new
                {
                    Token = tokenString,
                    TokenExpirationTime = ((DateTimeOffset)DateTime.Now.AddDays(365)).ToUnixTimeSeconds(),
                    Id = member.memberId,
                    member.name,
                    member.family,
                    mobile = member.mobile.TrimStart("0"),
                    member.amount,
                    member.verifyMobile
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet, Route("get"), Authorize]
        public async Task<IActionResult> Get()
        {
            var member = await _memberRepository.Value.GetAsync(memberId.Value);
            return Ok(member);
        }

        [HttpPost, Route("register"), Authorize]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            try
            {
                _memberRepository.Value.Update(new MemberServiceModel
                {
                    memberId = memberId.Value,
                    email = model.email,
                    family = model.family,
                    //mobile = model.mobile,
                    name = model.name,
                    password = model.password,
                    description = model.description,
                    location = model.location,
                    address = model.address,
                    areaId = model.areaId
                });
                await _uow.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                if ((ex.InnerException?.Message ?? "").Contains("UI_Mobile"))
                {
                    return BadRequest("UI_Mobile");
                }

                return BadRequest();
            }
        }

        [HttpGet, Route("resendVerify"), Authorize]
        public async Task<IActionResult> resendVerifyMember(string hash)
        {
            //int? CountVerifyCode = HttpContext.Session.GetInt32("CountVerifyCode");

            string verifyCode = new Random().Next(111111, 999999).ToString();
            //سشن جواب نمیدهد چون سشن ای دی در کوکی ذخیره می شود و کوکی نداریم
            //HttpContext.Session.SetString("verifyCode", tokenMobile);
            var status = _smsRepository.Value.Verify(verifyCode, mobilePhone, SmsTemplate.Register, memberId: memberId.Value);

            if (status == SmsStatus.NotValid)
                return BadRequest("تعداد مجاز ارسال پیامک تمام شده است");

            if (status == SmsStatus.Err)
                return BadRequest("خطایی در ارسال پیامک رخ داد");

            var token = JwtHandler.EncodeToken(new SmsVerifyModel
            {
                verifyCode = verifyCode,
                expireDate = DateTime.Now.AddMinutes(2)
            });

            return Ok(new { token });
        }

        [HttpPost, Route("verify"), Authorize]
        public async Task<IActionResult> VerifyMember([FromBody]VerifyMobileModel model)
        {
            //string verifyCode = HttpContext.Session.GetString("verifyCode");
            SmsVerifyModel obj = JwtHandler.DecodeToken<SmsVerifyModel>(model.hash);
            if (obj.verifyCode == model.verifyCode && obj.expireDate > DateTime.Now)
            {
                await _memberRepository.Value.AsQueryable()
                     .Where(w => w.memberId == memberId.Value)
                     .UpdateFromQueryAsync(u => new Member()
                     {
                         verifyMobile = true
                     });

                return NoContent();
            }
            else
            {
                return BadRequest("mistake");
            }
        }

        [HttpGet, Route("checkVerify"), Authorize]
        public async Task<IActionResult> checkVerifyMember()
        {
            var status = _memberRepository.Value.AsQueryable()
                     .Where(w => w.memberId == memberId.Value)
                     .Select(s => s.verifyMobile).FirstOrDefault();
            return Ok(new
            {
                status
            });
        }

        [HttpPost, Route("fastRegister"), AllowAnonymous]
        public async Task<IActionResult> FastRegister([FromBody]FastRegisterModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid client request");
                }

                string password = new Random().Next(111111, 999999).ToString();

                var member = _memberRepository.Value.Insert(new DomainLayer.ViewModel.MemberServiceModel()
                {
                    mobile = model.mobile.TrimStart("0"),
                    password = password,
                    registerDate = DateTime.Now,
                });
                await _uow.SaveAsync();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.MobilePhone, member.mobile.TrimStart("0")),
                         new Claim(ClaimTypes.Name, member.memberId.ToString()),
                    };

                var tokeOptions = new JwtSecurityToken(null, null,
                    claims: claims,
                    expires: DateTime.Now.AddDays(365),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                //string verifyCode = new Random().Next(111111, 999999).ToString();
                //سشن جواب نمیدهد چون سشن ای دی در کوکی ذخیره می شود و کوکی نداریم
                //HttpContext.Session.SetString("verifyCode", tokenMobile);

                string token = null;

                var status = _smsRepository.Value.Verify(password, model.mobile.TrimStart("0"), SmsTemplate.Register, memberId: member.memberId);
                if (status == SmsStatus.Successful)
                    token = JwtHandler.EncodeToken(new SmsVerifyModel
                    {
                        verifyCode = password,
                        expireDate = DateTime.Now.AddMinutes(2)
                    });

                return Ok(new
                {
                    Token = tokenString,
                    TokenExpirationTime = ((DateTimeOffset)DateTime.Now.AddDays(365)).ToUnixTimeSeconds(),
                    Id = member.memberId,
                    member.name,
                    member.family,
                    member.amount,
                    mobile = member.mobile.TrimStart("0"),
                    member.verifyMobile,
                    verifyMobileHash = token
                });
            }
            catch (Exception ex)
            {
                if ((ex.InnerException?.Message ?? "").Contains("UI_Mobile"))
                {
                    return BadRequest("UI_Mobile");
                }

                return BadRequest();
            }
        }

        [HttpPost, Route("forgetPassword"), AllowAnonymous]
        public async Task<IActionResult> forgetPassword([FromBody]ForgetPasswordModel model)
        {
            //int? CountVerifyCode = HttpContext.Session.GetInt32("CountVerifyCode");

            var member = _memberRepository.Value.AsQueryable().Where(w => w.mobile == model.mobile.TrimStart("0")).FirstOrDefault();

            if (member == null)
                return BadRequest("مشترکی با این شماره موبایل وجود ندارد");

            string verifyCode = new Random().Next(111111, 999999).ToString();
            //سشن جواب نمیدهد چون سشن ای دی در کوکی ذخیره می شود و کوکی نداریم
            //HttpContext.Session.SetString("verifyCode", tokenMobile);
            var status = _smsRepository.Value.Verify(verifyCode, model.mobile.TrimStart("0"), SmsTemplate.Register);

            if (status == SmsStatus.NotValid)
                return BadRequest("تعداد مجاز ارسال پیامک تمام شده است");

            if (status == SmsStatus.Err)
                return BadRequest("خطایی در ارسال پیامک رخ داد");

            var token = JwtHandler.EncodeToken(new SmsVerifyModel
            {
                verifyCode = verifyCode,
                expireDate = DateTime.Now.AddMinutes(2),
                mobile = model.mobile.TrimStart("0"),
                memberId = member.memberId
            });

            return Ok(new { token });
        }

        [HttpPost, Route("changePassword"), AllowAnonymous]
        public async Task<IActionResult> changePassword([FromBody]ChangePasswordModel model)
        {
            //string verifyCode = HttpContext.Session.GetString("verifyCode");
            SmsVerifyModel obj = JwtHandler.DecodeToken<SmsVerifyModel>(model.hash);
            if (obj.verifyCode == model.verifyCode && obj.expireDate > DateTime.Now)
            {
                await _memberRepository.Value.AsQueryable()
                     .Where(w => w.memberId == obj.memberId)
                     .UpdateFromQueryAsync(u => new Member()
                     {
                         verifyMobile = true,
                         password = IdentityCryptography.HashPassword(model.password),
                     });
                await _uow.SaveAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("mistake");
            }
        }

        [HttpGet, Route("createCaptcha")]
        public async Task<IActionResult> createCaptcha(string des)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            //Session["Captcha" + prefix] = a + b;

            //image stream
            //FileContentResult img = null;
            string base64String;
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                int i, r, x, y;
                var pen = new Pen(Color.Yellow);
                for (i = 1; i < 10; i++)
                {
                    pen.Color = Color.FromArgb(
                    (rand.Next(0, 255)),
                    (rand.Next(0, 255)),
                    (rand.Next(0, 255)));

                    r = rand.Next(0, (130 / 3));
                    x = rand.Next(0, 130);
                    y = rand.Next(0, 30);

                    gfx.DrawEllipse(pen, x - r, y - r, r, r);
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                base64String = Convert.ToBase64String(mem.GetBuffer());

                var token = JwtHandler.EncodeToken(new CaptchaModel
                {
                    verifyCode = a + b,
                    base64String = base64String,
                    des= des,
                    expireDate = DateTime.Now.AddMinutes(2)
                });

                return Ok(new { token });
            }
        }
    }
}