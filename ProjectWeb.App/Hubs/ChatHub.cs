using Microsoft.AspNetCore.SignalR;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.APIServices.Services;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Hubs
{
    public class ChatHub :  Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Đây là UserID từ SignalR
                                                 // Lưu UserID vào một cấu trúc dữ liệu hoặc cơ sở dữ liệu nếu cần thiết
            return base.OnConnectedAsync();
        }

        private readonly ISystemUserBackendAPI _user;
        public ChatHub(ISystemUserBackendAPI user)
        {
            _user = user;
        }

        public async Task SendMesaageToAll(string user, string message)
        {
            await Clients.All.SendAsync("MessageRevieced", user, message);
        }

        public async Task SendMesaageToReceiver(string receiver, string message)
        {
            await Clients.User(receiver).SendAsync("MessageRevieced", Context.UserIdentifier, message);
        }
    }
}
