var dataTable;

$(document).ready(function () {
    diagonisisResult();
});
function diagonisisResult() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/UserSubscription/GetAllHistory' },
        columns: [
            { data: 'patientName', width: "15%" },
            { data: 'age', width: "5%" },
            { data: 'gender', width: "5%", render: function (data) { return data ? data : "N/A"; } },
            { data: 'eye', width: "10%", render: function (data) { return data ? data : "N/A"; } },
            {
                data: 'uploadDate',
                width: "20%",
                render: function (data) {
                    if (data) {
                        var date = new Date(data);
                        // Format the date as MM/DD/YYYY, HH:mm:ss
                        return date.toLocaleDateString('en-US') + ' ' + date.toLocaleTimeString('en-US');
                    }
                    return "N/A";
                }
            },
            { data: 'isCataract', width: "25%" },
            {
                data: 'diagnosisId',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                          <a onClick="DownloadPdf('${data}')" class="btn btn-primary mx-2">Report</a>
                          <a onClick=Delete('/UserSubscription/delete/${data}') class="btn btn-danger mx-2">Delete</a>
                        </div>`;
                },
                width: "15%"
            }
        ],
        order: [[4, 'desc']], // Sort by uploadDate (4th column) in descending order
        columnDefs: [
            { targets: 4, type: 'datetime' } // Ensure correct sorting for the datetime column
        ]
    });
}

function DownloadPdf(diagnosisId) {
    // Make an AJAX call to request the PDF
    $.ajax({
        url: `/UserSubscription/DownloadReport?diagnosisId=${diagnosisId}`,
        method: 'GET',
        xhrFields: {
            responseType: 'blob' // Important for handling binary data
        },
        success: function (data, status, xhr) {
            // Create a new Blob object to handle the data
            var blob = new Blob([data], { type: 'application/pdf' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);


            // Specify the filename
            var filename = xhr.getResponseHeader('Content-Disposition').split('filename=')[1];
            // Perform the search
            // Regex to extract the desired part
            const regex = /(?<=_)([^;]+?\.pdf)/;

            // Perform the search
            const match = filename.match(regex);

            // Get the matched group if found
            const result = match ? match[0] : null;
            //var name = filename.split(';')[0];
            link.download = result;

            // Append the link to the body and trigger the click to download the file
            document.body.appendChild(link);
            link.click();

            // Remove the link from the DOM
            document.body.removeChild(link);
        },
        error: function () {
            toastr.error('Failed to download report.');
        }
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
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}