using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashOut.Models
{
    public class PurchaseModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Price * Quantity;
    }
}