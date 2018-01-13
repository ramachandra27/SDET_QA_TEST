using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDET_QA_TEST
{
    public class Result
    {
        public int id { get; set; }
        public string country { get; set; }
        public string name { get; set; }
        public string abbr { get; set; }       
        public string area { get; set; }
        public string largest_city { get; set; }
        public string capital { get; set; }
    }

    public class RestResponse
    {
        public List<string> messages { get; set; }
        public List<Result> result { get; set; }
    }

    public class RootObject
    {
        public RestResponse RestResponse { get; set; }

    }
}
