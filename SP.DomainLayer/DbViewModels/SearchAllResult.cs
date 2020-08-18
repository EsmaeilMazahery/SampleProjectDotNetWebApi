using SP.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.DomainLayer.DbViewModels
{
    public class SearchAllResult
    {
        public int userId { set; get; }
        public virtual Member user { set; get; }

        public int serviceId { set; get; }
        public virtual Service service { set; get; }

        public decimal score { set; get; }
    }
}
