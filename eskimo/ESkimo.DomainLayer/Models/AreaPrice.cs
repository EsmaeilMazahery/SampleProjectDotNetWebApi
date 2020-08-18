using System;
using System.Collections.Generic;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class AreaPrice
    {
        public int areaId { set; get; }
        public virtual Area area { set; get; }

        public decimal amountSend { set; get; }

        //مثلا برای ارسال سریع و روزانه ممکن است قیمت لحاظ شود
        public int? periodTypeId { set; get; }
        public virtual PeriodType periodType { set; get; }
    }
}
