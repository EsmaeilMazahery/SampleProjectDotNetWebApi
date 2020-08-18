using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class ProductPriceServiceModel : ProductPrice
    {
    }

    public class ProductPriceListViewModel : PaggingViewModel
    {
        public ProductPriceListViewModel()
        {
            sort = "digiCode";
            sortDirection = SortDirection.ASC;
        }
        
        public string name { set; get; }

        public int? productId { set; get; }
        public int? sellerId { set; get; }

        public IEnumerable<ProductPrice> list { set; get; }
    }
}
