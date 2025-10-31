🧩 ASP.NET Core 8.0 MVC Client Application

This project is an ASP.NET Core 8.0 MVC web application built using Razor views, acting as a client UI for consuming external WCF services.
It follows the Model–View–Controller (MVC) pattern to retrieve, process, and display data through rich and responsive UI components powered by Telerik Kendo UI.

🚀 Overview

The main purpose of this application is to demonstrate how an ASP.NET Core MVC front-end can consume data from WCF web services and display it dynamically using customizable Telerik Kendo Grids.

The project implements a clean separation between:

Controllers — manage the request/response pipeline.

Services — handle API/WCF communication logic.

Models — represent structured data entities.

Views (Razor) — display data using HTML and Kendo UI components.

⚙️ Features

✅ ASP.NET Core 8.0 — built with the latest .NET version
✅ MVC Architecture — clean separation of concerns
✅ WCF Integration — consumes remote data via configurable service URLs
✅ Dependency Injection — modular, testable, and extensible service design
✅ Telerik Kendo UI — advanced data grid with paging, sorting, filtering, and exporting
✅ JSON Serialization/Deserialization — robust communication with WCF services
✅ Bootstrap UI — responsive design and layout

🔗 WCF Service Integration

The client app connects to WCF services defined in the appsettings.json file:

"EndpointOption": {
  "ServicesBaseUrl": "http://localhost:54812/",
  "AllCustomers": "CustomerService.svc/GetCustomersByCountry?country="
}


These services are consumed through the MainService and HttpService layers using asynchronous HTTP requests.

🖥️ UI Components (Telerik Kendo UI)

The app uses Telerik Kendo Grid to display tabular data fetched from WCF services:

Dynamic grid rendering via Razor helpers

AJAX data binding using DataSource.Read()

Integrated sorting, paging, and column resizing

Export to Excel and PDF

Responsive Bootstrap styling

Example:

@(Html.Kendo().Grid<WebApplicationNewJob.Models.Customer>()
    .Name("gridCustomer")
    .Columns(columns =>
    {
        columns.Bound(c => c.CustomerID).Title("Customer ID");
        columns.Bound(c => c.CompanyName).Title("Company Name");
        columns.Bound(c => c.ContactName).Title("Contact Name");
        columns.Bound(c => c.Phone).Title("Phone");
        columns.Bound(c => c.Fax).Title("Fax");
    })
    .Pageable()
    .Sortable()
    .Scrollable()
    .DataSource(ds => ds
        .Ajax()
        .Read(read => read.Action("GetCustomersByCountry", "Main").Data("filterCustomer"))
    )
)

🧩 Technologies Used

Category	Technology
Framework	ASP.NET Core 8.0
UI Library	Telerik Kendo UI for ASP.NET Core (Trial 2025.3.1002)
Front-End	Razor, Bootstrap 5, jQuery
Service Layer	WCF Web Services (JSON-based)
Data Handling	Newtonsoft.Json
Dependency Injection	Built-in .NET Core DI Container
IDE	Visual Studio 2022 / 2019

⚙️ Configuration

Open appsettings.json and set your WCF endpoints:

"EndpointOption": {
    "ServicesBaseUrl": "http://localhost:54812/",
    "AllCustomers": "CustomerService.svc/GetCustomersByCountry?country="
}


Ensure your WCF project is running on the same port or update the base URL accordingly.

Restore NuGet packages:

dotnet restore


Run the project:

dotnet run

🧠 Example Workflow

User enters a country name in the filter textbox.

The system calls the WCF endpoint /CustomerService.svc/GetCustomersByCountry?country=....

JSON data is deserialized into a list of Customer objects.

The Telerik Kendo Grid displays the data dynamically on the Razor view.

🧩 Future Improvements

Implement authentication for WCF service calls

Add client-side search and filters in the Kendo Grid

Create additional pages for Orders and Products

Dockerize both client and WCF service for local testing

📜 License
This project is distributed for educational and evaluation purposes.
Telerik UI components used under a Trial License.
