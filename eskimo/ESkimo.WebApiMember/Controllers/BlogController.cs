using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiMember.Extension;
using ESkimo.WebApiMember.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiMember.Controllers
{
    [Produces("application/json")]
    [Route("api/blog")]
    public class BlogController : BaseApiController
    {
        Lazy<IBlogPostRepository> _blogPostRepository;
        Lazy<IBlogCategoryRepository> _blogCategoryRepository;
        Lazy<IBlogCommentRepository> _blogCommentRepository;

        public BlogController(IServiceProvider serviceProvider, IUnitOfWork uow,
            Lazy<IBlogCategoryRepository> BlogCategoryRepository,
            Lazy<IBlogCommentRepository> BlogCommentRepository,
            Lazy<IBlogPostRepository> BlogPostRepository) : base(serviceProvider, uow)
        {
            _blogPostRepository = BlogPostRepository;
            _blogCategoryRepository = BlogCategoryRepository;
            _blogCommentRepository = BlogCommentRepository;
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> Index(BlogPostListViewModel model)
        {
            var result = await _blogPostRepository.Value.GetAllMemberAsync(model);
            return Ok(result);
        }

        [HttpGet, Route("{Id}"), AllowAnonymous]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _blogPostRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost]
        [Route("{id}/registerComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post_BlogComment_Register(int id, RegisterBlogCommentViewModel model)
        {
            CaptchaModel obj = JwtHandler.DecodeToken<CaptchaModel>(model.captchaToken);

            if (string.IsNullOrEmpty(model.memberMobile) && string.IsNullOrEmpty(model.memberEmail))
            {
                return BadRequest("موبایل یا ایمیل را وارد کنید");
            }

            if (obj.des != "blogComment" || obj.verifyCode.ToString() != model.captchaVerify)
            {
                return BadRequest("مجموع اشتباه است");
            }

            var comment = _blogCommentRepository.Value.Insert(new BlogCommentServiceModel()
            {
                blogPostId = id,
                body = model.body,
                enable = true,
                memberEmail = model.memberEmail,
                memberMobile = model.memberMobile,
                memberName = model.memberName,
                registerDate = DateTime.Now,
                memberId = memberId,
            });

            // save
            await _uow.SaveAsync();

            // done
            return Ok(new { comment.blogCommentId });
        }
    }
}