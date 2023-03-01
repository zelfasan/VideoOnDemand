using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.HttpClients
{
    public class MembershipHttpClient
    {
        public HttpClient Client { get; }

        public MembershipHttpClient(HttpClient httpClient)
        {
            Client = httpClient;
        }

        public void AddBearerToken(string token)
        {
            Client.DefaultRequestHeaders.Remove("Authorization");
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }
    }
}
