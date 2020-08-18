using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class ProductTypeServiceModel : ProductType
    {

    }

    public class ProductTypeListViewModel : PaggingViewModel
    {
        public ProductTypeListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }
        
        public string name { set; get; }

        public IEnumerable<ProductType> list { set; get; }
    }
}
