/* ------------------------------------------------------------------------------
 *
 *  # Basic datatables
 *
 *  Demo JS code for datatable_basic.html page
 *
 * ---------------------------------------------------------------------------- */


// Setup initial modules
// ------------------------------

var DatatableBasic = function () {


    //
    // Setup module components
    //

    // Basic Datatable examples
    var _componentDatatableBasic = function () {
        if (!$().DataTable) {
            console.warn('Warning - datatables.min.js is not loaded.');
            return;
        }

        // Setting datatable defaults
        $.extend($.fn.dataTable.defaults, {
            autoWidth: false,
            columnDefs: [{
                orderable: false,
                width: 100,
            }],
            dom: '<"datatable-header"><"datatable-scroll"t><"datatable-footer"ip>',
            language: {
                search: '<span>Filter:</span> _INPUT_',
                searchPlaceholder: 'Type to filter...',
                lengthMenu: '<span>Show:</span> _MENU_',
                paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' }
            },
            "bProcessing": true,
            "sAutoWidth": false,
            "bDestroy": true,
            "sPaginationType": "bootstrap", // full_numbers
            "iDisplayStart ": 10,
            "iDisplayLength": 10,
            "bPaginate": false, //hide pagination
            "bFilter": false, //hide Search bar
            "bInfo": false, // hide showing entries
            "ordering": false
        });


        // Resize scrollable table when sidebar width changes
        $('.sidebar-control').on('click', function () {
            table.columns.adjust().draw();
        });
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _componentDatatableBasic();
        }
    }
}();



// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    DatatableBasic.init();
    reporteGenerar();
    const interval = setInterval(function () {
        reporteGenerar();

    }, 2000);


});

function reporteGenerar() {

    $.ajax({

        type: "GET",
        url: "/Home/fifo_2tonos_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                LoadCurrentReport(response);

            } else {
                console.log("Something went wrong");
            }
        },
        failure: function (response) {
            console.log(response.responseText);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });



}


function LoadCurrentReport(oResults) {

    $('#tabla').DataTable().destroy();

    // tableTorques.clear().draw();

    $('#tabla').dataTable({

        "order": [],


        data: oResults,
        "columns": [
            { "data": "posicion" },
            { "data": "vin" },
            { "data": "sobrehorneado" },
            { "data": "subhorneado" },
        ]
    });

    $('#loader').modal('hide');


}

