using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertCategoryViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        public int? parentId { set; get; }
    }

    public class UpdateCategoryViewModel
    {
        public int categoryId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        public int? parentId { set; get; }
    }
}
