var currentPageAtt = 1;
var currentSortColumnAtt = "";
var currentSortOrderAtt = "asc";

$(document).ready(function () {
    loadAttendances();

    $("#employeeFilter, #statusFilter").on("change", function () {
        currentPageAtt = 1;
        loadAttendances();
    });

    $("#btnAddAttendance").on("click", function () {
        $("#attendanceForm")[0].reset();
        $("#AttId").val(0);
        $("#attendanceModalLabel").text("Add Attendance");
        $("#attendanceModal").modal("show");
    });

    $(document).on("click", ".btnEditAtt", function () {
        var id = $(this).data("id");
        $.get("/Attendance/GetAttendanceById", { id: id }, function (attendance) {
            $("#AttId").val(attendance.id);
            $("#EmployeeId").val(attendance.employeeId);
            $("#Date").val(attendance.date.substring(0, 10));
            $("#Status").val(attendance.status);
            $("#attendanceModalLabel").text("Edit Attendance");
            $("#attendanceModal").modal("show");
        });
    });

    $(document).on("click", ".btnDeleteAtt", function () {
        var id = $(this).data("id");
        if (confirm("Are you sure you want to delete this record?")) {
            $.post("/Attendance/Delete", { id: id }, function () {
                loadAttendances();
            });
        }
    });

    $("#attendanceForm").on("submit", function (e) {
        e.preventDefault();
        var attendance = {
            Id: $("#AttId").val(),
            EmployeeId: $("#EmployeeId").val(),
            Date: $("#Date").val(),
            Status: $("#Status").val()
        };

        $.post("/Attendance/Save", attendance, function () {
            $("#attendanceModal").modal("hide");
            loadAttendances();
        });
    });

    $(document).on("click", ".sort-link-att", function () {
        var column = $(this).data("column");
        if (currentSortColumnAtt === column) {
            currentSortOrderAtt = currentSortOrderAtt === "asc" ? "desc" : "asc";
        } else {
            currentSortColumnAtt = column;
            currentSortOrderAtt = "asc";
        }
        loadAttendances();
    });

    $(document).on("click", "#btnNextPageAtt", function () {
        currentPageAtt++;
        loadAttendances();
    });

    $(document).on("click", "#btnPrevPageAtt", function () {
        if (currentPageAtt > 1) {
            currentPageAtt--;
            loadAttendances();
        }
    });
});

function loadAttendances() {
    $.get("/Attendance/GetAttendances", {
        employeeId: $("#employeeFilter").val(),
        status: $("#statusFilter").val(),
        sortColumn: currentSortColumnAtt,
        sortOrder: currentSortOrderAtt,
        page: currentPageAtt
    }, function (result) {
        $("#attendanceListContainer").html(result);
    });
}