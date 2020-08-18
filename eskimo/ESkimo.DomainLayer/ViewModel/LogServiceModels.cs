using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class LogServiceModel : Log
    {

    }

    public class LogListViewModel : PaggingViewModel
    {
        public LogListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }


        public string name { set; get; }

        public IEnumerable<Log> list { set; get; }
    }
}
