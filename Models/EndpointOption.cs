using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationNewJob.Models
{
    public class EndpointOption
    {
        public string BaseUrl { get; set; }
        public string AllProducts { get; set; }
        public string AllEmployees { get; set; }
        public string AddEmployee { get; set; }
        public string DeleteEmployee { get; set; }
        public string GetEmployeeById { get; set; }
        public string UpdateEmployee { get; set; }
        public string ServicesBaseUrl { get; set; }  
        public string AllCustomers { get; set; }
        public string CustomerOrdersInfo { get; set; }
    }
}
