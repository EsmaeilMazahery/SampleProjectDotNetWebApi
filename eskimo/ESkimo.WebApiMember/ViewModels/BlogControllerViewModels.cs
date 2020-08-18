using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.ViewModels
{
    public class RegisterBlogCommentViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string body { get; set; }

        [MaxLength(ConstantValidations.NameLength)]
        public string memberName { set; get; }

        [MaxLength(ConstantValidations.EmailLength)]
        public string memberEmail { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string memberMobile { set; get; }

        [Required]
        public bool enable { set; get; }

        [Required]
        public string captchaToken { set; get; }

        [Required]
        public string captchaVerify { set; get; }
    }
}
