using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOS.CustomerDtos;
using PedidosApi.Interfaces;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IMapper mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            this.customerService = customerService;
            this.mapper = mapper;
        }

        // Métodos del controlador

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            try
            {
                var customers = await customerService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            try
            {
                var newCustomer = await customerService.CreateCustomer(customerDto);
                return StatusCode(201, newCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var result = await customerService.DeleteCustomer(id);
                if (result == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCustomer(int id, JsonPatchDocument<PatchCustomerDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is required");
            }

            // Obtener entidad desde el servicio
            var customerDb = await customerService.GetCustomerById(id);
            if (customerDb == null)
            {
                return NotFound();
            }

            var customerPatchDto = mapper.Map<PatchCustomerDto>(customerDb);
            patchDoc.ApplyTo(customerPatchDto, ModelState);

            if (!TryValidateModel(customerPatchDto))
            {
                return ValidationProblem(ModelState);
            }

            var success = await customerService.PatchCustomer(patchDoc, customerDb);
            
            if (!success)
            {
                return StatusCode(500, "Error al aplicar el patch");
            }

            return NoContent();
        }
    }
}