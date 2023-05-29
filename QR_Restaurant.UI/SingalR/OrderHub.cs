using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.SingalR
{
    public class OrderHub : Hub
    {
        public void JoinGroup(string groupName)
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        }

        public async Task SendOrder(string gruopId, int tableNo)
        {
            if (Clients != null)
            {
                await Clients.Group(gruopId).SendAsync("getNewOrders", tableNo);
            }
        }

        public async Task CallTheWaiter(string gruopId, int tableNo, string tableName)
        {
            if (Clients != null)
            {
                await Clients.Group(gruopId).SendAsync("callTheWaiter", tableNo, tableName);
            }
        }

        public async Task CallTheBill(string gruopId, int tableNo, string tableName)
        {
            if (Clients != null)
            {
                await Clients.Group(gruopId).SendAsync("callTheBill", tableNo, tableName);
            }
        }

        public async Task ListenPayOrder(string gruopId, int orderId)
        {
            if (Clients != null)
            {
                await Clients.Group(gruopId).SendAsync("listenPayOrder", orderId);
            }
        }
    }
}
