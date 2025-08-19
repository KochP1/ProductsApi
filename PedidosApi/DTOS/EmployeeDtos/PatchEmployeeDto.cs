namespace PedidosApi.DTOS.EmployeeDtos
{
    public class PatchEmployeeDto
    {
        public string Name { get; set; }
        public string? Role { get; set; }

        public string? Phone { get; set; }

        public bool? Available { get; set; }
    }
}