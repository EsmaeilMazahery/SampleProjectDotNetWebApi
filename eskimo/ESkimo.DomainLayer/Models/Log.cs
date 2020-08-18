using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
   public class Log
    {
        public int logId { set; get; }

        public DateTime dateTime { set; get; }

        public string description { set; get; }

        public string logData { set; get; }

        public LogType type { set; get; } 
        
        public LogLevel level { set; get; }
    }
}
