using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using SP.DomainLayer.ViewModel;
using SP.Infrastructure.Enumerations;
using SP.ServiceLayer.Services;
using SP.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SP.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/setting")]
    public class SettingController : BaseApiController
    {
        Lazy<ISettingRepository> _settingRepository;

        public SettingController(IUnitOfWork uow, Lazy<ISettingRepository> SettingRepository) : base(uow)
        {
            _settingRepository = SettingRepository;
        }

        [HttpGet, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Get()
        {
            var colection = await _settingRepository.Value.read(
                SettingType.ZarinPalMerchent,
                SettingType.CustomerSite
                );

            UpdateSettingViewModel viewModel = new UpdateSettingViewModel()
            {
                zarinPalMerchent = colection[SettingType.ZarinPalMerchent]?.Value,
                customerSite= colection[SettingType.CustomerSite]?.Value,
            };

            return Ok(viewModel);
        }

        [HttpPut, Route(""), Authorize(Roles = "1")]
        public async Task<IActionResult> Update([FromBody]UpdateSettingViewModel model)
        {
            SettingCollection collection = new SettingCollection() {
                    { SettingType.ZarinPalMerchent,model.zarinPalMerchent },
                    { SettingType.CustomerSite,model.customerSite },
                };

            await _settingRepository.Value.write(collection);
            await _uow.SaveAsync();

            return NoContent();
        }
    }
}