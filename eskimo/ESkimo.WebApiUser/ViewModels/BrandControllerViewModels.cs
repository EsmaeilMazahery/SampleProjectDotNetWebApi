using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertBrandViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { set; get; }
    }

    public class UpdateBrandViewModel
    {
        public int brandId { set; get; }
        
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { set; get; }
    }
}
