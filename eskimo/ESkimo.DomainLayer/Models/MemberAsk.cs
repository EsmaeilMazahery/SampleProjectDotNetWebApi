using ESkimo.Infrastructure;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class MemberAsk
    {
        [Key]
        public int memberAskId { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string family { get; set; }

        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        [Required]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string answer { set; get; }

        public bool read { set; get; }

        public DateTime registerDate { set; get; } = DateTime.Now;

        public MemberAskType type { set; get; }
    }
}
