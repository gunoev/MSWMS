using MSWMS.Entities;
namespace MSWMS.Factories;

public static class OrderFactory
{
    public class OrderBuilder
    {
        private readonly Order _order = new() 
        {
            TransferOrderNumber = "",
            TransferShipmentNumber = "",
            Origin = null!,
            Destination = null!,
            ShipmentId = "",
            Type = Order.OrderType.Other,
            CreatedBy = null!,
            Items = null!
        };
        
        public OrderBuilder WithBasicInfo(
            string transferOrderNumber,
            string transferShipmentNumber,
            Location origin,
            Location destination,
            string shipmentId,
            Order.OrderType type)
        {
            _order.TransferOrderNumber = transferOrderNumber;
            _order.TransferShipmentNumber = transferShipmentNumber;
            _order.Origin = origin;
            _order.Destination = destination;
            _order.ShipmentId = shipmentId;
            _order.Type = type;
            return this;
        }

        public OrderBuilder WithItems(ICollection<Item> items)
        {
            _order.Items = items;
            return this;
        }

        public OrderBuilder WithBoxes(ICollection<Box> boxes)
        {
            _order.Boxes = boxes;
            return this;
        }

        public OrderBuilder WithScans(ICollection<Scan> scans)
        {
            _order.Scans = scans;
            return this;
        }

        public OrderBuilder WithUsers(ICollection<User> users)
        {
            _order.Users = users;
            return this;
        }

        public OrderBuilder WithCreatedBy(User createdBy)
        {
            _order.CreatedBy = createdBy;
            return this;
        }

        public OrderBuilder WithPriority(Order.OrderPriority priority)
        {
            _order.Priority = priority;
            return this;
        }

        public OrderBuilder WithRemark(string remark)
        {
            _order.Remark = remark;
            return this;
        }

        public Order Build()
        {
            return _order;
        }
    }

    public static OrderBuilder CreateRefillOrder(
        string transferOrderNumber,
        string transferShipmentNumber,
        Location origin,
        Location destination,
        string shipmentId
        )
    {
        return new OrderBuilder()
            .WithBasicInfo(
                transferOrderNumber, 
                transferShipmentNumber, 
                origin, 
                destination, 
                shipmentId, 
                Order.OrderType.Refill
                );
    }

    public static OrderBuilder CreateDistributionOrder(
        string transferOrderNumber,
        string transferShipmentNumber,
        Location origin,
        Location destination,
        string shipmentId
        )
    {
        return new OrderBuilder()
            .WithBasicInfo(
                transferOrderNumber, 
                transferShipmentNumber, 
                origin, 
                destination, 
                shipmentId, 
                Order.OrderType.Distribution
                );
    }

    public static OrderBuilder CreateOtherOrder(
        string transferOrderNumber,
        string transferShipmentNumber,
        Location origin,
        Location destination,
        string shipmentId
        )
    {
        return new OrderBuilder()
            .WithBasicInfo(
                transferOrderNumber, 
                transferShipmentNumber, 
                origin, 
                destination, 
                shipmentId, 
                Order.OrderType.Other
                );
    }
}