using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertAreaPriceViewModel
    {
        [Required]
        public int areaId { set; get; }

        public decimal amountSend { set; get; }

        public int? periodTypeId { set; get; }
    }

    public class UpdateAreaPriceViewModel
    {
        [Required]
        public int areaId { set; get; }

        public decimal amountSend { set; get; }

        public int? periodTypeId { set; get; }
    }
}
