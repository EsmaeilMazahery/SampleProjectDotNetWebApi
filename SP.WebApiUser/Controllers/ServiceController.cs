using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.DomainLayer.ViewModel;
using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Extensions;
using SP.ServiceLayer.Services;
using SP.WebApiMember.Controllers;
using SP.WebApiMember.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace SP.WebApiMember.Controllers
{
    [Route("api/service")]
    [Produces("application/json")]
    public class ServiceController : BaseApiController
    {
        public ServiceController(IUnitOfWork uow,
        Lazy<IServiceRepository> ServiceRepository,
            Lazy<ILogUserRepository> LogUserRepository,
            Lazy<IMediaRepository> MediaRepository,
            Lazy<IMemberRepository> MemberRepository,
            Lazy<ISmsRepository> SmsRepository,
            Lazy<ICallRepository> CallRepository,
            IDistributedCache distributedCache
        ) : base(uow,
            ServiceRepository,
            LogUserRepository,
            MediaRepository,
            MemberRepository,
            SmsRepository,
            CallRepository,
            distributedCache)
        {
        }

        [HttpGet, Authorize, Route("")]
        public IActionResult GetAction_List(int? businessId, int? count)
        {
            try
            {
                var query = _serviceRepository.Value.AsQueryable().Where(w => w.memberId == memberId);

                query = query.OrderBy(o => o.sort);

                if (count.HasValue)
                    query = query.Take(count.Value);

                return Ok(query.Select(s => new Service()
                {
                    serviceId = s.serviceId,
                    name = s.name,
                    description = s.description,
                    enable = s.enable
                }).ToList());
            }
            catch (Exception ex)
            {
                try
                {

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Authorize, Route("changeSort/{prevServiceId}/{currentServiceId}")]
        public IActionResult GetAction_ChangeSort(int prevServiceId, int currentServiceId)
        {
            try
            {
                var items = _serviceRepository.Value.AsQueryable()
                 .Where(w => w.memberId == memberId.Value)
                     .Where(w => w.serviceId == prevServiceId || w.serviceId == currentServiceId)
                     .Select(s => new
                     {
                         s.serviceId,
                         s.sort
                     }).ToList().ToDictionary(x => x.serviceId, x => x.sort);

                _serviceRepository.Value.Update(w => w.serviceId == prevServiceId && w.memberId == memberId.Value, u => new Service() { sort = items[currentServiceId] });
                _serviceRepository.Value.Update(w => w.serviceId == currentServiceId && w.memberId == memberId.Value, u => new Service() { sort = items[prevServiceId] });

                return NoContent();
            }
            catch (Exception ex)
            {
                try
                {

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Authorize, Route("")]
        public IActionResult register([FromBody]RegisterServiceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.memberId = base.memberId.Value;

                    _serviceRepository.Value.Delete(d => d.name == model.name && d.memberId == memberId.Value);

                    var service = _serviceRepository.Value.Register(model);

                    _uow.Save();

                    return Ok(service.serviceId);
                }
                else
                {
                    return BadRequest("err");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key row in object 'dbo.Services' with unique index"))
                    {

                    }
                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Authorize, Route("{serviceId}")]
        public IActionResult Get(int serviceId)
        {
            try
            {
                return Ok(_serviceRepository.Value.Read(serviceId));
            }
            catch (Exception ex)
            {
                try
                {

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Authorize, Route("")]
        public IActionResult edit([FromBody]EditServiceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _serviceRepository.Value.Edit(model);

                    _uow.Save();

                    return Ok();
                }
                else
                {
                    return BadRequest("err");
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.InnerException.Message.Contains("Cannot insert duplicate key row in object 'dbo.Services' with unique index"))
                    {

                    }
                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Authorize, Route("checkServiceNameNotTaken")]
        public IActionResult checkServiceNameNotTaken(string name, int? serviceId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(!_serviceRepository.Value.checkServiceNameNotTaken(memberId.Value, name, serviceId));
                }
                else
                {
                    return BadRequest("err");
                }
            }
            catch (Exception ex)
            {
                try
                {

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        //نام سرویس
        [HttpGet, Authorize, Route("listServiceNames")]
        public IActionResult listServiceNames(string name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(_serviceRepository.Value.AsQueryable()
                        .Where(w => w.name.Contains(name))
                        .Select(s => s.name)
                        .Distinct()
                        .Take(10)
                        .ToList());
                }
                else
                {
                    return BadRequest("err");
                }
            }
            catch (Exception ex)
            {
                try
                {

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }


        [HttpDelete, Authorize, Route("{serviceId}")]
        public IActionResult Get_delete(int serviceId)
        {
            try
            {
                var service = _serviceRepository.Value.AsQueryable(q => q.serviceId == serviceId).FirstOrDefault();
                if (service != null)
                {
                    _serviceRepository.Value.Delete(d => d.serviceId == serviceId && d.memberId == memberId.Value);

                    return NoContent();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                try
                {

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Authorize, Route("{serviceId}/{active}")]
        public IActionResult Get_active(int serviceId, string active)
        {
            try
            {
                if (!_serviceRepository.Value.Any(w => w.serviceId == serviceId && w.memberId == memberId.Value))
                {
                    return NotFound();
                }

                if (active == "active" || active == "unactive")
                {
                    _serviceRepository.Value.Update(w => w.serviceId == serviceId && w.memberId == memberId.Value, u => new Service() { enable = (active == "active") });
                    _logUserRepository.Value.AddLog(memberId.Value, (active == "active") ? LogUserType.EnableService : LogUserType.DisableService, Model: new { serviceId });
                }
                else
                {
                    return BadRequest("err");
                }
                return Ok();

            }
            catch (Exception ex)
            {
                try
                {

                }
                catch (Exception ex1)
                {
                    return BadRequest(ex1.Message);
                }

                return BadRequest(ex.Message);
            }
        }



    }
}
