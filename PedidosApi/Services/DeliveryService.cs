using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using PedidosApi.Data;
using PedidosApi.DTOS.DeliveryDtos;
using PedidosApi.Interfaces;
using PedidosApi.Models;

namespace PedidosApi.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public DeliveryService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Delivery?> GetDeliveryById(int id)
        {
            return await context.Deliveries.FindAsync(id);
        }

        public async Task<DeliveryStatusHistory> GetDeliveryStatusById(int id)
        {
            return await context.DeliveryStatusHistories.FindAsync(id);
        }

        public async Task<bool> PatchDelivery(JsonPatchDocument<PatchDeliveryDto> patchDoc, Delivery delivery)
        {
            try
            {
                var deliveryPatchDto = mapper.Map<PatchDeliveryDto>(delivery);

                patchDoc.ApplyTo(deliveryPatchDto);

                mapper.Map(deliveryPatchDto, delivery);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PatchDeliveryStatus(JsonPatchDocument<PatchDeliveryStatusDto> patchDoc, DeliveryStatusHistory delivery)
        {
            try
            {
                var deliveryPatchDto = mapper.Map<PatchDeliveryStatusDto>(delivery);

                patchDoc.ApplyTo(deliveryPatchDto);

                mapper.Map(deliveryPatchDto, delivery);

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