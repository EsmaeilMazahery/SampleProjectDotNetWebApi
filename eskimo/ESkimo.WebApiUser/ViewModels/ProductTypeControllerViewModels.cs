using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertProductTypeViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }
    }

    public class UpdateProductTypeViewModel
    {
        public int productTypeId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }
    }
}
