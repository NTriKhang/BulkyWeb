var dataTable;
$(document).ready(function () {
    loadDatatable();
});
function loadDatatable() {
    dataTable = $('#CompanyTable').DataTable({
        ajax: {
            url: "/Admin/Company/GetAll",
        },
        columns: [
            { "data": "name", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a class="btn btn-secondary m-2" href="/Admin/Company/Upsert?id=${data}">Edit</a>
                        <a class="btn btn-danger m-2" onClick=Delete('/Admin/Company/Delete/${data}')>Delete</a>
                    `
                },
                "width":"15%",
            }
        ]
    })
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        Swal.fire(
                            location.reload(),
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }
                    else {
                        Swal.fire(
                            'Deleted!',
                            'Your your file can not deleted.',
                        )
                    }
                }
            })
        }
    })
}