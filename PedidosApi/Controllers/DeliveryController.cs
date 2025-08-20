using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PedidosApi.DTOS.DeliveryDtos;
using PedidosApi.Interfaces;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/deliverys")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService deliveryService;
        private readonly IMapper mapper;

        public DeliveryController(IDeliveryService deliveryService, IMapper mapper)
        {
            this.deliveryService = deliveryService;
            this.mapper = mapper;
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDelivery(int id, JsonPatchDocument<PatchDeliveryDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is required");
            }

            // Obtener entidad desde el servicio
            var deliveryDb = await deliveryService.GetDeliveryById(id);
            if (deliveryDb == null)
            {
                return NotFound();
            }

            var deliveryPatchDto = mapper.Map<PatchDeliveryDto>(deliveryDb);
            patchDoc.ApplyTo(deliveryPatchDto, ModelState);

            if (!TryValidateModel(deliveryPatchDto))
            {
                return ValidationProblem(ModelState);
            }

            var success = await deliveryService.PatchDelivery(patchDoc, deliveryDb);

            if (!success)
            {
                return StatusCode(500, "Error al aplicar el patch");
            }

            return NoContent();
        }

        [HttpPatch("delivery-status-historie/{id}")]
                public async Task<IActionResult> PatchDeliveryStatusHistorie(int id, JsonPatchDocument<PatchDeliveryStatusDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is required");
            }

            // Obtener entidad desde el servicio
            var deliveryDb = await deliveryService.GetDeliveryStatusById(id);
            if (deliveryDb == null)
            {
                return NotFound();
            }

            var deliveryPatchDto = mapper.Map<PatchDeliveryStatusDto>(deliveryDb);
            patchDoc.ApplyTo(deliveryPatchDto, ModelState);

            if (!TryValidateModel(deliveryPatchDto))
            {
                return ValidationProblem(ModelState);
            }

            var success = await deliveryService.PatchDeliveryStatus(patchDoc, deliveryDb);

            if (!success)
            {
                return StatusCode(500, "Error al aplicar el patch");
            }

            return NoContent();
        }
    }
}