using AsmodatStandard.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatBitbucket
{
    public class Pagination<T>
    {
        public int pagelen;
        public T[] values;
        public int page;
        public int size;
        public string next;
        public string previous;

        [JsonIgnore]
        public bool IsLast
        {
            get
            {
                return next.IsNullOrEmpty();
            }
        }

        [JsonIgnore]
        public bool IsFirst
        {
            get
            {
                return next.IsNullOrEmpty();
            }
        }
    }
}
