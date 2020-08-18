
using System;
using System.ComponentModel.DataAnnotations;
using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Extensions;

namespace SP.WebApiMember.ViewModels
{
    public class LoginAgainModel
    {
        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }
    }

    public class LoginModel
    {
        [Display(Name = "موبایل یا ایمیل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }
    }

    public class ChangeTokenModel
    {
        [Required]
        public string token { get; set; }
    }

    public class ForgetPasswordModel
    {
        [Display(Name = "موبایل یا ایمیل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }
    }

    public class ForgetPasswordVerifyModel
    {
        [Display(Name = "موبایل یا ایمیل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string verifyCode { set; get; }
    }

    public class ForgetPasswordChangeModel
    {
        [Required]
        public string token { get; set; }

        [Display(Name = "رمزعبور جدید")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string newPassword { get; set; }
    }

    public class ForgetPasswordPayloadJwt
    {
        public string verifyCode { get; set; }
        public string username { get; set; }
        public int userId { get; set; }
        public DateTime expireDate { get; set; }
    }

    public class RegisterPayloadJwt
    {
        public string verifyCode { get; set; }
        public string mobile { get; set; }
        public int userId { get; set; }
        public DateTime expireDate { get; set; }
    }

    public class ChangeImageModel
    {
        [Display(Name = "آدرس")]
        public string address { get; set; }
    }

    public class ChangeMobileModel
    {
        [Display(Name = "موبایل جدید")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string newMobile { get; set; }
    }

    public class ChangeNameFamilyModel
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string family { get; set; }
    }

    public class ChangePasswordProfileModel
    {
        [Display(Name = "رمزعبور جدید")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string newPassword { get; set; }
    }

    public class ChangeEmailModel
    {
        [Display(Name = "ایمیل جدید")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string newEmail { get; set; }
    }


    public class ChangePasswordModel
    {
        [Display(Name = "موبایل یا ایمیل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string username { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string verifyCode { set; get; }
    }

    public class RegisterSetPasswordModel
    {
        [Required]
        public string token { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }
    }

    public class ProfileModel
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string family { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [Display(Name = "پست الکترونیکی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string email { get; set; }

        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string image { get; set; }
    }

    public class RegisterModel
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string family { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [Display(Name = "پست الکترونیکی")]
        public string email { get; set; }

        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string password { get; set; }
    }

    public class RegisterMobileModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }
    }


    public class RegisterVerify
    {
        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string verifyCode { set; get; }

        [Display(Name = "شماره موبایل")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }
    }

    public class VerifyMobileModel
    {
        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string verifyCode { set; get; }

        [Display(Name = "شماره موبایل")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }


        public bool withLogin { set; get; } = false;
    }

    public class VerifyEmailModel
    {
        [Display(Name = "ایمیل")]
        public string email { get; set; }

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "وارد کردن '{0}' الزامی است.")]
        public string verifyCode { set; get; }
    }

    public class SendVerifySmsModel
    {
        [Display(Name = "شماره موبایل")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }
    }


    public class CallVerifyModel
    {
        [Display(Name = "شماره موبایل")]
        [RegularExpression(Constants.MobileRegex)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }
    }
    public class SendVerifyEmailModel
    {
        [Display(Name = "ایمیل")]
        public string email { get; set; }
    }
}