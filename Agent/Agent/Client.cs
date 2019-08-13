using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Agent
{
    public class Client
    {
        private string controllerUrl;
        private string host;
        private HttpClient httpClient;

        public Client(string controllerUrl, string host)
        {
            this.controllerUrl = controllerUrl;
            this.host = host;
            httpClient = new HttpClient();
        }

        private async Task<bool> SendGetRequest(string url)
        {
            try
            {
                var args = HttpUtility.ParseQueryString(string.Empty);
                args["host"] = host;
                var requestUrl = url + "?" + args.ToString();
                Console.WriteLine("GET: " + requestUrl);
                var response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> Connect()
        {
            return await SendGetRequest(controllerUrl + "/api/agents/connect");
        }

        public async Task<bool> Disconnect()
        {
            return await SendGetRequest(controllerUrl + "/api/agents/disconnect");
        }
    }
}
