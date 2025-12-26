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


document.addEventListener('DOMContentLoaded', function () {

    Select2Selects.init();

    // get the name of tool 
    getBobinasNombres(1);
    // get the curretn atatus 
    status(0);
    getPLCs();
});

function getPLCs() {

    $.ajax({

        type: "GET",
        url: "/Herramientas/Get_PLCs_Habilitados_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                console.log(response);
                var data = [];
                response.forEach(function (response) {

                    var obj = {
                        id: response.id,
                        text: response.nombre + ' - ' + response.descripcion
                    };
                    data.push(obj);

                });


                $("#plcsDropdown").select2({
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

$("#plcsDropdown").on('change', function () {
    var plc = $("#plcsDropdown").val();
    getBobinasNombres(plc);
    status(plc-1)
});

function status(plc) {

    $.ajax({
        url: "/Control/get_status",
        data: { 'plc': plc },
        type: 'GET',
        cache: false,
        success: function (result) {

            result.forEach(function (item, index) {

                $('#bobina' + (index + 1)).empty();
                $('#bypass' + (index + 1)).empty();

                if (result[index].bobina == 1) {
                    $('#bobina' + (index + 1)).append($('<status-indicator  negative pulse></status-indicator>'));
                } else {
                    $('#bobina' + (index + 1)).append($('<status-indicator  ></status-indicator>'));
                }

                if (result[index].bypass == 1) {
                    $('#bypass' + (index + 1)).append($('<status-indicator  positive></status-indicator>'));
                } else {
                    $('#bypass' + (index + 1)).append($('<status-indicator  ></status-indicator>'));
                }
            });

            $('#loader').modal('hide');

        },
        failure: function (response) {
            console.log(response.responseText);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });

}
function getBobinasNombres(plc_id) {

    $.ajax({

        type: "GET",
        url: "/Control/Get_Nombres_Bobinas_Json",
        data: { 'plc_id': plc_id },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                for (let i = 0; i < 30; i++) {

                    var joint = 'nombreBobina' + (i + 1);
                    document.getElementById('nombreBobina' + (i + 1)).innerHTML = response[i].nombre;
                    //document.getElementById('nombreBobina10' ).innerHTML = response[i].nombre;

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



function boton_bobina(boton) {

    var plc = $("#plcsDropdown").val();

    $('#loader').modal('show');

    $.ajax({

        type: "GET",
        url: "/Control/Activar_Bobina",
        data: { 'plc': plc , 'bobina': boton },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                if (response.resultado == 1) {


                    setTimeout(function () {

                        var plc = $("#plcsDropdown").val();
                        status(plc - 1)

                    }, 1000);


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


function boton_bypass(boton) {

    var plc = $("#plcsDropdown").val();

    $('#loader').modal('show');

    $.ajax({

        type: "GET",
        url: "/Control/Activar_Bypass",
        data: { 'plc': plc, 'bypass': boton },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                if (response.resultado == 1) {


                    setTimeout(function () {

                        var plc = $("#plcsDropdown").val();
                        status(plc - 1)

                    }, 1000);


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

