using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationNewJob.Models;
using WebApplicationNewJob.Services.Main;

namespace WebApplicationNewJob.Controllers
{
    public class MainController : Controller
    {

        private readonly string APIBaseURL;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly string logPath;
        private readonly IMainService _MainService;

        public MainController(
            IConfiguration configuration,
            IWebHostEnvironment env,
            IMainService mainService
        )
        {
            _configuration = configuration;
            _env = env;
            APIBaseURL = _configuration.GetValue<string>("WebAPIBaseUrl");
            logPath = _env.ContentRootPath;
            _MainService = mainService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CustomersByCountry()
        {
            return View("~/Views/Customer/CustomersByCountry.cshtml");
        }

        public IActionResult CustomerOrdersInformation(string customerID)
        {
            return View("~/Views/Customer/CustomerOrdersInformation.cshtml", customerID);
        }

        //public async Task<ActionResult<string>> GetCustomersByCountry(string country)
        public async Task<ActionResult> GetCustomersByCountry([DataSourceRequest] DataSourceRequest request, string country)
        {
            try
            {
                var customers = await _MainService.ListFromApiAsync(country);
                var dsResult = customers.ToDataSourceResult(request);
                // Serialize with Newtonsoft using DefaultContractResolver so keys are PascalCase
                var json = JsonConvert.SerializeObject(dsResult, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                });

                return Content(json, "application/json");

            }
            catch (Exception)
            {

                return NotFound();
            }
        }


        public async Task<ActionResult> GetCustomerOrdersInformation([DataSourceRequest] DataSourceRequest request, string customerID)
        {
            try
            {
                var customers = await _MainService.GetCustomerOrdersInformation(customerID);
                var dsResult = customers.ToDataSourceResult(request);

                // Serialize with Newtonsoft using DefaultContractResolver so keys are PascalCase
                var json = JsonConvert.SerializeObject(dsResult, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                });

                return Content(json, "application/json");

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

    }
}
