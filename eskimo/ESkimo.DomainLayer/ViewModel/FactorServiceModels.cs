using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class FactorServiceModel : Factor
    {
    }

    public class FactorListViewModel : PaggingViewModel
    {
        public FactorListViewModel()
        {
            sort = "dateTime";
            sortDirection = SortDirection.ASC;
        }

        public DateTime? startDateTime { set; get; }
        public DateTime? endDateTime { set; get; }
        public FactorStatus? status { set; get; }

        public IEnumerable<Factor> list { set; get; }
    }
}
