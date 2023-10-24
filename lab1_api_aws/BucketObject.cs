using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1_api_aws
{
    public class BucketObject
    {
        public string? ObjectName { get; set; }

        public long? Size { get; set; }
    }
}
