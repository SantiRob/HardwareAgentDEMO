using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HardwareAgent
{
    public class SocketClient
    {
        private const string serverIp = "127.0.0.1"; // IP del servidor SignalR
        private const int serverPort = 5555; // Puerto del servidor SignalR

        public void SendData(string data)
        {
            try
            {
                // Crear un socket TCP/IP
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Conectar al servidor
                socket.Connect(IPAddress.Parse(serverIp), serverPort);

                // Convertir la cadena de datos en bytes
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Enviar los datos al servidor
                socket.Send(byteData);

                // Cerrar el socket
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar datos al servidor: {ex.Message}");
            }
        }
    }
}
