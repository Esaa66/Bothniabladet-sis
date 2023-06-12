using System;
using System.Collections.Generic;
using System.Linq;

namespace Bothniabladet.Models.Checkout
{
    public class ReceiptModel
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string ReceiptId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<PurchasedItem> PurchasedItems { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class PurchasedItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
