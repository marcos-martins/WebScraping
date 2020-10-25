using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScrapingAPI.Model
{
    public class SeleniumConfigurations
    {
        public string PathDriverChrome { get; set; }
        public string Url { get; set; }
        public int Timeout { get; set; }
    }
}
