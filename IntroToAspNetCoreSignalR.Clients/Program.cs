using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
namespace IntroToAspNetCoreSignalR.Clients
{
    class Program
    {
        static async Task ChatHubServer1()
        {
            string url = "https://localhost:44397/chathub";
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();
            //because the server keep alive interval is 1 minute
            //so the client server timeout must be set to 2 minutes
            connection.ServerTimeout = TimeSpan.FromMinutes(2);

            Console.WriteLine("Press CTRL+C to quit");
            Console.Write(">> ");
            string name = "Mirza";
            BlockingCollection<string> messages = new BlockingCollection<string>();
            connection.Closed += async (error) =>
            {
                Console.WriteLine("Reconnecting...");
                await Task.Delay(1000);
                await connection.StartAsync();
                Console.WriteLine($"Connected to {url}");
            };
            connection.On<string, string>("ReceiveMessage", (user, message) =>
              {
                  Console.Clear();
                  messages.Add($"<<{user}>> `{message}`");
                  foreach(string data in messages)
                  {
                      Console.WriteLine(data);
                  }
                  Console.Write("\n>> ");


              });
            await connection.StartAsync();
            while (true)
            {
                string message = Console.ReadLine();
                await connection.SendAsync("SendMessage", name, message);

            }
        }
        static async Task ChatHubServer2()
        {
            try
            {

                string url = "https://localhost:44327/chathub";
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(url, options =>
                    {
                        Cookie authCookie = new Cookie();
                        string username = "ghulamcyber@hotmail.com";
                        string password = "future";
                        string authEndpoint = "https://localhost:44327/account/login";
                        FormUrlEncodedContent content = new FormUrlEncodedContent(
                                new KeyValuePair<string, string>[]
                                {
                                    new KeyValuePair<string, string>("Email", username),
                                    new KeyValuePair<string, string>("Password", password)
                                }
                            );
                        HttpWebRequest request = WebRequest.Create(authEndpoint) as HttpWebRequest;
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.CookieContainer = new CookieContainer();
                        using (Stream stream = request.GetRequestStream())
                        {
                            string payload = string.Concat("Email=", username, "&Password=", password);
                            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
                            stream.Write(payloadBytes, 0, payloadBytes.Length);
                        }
                        using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                        {
                            authCookie = response.Cookies[".AspNetCore.Identity.Application"];

                        }
                        if (authCookie != null)
                        {
                            options.Cookies.Add(authCookie);
                        }
                    })
                    .Build();
                //because the server keep alive interval is 1 minute
                //so the client server timeout must be set to 2 minutes
                connection.ServerTimeout = TimeSpan.FromMinutes(2);

                Console.WriteLine("Press CTRL+C to quit");
                Console.Write(">> ");
                string name = "Mirza";
                BlockingCollection<string> messages = new BlockingCollection<string>();
                connection.Closed += async (error) =>
                {
                    Console.WriteLine("Reconnecting...");
                    await Task.Delay(1000);
                    await connection.StartAsync();
                    Console.WriteLine($"Connected to {url}");
                };
                connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    Console.Clear();
                    messages.Add($"<<{user}>> `{message}`");
                    foreach (string data in messages)
                    {
                        Console.WriteLine(data);
                    }
                    Console.Write("\n>> ");


                });
                await connection.StartAsync();
                while (true)
                {
                    string message = Console.ReadLine();
                    await connection.SendAsync("SendMessage", name, message);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void Main(string[] args)
        {

            //ChatHubServer1().Wait();
            ChatHubServer2().Wait();
            Console.ReadLine();

        }
    }
}
