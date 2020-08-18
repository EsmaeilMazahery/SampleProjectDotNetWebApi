using ESkimo.Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.Extension
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
