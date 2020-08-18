using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiUser.Controllers
{
    [Route("")]
    public class DefaultController : BaseApiController
    {
        public DefaultController(IUnitOfWork uow) : base(uow)
        {

        }
        
        [Route("")]
        public string Home()
        {
            return "ESkimo";
        }

        [Route("keep-alive")]
        public string keepalive(int t)
        {
            return "ESkimo";
        }
    }

}