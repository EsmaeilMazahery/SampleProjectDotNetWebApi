using System;
using System.Collections.Generic;
using System.Text;

namespace SP.DomainLayer.Models
{
    public class SmsLog
    {
        public int smsLogId { set; get; }
        public string message { set; get; }
        public string receptor { set; get; }
        public DateTime dateTime { set; get; }
        public int status { get; set; }

        public int? memberId { set; get; }
        public virtual Member member { set; get; }

        public int? userId { set; get; }
        public virtual User user { set; get; }
    }
}
