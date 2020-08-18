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
    [Route("api/brand")]
    public class BrandController : BaseApiController
    {

        Lazy<IBrandRepository> _brandRepository;

        public BrandController(IUnitOfWork uow, Lazy<IBrandRepository> BrandRepository) : base(uow)
        {
            _brandRepository = BrandRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(BrandListViewModel model)
        {
            model = await _brandRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _brandRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertBrandViewModel model)
        {
            try
            {
                var brand = _brandRepository.Value.Insert(new BrandServiceModel
                {
                     name=model.name,
                     image=model.image,
                });
                await _uow.SaveAsync();

                return Ok(new { brand.brandId });

            }
            catch
            {
                return BadRequest();
            }          
        }



        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateBrandViewModel model)
        {
            _brandRepository.Value.Update(new BrandServiceModel
            {
                name=model.name,
                image=model.image,
                brandId=model.brandId
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _brandRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _brandRepository.Value.GetFilterAsync(name, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }
    }
}