using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class CommentServiceModel : Comment
    {
    }

    public class CommentListViewModel : PaggingViewModel
    {
        public CommentListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }

        

        public int? sellerId { set; get; }

        public string name { set; get; }

        public IEnumerable<Comment> list { set; get; }
    }
}
