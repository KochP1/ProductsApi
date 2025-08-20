using Microsoft.AspNetCore.JsonPatch;
using PedidosApi.DTOS.DeliveryDtos;
using PedidosApi.Models;

namespace PedidosApi.Interfaces
{
    public interface IDeliveryService
    {
        public Task<Delivery?> GetDeliveryById(int id);
        public Task<DeliveryStatusHistory> GetDeliveryStatusById(int id);
        public Task<bool> PatchDelivery(JsonPatchDocument<PatchDeliveryDto> patchDoc, Delivery delivery);
        public Task<bool> PatchDeliveryStatus(JsonPatchDocument<PatchDeliveryStatusDto> patchDoc, DeliveryStatusHistory delivery);
    }
}