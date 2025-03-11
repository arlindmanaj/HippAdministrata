using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using HippAdministrata.Services.Interfaces;
using HippAdministrata.Models.Domains;
using Microsoft.EntityFrameworkCore;


namespace HippAdministrata.Services.Implementation
{
    public class OrderRequestService : IOrderRequestService
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationService _notificationService;


        public OrderRequestService(
            IOrderRequestRepository orderRequestRepository,
            IOrderRepository orderRepository,
            INotificationService notificationService)
        {
            _orderRequestRepository = orderRequestRepository;
            _orderRepository = orderRepository;
            _notificationService = notificationService;
        }

        public async Task<OrderRequest> CreateOrderRequestAsync(int orderId, int clientId, string requestType, string? reason)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.ClientId != clientId)
                throw new Exception("Order not found or does not belong to this client.");

            var orderRequest = new OrderRequest
            {
                OrderId = orderId,
                ClientId = clientId,
                RequestType = requestType, // "Update" or "Delete"
                Status = "Pending",
                Reason = reason
            };

            await _orderRequestRepository.AddAsync(orderRequest);

            // Notify managers
            await _notificationService.AddNotificationForRoleAsync(
                3, // Role ID for Managers
                $"New {requestType} request for Order ID {orderId}.",
                "Order Request"
            );

            return orderRequest;
        }

        public async Task<List<OrderRequest>> GetPendingRequestsAsync()
        {
            return await _orderRequestRepository.GetPendingRequestsAsync();
        }

        //public async Task<bool> ApproveRequestAsync(int requestId)
        //{
        //    var request = await _orderRequestRepository.GetByIdAsync(requestId);
        //    if (request == null || request.Status != "Pending")
        //        throw new Exception("Request not found or already processed.");

        //    if (request.RequestType == "Delete")
        //    {
        //        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        //        if (order == null)
        //            throw new Exception("Order not found.");

        //        await _orderRepository.DeleteAsync(order.Id);
        //    }

        //    request.Status = "Approved";
        //    request.ReviewedAt = DateTime.UtcNow;
        //    await _orderRequestRepository.UpdateAsync(request);

        //    return true;
        //}
        //public async Task<bool> ApproveRequestAsync(int requestId)
        //{
        //    var request = await _orderRequestRepository.GetByIdAsync(requestId);
        //    if (request == null || request.Status != "Pending")
        //        throw new Exception("Request not found or already processed.");

        //    request.Status = "Approved";
        //    request.ReviewedAt = DateTime.UtcNow;
        //    await _orderRequestRepository.UpdateAsync(request); // Update first!

        //    if (request.RequestType == "Delete")
        //    {
        //        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        //        if (order == null)
        //            throw new Exception("Order not found.");

        //        await _orderRepository.DeleteAsync(order.Id);
        //    }

        //    return true;
        //}
        public async Task<bool> ApproveRequestAsync(int requestId)
        {
            var request = await _orderRequestRepository.GetByIdAsync(requestId);
            if (request == null || request.Status != "Pending")
                throw new Exception("Request not found or already processed.");

            request.Status = "Approved";
            request.ReviewedAt = DateTime.UtcNow;
            await _orderRequestRepository.UpdateAsync(request); // Save request first

            if (request.RequestType == "Delete")
            {
                var orderExists = await _orderRepository.GetByIdAsync(request.OrderId);
                if (orderExists != null) // Ensure order is still there before deleting
                {
                    await _orderRepository.DeleteAsync(request.OrderId);
                }
            }

            return true;
        }






        public async Task<bool> RejectRequestAsync(int requestId, string reason)
        {
            var request = await _orderRequestRepository.GetByIdAsync(requestId);
            if (request == null || request.Status != "Pending")
                throw new Exception("Request not found or already processed.");

            request.Status = "Rejected";
            request.Reason = reason;
            request.ReviewedAt = DateTime.UtcNow;
            await _orderRequestRepository.UpdateAsync(request);

            return true;
        }

    }
}
