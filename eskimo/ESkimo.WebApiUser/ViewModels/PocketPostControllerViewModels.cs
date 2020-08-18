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
    public class InsertPocketPostViewModel
    {
        public DateTime registerDateTime { set; get; }
        public DateTime sendDateTime { set; get; }

        public decimal amount { set; get; }

        public int count { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public int userSenderId { set; get; }

        public List<int> selectedFactors { set; get; } = new List<int>();
    }

}
