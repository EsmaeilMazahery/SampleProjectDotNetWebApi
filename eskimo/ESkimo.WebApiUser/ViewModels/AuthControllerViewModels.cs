
using System.ComponentModel.DataAnnotations;
using ESkimo.Infrastructure.Enumerations;

namespace ESkimo.WebApiUser.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }
    }

    public class ForgetPasswordModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }
    }
}