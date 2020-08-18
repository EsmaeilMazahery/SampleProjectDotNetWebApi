using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class DashboardViewModel
    {
        public int todayVisitMembers { set; get; }
        public int todayRegisterMembers { set; get; }
        public int countOnlineMembers { set; get; }
    }
}
