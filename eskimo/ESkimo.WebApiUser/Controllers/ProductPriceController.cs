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
    [Route("api/ProductPrice")]
    public class ProductPriceController : BaseApiController
    {

        Lazy<IProductPriceRepository> _productPriceRepository;

        public ProductPriceController(IUnitOfWork uow, Lazy<IProductPriceRepository> ProductPriceRepository) : base(uow)
        {
            _productPriceRepository = ProductPriceRepository;
        }

        [HttpGet, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Index(ProductPriceListViewModel model)
        {
            model = await _productPriceRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize(Roles = "1")]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _productPriceRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Insert([FromBody]InsertProductPriceViewModel model)
        {
            try
            {
                var productPrice = _productPriceRepository.Value.Insert(new ProductPriceServiceModel
                {
                    enable = model.enable,
                    productId = model.productId,
                });
                await _uow.SaveAsync();

                return Ok(new { productPrice.productPriceId });

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Update([FromBody]UpdateProductPriceViewModel model)
        {
            _productPriceRepository.Value.Update(new ProductPriceServiceModel
            {
                productPriceId=model.productPriceId,
                enable = model.enable,
                productId = model.productId
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _productPriceRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize(Roles = "1")]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _productPriceRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }


    }
}