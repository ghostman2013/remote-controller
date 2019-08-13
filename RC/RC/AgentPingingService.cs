using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RC.Database;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RC
{
    public class AgentPingingService: IHostedService, IDisposable
    {
        public const int RefreshPeriod = 2;

        private HttpClient httpClient;
        private Timer timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            httpClient = new HttpClient();
            timer = new Timer(e => Run(), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(RefreshPeriod));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void Run()
        {
            using (var db = new AppDbContext())
            {
                var agents = db.Agents.ToList();

                if (!agents.Any())
                {
                    return;
                }

                foreach (var agent in agents)
                {
                    if (agent.Attempts >= 3)
                    {
                        db.Agents.Remove(agent);
                    }
                    else
                    {
                        var task = Task.Run(async () => await FetchData(agent.Host));
                        task.Wait();
                        var agentResponse = task.Result;

                        if (agentResponse != null)
                        {
                            db.AgentResponses.Add(agentResponse);
                        }
                        else
                        {
                            ++agent.Attempts;
                            db.Entry(agent).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        private async Task<AgentResponse> FetchData(string host)
        {
            try
            {
                using (var httpResponse = await httpClient.GetAsync(host))
                {

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var json = await httpResponse.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<AgentResponse>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public void Dispose()
        {
            timer?.Dispose();
            httpClient?.Dispose();
        }
    }
}
