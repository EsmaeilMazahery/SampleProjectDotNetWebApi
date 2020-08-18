using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class BlogComment
    {
        public int blogCommentId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string body { get; set; }

        public DateTime registerDate { set; get; }

        [Required]
        public bool enable { set; get; }

        [MaxLength(ConstantValidations.NameLength)]
        public string memberName { set; get; }

        [MaxLength(ConstantValidations.EmailLength)]
        public string memberEmail { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string memberMobile { set; get; }

        public int? memberId { get; set; } = null;
        public virtual Member member { get; set; }

        public int? userId { get; set; } = null;
        public virtual User user { get; set; }

        public int blogPostId { set; get; }
        public virtual BlogPost blogPost { set; get; }
    }
}
