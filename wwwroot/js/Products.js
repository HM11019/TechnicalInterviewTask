let table;

$(document).ready(function () {
    $.ajax({
        url: '/Main/GetAllProducts',
        type: 'GET',
        success: function (data) {
            const products = JSON.parse(data);

            // Inicializar Tabulator y guardar referencia en variable 'table'
            table = new Tabulator("#productGrid", {
                data: products,
                layout: "fitColumns",
                placeholder: "No se encontraron productos",
                columns: [
                    { title: "Product", field: "productName" },
                    { title: "Price", field: "unitPrice" },
                    { title: "Category", field: "categoryName" },
                    { title: "Description", field: "description" },
                    { title: "Quantity", field: "quantity" },
                    { title: "Country", field: "shipCountry" },
                    { title: "Employee", field: "employee" },
                    {
                        title: "Acciones",
                        width: 2000,
                        formatter: function (cell, formatterParams) {
                            return `
                                <button class="btn btn-sm btn-primary edit-btn">✏️ Edit</button>
                                <button class="btn btn-sm btn-danger delete-btn">🗑️ Del</button>
                            `;
                        },
                        width: 150,
                        hozAlign: "center",
                        cellClick: function (e, cell) {
                            const rowData = cell.getRow().getData();

                            if (e.target.classList.contains('edit-btn')) {
                                alert(`Editar producto: ${rowData.productName}`);
                                // Aquí puedes abrir un modal, redirigir o cargar el producto en un formulario
                            }

                            if (e.target.classList.contains('delete-btn')) {
                                const confirmDelete = confirm(`¿Deseas eliminar el producto: ${rowData.productName}?`);
                                if (confirmDelete) {
                                    cell.getRow().delete(); // Elimina solo visualmente
                                    // También podrías hacer una llamada AJAX para eliminar en el servidor
                                }
                            }
                        }
                    }
                ]
            });

            // Manejador de filtro general
            $("#globalFilter").on("keyup", function () {
                const value = $(this).val();
                if (value === "") {
                    table.clearFilter();
                } else {
                    table.setFilter([
                        [
                            { field: "productName", type: "like", value: value },
                            { field: "unitPrice", type: "like", value: value },
                            { field: "categoryName", type: "like", value: value },
                            { field: "description", type: "like", value: value },
                            { field: "quantity", type: "like", value: value },
                            { field: "shipCountry", type: "like", value: value },
                            { field: "employee", type: "like", value: value },
                        ]
                    ]);
                }
            });
        },
        error: function (xhr, status, error) {
            console.error("Error al obtener los productos:", error);
        }
    });
});
