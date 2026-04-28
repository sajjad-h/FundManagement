using Microsoft.AspNetCore.SignalR;

namespace FundManagement.Api.Hubs
{
    public class NAVHub : Hub
    {
        public async Task SubscribeToFund(int fundId) =>
            await Groups.AddToGroupAsync(Context.ConnectionId, $"fund-{fundId}");

        public async Task UnsubscribeFromFund(int fundId) =>
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"fund-{fundId}");
    }
}
