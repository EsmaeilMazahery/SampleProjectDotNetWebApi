using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SP.WebApiUser.Controllers
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
            return "SP";
        }
    }

}