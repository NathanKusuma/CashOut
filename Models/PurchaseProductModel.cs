using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashOut.Models
{
    public class PurchaseProductModel
    {
        public Guid ProductId { get; set; }
        public string PurchaseProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}