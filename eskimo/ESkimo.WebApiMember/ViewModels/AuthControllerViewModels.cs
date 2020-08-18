
using System;
using System.ComponentModel.DataAnnotations;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using GeoAPI.Geometries;

namespace ESkimo.WebApiMember.ViewModels
{
    public class LoginModel
    {
        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string family { get; set; }

        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 11, MinimumLength = 10)]
        public string mobile { get; set; }

        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        public int areaId { set; get; }

        public LocationViewModel location { set; get; }
    }

    public class FastRegisterModel
    {
        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 11, MinimumLength = 10)]
        public string mobile { get; set; }
    }

    public class VerifyMobileModel
    {
        [Required]
        public string verifyCode { get; set; }

        [Required]
        public string hash { set; get; }
    }

    public class ForgetPasswordModel
    {
        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 11, MinimumLength = 10)]
        public string mobile { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        public string hash { set; get; }

        [Required]
        public string verifyCode { get; set; }

        [Required]
        public string password { get; set; }
    }

    public class SmsVerifyModel
    {
        public string verifyCode { set; get; }
        public DateTime expireDate { set; get; }
        public string mobile { get; set; }
        public int memberId { get; set; }
    }

    public class CaptchaModel
    {
        public int verifyCode { set; get; }
        public string base64String { set; get; }
        public DateTime expireDate { set; get; }
        public string des { set; get; }
    }
}