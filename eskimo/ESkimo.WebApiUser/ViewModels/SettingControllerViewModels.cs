using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class UpdateSettingViewModel
    {
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string zarinPalMerchent { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string customerSite { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string smsApiKey { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string smsSender { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string descriptionPayment { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string banner { set; get; }
    }

    public class UpdateBannerViewModel
    {
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string bannerImage { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string bannerLink { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string bannerImage1 { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string bannerLink1 { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string bannerImage2 { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string bannerLink2 { set; get; }
    }

    public class UpdateClipViewModel
    {
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string clipLink { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string clipTitle { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string clipVideo { set; get; }
    }


}
