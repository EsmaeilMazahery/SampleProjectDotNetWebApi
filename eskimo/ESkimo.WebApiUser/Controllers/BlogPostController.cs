using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/blogPost")]
    public class BlogPostController : BaseApiController
    {

        Lazy<IBlogPostRepository> _blogPostRepository;

        public BlogPostController(IUnitOfWork uow, Lazy<IBlogPostRepository> BlogPostRepository) : base(uow)
        {
            _blogPostRepository = BlogPostRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(BlogPostListViewModel model)
        {
            model = await _blogPostRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _blogPostRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertBlogPostViewModel model)
        {
            try
            {
                var blogPost = _blogPostRepository.Value.Insert(new BlogPostServiceModel
                {
                    title = model.title,
                    image = model.image,
                    publishDate = model.publishDate,
                    url = model.url,
                    content = model.content,
                    enableComment = model.enableComment,
                    blogCategoryId = model.blogCategoryId,
                    registerDateTime = DateTime.Now,
                    enable = model.enable,
                    userId = userId.Value,
                });
                await _uow.SaveAsync();

                return Ok(new { blogPost.blogPostId });

            }
            catch
            {
                return BadRequest();
            }          
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateBlogPostViewModel model)
        {
            _blogPostRepository.Value.Update(new BlogPostServiceModel
            {
                title = model.title,
                image = model.image,
                publishDate = model.publishDate,
                url = model.url,
                content = model.content,
                enableComment = model.enableComment,
                blogCategoryId = model.blogCategoryId,
                registerDateTime = DateTime.Now,
                enable = model.enable,
                blogPostId =model.blogPostId
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _blogPostRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _blogPostRepository.Value.GetFilterAsync(name, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }
    }
}