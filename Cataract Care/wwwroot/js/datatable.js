var dataTable;

$(document).ready(function () {
    loadSubscriptionPlans();
});
function loadSubscriptionPlans() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: '/Packages/GetAllPlans'
        },
        columns: [
            { data: 'packageName', width: "15%" },
            { data: 'price', width: "15%" },
            { data: 'validityPeriod', width: "10%", render: function (data) { return data ? data : "Unlimited"; } },
            { data: 'maxPhotoLimit', width: "10%", render: function (data) { return data ? data : "Unlimited"; } },
            { data: 'description', width: "15%", render: function (data) { return data ? data : "No records"; } },
            {
                data: 'isActive',
                render: function (data, type, row) {
                    if (data) {
                        return `<div class="w-75 btn-group" role="group">
                                  <a onClick=Disable('/Packages/Disable/${row.subscriptionId}') class="btn btn-danger mx-2"></i> Disable</a>
                                </div>`;
                    } else {
                        return `<div class="w-75 btn-group" role="group">
                                  <a onClick=Activate('/Packages/Activate/${row.subscriptionId}') class="btn btn-info mx-2">Activate</a>
                                </div>`;
                    }
                },
                width: "15%"
            }
        ]
    });
}


function Disable(url) {
    Swal.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, disable it!'
    }).then((result) => {
        if (result.isConfirmed) {
          
            $.ajax({
                url: url,
                type: 'PUT',
                success: function (data) {
                    Swal.fire({
                        title: "Good job!",
                        text: "Package Disabled!",
                        icon: "success"
                    });
                    dataTable.ajax.reload();
                    
                }
            })
        }
    })
}
function Activate(url) {
    Swal.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, activate it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'PUT',
                success: function (data) {
                    Swal.fire({
                        title: "Good job!",
                        text: "Package Activated!",
                        icon: "success"
                    });
                    dataTable.ajax.reload();
                    
                }
            })
        }
    })
}