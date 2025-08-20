using AutoMapper;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.DTOS.DeliveryDtos;
using PedidosApi.DTOS.EmployeeDtos;
using PedidosApi.DTOS.OrderDetailDtos;
using PedidosApi.DTOS.OrderDtos;
using PedidosApi.DTOS.ProductDtos;
using PedidosApi.Models;

namespace PedidosApi.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // CUSTOMER
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, PatchCustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerWithOrdersDto>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders));

            // EMPLOYEE
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, PatchEmployeeDto>().ReverseMap();

            // PRODUCTS

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, PatchProductDto>().ReverseMap();

            // ORDERS

            CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetail))
            .ForMember(dest => dest.Deliveries, opt => opt.MapFrom(src => new List<CreateDeliveryDto> { src.Delivery }))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ORDER DETAIL
            CreateMap<CreateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<Order, OrderWithDetailDto>();
            CreateMap<Order, OrderCustomerDto>().ReverseMap();

            // DELIVERY

            CreateMap<CreateDeliveryDto, Delivery>().ForMember(dest => dest.DeliveryStatusHistories, opt => opt.MapFrom(src => new List<CreateDeliveryStatusDto> { src.DeliveryStatus }))
                .ForMember(dest => dest.EstimatedDelivery, opt => opt.MapFrom(src => DateTime.UtcNow.AddDays(3)))
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(src => "Asignado")).ForMember(dest => dest.ActualDelivery, opt => opt.MapFrom(src => (DateTime?)null));

            CreateMap<DeliveryDto, Delivery>().ReverseMap();
            CreateMap<Delivery, DeliveryDto>()
                .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => 
                    src.DeliveryStatusHistories != null ? 
                    src.DeliveryStatusHistories.OrderByDescending(dsh => dsh.StatusDate).FirstOrDefault() : 
                    null));

            CreateMap<DeliveryStatusHistory, DeliveryStatusDto>();
            CreateMap<DeliveryStatusDto, Delivery>().ReverseMap();
            CreateMap<CreateDeliveryStatusDto, DeliveryStatusHistory>()
            .ForMember(dest => dest.StatusDate, opt => opt.MapFrom(src => src.StatusDate ?? DateTime.UtcNow));
            CreateMap<DeliveryStatusHistory, DeliveryStatusDto>();

        }
    }
}