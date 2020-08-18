using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertPeriodTypeViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public int day { set; get; }

        public int month { set; get; }

        public decimal percentDiscount { set; get; }

        public decimal maxDiscount { set; get; }
    }

    public class UpdatePeriodTypeViewModel
    {
        public int periodTypeId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public int day { set; get; }

        public int month { set; get; }

        public decimal percentDiscount { set; get; }

        public decimal maxDiscount { set; get; }
    }
}
