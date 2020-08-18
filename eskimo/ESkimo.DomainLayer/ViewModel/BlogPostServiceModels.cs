using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class BlogPostServiceModel : BlogPost
    {
    }

    public class BlogPostListViewModel : PaggingViewModel
    {
        public BlogPostListViewModel()
        {
            sort = "title";
            sortDirection = SortDirection.ASC;
        }
        
        public string title { set; get; }
        public int? categoryId { set; get; }

        public IEnumerable<BlogPost> list { set; get; }
    }
}
