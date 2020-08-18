using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Factor
    {
        public int factorId { set; get; }

        public decimal amount { set; get; }

        //+
        public decimal amountSend { set; get; }

        //+
        public decimal tax { set; get; }

        //-
        public decimal discountOfFactor { set; get; } = 0;
        //-
        public decimal discountOfCode { set; get; } = 0;
        //-
        public decimal discountOfPeriod { set; get; } = 0;

        //زمان ثبت
        public DateTime dateTime { set; get; } = DateTime.Now;

        //زمان ارسال
        public DateTime sendDateTime { set; get; } = DateTime.Now;

        public bool sent { set; get; } = false;

        public FactorStatus status { set; get; }

        public bool Delete { set; get; } = false;

        public int? periodTypeId { set; get; }
        public virtual PeriodType periodType { set; get; }

        public int memberId { set; get; }
        public virtual Member member { set; get; }

        public int? discountFactorId { set; get; }
        public virtual DiscountFactor discountFactor { set; get; }

        [MaxLength(ConstantValidations.NameLength)]
        public string discountCodeName { set; get; }

        public int? discountCodeId { set; get; }
        public virtual DiscountCode discountCode { set; get; }

        public int memberLocationId { set; get; }
        public virtual MemberLocation memberLocation { set; get; }

        public int? pocketPostId { set; get; }
        public virtual PocketPost pocketPost { set; get; }

        public int? paymentId { set; get; }
        public virtual Payment payment { set; get; }

        public int? memberOrderPeriodId { set; get; }
        public virtual MemberOrderPeriod memberOrderPeriod { get; set; }

        public virtual IList<FactorItem> factorItems { get; set; }


        [NotMapped]
        public string statusDescription
        {
            get
            {
                return status.GetDescriptionOrDefault();
            }
        }
    }
}
