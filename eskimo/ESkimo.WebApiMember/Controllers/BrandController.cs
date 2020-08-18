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
    [Route("api/brand")]
    //[ApiVersion("1.0")]
    //[Route("api/v{version:apiVersion}/[Controller]")]
    public class BrandController : BaseApiController
    {

        Lazy<IBrandRepository> _brandRepository;

        public BrandController(IServiceProvider serviceProvider, IUnitOfWork uow, Lazy<IBrandRepository> BrandRepository) : base(serviceProvider, uow)
        {
            _brandRepository = BrandRepository;
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> Index()
        {
           var model = await _brandRepository.Value.GetAllMemberAsync();
            return Ok(model);
        }

        [HttpGet, Route("priceTable"), AllowAnonymous]
        public async Task<IActionResult> PriceTable()
        {
           var model = await _brandRepository.Value.GetPriceTableAsync();
            return Ok(model);
        }

        [HttpGet, Route("wholeSale"), AllowAnonymous]
        public async Task<IActionResult> WholeSale()
        {
            var model = await _brandRepository.Value.GetWholeSaleAsync();
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), AllowAnonymous]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _brandRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpGet, Route("filter"), AllowAnonymous]
        public async Task<IActionResult> Filter(string name = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _brandRepository.Value.GetFilterAsync(name, page, rowsPerPage);
            return Ok(new { result.list, result.allRows });
        }
    }
}