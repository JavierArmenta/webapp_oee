

document.addEventListener('DOMContentLoaded', function () {

       // get the name of tool 
    getBobinasNombres(1);
    // get the curretn atatus 
    status(0);
    getPLCs();


    const interval = setInterval(function () {
        var plc = $("#plcsDropdown").val();
        status(plc - 1)
        getBobinasNombres(plc);
    }, 5000);

});





$("#plcsDropdown").on('change', function () {
    var plc = $("#plcsDropdown").val();
    getBobinasNombres(plc);
    status(plc - 1)
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

function status(plc) {

    $.ajax({
        url: "/Control/get_status",
        data: { 'plc': plc },
        type: 'GET',
        cache: false,
        success: function (result) {


            result.forEach(function (item, index) {

                $('#bobina' + (index + 1) ).empty();

                if (result[index].bobina == 1) {
                    $('#bobina' + (index + 1)).append($('<status-indicator  negative pulse></status-indicator>'));
                } else {
                    $('#bobina' + (index + 1)).append($('<status-indicator  ></status-indicator>'));
                }



                
            });

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

                    var joint = 'nombreBobina' + (i+1);
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