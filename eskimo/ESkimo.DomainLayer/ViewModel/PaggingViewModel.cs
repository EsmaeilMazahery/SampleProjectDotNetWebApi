using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class PaggingViewModel : ViewModelBase
    {
        public bool excel { set; get; } = false;
        public int allRows { set; get; }
        public int page { set; get; }
        public int rowsPerPage { set; get; }

        public string sort { set; get; }
        public SortDirection sortDirection { set; get; }
    }

}
