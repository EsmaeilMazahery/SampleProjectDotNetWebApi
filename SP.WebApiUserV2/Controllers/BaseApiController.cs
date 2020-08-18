using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SP.WebApiUser.Controllers
{
    [AllowAnonymous]
    public class BaseApiController : Controller
    {
        public IUnitOfWork _uow { get; set; }
        
        public BaseApiController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public int? userId
        {
            get
            {
                string userId = User?.Identity.Name;
                return userId != null ? int.Parse(userId) : (int?)null;
            }
        }
    }

}