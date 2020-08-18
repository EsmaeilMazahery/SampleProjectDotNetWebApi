using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SP.DomainLayer.Models
{
    public class LogUser
    {
        public int logUserId { set; get; }

        public DateTime dateTime { set; get; } = DateTime.Now;

        public LogUserType type { set; get; }

        public string description { set; get; }

        public int userId { set; get; }
        public virtual Member user { set; get; }
    }
}
