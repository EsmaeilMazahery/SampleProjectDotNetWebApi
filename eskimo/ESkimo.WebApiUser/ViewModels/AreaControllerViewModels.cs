using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertAreaViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        public LocationViewModel location { set; get; }

        public decimal amountSend { set; get; }

        public string sendDaies { set; get; }
    }

    public class UpdateAreaViewModel
    {
        public int areaId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        public LocationViewModel location { set; get; }

        public decimal amountSend { set; get; }

        public string sendDaies { set; get; }
    }
}
