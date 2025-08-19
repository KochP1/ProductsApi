using Microsoft.AspNetCore.JsonPatch;
using PedidosApi.DTOS.EmployeeDtos;
using PedidosApi.Models;

namespace PedidosApi.Interfaces
{
    public interface IEmployeeService
    {
        public Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        public Task<Employee> GetEmployeeById(int id);
        public Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        public Task<EmployeeDto> CreateEmployee(EmployeeDto employeeDto);
        public Task<int> DeleteEmployee(int id);
        public Task<bool> PatchEmployee(JsonPatchDocument<PatchEmployeeDto> patchDoc, Employee employeeDb);
    }
}