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
    public class MemberServiceModel : Member
    {
        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        public LocationViewModel location { set; get; }

        public int areaId { set; get; }
    }

    public class MemberListViewModel : PaggingViewModel
    {
        public MemberListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }
        
        public string name { set; get; }
        public string family { set; get; }

        public IEnumerable<Member> list { set; get; }
    }
}
