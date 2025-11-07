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
        

    }
}