using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiUser.Controllers
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

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Get()
        {
            var colection = await _settingRepository.Value.read(
                SettingType.ZarinPalMerchent,
                SettingType.CustomerSite,
                SettingType.SmsApiKey,
                SettingType.SmsSender,
                SettingType.DescriptionPayment
                );

            UpdateSettingViewModel viewModel = new UpdateSettingViewModel()
            {
                zarinPalMerchent = colection[SettingType.ZarinPalMerchent]?.Value,
                customerSite = colection[SettingType.CustomerSite]?.Value,
                smsApiKey = colection[SettingType.SmsApiKey]?.Value,
                smsSender = colection[SettingType.SmsSender]?.Value,
                descriptionPayment = colection[SettingType.DescriptionPayment]?.Value,
            };

            return Ok(viewModel);
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateSettingViewModel model)
        {
            SettingCollection collection = new SettingCollection() {
                    { SettingType.ZarinPalMerchent,model.zarinPalMerchent },
                    { SettingType.CustomerSite,model.customerSite },
                    { SettingType.SmsApiKey,model.smsApiKey },
                    { SettingType.SmsSender,model.smsSender },
                    { SettingType.DescriptionPayment,model.descriptionPayment },
                };

            await _settingRepository.Value.write(collection);
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpGet, Route("banner"), Authorize]
        public async Task<IActionResult> GetBanner()
        {
            var colection = await _settingRepository.Value.read(
                SettingType.BannerImage,
                SettingType.BannerLink,
                SettingType.BannerImage1,
                SettingType.BannerLink1,
                SettingType.BannerImage2,
                SettingType.BannerLink2
                );

            UpdateBannerViewModel viewModel = new UpdateBannerViewModel()
            {
                bannerImage = colection[SettingType.BannerImage]?.Value,
                bannerLink = colection[SettingType.BannerLink]?.Value,

                bannerImage1 = colection[SettingType.BannerImage1]?.Value,
                bannerLink1 = colection[SettingType.BannerLink1]?.Value,

                bannerImage2 = colection[SettingType.BannerImage2]?.Value,
                bannerLink2 = colection[SettingType.BannerLink2]?.Value,
            };

            return Ok(viewModel);
        }

        [HttpPut, Route("banner"), Authorize]
        public async Task<IActionResult> UpdateBanner([FromBody]UpdateBannerViewModel model)
        {
            SettingCollection collection = new SettingCollection() {
                    { SettingType.BannerImage,model.bannerImage },
                    { SettingType.BannerLink,model.bannerLink },

                     { SettingType.BannerImage1,model.bannerImage1 },
                    { SettingType.BannerLink1,model.bannerLink1 },

                     { SettingType.BannerImage2,model.bannerImage2 },
                    { SettingType.BannerLink2,model.bannerLink2 },
                };

            await _settingRepository.Value.write(collection);
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpGet, Route("clip"), Authorize]
        public async Task<IActionResult> GetClip()
        {
            var colection = await _settingRepository.Value.read(
                SettingType.ClipLink,
                SettingType.ClipTitle,
                SettingType.ClipVideo
                );

            UpdateClipViewModel viewModel = new UpdateClipViewModel()
            {
                clipLink = colection[SettingType.ClipLink]?.Value,
                clipTitle = colection[SettingType.ClipTitle]?.Value,
                clipVideo = colection[SettingType.ClipVideo]?.Value,
            };

            return Ok(viewModel);
        }

        [HttpPut, Route("clip"), Authorize]
        public async Task<IActionResult> UpdateClip([FromBody]UpdateClipViewModel model)
        {
            SettingCollection collection = new SettingCollection() {
                    { SettingType.ClipVideo,model.clipVideo },
                    { SettingType.ClipTitle,model.clipTitle },
                     { SettingType.ClipLink,model.clipLink },
                };

            await _settingRepository.Value.write(collection);
            await _uow.SaveAsync();

            return NoContent();
        }


    }
}