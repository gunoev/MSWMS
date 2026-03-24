using Microsoft.EntityFrameworkCore;
using MSWMS.Entities;

namespace MSWMS.Services;

public class ShipmentsService(AppDbContext context)
{
    public async Task<IEnumerable<Shipment>> IsOrderAssignedTo(int orderId, DateTime date, CancellationToken ct = default)
    {
        var order = await context.Orders
                .Include(o => o.Destination)
                .Include(o => o.Origin)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

        if (order == null)
        {
            throw new Exception("Order not found");
        }
        
        var shipments = await context.Shipments
            .Where(s => s.Orders.Any(o => o.Id == orderId) 
                           && s.Scheduled.Date == date.Date 
                           && s.Destination == order.Destination
                           && s.Origin == order.Origin
                           && s.Status == ShipmentStatus.Scheduled)
            .ToListAsync(ct);
        
        return shipments;
    }
    
    public async Task<int> AssignOrderToShipment(int orderId, DateTime date, CancellationToken ct = default)
    {
        var order = await context.Orders
            .Include(o => o.Destination)
            .Include(o => o.Origin)
            .Include(o => o.CreatedBy)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
        
        if (order == null)
        {
            throw new Exception("Order not found");
        }

        var shipment = await context.Shipments
            .Where(s => s.Scheduled.Date == date.Date
                        && s.Status == ShipmentStatus.Scheduled
                        && s.Destination == order.Destination)
                        .FirstOrDefaultAsync(ct);

        if (shipment == null)
        {
            shipment = new Shipment
            {
                CreatedBy = order.CreatedBy,
                Origin = order.Origin,
                Destination = order.Destination,
            };
            
            await context.Shipments.AddAsync(shipment, ct);
        }
        
        shipment.Orders = new List<Order> { order };
        
        await context.SaveChangesAsync(ct);
        
        return shipment.Id;
    }

    public async Task<IEnumerable<Shipment>> GetShipments(
        DateTime date, 
        int originId, 
        int destinationId, 
        ShipmentStatus status, 
        CancellationToken ct = default)
    {
        return await context.Shipments
            .Where(s => s.Scheduled.Date == date.Date
                        && s.Status == status
                        && s.Origin.Id == originId
                        && s.Destination.Id == destinationId)
                        .ToListAsync(ct);
    }

    public async Task<Shipment> CreateShipment(Shipment shipment, CancellationToken ct = default)
    {
        context.Shipments.Add(shipment);
        await context.SaveChangesAsync(ct);
        return shipment;
    }

    public async Task<bool> RetractOrderFromShipment(int orderId, int shipmentId, CancellationToken ct = default)
    {
        var shipment = await context.Shipments
            .Where(s => s.Id == shipmentId)
            .Include(s => s.Orders)
            .FirstOrDefaultAsync(ct);

        if (shipment == null || shipment.Orders == null || shipment.Orders.All(o => o.Id != orderId))
        {
            return false;
        }
        
        shipment.Orders.Remove(shipment.Orders.Single(o => o.Id == orderId));
        await context.SaveChangesAsync(ct);
        
        return true;
    }

    public async Task RescheduleOrdersForToday(DateTime fromDate, CancellationToken ct = default)
    {
        var today = DateTime.Now;

        var orders = await context.Orders
            .Include(o => o.Destination)
            .Include(o => o.Origin)
            .Include(o => o.CreatedBy)
            .Include(o => o.Boxes)
            .Where(o => o.CreatedDateTime >= fromDate)
            .ToListAsync(ct);

        if (orders.Count == 0)
        {
            return;
        }

        var locationIds = orders
            .SelectMany(o => new[] { o.Destination.Id, o.Origin.Id })
            .Distinct()
            .ToList();

        var locations = await context.Locations
            .Where(l => locationIds.Contains(l.Id))
            .ToDictionaryAsync(l => l.Id, ct);

        var shipments = await context.Shipments
            .Where(s => s.Scheduled.Date == today && s.Status == ShipmentStatus.Scheduled)
            .Include(s => s.Destination)
            .Include(s => s.Origin)
            .Include(s => s.Orders)
            .ToListAsync(ct);

        foreach (var locationId in locationIds)
        {
            if (shipments.Any(s => s.Destination.Id == locationId))
            {
                continue;
            }

            if (!locations.TryGetValue(locationId, out var destination))
            {
                continue;
            }

            var sourceOrder = orders.First(o => o.Destination.Id == locationId || o.Origin.Id == locationId);

            var shipment = new Shipment
            {
                CreatedBy = sourceOrder.CreatedBy,
                Origin = sourceOrder.Origin,
                Destination = destination,
                CreatedAt = today,
                Scheduled = today,
                Status = ShipmentStatus.Scheduled,
                Orders = new List<Order>()
            };

            shipments.Add(shipment);
            await context.Shipments.AddAsync(shipment, ct);
        }

        var orderIds = orders.Select(o => o.Id).ToList();

        var loadedEventsByOrderId = await context.ShipmentEvents
            .Where(e =>
                e.Order != null &&
                orderIds.Contains(e.Order.Id) &&
                e.Action == ShipmentEvent.ShipmentAction.Load &&
                e.Status == ShipmentEvent.EventStatus.Ok)
            .GroupBy(e => e.Order.Id)
            .Select(g => new { OrderId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.OrderId, x => x.Count, ct);

        foreach (var order in orders)
        {
            var boxesCount = order.Boxes?.Count ?? 0;
            var loadedEventsCount = loadedEventsByOrderId.GetValueOrDefault(order.Id, 0);

            if (boxesCount <= loadedEventsCount)
            {
                continue;
            }

            var targetShipment = shipments.FirstOrDefault(s => s.Destination.Id == order.Destination.Id);
            if (targetShipment == null)
            {
                continue;
            }

            targetShipment.Orders ??= new List<Order>();
            if (targetShipment.Orders.All(o => o.Id != order.Id))
            {
                targetShipment.Orders.Add(order);
            }
        }

        await context.SaveChangesAsync(ct);
    }
}