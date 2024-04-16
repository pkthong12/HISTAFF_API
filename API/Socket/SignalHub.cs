using CORE.SignalHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace API.Socket
{
    public class SignalHub : Hub
    {
        public override async Task OnConnectedAsync()
        {

            await Clients.All.SendAsync("ReceiveSystemMessage", $"{Context.UserIdentifier} joined Hub.");
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(SignalMessage message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            return;
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group {groupName}.");
            return;
        }

        public async Task SendGroupMessage(SignalMessage message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
        public Task SendPrivateMessage(SignalMessage message, string user)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            /*
            By default, SignalR uses the ClaimTypes.NameIdentifier from the ClaimsPrincipal associated with the connection as the user identifier. 
            To customize this behavior, see:
            https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-6.0#use-claims-to-customize-identity-handling
            */

            var httpContext = connection.GetHttpContext();
            if (httpContext != null)
            {
                string? jwtToken = httpContext.Request.Query["access_token"];
                var handler = new JwtSecurityTokenHandler();
                if (jwtToken != null)
                {
                    IdentityModelEventSource.ShowPII = true;
                    var token = handler.ReadJwtToken(jwtToken);
                    var jti = token.Claims.First(claim => claim.Type == "typ").Value;
                    if (jti != null && jti != "")
                    {
                        // do something...
                        Console.WriteLine(jti);
                        return jti;
                    }
                    else
                    {
                        return "typ claim was null";
                    }
                }
                else
                {
                    return "jwtToken was null";
                }
            }
            else
            {
                return "connection.GetHttpContext() was null";
            }
        }
    }
}

