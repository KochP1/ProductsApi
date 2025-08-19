using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOS.EmployeeDtos;
using PedidosApi.Interfaces;
using PedidosApi.Services;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        // Define your actions here

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeDto employeeDto)
        {
            var createdEmployee = await employeeService.CreateEmployee(employeeDto);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await employeeService.DeleteEmployee(id);
            if (result == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee(int id, [FromBody] JsonPatchDocument<PatchEmployeeDto> patchDoc)
        {
            var employeeDb = await employeeService.GetEmployeeById(id);
            if (employeeDb == null)
            {
                return NotFound();
            }

            var employeePatchDto = mapper.Map<PatchEmployeeDto>(employeeDb);
            patchDoc.ApplyTo(employeePatchDto, ModelState);

            if (!TryValidateModel(employeePatchDto))
            {
                return ValidationProblem(ModelState);
            }

            var success = await employeeService.PatchEmployee(patchDoc, employeeDb);
            if (!success)
            {
                return StatusCode(500, "Error applying patch");
            }

            return NoContent();
        }
    }
}