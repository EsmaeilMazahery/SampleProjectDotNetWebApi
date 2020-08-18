using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class MemberLocationServiceModel : MemberLocation
    {
        public new LocationViewModel location { set; get; }
    }

    public class MemberLocationListViewModel : PaggingViewModel
    {
        public MemberLocationListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }

        

        public int? sellerId { set; get; }

        public string name { set; get; }

        public IEnumerable<MemberLocation> list { set; get; }
    }
}
