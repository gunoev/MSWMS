using Microsoft.AspNetCore.SignalR;

namespace MSWMS.Hubs;

public class ScanHub : Hub
{
    public async Task JoinOrderGroup(int orderId)
    {
        var groupName = $"Order_{orderId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveOrderGroup(int orderId)
    {
        var groupName = $"Order_{orderId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task NewMessage(long username, string message) =>
        await Clients.All.SendAsync("messageReceived", username, message);

    public async Task ScanProcessed(int orderId, int scanId, int boxId)
    {
        var groupName = $"Order_{orderId}";
        await Clients.Group(groupName).SendAsync("scanProcessed", orderId, scanId, boxId);
    }

    public async Task ScanRemoved(int orderId, int scanId, int boxId)
    {
        var groupName = $"Order_{orderId}";
        await Clients.Group(groupName).SendAsync("scanRemoved", orderId, scanId, boxId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}