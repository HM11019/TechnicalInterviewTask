using Newtonsoft.Json;
using System;

namespace WebApplicationNewJob.Models
{
    public class CustomerOrderInfoList
    {
        [JsonProperty("orderID")]
        public int OrderID { get; set; }

        [JsonProperty("customerID")]
        public string CustomerID { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("shippedDate")]
        public DateTime ShippedDate { get; set; }
    }
}
