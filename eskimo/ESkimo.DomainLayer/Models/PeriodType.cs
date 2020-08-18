using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class PeriodType
    {
        public int periodTypeId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public int day { set; get; }

        public int month { set; get; }

        public decimal percentDiscount { set; get; }

        public decimal? maxDiscount { set; get; }

        public bool enable { set; get; } = true;

        public virtual IList<AreaPrice> areas { get; set; }

        public virtual IList<Factor> factors { get; set; }

        public virtual IList<MemberOrderPeriod> memberOrderPeriods { get; set; }
    }
}
