using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure
{
    public class Token
    {
        public Token(bool isExpired,bool isExpiredToken,IDictionary<string,object> dataInDicformat,string dataInStringFormat,DateTime expiredtime)
        {
            IsExpired = isExpired;
            IsExpiredToken = isExpiredToken;
            DataInDictionaryFormat = dataInDicformat;
            DataInStringFormat = dataInStringFormat;
            ExpiredTime = expiredtime;
        }
        public bool IsExpired { get; }
        public bool IsExpiredToken { get;  }
        public IDictionary<string,object> DataInDictionaryFormat { get; }
        public string DataInStringFormat { get;  }
        public DateTime ExpiredTime { get; set; }
    }
}
