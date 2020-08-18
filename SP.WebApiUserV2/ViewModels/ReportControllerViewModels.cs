using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class MetricsViewModel
    {
        public DateTime? fromDate { set; get; }
        public DateTime? toDate { set; get; }
        
        public int VisitMembers { set; get; }
        public int RegisterMembers { set; get; }
        public int VisitUsers { set; get; }
        public int RegisterUsers { set; get; }
    }

    public class ChartsViewModel
    {
        public DateTime? fromDate { set; get; }
        public DateTime? toDate { set; get; }
        public int timeSection { set; get; }
    }
}
