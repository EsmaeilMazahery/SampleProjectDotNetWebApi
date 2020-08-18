
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.ViewModel
{
    public class PaggingViewModelBase
    {
        public bool excel { set; get; } = false;
        public int allRows { set; get; }
        public int page { set; get; }
        public int rowsInPage { set; get; }

        public string sort { set; get; }
        public SortDirection sortDirection { set; get; }
    }
}
