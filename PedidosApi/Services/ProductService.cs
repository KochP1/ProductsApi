using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosApi.Data;
using PedidosApi.DTOS.ProductDtos;
using PedidosApi.Interfaces;
using PedidosApi.Models;

namespace PedidosApi.Services
{
    public class ProductService : IProductoService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ProductDto> GetProductoByIdAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            return mapper.Map<ProductDto>(product);
        }

        public async Task<Product> GetProductoById(int id)
        {
            var producto = await context.Products.FindAsync(id);
            return producto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductosAsync()
        {
            var products = await context.Products.OrderBy(x => x.Name).ToListAsync();
            var productsDto = mapper.Map<IEnumerable<ProductDto>>(products);
            return productsDto;
        }

        public async Task<ProductDto> CreateProducto(ProductDto productdto)
        {
            var product = mapper.Map<Product>(productdto);
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return mapper.Map<ProductDto>(product);
        }

        public async Task<int> DeleteProducto(int id)
        {
            var record = await context.Products.Where(x => x.Id == id).ExecuteDeleteAsync();
            return record;
        }

        public async Task<bool> PatchProducto(JsonPatchDocument<PatchProductDto> patchDoc, Product productDb)
        {
            try
            {
                var productPatchDto = mapper.Map<PatchProductDto>(productDb);

                patchDoc.ApplyTo(productPatchDto);

                mapper.Map(productPatchDto, productDb);

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