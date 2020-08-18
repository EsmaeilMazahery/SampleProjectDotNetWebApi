using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertBrandViewModel
    {
        [Required]
        public int digiCode { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }
    }

    public class UpdateBrandViewModel
    {
        public int brandId { set; get; }

        [Required]
        public int digiCode { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }


        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }
    }
}
