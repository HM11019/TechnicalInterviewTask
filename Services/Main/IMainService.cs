using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationNewJob.Models;

namespace WebApplicationNewJob.Services.Main
{
    public interface IMainService
    {

        // Rename
        Task<List<Customer>> ListFromApiAsync(string country);
        Task<List<CustomerOrderInfoList>> GetCustomerOrdersInformation(string customerID);
    
    }
}
