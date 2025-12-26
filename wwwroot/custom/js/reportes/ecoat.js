/* ------------------------------------------------------------------------------
 *
 *  # Basic datatables
 *
 *  Demo JS code for datatable_basic.html page
 *
 * ---------------------------------------------------------------------------- */


// Setup initial modules
// ------------------------------


var DatatableFixedColumns = function () {


    //
    // Setup module components
    //

    // Basic Datatable examples
    var _componentDatatableFixedColumns = function () {
        if (!$().DataTable) {
            console.warn('Warning - datatables.min.js is not loaded.');
            return;
        }

        // Setting datatable defaults
        $.extend($.fn.dataTable.defaults, {
            columnDefs: [{
                orderable: false,
                width: 100,
                targets: [5]
            }],
            dom: '<"datatable-header"fBl><"datatable-scroll datatable-scroll-wrap"t><"datatable-footer"ip>',
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


        // Left fixed column example
        $('.datatable-fixed-left').DataTable({
            columnDefs: [
                {
                    orderable: false,
                    targets: [5]
                },
                {
                    width: "200px",
                    targets: [0]
                },
                {
                    width: "300px",
                    targets: [1]
                },
                {
                    width: "200px",
                    targets: [5, 6]
                },
                {
                    width: "100px",
                    targets: [4]
                }
            ],
            scrollX: true,
            scrollY: '350px',
            scrollCollapse: true,
            fixedColumns: true,
            buttons: {
                dom: {
                    button: {
                        className: 'btn btn-light'
                    }
                },
                buttons: [
                    'copyHtml5',
                    'excelHtml5',
                    'csvHtml5'
                ]
            },
            "bDestroy": true
        });



        // Adjust columns on window resize
        setTimeout(function () {
            $(window).on('resize', function () {
                table.columns.adjust();
            });
        }, 100);
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _componentDatatableFixedColumns();
        }
    }
}();


var DateTimePickers = function () {


    //
    // Setup module components
    //

    // Daterange picker
    var _componentDaterange = function () {
        if (!$().daterangepicker) {
            console.warn('Warning - daterangepicker.js is not loaded.');
            return;

        }

        // Basic initialization
        $('.daterange-basic').daterangepicker({
            parentEl: '.content-inner',

        });

        $('.daterange-time').daterangepicker({
            parentEl: '.content-inner',
            timePicker: true,
            locale: {
                format: 'MM/DD/YYYY h:mm a'
            }
        });
    };

    // Pickadate picker
    var _componentPickadate = function () {
        if (!$().pickadate) {
            console.warn('Warning - picker.js and/or picker.date.js is not loaded.');
            return;
        }

        // Basic options
        $('.pickadate').pickadate();


    };

    // Pickatime picker
    var _componentPickatime = function () {
        if (!$().pickatime) {
            console.warn('Warning - picker.js and/or picker.time.js is not loaded.');
            return;
        }

        // Default functionality
        $('.pickatime').pickatime();


    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _componentDaterange();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    DatatableFixedColumns.init();
    DateTimePickers.init();


});

function reporteGenerar() {


    $('#loader').modal('show');

    var fechaInicial = $('#datetimepicker').data('daterangepicker').startDate.format('YYYY-MM-DD HH:mm');
    var fechaFinal = $('#datetimepicker').data('daterangepicker').endDate.format('YYYY-MM-DD HH:mm');

    $.ajax({

        type: "GET",
        url: "/Reportes/reporte_get_ecoat_Json",
        data: { 'fechaInicial': fechaInicial, 'fechaFinal': fechaFinal },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                LoadCurrentReport(response);
                DatatableFixedColumns.init();

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

        select: {
            style: 'single'
        },

        "order": [],


        data: oResults,
        "columns": [
            { "data": "vin" },
            { "data": "temp_1" },
            { "data": "temp_2" },
            { "data": "temp_3" },
            { "data": "temp_4" },
            { "data": "temp_5" },
            { "data": "temp_6" },
            { "data": "temp_7" },
            { "data": "temp_8" },
            { "data": "temp_9" },
            { "data": "temp_10" },
            { "data": "temp_11" },
            { "data": "temp_12" },
            { "data": "temp_13" },
            { "data": "temp_14" },
            { "data": "temp_15" },
            //{ 'data': null, title: 'Action', wrap: true, "render": function () { return '<div class="btn-group"> <button type="button" onclick=" rowDataGet () " class="btn btn-light" >Memo</button></div>' } },       
        ],
        initComplete: function () {
            setTimeout(function () {
                $('#loader').modal('hide');
            }, 500);


        }
    });

    $('#loader').modal('hide');


}

function getLineas() {

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

                console.log(data);

                $("#lineasDropdown").select2({
                    data: data
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
