using Intranet_NEW.Models;
using Intranet_NEW.Models.WEB;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Intranet_NEW.Services.Handlers
{
    public class ChatHubService : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHubService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);

            int perfil = Convert.ToInt32(Context.User.FindFirst(ClaimTypes.PrimaryGroupSid)?.Value);

            if(PerfilModel.Operador.Contains(perfil))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Operadores");
            }

            await base.OnConnectedAsync();

        }
        public async Task MensagemTodos(string user, string message)
        {
            await Clients.Group("Operadores").SendAsync("Receber", user, message);
        }
        public async Task MensagemPrivada(string operatorId, string user, string message)
        {
            await Clients.User(operatorId).SendAsync("MensagemPrivada", user, message);
        }
    }
}
