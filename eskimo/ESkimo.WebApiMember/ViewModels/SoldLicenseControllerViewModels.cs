using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.ViewModels
{
    public class InsertSoldLicenseViewModel
    {

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public decimal price { set; get; }

        public int day { set; get; }

        public int count { set; get; }
    }

    public class UpdateSoldLicenseViewModel
    {
        public int soldLicenseId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public decimal price { set; get; }

        public int day { set; get; }

        public int count { set; get; }
    }
}
