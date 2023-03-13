using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashOut.Models
{
    public class ProductListModel
    {
        public Guid ProductListId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }

    }
}
