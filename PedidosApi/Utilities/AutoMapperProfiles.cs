using AutoMapper;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.DTOS.EmployeeDtos;
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

            // EMPLOYEE
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, PatchEmployeeDto>().ReverseMap();

            // PRODUCTS

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, PatchProductDto>().ReverseMap();

            // ORDERS

            CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetail));
            CreateMap<Order, CreateOrderDto>();
        
            CreateMap<CreateOrderDetailDto, OrderDetail>();

        }
    }
}