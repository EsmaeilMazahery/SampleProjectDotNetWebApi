using System;
using System.Collections.Generic;
using System.Text;

namespace SP.DomainLayer.ViewModel
{
    public class AppSettings
    {
        public string websiteScheduler { set; get; }
        public string websiteClient { set; get; }
        public string websiteUser { set; get; }

        public bool ActiveSchedulerRequest { set; get; }
        public bool ActiveSearchRobotService { set; get; }
        public bool ActiveSearchRobotSample { set; get; }
        public bool ActiveSearchRobotPrice { set; get; }
    }
}
