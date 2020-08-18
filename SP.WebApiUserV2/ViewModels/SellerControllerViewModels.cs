using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertSellerViewModel
    {
        //کد دیجی کالا
        public int sellerCode { set; get; }

        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        //تامین به موقع
        public decimal timelySupply { set; get; }

        //تعهد ارسال
        public decimal postingCommitment { set; get; }

        //بدون مرجوعی
        public decimal noReference { set; get; }

        //  رضایت خرید
        public decimal SatisfyingSale { set; get; }

        //رای 
        public decimal vote { set; get; }

        //عضویت 
        public decimal membership { set; get; }
        
        public int memberId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string digiUsername { get; set; }

        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string digiPassword { get; set; }

        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string DKMSUsername { get; set; }

        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string DKMSPassword { get; set; }
    }

    public class UpdateSellerViewModel
    {
        public int sellerId { set; get; }

        //کد دیجی کالا
        public int sellerCode { set; get; }

        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        //تامین به موقع
        public decimal timelySupply { set; get; }

        //تعهد ارسال
        public decimal postingCommitment { set; get; }

        //بدون مرجوعی
        public decimal noReference { set; get; }

        //  رضایت خرید
        public decimal SatisfyingSale { set; get; }

        //رای 
        public decimal vote { set; get; }

        //عضویت 
        public decimal membership { set; get; }


        public int memberId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string digiUsername { get; set; }

        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string digiPassword { get; set; }

        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string DKMSUsername { get; set; }

        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string DKMSPassword { get; set; }
    }
}
