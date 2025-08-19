using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOS.ProductDtos;
using PedidosApi.Interfaces;
using PedidosApi.Services;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductoService productoService;
        private readonly IMapper mapper;

        public ProductController(IProductoService productoService, IMapper mapper)
        {
            this.productoService = productoService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            try
            {
                var product = await productoService.GetProductoByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            try
            {
                var products = await productoService.GetAllProductosAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductDto product)
        {
            try
            {
                var newProduct = await productoService.CreateProducto(product);
                return StatusCode(201, newProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var record = await productoService.DeleteProducto(id);

                if (record <= 0)
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
        public async Task<IActionResult> PatchProduct(int id, JsonPatchDocument<PatchProductDto> patchDoc)
        {
            try
            {
                var product = await productoService.GetProductoById(id);
                if (product == null)
                {
                    return NotFound();
                }

                var productPatchDto = mapper.Map<PatchProductDto>(product);
                patchDoc.ApplyTo(productPatchDto, ModelState);

                if (!TryValidateModel(productPatchDto))
                {
                    return ValidationProblem(ModelState);
                }

                var success = await productoService.PatchProducto(patchDoc, product);
                if (!success)
                {
                    return StatusCode(500, "Error applying patch");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
    }
}