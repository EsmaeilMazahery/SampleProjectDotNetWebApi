using System;
using System.Collections.Generic;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class FilterViewModel : ViewModelBase
    {
        public int id { set; get; }
        public string value { set; get; }
    }

    public class FilterPaggingViewModel
    {
        public int allRows { set; get; }

        public IEnumerable<FilterViewModel> list { set; get; }
    }

}
