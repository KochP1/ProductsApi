using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOS.OrderDtos;
using PedidosApi.Interfaces;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/orders")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var orders = await orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            var order = await orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<CreateOrderDto>> Post(CreateOrderDto createOrderDto)
        {
            var customerExist = await orderService.ValidateCustomer(createOrderDto);

            if (!customerExist)
            {
                return NotFound("El cliente no existe");
            }

            var productsExist = await orderService.ValidateProducts(createOrderDto);

            if (!productsExist)
            {
                return NotFound("Algunos de los productos no existen");
            }

            var response = await orderService.CreateOrder(createOrderDto);
            return StatusCode(201, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await orderService.DeleteOrder(id);

            if (record <= 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}