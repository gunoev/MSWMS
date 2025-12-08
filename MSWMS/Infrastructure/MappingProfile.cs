using AutoMapper;
using MSWMS.Entities;
using MSWMS.Models.Responses;

namespace MSWMS.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Origin.Name))
            .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination.Name))
            .ForMember(dest => dest.OriginCode, opt => opt.MapFrom(src => src.Origin.Code))
            .ForMember(dest => dest.DestinationCode, opt => opt.MapFrom(src => src.Destination.Code))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedDateTime))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.LastChangeDateTime))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.Name : "Unknown"))
            .ForMember(dest => dest.TotalBoxes, opt => opt.MapFrom(src => src.Boxes != null ? src.Boxes.Count : 0))
            .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.Items.Sum(i => i.NeededQuantity)))
            .ForMember(dest => dest.TotalScanned, opt => opt.MapFrom(src => src.Scans != null ? src.Scans.Count(s => s.Status == Scan.ScanStatus.Ok) : 0));

        CreateMap<Location, LocationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.ShortName))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code));
        
        CreateMap<Order, ShipmentOrderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ShipmentId, opt => opt.Ignore())
            .ForMember(dest => dest.DbCode, opt => opt.MapFrom(src => src.ShipmentId))
            .ForMember(dest => dest.TransferOrderNumber, opt => opt.MapFrom(src => src.TransferOrderNumber))
            .ForMember(dest => dest.TransferShipmentNumber, opt => opt.MapFrom(src => src.TransferShipmentNumber))
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Origin.Name))
            .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.Boxes != null ? src.Boxes.Count : 0))
            .ForMember(dest => dest.LoadedBoxes, opt => opt.Ignore())
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.Items.Sum(i => i.NeededQuantity)))
            .ForMember(dest => dest.TotalCollectedItems, opt => opt.MapFrom(src => src.Scans != null ? src.Scans.Count : 0));

        CreateMap<Box, BoxDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()))
            .ForMember(dest => dest.BoxNumber, opt => opt.MapFrom(src => src.BoxNumber))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Scans.Count(s => s.Status == Scan.ScanStatus.Ok)))
            .ForMember(dest => dest.HasShipmentEvents, opt => opt.Ignore());
        
        CreateMap<Item, ItemDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.NeededQuantity))
            .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.ItemInfo.FirstOrDefault().Barcode))
            .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(src => src.ItemInfo.FirstOrDefault().ItemNumber))
            .ForMember(dest => dest.Variant, opt => opt.MapFrom(src => src.ItemInfo.FirstOrDefault().Variant))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ItemInfo.FirstOrDefault().Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ItemInfo.FirstOrDefault().Price))
            .ForMember(dest => dest.DiscountPrice, opt => opt.MapFrom(src => src.ItemInfo.FirstOrDefault().DiscountPrice))
            .ForMember(dest => dest.Remaining, opt => opt.Ignore())
            .ForMember(dest => dest.Scanned, opt => opt.Ignore());
        
        CreateMap<Scan, ScanDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp))
            .ForMember(dest => dest.BoxId, opt => opt.MapFrom(src => src.Box.Id))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Item == null ? 0 : src.Item.Id))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Order.Id))
            .ForMember(dest => dest.BoxNumber, opt => opt.MapFrom(src => src.Box.BoxNumber))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id));
            
    }
}