using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationNewJob.Models
{
    public class ProductList
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public string ShipCountry { get; set; }
        public string Employee { get; set; }
    }
}
