using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace HardwareAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var agent = new Agent();
            //agent.Run();
            SystemInfo _s = new SystemInfo();
            var systemData = _s.GetSystemData();
            Console.WriteLine(systemData);
            Console.ReadLine();

        }
    }

    class Agent
    {
        private SystemInfo _systemInfo;
        private SocketClient _socketClient;

        public Agent()
        {
            // Inicializar la clase SystemInfo para recopilar información del sistema
            _systemInfo = new SystemInfo();

            // Inicializar el cliente de socket para enviar datos al servidor SignalR
            _socketClient = new SocketClient();
        }

        public void Run()
        {
            // Iniciar el bucle principal del agente
            while (true)
            {
                // Recopilar datos del sistema
                var systemData = _systemInfo.GetSystemData();

                // Enviar los datos al servidor SignalR a través del cliente de socket
                _socketClient.SendData(systemData);

                // Esperar un tiempo antes de recopilar nuevos datos
                // Puedes ajustar este tiempo según tus necesidades
                System.Threading.Thread.Sleep(5000); // Esperar 5 segundos
            }
        }
    }
}
