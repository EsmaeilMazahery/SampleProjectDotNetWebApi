using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertColorViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.ColorLength)]
        [RegularExpression(ConstantValidations.ColorRegEx)]
        public string colorCode { set; get; }
    }

    public class UpdateColorViewModel
    {
        public int colorId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.ColorLength)]
        [RegularExpression(ConstantValidations.ColorRegEx)]
        public string colorCode { set; get; }
    }
}
