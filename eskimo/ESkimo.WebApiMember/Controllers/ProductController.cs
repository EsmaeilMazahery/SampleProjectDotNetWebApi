using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiMember.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiMember.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : BaseApiController
    {

        Lazy<IProductRepository> _productRepository;

        public ProductController(IServiceProvider serviceProvider, IUnitOfWork uow, Lazy<IProductRepository> ProductRepository) : base(serviceProvider, uow)
        {
            _productRepository = ProductRepository;
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery]ProductListViewModel model)
        {
            model = await _productRepository.Value.GetAllMemberAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("wholesale"), AllowAnonymous]
        public async Task<IActionResult> IndexWholesale([FromQuery]ProductListViewModel model)
        {
            model = await _productRepository.Value.GetAllMemberWholesaleAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), AllowAnonymous]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _productRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpGet, Route("filter"), AllowAnonymous]
        public async Task<IActionResult> Filter(string name)
        {
            var query = await _productRepository.Value.GetFilterAsync(name);
            return Ok(new { List = query.ToList() });
        }
    }
}