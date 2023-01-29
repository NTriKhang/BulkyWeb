var dataTable;
$(document).ready(function () {
    loadDatatable();
});
function loadDatatable() {
    dataTable = $('#ProductTable').DataTable({
        ajax: {
            url : "/Admin/Product/GetAll",
        },
        columns: [
            { "data": "title", "width": "10%" },
            { "data": "isbn", "width": "10%" },
            { "data": "author", "width": "10%" },
            { "data": "price", "width": "10%" },
            { "data": "category.name", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a class="btn btn-secondary m-2" href="/Admin/Product/Upsert?id=${data}">Edit</a>

                        <a class="btn btn-danger m-2" onClick=Delete('/Admin/Product/Delete/${data}')>Delete</a>
                    `
                },
                "width": "15%",
            }
        ]
    });
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