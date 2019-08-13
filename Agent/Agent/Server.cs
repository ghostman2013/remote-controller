using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Agent
{
    public class Server
    {
        private readonly Agent agent;
        private readonly Thread thread;
        private readonly HttpListener httpListener;

        public Server(string host, Agent agent)
        {
            this.agent = agent;
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(host);
            thread = new Thread(Listen);
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            thread.Abort();
            httpListener.Stop();
        }

        private void Listen()
        {
            httpListener.Start();

            for (; ; )
            {
                try
                {
                    var context = httpListener.GetContext();
                    Process(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void Process(HttpListenerContext context)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    agent.GenerateMessage(stream);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength64 = stream.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified", DateTime.Now.ToString("r"));

                    var buffer = new byte[1024 * 16];
                    int nbytes;

                    while ((nbytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        context.Response.OutputStream.Write(buffer, 0, nbytes);
                    }

                    stream.Close();
                }

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.OutputStream.Close();
        }
    }
}
