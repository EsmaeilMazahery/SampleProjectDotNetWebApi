using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class AreaServiceModel : Area
    {
        public new LocationViewModel location { set; get; } = new LocationViewModel();
    }

    public class AreaListViewModel : PaggingViewModel
    {
        public AreaListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }


        public string name { set; get; }

        public IEnumerable<Area> list { set; get; }
    }
}
