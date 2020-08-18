using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.DomainLayer.Models
{
    public class Notify
    {
        public int NotifyId { set; get; }

        public string Message { set; get; }

        public DateTime DateTime { set; get; }

        public string Data { set; get; }

        public NotifyType  Type { set; get; }

        public int UserId { set; get; }
        public virtual Member User { set; get; }

    }
}
