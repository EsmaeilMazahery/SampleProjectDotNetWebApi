using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SP.DomainLayer.Models
{
    public class UserVisit
    {
        [Key]
        public int userVisitId { get; set; }

        public int memberId { set; get; }
        public virtual Member member { set; get; }

        public DateTime dateTime { set; get; }

        public string ip { set; get; }
    }
}
