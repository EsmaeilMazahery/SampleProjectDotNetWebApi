using SP.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiMember.Extension
{
    public class JwtHandler
    {
        public static string EncodeToken(object payload)
        {
            return Cryptography.EncryptStringAES(JsonConvert.SerializeObject(payload), Security.secretKey);
        }

        public static T DecodeToken<T>(string hash)
        {
            return JsonConvert.DeserializeObject<T>(Cryptography.DecryptStringAES(hash, Security.secretKey));
        }
    }
}
