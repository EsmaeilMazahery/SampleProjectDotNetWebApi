using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class DiscountCodeServiceModel : DiscountCode
    {
        public List<int> selectedCategories { set; get; } 
        public List<int> selectedBrands { set; get; } 
    }

    public class DiscountCodeListViewModel : PaggingViewModel
    {
        public DiscountCodeListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }

        

        public int? sellerId { set; get; }

        public string name { set; get; }

        public IEnumerable<DiscountCode> list { set; get; }
    }
}
