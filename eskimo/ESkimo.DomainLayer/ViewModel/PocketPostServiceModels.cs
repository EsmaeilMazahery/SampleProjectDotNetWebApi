using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class PocketPostServiceModel : PocketPost
    {
        public List<int> selectedFactors { set; get; } = new List<int>();
    }

    public class PocketPostListViewModel : PaggingViewModel
    {
        public PocketPostListViewModel()
        {
            sort = "sendDateTime";
            sortDirection = SortDirection.ASC;
        }

        public DateTime? startSendDateTime { set; get; }
        public DateTime? endSendDateTime { set; get; }

        public IEnumerable<PocketPost> list { set; get; }
    }
}
