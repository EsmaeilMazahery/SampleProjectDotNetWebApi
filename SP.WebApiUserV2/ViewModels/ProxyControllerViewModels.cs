using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertProxyViewModel
    {

        public string port { get; set; }

        [Required]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string server { get; set; }

        [MaxLength(ConstantValidations.UsernameLength)]
        public string username { get; set; }

        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [Required]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string domain { get; set; }

        public int countErr { set; get; }

        public bool active { set; get; }
    }

    public class UpdateProxyViewModel
    {
        public int proxyId { set; get; }


        public string port { get; set; }

        [Required]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string server { get; set; }

        [MaxLength(ConstantValidations.UsernameLength)]
        public string username { get; set; }

        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [Required]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string domain { get; set; }

        public int countErr { set; get; }

        public bool active { set; get; }
    }
}
