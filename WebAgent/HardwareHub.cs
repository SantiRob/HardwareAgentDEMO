using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAgent.Hubs
{
    public class HardwareHub : Hub
    {
        public async Task SendHardwareInfo(object hardwareInfo)
        {
            await Clients.All.SendAsync("ReceiveHardwareInfo", hardwareInfo);
        }
    }
}