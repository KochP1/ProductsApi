using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
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
        public async Task<ActionResult<OrderWithDetailDto>> Post(CreateOrderDto createOrderDto)
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<PatchOrderDto> patchDoc)
        {
            var orderDb = await orderService.GetOrderByIdAsync(id);
            if (orderDb == null)
            {
                return NotFound();
            }

            var orderPatch = mapper.Map<PatchOrderDto>(orderDb);
            patchDoc.ApplyTo(orderPatch, ModelState);

            if (!TryValidateModel(orderPatch))
            {
                return ValidationProblem(ModelState);
            }

            var result = await orderService.PatchOrder(patchDoc, orderDb);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch("order-detail/{id}")]
        public async Task<IActionResult> PatchOrderDetail(int id, JsonPatchDocument<PatchOrderDetailDto> patchDoc)
        {
            var orderDetailDb = await orderService.GetOrderDetailByIdAsync(id);
            if (orderDetailDb == null)
            {
                return NotFound();
            }

            var orderDetailPatch = mapper.Map<PatchOrderDetailDto>(orderDetailDb);
            patchDoc.ApplyTo(orderDetailPatch, ModelState);

            if (!TryValidateModel(orderDetailPatch))
            {
                return ValidationProblem(ModelState);
            }

            var result = await orderService.PatchOrderDetail(patchDoc, orderDetailDb);
            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}