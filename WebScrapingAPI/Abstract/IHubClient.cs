using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScrapingAPI.Abstract
{
    public interface IHubClient
    {
        Task EnviarImagemCapcha(string base64);
        Task EnviarCaptura(string base64);
    }
}
