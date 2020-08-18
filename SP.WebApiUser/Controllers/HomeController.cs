using SP.DataLayer.Context;
using SP.ServiceLayer.Services;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SP.WebApiMember.Controllers
{

    [Route("")]
    public class HomeController : BaseApiController
    {
        public HomeController(DbContextOptions<DatabaseContext> Builder, IUnitOfWork uow,
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

        [HttpGet, Route("")]
        public IActionResult Home()
        {
            return Ok("User");
        }

        [HttpGet, Route("TestNotify")]
        public async Task<IActionResult> TestNotify()
        {
            // This registration token comes from the client FCM SDKs.
            var registrationToken = "e-fbWaSX5P6_LCLy_tT99l:APA91bHAYmdZOnA8s8iNsxwtw3gPOGwrEfd_Frv4fi_ziwcgVjl0CZ5ilsjq8DXwUpE0255208ZJcXFRfVDG1hPbPlgMPzgfHvcpFFtqua_z7NSJsnPgiGqbrsbyALx5et60sbR8izme";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                    {
                        { "score", "850" },
                        { "time", "2:45" },
                    },
                Notification=new Notification()
                {
                    Body="aaaaaaaaaaaa",
                    Title="fffffffffffffff"
                },
                Token = registrationToken,
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);


            return Ok("TestNotify");
        }


        [HttpGet, Route("getIpInfo")]
        public IActionResult GetIpInfo(string ip)
        {
            var address = "http://api.ipstack.com/" + ip + "?access_key=ed89a9797cb44836ff76f66bfda657b6";
            var client = new RestClient(address);

            var request = new RestRequest(Method.POST);
            dynamic response = client.Execute<dynamic>(request);


            //            {
            //                "ip": "45.8.163.12",
            //    "type": "ipv4",
            //    "continent_code": "AS",
            //    "continent_name": "Asia",
            //    "country_code": "IR",
            //    "country_name": "Iran",
            //    "region_code": "04",
            //    "region_name": "Isfahan",
            //    "city": "Isfahan",
            //    "zip": "81494",
            //    "latitude": 32.67277908325195,
            //    "longitude": 51.68611145019531,
            //    "location": {
            //                    "geoname_id": 418863,
            //        "capital": "Tehran",
            //        "languages": [
            //            {
            //                "code": "fa",
            //                "name": "Persian",
            //                "native": "فارسی",
            //                "rtl": 1
            //            }
            //        ],
            //        "country_flag": "http://assets.ipstack.com/flags/ir.svg",
            //        "country_flag_emoji": "🇮🇷",
            //        "country_flag_emoji_unicode": "U+1F1EE U+1F1F7",
            //        "calling_code": "98",
            //        "is_eu": false
            //    }
            //}


            return Ok(new { response.country_name });
        }
    }
}