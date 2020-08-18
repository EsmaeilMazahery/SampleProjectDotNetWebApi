using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class DiscountFactorServiceModel : DiscountFactor
    {
    }

    public class DiscountFactorListViewModel : PaggingViewModel
    {
        public DiscountFactorListViewModel()
        {
            sort = "discountFactorId";
            sortDirection = SortDirection.ASC;
        }

        public IEnumerable<DiscountFactor> list { set; get; }
    }
}
