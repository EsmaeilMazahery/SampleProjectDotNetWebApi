using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class AreaPriceServiceModel : AreaPrice
    {
    }

    public class AreaPriceListViewModel : PaggingViewModel
    {
        public AreaPriceListViewModel()
        {
            sort = "areaId";
            sortDirection = SortDirection.ASC;
        }

        public int? areaId { set; get; }
        public int? periodTypeId { set; get; }

        public IEnumerable<AreaPrice> list { set; get; }
    }
}
