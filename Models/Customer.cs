using Newtonsoft.Json;

namespace WebApplicationNewJob.Models
{
    public class Customer
    {
        [JsonProperty("customerID")]
        public string CustomerID { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("contactName")]
        public string ContactName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }
    }
}
