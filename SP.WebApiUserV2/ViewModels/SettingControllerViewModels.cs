using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class UpdateSettingViewModel
    {
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string zarinPalMerchent { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string customerSite { set; get; }
    }
}
