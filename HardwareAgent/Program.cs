using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace HardwareAgent
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var agent = new Agent();
            await agent.RunAsync();
            //SystemInfo s = new SystemInfo();
            //var data = s.GetSystemData();
            //Console.WriteLine(data);
            //Console.ReadLine();
        }
    }

    class Agent
    {
        private SystemInfo _systemInfo;
        private HubConnection _hubConnection;

        public Agent()
        {
            _systemInfo = new SystemInfo();
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7211/infoHardwareHub")
                .Build();

            _hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hubConnection.StartAsync();
            };
        }

        public async Task RunAsync()
        {
            try
            {
                await _hubConnection.StartAsync();


                while (true)
                {
                    var systemData = _systemInfo.GetSystemData();

                    var jsonData = JsonConvert.SerializeObject(systemData);

                    await _hubConnection.InvokeAsync("SendHardwareInfo", jsonData);

                    await Task.Delay(5000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el agente: {ex.Message}");
            }
        }
    }
}