namespace PedidosApi.DTOS.CustomerDtos
{
    public class PatchCustomerDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}