using Microsoft.AspNetCore.Mvc;
using RC.Database;
using System.Collections.Generic;
using System.Linq;

namespace RC.Controllers
{
    [Route("api/agents")]
    public class AgentsController : Controller
    {
        public const int MaxRecordsCount = 25;

        [HttpGet("responses")]
        public IEnumerable<AgentResponse> Responses()
        {
            using (var db = new AppDbContext())
            {
                return db.AgentResponses
                    .OrderByDescending(m => m.CreatedAt)
                    .ThenBy(m => m.Name)
                    .Take(MaxRecordsCount)
                    .ToList();
            }
        }

        [HttpGet("connect")]
        public void Connect(string host)
        {
            using (var db = new AppDbContext())
            {
                if (db.Agents.FirstOrDefault(m => m.Host == host) == null)
                {
                    db.Agents.Add(new Agent
                    {
                        Host = host
                    });
                    db.SaveChanges();
                }
            }
        }

        [HttpGet("disconnect")]
        public void Disconnect(string host)
        {
            using (var db = new AppDbContext())
            {
                var agent = db.Agents.FirstOrDefault(m => m.Host == host);

                if (agent != null)
                {
                    db.Agents.Remove(agent);
                    db.SaveChanges();
                }
            }
        }
    }
}
