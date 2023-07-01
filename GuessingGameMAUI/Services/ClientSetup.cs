using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GuessingGameMAUI.Services
{
    public class ClientSetup
    {

        private HttpClient client = new();

        private void ClientConfig(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:5053/api/data");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public HttpClient GetClient() 
        { 
            ClientConfig(client);
            return client;
        }

        public ClientSetup()
        {
            ClientConfig(client);
        }

    }
}
