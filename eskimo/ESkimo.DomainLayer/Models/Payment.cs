using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Payment
    {
        public int paymentId { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public PaymentType paymentType{set;get;}

        public bool success { set; get; }

        [MaxLength(ConstantValidations.IdentityLength)]
        public string trackingCode { set; get; }

        public decimal amount { set; get; }

        public DateTime dateTime { set; get; }

        public int memberId { set; get; }
        public virtual Member member { set; get; }

        public int? factorId { set; get; }
        public virtual Factor factor { set; get; }

        public int? memberOrderPeriodId { set; get; }
        public virtual MemberOrderPeriod memberOrderPeriod { set; get; }
    }
}
