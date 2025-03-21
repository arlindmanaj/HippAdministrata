﻿using System;

namespace HippAdministrata.Models.Domains
{
    //public class OrderRequest
    //{
    //    public int Id { get; set; } // Primary Key
    //    public int OrderId { get; set; } // Foreign Key to Orders
    //    public int ClientId { get; set; } // Foreign Key to Client
    //    public string RequestType { get; set; } = string.Empty; // "Update" or "Delete"
    //    public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"
    //    public string? Reason { get; set; } // Optional reason provided by the client
    //    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //    public DateTime? ReviewedAt { get; set; } // When manager reviews the request

    //    // Navigation Properties
    //    public Order Order { get; set; }
    //    public Client Client { get; set; }
    //}
    public class OrderRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ClientId { get; set; }

        public string RequestType { get; set; } = string.Empty; // "Update" or "Delete"
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }

        // Fields for Update Requests
        public string? NewDeliveryDestination { get; set; }
        public int? NewQuantity { get; set; }
        public int? NewProductId { get; set; }
        public int? UnlabeledQuantity { get; set; } // Add this field

        // Navigation Properties
        public Order Order { get; set; }
        public Client Client { get; set; }
    }

}
