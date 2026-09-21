var currentPage = 1;
var currentSortColumn = "";
var currentSortOrder = "asc";

$(document).ready(function () {
    loadEmployees();

    $("#searchTerm").on("keyup", function () {
        currentPage = 1;
        loadEmployees();
    });

    $("#departmentFilter, #statusFilter").on("change", function () {
        currentPage = 1;
        loadEmployees();
    });

    $("#btnAddEmployee").on("click", function () {
        $("#employeeForm")[0].reset();
        $("#Id").val(0);
        $("#employeeModalLabel").text("Add Employee");
        $("#employeeModal").modal("show");
    });

    $(document).on("click", ".btnEdit", function () {
        var id = $(this).data("id");
        $.get("/Employee/GetEmployeeById", { id: id }, function (employee) {
            $("#Id").val(employee.id);
            $("#Code").val(employee.code);
            $("#Name").val(employee.name);
            $("#Email").val(employee.email);
            $("#Department").val(employee.department);
            $("#Salary").val(employee.salary);
            $("#JoiningDate").val(employee.joiningDate.substring(0, 10));
            $("#IsActive").prop("checked", employee.isActive);
            $("#employeeModalLabel").text("Edit Employee");
            $("#employeeModal").modal("show");
        });
    });

    $(document).on("click", ".btnDelete", function () {
        var id = $(this).data("id");
        if (confirm("Are you sure you want to delete this employee?")) {
            $.post("/Employee/Delete", { id: id }, function () {
                loadEmployees();
            });
        }
    });



    $("#employeeForm").on("submit", function (e) {
        e.preventDefault();
        clearValidationErrors();

        var employee = {
            Id: $("#Id").val(),
            Code: $("#Code").val(),
            Name: $("#Name").val(),
            Email: $("#Email").val(),
            Department: $("#Department").val(),
            Salary: $("#Salary").val(),
            JoiningDate: $("#JoiningDate").val(),
            IsActive: $("#IsActive").is(":checked")
        };

        $.post("/Employee/Save", employee)
            .done(function (response) {
                if (response.success) {
                    $("#employeeModal").modal("hide");
                    loadEmployees();
                } else {
                    showValidationErrors(response.errors);
                }
            })
            .fail(function () {
                alert("Failed to save employee. Please try again.");
            });
    });

    function showValidationErrors(errors) {
        for (var field in errors) {
            $("#" + field + "Error").text(errors[field]);
        }
    }

    function clearValidationErrors() {
        $("[id$='Error']").text("");
    }






    $(document).on("click", ".sort-link", function () {
        var column = $(this).data("column");
        if (currentSortColumn === column) {
            currentSortOrder = currentSortOrder === "asc" ? "desc" : "asc";
        } else {
            currentSortColumn = column;
            currentSortOrder = "asc";
        }
        loadEmployees();
    });

    $(document).on("click", "#btnNextPage", function () {
        currentPage++;
        loadEmployees();
    });

    $(document).on("click", "#btnPrevPage", function () {
        if (currentPage > 1) {
            currentPage--;
            loadEmployees();
        }
    });


    $(document).on("blur", "#Code", function () {
        var code = $("#Code").val();
        var id = $("#Id").val();

        if (code) {
            $.get("/Employee/CheckCodeExists", { code: code, id: id })
                .done(function (exists) {
                    if (exists) {
                        $("#CodeError").text("This Employee Code is already in use");
                    } else {
                        $("#CodeError").text("");
                    }
                })
                .fail(function () {
                    console.log("Could not check code uniqueness.");
                });
        }
    });
});

function loadEmployees() {
    $.get("/Employee/GetEmployees", {
        searchTerm: $("#searchTerm").val(),
        department: $("#departmentFilter").val(),
        status: $("#statusFilter").val(),
        sortColumn: currentSortColumn,
        sortOrder: currentSortOrder,
        page: currentPage
    }, function (result) {
        $("#employeeListContainer").html(result);
    });
}