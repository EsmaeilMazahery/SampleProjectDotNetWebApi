using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class CategoryServiceModel : Category
    {
    }

    public class CategoryListViewModel : PaggingViewModel
    {
        public CategoryListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }
        
        public string name { set; get; }

        public IEnumerable<Category> list { set; get; }
    }
}
