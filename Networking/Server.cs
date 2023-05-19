using System.Text;
using System.Net;
using System.Threading;
using API.DTOS;
using System.Text.Json;

namespace API.Networking
{
    public class Server
    {
        private readonly HttpListener listener;
        private Thread serverThread;

        public Server()
        {
            listener = new HttpListener();
            serverThread = null;
        }

        public void CreateAndStartServer()
        {
            listener.Prefixes.Add("http://localhost:7821/");
            listener.Start();

            Plugin.Log.LogInfo("Listener started. waiting for requests...");

            serverThread = new(() =>
            {
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();

                    ThreadPool.QueueUserWorkItem((state) =>
                    {
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;

                        if (request.HttpMethod == "GET")
                        {
                            response.StatusCode = 200;
                            response.ContentType = "application/json";
                            string json = JsonSerializer.Serialize(Plugin.rootDTO);
                            byte[] buffer = Encoding.UTF8.GetBytes(json);
                            System.IO.Stream output = response.OutputStream;
                            output.Write(buffer, 0, buffer.Length);
                            output.Close();
                        }
                    });
                }
            });

            serverThread.Start();
            Plugin.Log.LogInfo("Server started.");
        }

        public void CloseServer()
        {
            listener.Stop();
            if(serverThread != null)
            {
                serverThread.Join();
            } else
            {
                Plugin.Log.LogInfo("No threaded server running");
            }
            Plugin.Log.LogInfo("Server closed");
        }
    }
}