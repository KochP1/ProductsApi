using PedidosApi.DTOS.OrderDtos;

namespace PedidosApi.DTOS.CustomerDtos
{
    public class CustomerWithOrdersDto : CustomerDto
    {
        public IEnumerable<OrderCustomerDto> Orders { get; set; } = [];
    }
}