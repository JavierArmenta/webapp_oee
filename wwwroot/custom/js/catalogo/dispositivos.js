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
            dropdownParent: $('#editarModal')
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
    Select2Selects.init();

    getLineas();
    getMaquinas();
    getConexiones();


    reporteGenerar();


});

function reporteGenerar() {

    $.ajax({

        type: "GET",
        url: "/Catalogo/get_dispositivos_Json",
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


    $('#torques').DataTable().destroy();

    // tableTorques.clear().draw();

    $('#torques').dataTable({

        "order": [],

        data: oResults,
        "columns": [
            { "data": "nombre" },
            { "data": "descripcion" },
            { "data": "direccion_ip" },
            { "data": "puerto" },
            { "data": "linea" },
            { "data": "maquina" },
            { "data": "conexion" },
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

function editar_dispositivo() {

    var table = $('#torques').DataTable();

    if (table.rows('.selected').any()) {


        var id = table.rows({ selected: true }).data()[0].id;

        document.getElementById("nombreEditar").value = table.rows({ selected: true }).data()[0].nombre;
        document.getElementById("descripcionEditar").value = table.rows({ selected: true }).data()[0].descripcion;
        document.getElementById("direccionIpEditar").value = table.rows({ selected: true }).data()[0].direccion_ip;
        document.getElementById("puertoEditar").value = table.rows({ selected: true }).data()[0].puerto;
    
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

    var id = table.rows({ selected: true }).data()[0].dispositivo_id;

    var nombre = $("#nombreEditar").val();
    var descripcion = $("#descripcionEditar").val();
    var direccion = $("#direccionIpEditar").val();
    var puerto = $("#puertoEditar").val();
    var linea = $("#lineaDropdownEditar").val();
    var maquina = $("#maquinaDropdownEditar").val();
    var conexion = $("#conexionDropdownEditar").val();


    $.ajax({

        type: "GET",
        url: "/Catalogo/edit_dispositivo_Json",
        data: { 'id': id, 'nombre': nombre, 'descripcion': descripcion, 'direccion': direccion, 'puerto': puerto, 'linea_id': linea, 'maquina_id': maquina, 'conexion_id': conexion },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                if (response['resultado'] == 0) {

                    $('#editar_modal').modal('hide');

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
                        title: 'Error!',
                        text: response['mensaje'],
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



function agregar_dispositivo() {

    document.getElementById("nombreAgregar").value = '';
    document.getElementById("descripcionAgregar").value = '';
    document.getElementById("direccionIpAgregar").value = '';
    document.getElementById("puertoAgregar").value = '';
    document.getElementById("nombreAgregar").value = '';


    $('#agregar_modal').modal('show');


}


function submit_agregar() {

    var nombre = $("#nombreAgregar").val();
    var descripcion = $("#descripcionAgregar").val();
    var direccion = $("#direccionIpAgregar").val();
    var puerto = $("#puertoAgregar").val();

    var linea_id = $("#lineaDropdownAgregar").val();
    var maquina_id = $("#maquinaDropdownAgregar").val();
    var conexion_id = $("#conexionDropdownAgregar").val();



    $.ajax({

        type: "GET",
        url: "/Catalogo/add_dispositivo_Json",
        data: { 'nombre': nombre, 'descripcion': descripcion, 'direccion': direccion, 'puerto': puerto, 'linea_id': linea_id, 'maquina_id': maquina_id, 'conexion_id': conexion_id },
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


/// 
// ------------------------
// dropdown 


function getLineas() {


    $.ajax({

        type: "GET",
        url: "/Catalogo/get_lineas_dropdown_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                var data = [];
                response.forEach(function (response) {

                    var obj = {
                        id: response.id,
                        text: response.nombre
                    };
                    data.push(obj);

                });


                $("#lineaDropdownAgregar").select2({
                    data: data,
                    dropdownParent: $('#agregar_modal')
                });

                $("#lineaDropdownEditar").select2({
                    data: data,
                    dropdownParent: $('#editar_modal')
                });

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

                
function getConexiones() {


    $.ajax({

        type: "GET",
        url: "/Catalogo/get_conexiones_dropdown_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                var data = [];
                response.forEach(function (response) {

                    var obj = {
                        id: response.id,
                        text: response.nombre
                    };
                    data.push(obj);

                });


                $("#conexionDropdownAgregar").select2({
                    data: data,
                    dropdownParent: $('#agregar_modal')
                });

                $("#conexionDropdownEditar").select2({
                    data: data,
                    dropdownParent: $('#editar_modal')
                });

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
                
function getMaquinas() {


    $.ajax({

        type: "GET",
        url: "/Catalogo/get_maquinas_dropdown_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                var data = [];
                response.forEach(function (response) {

                    var obj = {
                        id: response.id,
                        text: response.nombre
                    };
                    data.push(obj);

                });


                $("#maquinaDropdownAgregar").select2({
                    data: data,
                    dropdownParent: $('#agregar_modal')
                });

                $("#maquinaDropdownEditar").select2({
                    data: data,
                    dropdownParent: $('#editar_modal')
                });

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

