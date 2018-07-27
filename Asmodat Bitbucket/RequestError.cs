using AsmodatStandard.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatBitbucket
{
    public class RequestErrorInfo
    {
        public string message;
        public string detail;
        public object data;
    }

    public class RequestError
    {
        public string type;
        public RequestErrorInfo error;
    }
}
