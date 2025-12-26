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




// ------------------------------
// Initialize module




document.addEventListener('DOMContentLoaded', function () {
    DatatableBasic.init();

    reporteGenerar();


});

function reporteGenerar() {

    $.ajax({

        type: "GET",
        url: "/Catalogo/get_lineas_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                console.log(response);

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


    $('#torques').DataTable().destroy();

    // tableTorques.clear().draw();

    $('#torques').dataTable({

        "order": [],

        data: oResults,
        "columns": [
            { "data": "nombre" },
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

function editar_herramienta(){

    var table = $('#torques').DataTable();

    if (table.rows('.selected').any()) {

        var id = table.rows({ selected: true }).data()[0].id;

        document.getElementById("nombre").value = table.rows({ selected: true }).data()[0].nombre;

        $('#modal_form_vertical').modal('show');


    } else {

        swalInit.fire({
            title: '¡Sin selección!',
            text: 'Seleccionar una herraimenta',
            icon: 'warning',
            showCloseButton: true
        });

    }

}

function submit_herramienta() {

    var table = $('#torques').DataTable();

    var id = table.rows({ selected: true }).data()[0].id;

    var nombre = $("#nombre").val();


    $.ajax({

        type: "GET",
        url: "/Catalogo/edit_linea_Json",
        data: { 'id': id, 'nombre': nombre },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                if (response['resultado'] == 0) {

                    $('#modal_form_vertical').modal('hide');

                    reporteGenerar();

                    swalInit.fire({
                        title: '¡Editado Correctemente!',
                        text: 'Se ha editado la herramienta correctamente',
                        icon: 'success'
                    });
                    

                }
                else {

                    $('#editarEstacion').modal('hide');
                    swalInit.fire({
                        title: 'Good job!',
                        text: 'You clicked the button!',
                        icon: 'success'
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



function agregar_herramienta() {

        document.getElementById("nombreAgregar").value = '';

        $('#agregar_modal').modal('show');


}

function submit_agregar() {

    var table = $('#torques').DataTable();

    var nombre = $("#nombreAgregar").val();


    $.ajax({

        type: "GET",
        url: "/Catalogo/add_linea_Json",
        data: { 'nombre': nombre },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                if (response['resultado'] == 0) {

                    $('#agregar_modal').modal('hide');

                    reporteGenerar();

                    swalInit.fire({
                        title: '¡Agregado Correctamente!',
                        text: 'Se ha agregado correctamente',
                        icon: 'success'
                    });


                }
                else {

                    $('#editarEstacion').modal('hide');
                    swalInit.fire({
                        title: 'Good job!',
                        text: 'You clicked the button!',
                        icon: 'success'
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

