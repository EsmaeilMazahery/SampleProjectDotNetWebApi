using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class BlogCommentServiceModel : BlogComment
    {
    }

    public class BlogCommentListViewModel : PaggingViewModel
    {
        public BlogCommentListViewModel()
        {
            sort = "body";
            sortDirection = SortDirection.ASC;
        }
        
        public string body { set; get; }

        public IEnumerable<BlogComment> list { set; get; }
    }
}
