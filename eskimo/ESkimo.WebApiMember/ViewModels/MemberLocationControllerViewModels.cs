using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.ViewModels
{
    public class InsertMemberLocationViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string phone { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string postalCode { set; get; }

        public LocationViewModel location { set; get; }

        public int areaId { set; get; }
    }

    public class UpdateMemberLocationViewModel
    {
        public int memberLocationId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string phone { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string postalCode { set; get; }

        public LocationViewModel location { set; get; }

        public int areaId { set; get; }
    }
}
