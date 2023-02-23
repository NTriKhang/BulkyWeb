var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("pending")) {
        loadDatatable("pending");
    }
    else if (url.includes("approve")) {
        loadDatatable("approve");
    }
    else if (url.includes("process")) {
        loadDatatable("process");
    }
    else {
        loadDatatable("all");
    }

});
function loadDatatable(status) {
    dataTable = $('#OrderMngTable').DataTable({
        ajax: {
            url: "/Admin/OrderManagement/GetAll?status=" + status ,
        },
        columns: [
            { "data": "name", "width": "15%" },
            { "data": "paymentStatus", "width": "15%" },
            { "data": "orderDate", "width": "15%" },
            { "data": "orderTotal", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a class="btn btn-primary m-2">Detail</a>
                    `
                },
                "width": "15%",
            }
        ]
    })
}
