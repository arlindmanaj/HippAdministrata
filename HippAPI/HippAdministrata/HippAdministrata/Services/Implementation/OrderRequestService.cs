using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using HippAdministrata.Services.Interfaces;
using HippAdministrata.Models.Domains;
using Microsoft.EntityFrameworkCore;
using HippAdministrata.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace HippAdministrata.Services.Implementation
{
    public class OrderRequestService : IOrderRequestService
    {
        private readonly IOrderRequestRepository _orderRequestRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;


        public OrderRequestService(
            IOrderRequestRepository orderRequestRepository,
            IOrderRepository orderRepository,
            INotificationService notificationService)
        {
            _orderRequestRepository = orderRequestRepository;
            _orderRepository = orderRepository;
            _notificationService = notificationService;
        }


        public async Task<OrderRequest> CreateOrderRequestAsync(
            int orderId,
            int clientId,
            string requestType,
            string? reason,
            string? newDeliveryDestination = null,
            int? newQuantity = null,
            int? newProductId = null,
            int? UnlabeledQuantity = null)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.ClientId != clientId)
                throw new Exception("Order not found or does not belong to this client.");

            // Ensure UnlabeledQuantity matches newQuantity if provided
            if (newQuantity.HasValue)
            {
                UnlabeledQuantity = newQuantity;
            }

            var orderRequest = new OrderRequest
            {
                OrderId = orderId,
                ClientId = clientId,
                RequestType = requestType, // "Update" or "Delete"
                Status = "Pending",
                Reason = reason,
                NewDeliveryDestination = newDeliveryDestination,
                NewQuantity = newQuantity,
                NewProductId = newProductId,
                UnlabeledQuantity = UnlabeledQuantity // Will be the same as newQuantity if provided
            };

            await _orderRequestRepository.AddAsync(orderRequest);

            // Notify managers
            await _notificationService.AddNotificationForRoleAsync(
                3, // Role ID for Managers
                $"New {requestType} request for Order ID {orderId}.",
                "Order Request"
            );
            if (requestType == "Update")
            {
                await _notificationService.AddNotificationForRoleAsync(
                6, // Role ID for Clients
                $"Your request to update Order: {orderId} is pending approval.",
                "Order Request"
                 );
            }
            else if (requestType == "Delete")
            {
                await _notificationService.AddNotificationForRoleAsync(
                6, // Role ID for Clients
                $"Your request to delete Order {orderId} is pending approval.",
                "Order Request"
                 );
            }

            return orderRequest;
        }


        public async Task<List<OrderRequest>> GetPendingRequestsAsync()
        {
            return await _orderRequestRepository.GetPendingRequestsAsync();
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
            if (request.RequestType == "Update")
            {
                await _notificationService.AddNotificationForRoleAsync(
                6,
                $"Your request to update Order: {request.OrderId} was rejected.",
                "Order Request"
                 );
            }
            else if (request.RequestType == "Delete")
            {
                await _notificationService.AddNotificationForRoleAsync(
                6, 
                $"Your request to delete Order {request.OrderId} was rejected.",
                "Order Request"
                 );
            }
            return true;
        }


        //public async Task<bool> ApproveRequestAsync(int requestId)
        //{
        //    var request = await _orderRequestRepository.GetByIdAsync(requestId);
        //    if (request == null || request.Status != "Pending")
        //        throw new Exception("Request not found or already processed.");

        //    request.Status = "Approved";
        //    request.ReviewedAt = DateTime.UtcNow;
        //    await _orderRequestRepository.UpdateAsync(request); // Save request first

        //    var order = await _orderRepository.GetByIdAsync(request.OrderId);
        //    if (order == null)
        //        throw new Exception($"Order with ID {request.OrderId} not found."); // Better error message

        //    if (request.RequestType == "Update")
        //    {
        //        // Apply updates to order
        //        if (!string.IsNullOrEmpty(request.NewDeliveryDestination))
        //            order.DeliveryDestination = request.NewDeliveryDestination;

        //        if (request.NewQuantity.HasValue)
        //        {
        //            order.Quantity = request.NewQuantity.Value;
        //            order.UnlabeledQuantity = request.NewQuantity.Value; // Update unlabeled quantity
        //        }

        //        if (request.NewProductId.HasValue)
        //            order.ProductId = request.NewProductId.Value;

        //        await _notificationService.AddNotificationForRoleAsync(
        //        6, // Role ID for Managers
        //        $"Your request to update Order: {request.OrderId} was approved.",
        //        "Order Request"
        //         );
        //        await _orderRepository.UpdateAsync(order);
        //    }
        //    else if (request.RequestType == "Delete")
        //    {

        //        await _orderRepository.DeleteAsync(request.OrderId);
        //        await _notificationService.AddNotificationForRoleAsync(
        //        6, // Role ID for Managers
        //        $"Your request to delete Order {request.OrderId} was approved.",
        //        "Order Request"
        //         );
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

            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new Exception($"Order with ID {request.OrderId} not found.");

            if (request.RequestType == "Update")
            {
                // Apply updates to order
                if (!string.IsNullOrEmpty(request.NewDeliveryDestination))
                    order.DeliveryDestination = request.NewDeliveryDestination;

                if (request.NewQuantity.HasValue)
                {
                    order.Quantity = request.NewQuantity.Value;
                    order.UnlabeledQuantity = request.NewQuantity.Value;
                }

                if (request.NewProductId.HasValue)
                    order.ProductId = request.NewProductId.Value;

                await _orderRepository.UpdateAsync(order);
            }
            else if (request.RequestType == "Delete")
            {
                await _orderRepository.DeleteAsync(request.OrderId);
            }

            // Send notification to Clients (Role ID = 6) - Same as CreateMultipleOrdersAsync
            var rolesToNotify = new List<int> { 6 }; // Clients have Role ID 6

            foreach (var roleId in rolesToNotify)
            {
                await _notificationService.AddNotificationForRoleAsync(
                    roleId,
                    $"Your request regarding Order ID {request.OrderId} has been approved!",
                    "Order Request Approved"
                );
            }

            return true;
        }




    }
}
