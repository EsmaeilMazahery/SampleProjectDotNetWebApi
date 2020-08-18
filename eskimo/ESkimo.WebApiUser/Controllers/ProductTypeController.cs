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
    [Route("api/productType")]
    public class ProductTypeController : BaseApiController
    {

        Lazy<IProductTypeRepository> _productTypeRepository;

        public ProductTypeController(IUnitOfWork uow, Lazy<IProductTypeRepository> ProductTypeRepository) : base(uow)
        {
            _productTypeRepository = ProductTypeRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(ProductTypeListViewModel model)
        {
            model = await _productTypeRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _productTypeRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertProductTypeViewModel model)
        {
            try
            {
                var digiType = _productTypeRepository.Value.Insert(new ProductTypeServiceModel
                {
                    name = model.name,
                });
                await _uow.SaveAsync();

                return Ok(new { digiType.productTypeId });
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateProductTypeViewModel model)
        {
            _productTypeRepository.Value.Update(new ProductTypeServiceModel
            {
                productTypeId = model.productTypeId,
                name = model.name,
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _productTypeRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _productTypeRepository.Value.GetFilterAsync(name, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }
    }
}