using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class ProductServiceModel : Product
    {

    }

    public class ProductListViewModel : PaggingViewModel
    {
        public ProductListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }

        public string name { set; get; }
        public int? brandId { set; get; }
        public IList<int> products = null;


        public IEnumerable<Product> list { set; get; }
    }
}
