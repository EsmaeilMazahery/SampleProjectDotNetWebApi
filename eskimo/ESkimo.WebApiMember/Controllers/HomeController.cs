using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiMember.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiMember.Controllers
{
    [Produces("application/json")]
    [Route("api/home")]
    public class HomeController : BaseApiController
    {
        Lazy<IBrandRepository> _brandRepository;
        Lazy<ISettingRepository> _settingRepository;

        public HomeController(IServiceProvider serviceProvider, IUnitOfWork uow,
            Lazy<ISettingRepository> SettingRepository,
            Lazy<IBrandRepository> BrandRepository) : base(serviceProvider, uow)
        {
            _brandRepository = BrandRepository;
            _settingRepository = SettingRepository;
        }

        [HttpGet, Route(""), AllowAnonymous]
        public async Task<IActionResult> Home()
        {
            var model = await _brandRepository.Value.GetAllMemberAsync();
            string bannerImage = await _settingRepository.Value.read(SettingType.BannerImage);
            string bannerLink = await _settingRepository.Value.read(SettingType.BannerLink);

            string bannerImage1 = await _settingRepository.Value.read(SettingType.BannerImage1);
            string bannerLink1 = await _settingRepository.Value.read(SettingType.BannerLink1);

            string bannerImage2 = await _settingRepository.Value.read(SettingType.BannerImage2);
            string bannerLink2 = await _settingRepository.Value.read(SettingType.BannerLink2);

            string clipLink = await _settingRepository.Value.read(SettingType.ClipLink);
            string clipTitle = await _settingRepository.Value.read(SettingType.ClipTitle);
            string clipVideo = await _settingRepository.Value.read(SettingType.ClipVideo);

            return Ok(new
            {
                brands = model,
                banner = new
                {
                    bannerImage,
                    bannerLink,

                    bannerImage1,
                    bannerLink1,

                    bannerImage2,
                    bannerLink2,

                    clipLink,
                    clipTitle,
                    clipVideo
                }
            });
        }
    }
}