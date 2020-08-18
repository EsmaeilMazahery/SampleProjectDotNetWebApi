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
    [Route("api/blogCategory")]
    public class BlogCategoryController : BaseApiController
    {

        Lazy<IBlogCategoryRepository> _blogCategoryRepository;

        public BlogCategoryController(IUnitOfWork uow, Lazy<IBlogCategoryRepository> BlogCategoryRepository) : base(uow)
        {
            _blogCategoryRepository = BlogCategoryRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(BlogCategoryListViewModel model)
        {
            model = await _blogCategoryRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _blogCategoryRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertBlogCategoryViewModel model)
        {
            try
            {
                var blogCategory = _blogCategoryRepository.Value.Insert(new BlogCategoryServiceModel
                {
                    description = model.description,
                    title = model.title,
                    parentId = model.parentId,
                    enable=model.enable
                });
                await _uow.SaveAsync();

                return Ok(new { blogCategory.blogCategoryId });
            }
            catch
            {
                return BadRequest();
            }
        }



        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateBlogCategoryViewModel model)
        {
            _blogCategoryRepository.Value.Update(new BlogCategoryServiceModel
            {
                blogCategoryId=model.blogCategoryId,
                description = model.description,
                title = model.title,
                parentId = model.parentId,
                enable=model.enable
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _blogCategoryRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string title = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _blogCategoryRepository.Value.GetFilterAsync(title, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }
    }
}