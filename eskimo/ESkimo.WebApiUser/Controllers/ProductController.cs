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
    [Route("api/product")]
    public class ProductController : BaseApiController
    {

        Lazy<IProductRepository> _productRepository;

        public ProductController(IUnitOfWork uow, Lazy<IProductRepository> ProductRepository) : base(uow)
        {
            _productRepository = ProductRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(ProductListViewModel model)
        {
            model = await _productRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _productRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertProductViewModel model)
        {
            try
            {
                var product = _productRepository.Value.Insert(new ProductServiceModel
                {
                    brandId = model.brandId,
                    categoryId = model.categoryId,
                    name = model.name,
                    amountBase=model.amountBase,
                    productTypeId =model.productTypeId,
                    attributes=model.attributes,
                    description=model.description,
                    enable=model.enable,
                    imageAddress=model.imageAddress,
                    productPrices=model.productPrices,
                    productPriceWholesales= model.productPriceWholesales
                });
                await _uow.SaveAsync();

                return Ok(new { product.productId });

            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateProductViewModel model)
        {
            _productRepository.Value.Update(new ProductServiceModel
            {
                productId = model.productId,
                brandId = model.brandId,
                categoryId = model.categoryId,
                name = model.name,
                amountBase=model.amountBase,
                productTypeId = model.productTypeId,
                attributes = model.attributes,
                description = model.description,
                enable = model.enable,
                imageAddress = model.imageAddress,
                productPrices = model.productPrices,
                productPriceWholesales = model.productPriceWholesales

            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _productRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _productRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }
    }
}