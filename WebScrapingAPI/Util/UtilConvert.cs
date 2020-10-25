using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScrapingAPI.Util
{
    public static class UtilConvert
    {
        public static int ConvertInt(this string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }
    }
}
