using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class MemberOrderPeriodServiceModel : MemberOrderPeriod
    {
    }

    public class MemberOrderPeriodListViewModel : PaggingViewModel
    {
        public MemberOrderPeriodListViewModel()
        {
            sort = "memberOrderPeriodId";
            sortDirection = SortDirection.ASC;
        }

        
        public IEnumerable<MemberOrderPeriod> list { set; get; }
    }
}
