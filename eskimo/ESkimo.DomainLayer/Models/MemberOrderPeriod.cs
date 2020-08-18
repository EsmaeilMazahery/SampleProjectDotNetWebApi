using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class MemberOrderPeriod
    {
        public int memberOrderPeriodId { set; get; }

        public PayType payType { set; get; }

        public int periodTypeId { set; get; }
        public virtual PeriodType periodType { set; get; }

        public int memberId { set; get; }
        public virtual Member member { set; get; }

        public virtual IList<Payment> payments { get; set; }
        public virtual IList<Factor> factors { get; set; }

        [NotMapped]
        public virtual Factor targetFactor { set; get; }
    }
}
