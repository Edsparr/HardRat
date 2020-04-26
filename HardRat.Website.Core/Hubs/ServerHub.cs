using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HardRat.Website.Core.Hubs
{
    public class ServerHub : Hub
    {
        internal static IDictionary<string, string> sessions = new Dictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var remoteIpAddress = Context.GetHttpContext().Connection.RemoteIpAddress;

            sessions.Add(Context.ConnectionId, remoteIpAddress.ToString());


            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            sessions.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
