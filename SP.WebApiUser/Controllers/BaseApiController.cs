
using SP.DataLayer.Context;
using SP.ServiceLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SP.WebApiMember.Controllers
{
    public class BaseApiController : Controller
    {
        public IUnitOfWork _uow;
        public Lazy<IServiceRepository> _serviceRepository;
        public Lazy<ILogUserRepository> _logUserRepository;
        public Lazy<IMediaRepository> _mediaRepository;
        public Lazy<IMemberRepository> _memberRepository;
        public Lazy<ISmsRepository> _smsRepository;
        public Lazy<ICallRepository> _callRepository;
        public IDistributedCache _distributedCache;

        public BaseApiController(IUnitOfWork uow,
            Lazy<IServiceRepository> ServiceRepository,
            Lazy<ILogUserRepository> LogUserRepository,
            Lazy<IMediaRepository> MediaRepository,
            Lazy<IMemberRepository> MemberRepository,
            Lazy<ISmsRepository> SmsRepository,
            Lazy<ICallRepository> CallRepository,
            IDistributedCache distributedCache
        )
        {
            _uow = uow;
            _serviceRepository = ServiceRepository;
            _logUserRepository = LogUserRepository;
            _mediaRepository = MediaRepository;
            _memberRepository = MemberRepository;
            _smsRepository = SmsRepository;
            _callRepository = CallRepository;
            _distributedCache = distributedCache;
        }

        public int? memberId
        {
            get
            {
                string memberId = User?.Identity.Name;
                return memberId != null ? int.Parse(memberId) : (int?)null;
            }
        }

        public DateTime? loginDate
        {
            get
            {
                var identity1 = User?.Identity as ClaimsIdentity;
                var date = identity1?.FindFirst(ClaimTypes.Expiration)?.Value;
                if (date != null)
                    return DateTime.Parse(date).AddHours(-12);
                else
                    return null;
            }
        }
    }
}
