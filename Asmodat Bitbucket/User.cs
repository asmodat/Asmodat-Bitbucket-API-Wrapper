using AsmodatStandard.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatBitbucket
{
    public class UserLinks
    {
        public Link self;
        public Link html;
        public Link avatar;
    }

    public class User
    {
        public UserLinks links;
        public string username;
        public string display_name;
        public string account_id;
        public string uuid;
    }
}
