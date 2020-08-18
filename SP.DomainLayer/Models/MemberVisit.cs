using System;
using System.Collections.Generic;
using System.Text;

namespace SP.DomainLayer.Models
{
    public class MemberVisit
    {
        public int memberVisitId { set; get; }

        public string ip { set; get; }
        public DateTime dateTime { set; get; }
    }
}
