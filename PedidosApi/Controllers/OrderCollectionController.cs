using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOS.OrderDtos;
using PedidosApi.Interfaces;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/orders-detail")]
    public class OrderCollectionController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderCollectionController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderWithDetailDto>>> Get()
        {
            var orders = await orderService.GetOrderCollection();
            return Ok(orders);
        }
    }
}