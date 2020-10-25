using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScrapingAPI.Abstract
{
    public interface ICnpjService
    {
        void Carregar(string cnpj, string idConnection);
        void ConsultaCnpj(string capcha, string idConnection);
    }
}
