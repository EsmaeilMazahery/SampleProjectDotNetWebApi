using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiUser.Controllers
{
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