/* ------------------------------------------------------------------------------
 *
 *  # Basic datatables
 *
 *  Demo JS code for datatable_basic.html page
 *
 * ---------------------------------------------------------------------------- */


// Initialize module
// ------------------------------

var swalInit = swal.mixin({
    buttonsStyling: false,
    customClass: {
        confirmButton: 'btn btn-primary',
        cancelButton: 'btn btn-light',
        denyButton: 'btn btn-light',
        input: 'form-control'
    }
});

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
                targets: [0]
            }],
            dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
            language: {
                search: '<span>Filter:</span> _INPUT_',
                searchPlaceholder: 'Type to filter...',
                lengthMenu: '<span>Show:</span> _MENU_',
                paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' }
            }
        });

        // Apply custom style to select
        $.extend($.fn.dataTableExt.oStdClasses, {
            "sLengthSelect": "custom-select"
        });

        // Basic datatable
        tableTorques = $('.datatable-basic').DataTable();


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



var Select2Selects = function () {


    //
    // Setup module components
    //

    // Select2 examples
    var _componentSelect2 = function () {
        if (!$().select2) {
            console.warn('Warning - select2.min.js is not loaded.');
            return;
        }


        //
        // Basic examples
        //

        // Select with search
        $('.select-search').select2({
            dropdownParent: $('#modal_form_vertical')
        });


    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _componentSelect2();
        }
    }
}();



// ------------------------------
// Initialize module




document.addEventListener('DOMContentLoaded', function () {
    DatatableBasic.init();
    Select2Selects.init();
    reporteGenerar();


});

function reporteGenerar() {

    $.ajax({

        type: "GET",
        url: "/Catalogo/get_ecoat_Json",
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

    console.log(oResults);

    $('#torques').DataTable().destroy();

    // tableTorques.clear().draw();

    $('#torques').dataTable({

        "order": [],

        data: oResults,
        "columns": [
            { "data": "minuto" },
            { "data": "temperatura_min" },
            { "data": "temperatura_max" },
        ],
        initComplete: function () {
            setTimeout(function () {
                $('#loader').modal('hide');
            }, 500);


        },
        select: {
            style: 'single'
        }


    });




}

function editar_temperatura() {

    var table = $('#torques').DataTable();

    if (table.rows('.selected').any()) {


        var id = table.rows({ selected: true }).data()[0].horno_id;

        document.getElementById("temperaturaAlta").value = table.rows({ selected: true }).data()[0].temperatura_max;
        document.getElementById("temperaturaBaja").value = table.rows({ selected: true }).data()[0].temperatura_min;

        $('#editar_modal').modal('show');


    } else {

        swalInit.fire({
            title: '¡Sin selección!',
            text: 'Seleccionar una herraimenta',
            icon: 'warning',
            showCloseButton: true
        });

    }

}


function submit_editar() {

    var table = $('#torques').DataTable();

    var id = table.rows({ selected: true }).data()[0].id;

    var temperaturaAlta = document.getElementById("temperaturaAlta").value;
    var temperaturaBaja = document.getElementById("temperaturaBaja").value;


    $.ajax({

        type: "GET",
        url: "/Catalogo/edit_temp_wet_Json",
        data: { 'id': id, 'temperaturaAlta': temperaturaAlta, 'temperaturaBaja': temperaturaBaja },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                if (response['resultado'] == 0) {

                    $('#editar_modal').modal('hide');

                    reporteGenerar();

                    swalInit.fire({
                        title: '¡Editado Correctemente!',
                        text: 'Se ha editado el horno correctamente',
                        icon: 'success'
                    });


                }
                else {

                    $('#editarEstacion').modal('hide');
                    swalInit.fire({
                        title: '¡Error!',
                        text: response['mensaje'],
                        icon: 'error'
                    });

                }


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
