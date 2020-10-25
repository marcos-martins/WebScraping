using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScrapingAPI.Abstract;

namespace WebScrapingAPI.Hub
{
    public class HubClient : Hub<IHubClient>
    {
       private readonly ICnpjService _cnpjService;
       public HubClient(ICnpjService cnpjService)
       {
            _cnpjService = cnpjService;
       }

       public void Carregar(string cnpj)
       {
            string connectionId = Context.ConnectionId;
            _cnpjService.Carregar(cnpj, connectionId);
       }

        public void Consultar(string capcha)
        {
            string connectionId = Context.ConnectionId;
            _cnpjService.ConsultaCnpj(capcha, connectionId);
        }
    }
}
