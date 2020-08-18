using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class BlogCategoryServiceModel : BlogCategory
    {
    }

    public class BlogCategoryListViewModel : PaggingViewModel
    {
        public BlogCategoryListViewModel()
        {
            sort = "title";
            sortDirection = SortDirection.ASC;
        }
        
        public string title { set; get; }

        public IEnumerable<BlogCategory> list { set; get; }
    }
}
