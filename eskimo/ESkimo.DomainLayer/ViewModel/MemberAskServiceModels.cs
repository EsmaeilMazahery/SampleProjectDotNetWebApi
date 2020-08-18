using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class MemberAskServiceModel : MemberAsk
    {

    }

    public class MemberAskListViewModel : PaggingViewModel
    {
        public MemberAskListViewModel()
        {
            sort = "registerDate";
            sortDirection = SortDirection.DESC;
        }
        
        public string name { set; get; }
        public string family { set; get; }

        public IEnumerable<MemberAsk> list { set; get; }
    }
}
