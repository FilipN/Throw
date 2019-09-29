using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Throw.Hubs
{
    public class ProjectHub : Hub
    {

        public ProjectHub()
        {

        }
        public async Task JoinGroup(string groupName)
        {
            string[] sp = groupName.Split('?');
            string gid = sp[0];
            Guid g;
            bool validGuid = Guid.TryParse(gid, out g);
            if(validGuid)
                await Groups.AddToGroupAsync(Context.ConnectionId, gid);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task CodeChange(string input)
        {
            Console.WriteLine(input);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //uklanjanje korisnika Context.ConnectionId koji se diskonektovao
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

    }
}