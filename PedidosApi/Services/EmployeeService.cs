using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.DTOS.EmployeeDtos;
using PedidosApi.Interfaces;
using PedidosApi.Models;

namespace PedidosApi.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EmployeeService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            return mapper.Map<EmployeeDto>(employee);
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            return employee;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await context.Employees.ToListAsync();
            return mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> CreateEmployee(EmployeeDto employeeDto)
        {
            var employee = mapper.Map<Employee>(employeeDto);
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return mapper.Map<EmployeeDto>(employee);
        }

        public async Task<int> DeleteEmployee(int id)
        {
            var record = await context.Employees.Where(x => x.Id == id).ExecuteDeleteAsync();
            return record;
        }

        public async Task<bool> PatchEmployee(JsonPatchDocument<PatchEmployeeDto> patchDoc, Employee employeeDb)
        {
            try
            {
                var employeePatchDto = mapper.Map<PatchEmployeeDto>(employeeDb);

                patchDoc.ApplyTo(employeePatchDto);

                mapper.Map(employeePatchDto, employeeDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}   