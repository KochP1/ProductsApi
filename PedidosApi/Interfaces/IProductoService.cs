using Microsoft.AspNetCore.JsonPatch;
using PedidosApi.DTOS.ProductDtos;
using PedidosApi.Models;

namespace PedidosApi.Interfaces
{
    public interface IProductoService
    {
        public Task<ProductDto> GetProductoByIdAsync(int id);
        public Task<Product> GetProductoById(int id);
        public Task<IEnumerable<ProductDto>> GetAllProductosAsync();
        public Task<ProductDto> CreateProducto(ProductDto productoDto);
        public Task<int> DeleteProducto(int id);
        public Task<bool> PatchProducto(JsonPatchDocument<PatchProductDto> patchDoc, Product productoDb);
    }
}