using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiMember.Controllers
{
    [Route("")]
    public class DefaultController : BaseApiController
    {
        public DefaultController(IServiceProvider serviceProvider, IUnitOfWork uow) : base(serviceProvider, uow)
        {

        }
        
        [Route("")]
        public string Home()
        {
            return "ESkimo Member";
        }

        [Route("keep-alive")]
        public string keepalive(int t)
        {
            return "ESkimo";
        }
    }

}