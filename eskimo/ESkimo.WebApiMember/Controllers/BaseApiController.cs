using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.ServiceLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiMember.Controllers
{
    public class BaseApiController : Controller
    {
        public IUnitOfWork _uow { get; set; }
        public Lazy<ILogRepository> logger;

        public BaseApiController(IServiceProvider serviceProvider,IUnitOfWork uow)
        {
            _uow = uow;
            logger = (Lazy<ILogRepository>)serviceProvider.GetService(typeof(Lazy<ILogRepository>));
        }

        public int? memberId
        {
            get
            {
                string memberId = User?.Identity.Name;
                return memberId != null ? int.Parse(memberId) : (int?)null;
            }
        }

        public string mobilePhone
        {
            get
            {
                string mobilePhone = User.Claims.Where(w=>w.Type== ClaimTypes.MobilePhone).FirstOrDefault().Value;
                return mobilePhone;
            }
        }
    }

}