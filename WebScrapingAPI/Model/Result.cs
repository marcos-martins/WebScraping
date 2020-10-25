using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScrapingAPI.Model
{
    public class Result
    {
        public bool Success { get; set; }
        public List<string> Error { get; set; }
    }
}
