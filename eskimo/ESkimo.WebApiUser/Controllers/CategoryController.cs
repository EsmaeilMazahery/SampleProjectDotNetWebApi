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
    [Route("api/category")]
    public class CategoryController : BaseApiController
    {

        Lazy<ICategoryRepository> _categoryRepository;

        public CategoryController(IUnitOfWork uow, Lazy<ICategoryRepository> CategoryRepository) : base(uow)
        {
            _categoryRepository = CategoryRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(CategoryListViewModel model)
        {
            model = await _categoryRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _categoryRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertCategoryViewModel model)
        {
            try
            {
                var category = _categoryRepository.Value.Insert(new CategoryServiceModel
                {
                    name = model.name,
                    parentId = model.parentId
                });
                await _uow.SaveAsync();

                return Ok(new { category.categoryId });

            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateCategoryViewModel model)
        {
            _categoryRepository.Value.Update(new CategoryServiceModel
            {
                categoryId=model.categoryId,
                name = model.name,
                parentId = model.parentId
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _categoryRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name=null,int? page=null,int? rowsPerPage=null)
        {
            var result = await _categoryRepository.Value.GetFilterAsync(name, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }

    }
}