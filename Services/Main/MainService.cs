using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationNewJob.Classes;
using WebApplicationNewJob.Models;
using WebApplicationNewJob.Services.Http;

namespace WebApplicationNewJob.Services.Main
{
    public class MainService : IMainService
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly string logPath;
        private readonly IHttpService _httpService;
        private readonly string APIBaseURL;
        private readonly string WCFBaseURL;
        private readonly EndpointOption _endpointOption;
        private readonly string DBSettings;

        public MainService(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IHttpService httpService,
            IOptions<EndpointOption> endpointOpt
        )
        {
            _configuration = configuration;
            _env = env;
            _httpService = httpService;
            logPath = _env.ContentRootPath;
            _endpointOption = endpointOpt.Value; //Option pattern, get all the values saved on appsetting.json about EndpointOption
            APIBaseURL = _endpointOption.BaseUrl;
            WCFBaseURL = _endpointOption.ServicesBaseUrl;
            DBSettings = _configuration.GetValue<string>("DatabaseSettings"); //Get database connection string

        }

        //********** ENTITY FRAMEWORK: Interact with database using Entity Framework from API **********//

        public async Task<List<Customer>> ListFromApiAsync(string country)
        {
            List<Customer> Customers = new List<Customer>();

            try
            {
                //Customers = await _httpService.ListFromApiAsync(model, $"{WCFBaseURL}{_endpointOption.AllCustomers}{country}");
                Customers = _httpService.ListFromApiAsync(
                    new Customer(),
                    $"{WCFBaseURL}{_endpointOption.AllCustomers}{country}").Result;

            }
            catch (Exception ex)
            {

                throw;
            }
            return Customers;
        }

        public async Task<List<CustomerOrderInfoList>> GetCustomerOrdersInformation(string customerID)
        {
            List<CustomerOrderInfoList> CustomerOrderInfo = new List<CustomerOrderInfoList>();

            try
            {
                CustomerOrderInfo = _httpService.ListFromApiAsync(
                    new CustomerOrderInfoList(),
                    $"{WCFBaseURL}{_endpointOption.CustomerOrdersInfo}{customerID}").Result;

            }
            catch (Exception ex)
            {

                throw;
            }
            return CustomerOrderInfo;
        }

    }
}
