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
    [Route("api/periodType")]
    public class PeriodTypeController : BaseApiController
    {

        Lazy<IPeriodTypeRepository> _periodTypeRepository;

        public PeriodTypeController(IServiceProvider serviceProvider, IUnitOfWork uow, Lazy<IPeriodTypeRepository> PeriodTypeRepository) : base(serviceProvider, uow)
        {
            _periodTypeRepository = PeriodTypeRepository;
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            PeriodTypeListViewModel model = await _periodTypeRepository.Value.GetAllMemberAsync();
            return Ok(model);
        }



    }
}