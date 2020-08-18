using ESkimo.Infrastructure;
using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Member
    {
        [Key]
        public int memberId { get; set; }
        
        [MaxLength(ConstantValidations.NameLength)]
        public string name { get; set; }

        [MaxLength(ConstantValidations.NameLength)]
        public string family { get; set; }

        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        public bool verifyMobile { set; get; } = false;

        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        public decimal amount { set; get; }

        public decimal sumPayment { set; get; }

        public decimal sumFactors { set; get; }

        public bool enable { get; set; } = true;

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public DateTime registerDate { set; get; } = DateTime.Now;

        public bool Delete { set; get; } = false;

        public virtual IList<MemberLocation> memberLocations { get; set; } = new List<MemberLocation>();
        public virtual IList<Comment> comments { get; set; }
        public virtual IList<DiscountCode> discountCodes { get; set; }
        public virtual IList<Factor> factors { get; set; }
        public virtual IList<Payment> payments { get; set; }
        public virtual IList<MemberOrderPeriod> memberOrderPeriods { get; set; }
        public virtual IList<BlogComment> blogComments { get; set; }
        public virtual IList<SmsLog> smsLogs { get; set; }
    }
}
