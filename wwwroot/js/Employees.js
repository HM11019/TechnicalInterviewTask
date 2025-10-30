//const { isEmptyObject } = require("jquery");

let table;
let IsEdit = false;

$(document).ready(function () {
    $.ajax({
       /* url: '/Main/GetEmployees',*/ //Using API with Entity Framework
        url: '/Main/GetEmployeesAsyncADONET', // Using ADO.NET
        type: 'GET',
        success: function (employees) {
            
            // Uncomment To use with API endpoint Entity Framework
            //const employees = JSON.parse(employees);

            //Tabulator
            table = new Tabulator("#employeeGrid", {
                data: employees,
                layout: "fitColumns", // O usa "fitDataFill" si prefieres ajustar al contenido
                placeholder: "No se encontraron empleados",
                pagination: "local",
                paginationSize: 10,
                paginationSizeSelector: [5, 10, 25, 50, true],

                columns: [
                    { title: "ID", field: "employeeID", width: 80 },
                    { title: "First Name", field: "firstName", editor: "input" },
                    { title: "Last Name", field: "lastName", editor: "input" },
                    { title: "Title", field: "title", editor: "input" },
                    { title: "Address", field: "address", editor: "input" },
                    { title: "Country", field: "country", editor: "input" },
                    {
                        title: "Acciones",
                        width: 150,
                        hozAlign: "center",
                        formatter: function (cell, formatterParams) {
                            return `<button class="btn btn-sm btn-primary edit-btn">✏️ Edit</button>
                                    <button class="btn btn-sm btn-danger delete-btn">🗑️ Del</button>`;
                        },
                        cellClick: function (e, cell) {
                            const rowData = cell.getRow().getData();
                            IsEdit = true; //The employee will be edit

                            if (e.target.classList.contains('edit-btn')) {
                                // Cargar datos en el formulario del modal
                                document.getElementById("EmployeeID").value = rowData.employeeID;
                                document.getElementById("FirstName").value = rowData.firstName;
                                document.getElementById("LastName").value = rowData.lastName;
                                document.getElementById("Title").value = rowData.title;
                                document.getElementById("Address").value = rowData.address;
                                document.getElementById("Country").value = rowData.country;

                                $('#modalEmployee').show();
                            }

                            if (e.target.classList.contains('delete-btn')) {
                                const confirmDelete = confirm(`¿Deseas eliminar al empleado: ${rowData.firstName} ${rowData.lastName}?`);
                                if (confirmDelete) {
                                    cell.getRow().delete(); // Solo visual

                                    //Get employeeId
                                    const employeeId = rowData.employeeID;
                                    // Lanzar petición AJAX para eliminar en base de datos
                                    DeleteEmployee(employeeId);
                                }
                            }
                        }
                    }
                ]
            });

            // Filtro global
            $("#globalFilter").on("keyup", function () {
                const value = $(this).val();
                if (value === "") {
                    table.clearFilter();
                } else {
                    table.setFilter([
                        [
                            { field: "firstName", type: "like", value: value },
                            { field: "lastName", type: "like", value: value },
                            { field: "title", type: "like", value: value },
                            { field: "address", type: "like", value: value },
                            { field: "country", type: "like", value: value },
                        ]
                    ]);
                }
            });
        },
        error: function (xhr, status, error) {
            console.error("Error al obtener empleados:", error);
        }
    });

    // Evento onclick dentro del onReady
    $("#btnNewEmployee").on("click", function () {
        IsEdit = false;
        //Clean form fields
        $('#FirstName').val('');
        $('#LastName').val('');
        $('#Title').val('');
        $('#Address').val('');
        $('#Country').val('');

        $('#modalEmployee').show();

    });

});


//************************************************ CRUD ************************************************//

function DeleteEmployee(employeeId) {

    $.ajax({
        /*  url: '/Main/DeleteEmployee',*/
        url: '/Main/DeleteEmployeeADONET',
        type: 'POST',
        data: JSON.stringify(employeeId), 
        contentType: 'application/json',
        success: function (result) {

            RefreshEmployeeGrid();
            alert("The employee was successfully removed.");

        },
        error: function (error) {
            alert("Sorry, could not delete employee.");
        }
    });

}


function SaveOrEditEmployee() {
    
    if (IsEdit) {
        UpdateEmployee();
    }
    else {
        AddEmployee();
    }
}

function AddEmployee() {
    var errors = ValidateFieldsEmployee();

    if (errors == 0) {
        var _employee = GetDataEmployee("Add");
        SendInfoToAddEmployee(_employee);
    }
    else {
        alert('Please complete required fields.');
    }

}

function SendInfoToAddEmployee(_employee) {
    
    $.ajax({
        /*url: '/Main/CreateEmployee',*/
        url: '/Main/CreateEmployeeADONET',
        type: "POST",
        dataType: 'html',
        data: JSON.stringify(_employee),
        contentType: 'application/json',
        statusCode: {
            200: function (employeeId) {
                console.log(employeeId);
                $('#modalEmployee').hide();

                //Refresh Tabulator
                RefreshEmployeeGrid();
                alert("Employee successfully created.");
            },
            400: function () {
                $.alert({
                    // title: $.cookie("applicationName"),
                    content: "Sorry, could not add employee.",
                });
            }
        }

    });
}

function UpdateEmployee() {
    var errors = ValidateFieldsEmployee();

    if (errors == 0) {
        var _employee = GetDataEmployee("Update");
        SendInfoToUpdateEmployee(_employee);
    }
    else {
        alert('Please complete required fields.');
    }
}

function SendInfoToUpdateEmployee(_employee) {
    $.ajax({
        /*url: '/Main/UpdateEmployee',*/
        url: '/Main/UpdateEmployeeADONET',
        type: "POST",
        dataType: 'html',
        data: JSON.stringify(_employee),
        contentType: 'application/json',
        statusCode: {

            200: function () {
                $('#modalEmployee').hide();

                //Refresh Tabulator
                RefreshEmployeeGrid();
                alert("Employee successfully updated.");
            },
            400: function () {
                $.alert({
                    content: "Sorry, could not update employee.",
                });
            }
        }

    });
}


function CancelInsert() {
    $('#modalEmployee').hide();

    // Remove validate red border
    $('#FirstName').removeClass('validate-required-fields');
    $('#LastName').removeClass('validate-required-fields');
}

function GetDataEmployee(action) {
    // TextBox values
    let EmployeeID = $("#EmployeeID").val();
    let FirstName = $("#FirstName").val();
    let LastName = $("#LastName").val();
    let Title = $("#Title").val();
    let Address = $("#Address").val();
    let Country = $("#Country").val();

    if (action == "Add") {
        var Employee = {
            "EmployeeID": 0,
            "FirstName": FirstName,
            "LastName": LastName,
            "Title": Title,
            "Address": Address,
            "Country": Country
        }
        return Employee;
    }
    else {

        var Employee = {
            "EmployeeID": EmployeeID,
            "FirstName": FirstName,
            "LastName": LastName,
            "Title": Title,
            "Address": Address,
            "Country": Country
        }

        console.log(Employee)
        return Employee;
    }
}


function RefreshEmployeeGrid() {
    $.ajax({
        /* url: '/Main/GetEmployees', //Using API with Entity Framework*/
        url: '/Main/GetEmployeesAsyncADONET', // Using ADO.NET
        type: 'GET',
        success: function (empleados) {

            // Uncomment To use with API endpoint Entity Framework
            //const empleados = JSON.parse(data);
            table.setData(empleados);

        },
        error: function (xhr, status, error) {
            console.error("Error al refrescar empleados:", error);
        }
    });
}


//************************************************ VALIDATIONS ************************************************//
function ValidateFieldsEmployee() {
    var errors = 0;

    let FirstName = $("#FirstName").val();
    let LastName = $("#LastName").val();

    if (FirstName.trim().length == 0) {
        errors = errors + 1;
        $('#FirstName').addClass('validate-required-fields');
    } else {
        $('#FirstName').removeClass('validate-required-fields');
    }

    if (LastName.trim().length == 0) {
        errors = errors + 1;
        $('#LastName').addClass('validate-required-fields');
    } else {
        $('#LastName').removeClass('validate-required-fields');
    }

    return errors;
}
