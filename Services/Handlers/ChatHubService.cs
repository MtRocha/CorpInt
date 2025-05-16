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
            var user = Context.User;
            int TipoAcesso = Convert.ToInt32(user.FindFirst(ClaimTypes.Role)?.Value);
            int Funcao = Convert.ToInt32(user.FindFirst(ClaimTypes.PrimaryGroupSid)?.Value);

            string connectionId = Context.ConnectionId;

      
            await Groups.AddToGroupAsync(connectionId, "Todos");

            switch (TipoAcesso)
            {
                case 10:
                    await Groups.AddToGroupAsync(connectionId, "Operadores");
                    await Groups.AddToGroupAsync(connectionId, "Qualidade");
                    break;

                case 3:
                    await Groups.AddToGroupAsync(connectionId, "Operadores");
                    await Groups.AddToGroupAsync(connectionId, "Qualidade");
                    await Groups.AddToGroupAsync(connectionId, "Supervisores");
                    break;

                case 2:
                    await Groups.AddToGroupAsync(connectionId, "Supervisores");
                    await Groups.AddToGroupAsync(connectionId, "Qualidade");

                    break;

                case 0:
                    await Groups.AddToGroupAsync(connectionId, "Operadores");
                    await Groups.AddToGroupAsync(connectionId, "Qualidade");
                    await Groups.AddToGroupAsync(connectionId, "Supervisores");
                    break;
            }

            if (PerfilModel.Qualidade.Contains(Funcao))
            {
                await Groups.AddToGroupAsync(connectionId, "Qualidade");
                await Groups.AddToGroupAsync(connectionId, "QualidadeInterno");
            }

            await base.OnConnectedAsync();
        }

        public async Task Receber(string grupo,string componente,MensagemModel model)
        {
            await Clients.Group(grupo).SendAsync("Receber", grupo,componente,model);
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
