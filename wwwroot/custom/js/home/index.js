

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

var Grafica_3Wet_element = document.getElementById('Grafica_3Wet');
var Grafica_3Wet_options = {

    // Define colors

    // Global text styles
    textStyle: {
        fontFamily: 'Roboto, Arial, Verdana, sans-serif',
        fontSize: 13
    },

    // Chart animation duration
    animationDuration: 750,

    // Setup grid
    grid: {
        left: 0,
        right: 40,
        top: 35,
        bottom: 0,
        containLabel: true
    },

    // Add legend
    legend: {
        data: ['Maximum', 'Minimum'],
        itemHeight: 8,
        itemGap: 20
    },

     // Add tooltip
    tooltip: {
        trigger: 'axis',
        backgroundColor: 'rgba(0,0,0,0.75)',
        padding: [10, 15],
        textStyle: {
            fontSize: 13,
            fontFamily: 'Roboto, sans-serif'
        }
    },

    // Horizontal axis
    xAxis: [{
        type: 'category',
        boundaryGap: false,
        data: ['0 Min', '5 Min', '10 Min', '15 Min', '20 Min', '25 Min', '35 Min', '40 Min', '45 Min', '50 Min', '55 Min', '60 Min', '65 Min', '70 Min'],
        axisLabel: {
            color: '#333'
        },
        axisLine: {
            lineStyle: {
                color: '#999'
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        }
    }],

    // Vertical axis
    yAxis: [{
        type: 'value',
        min: 100,

        axisLabel: {
            formatter: '{value} °C',
            color: '#333'
        },
        axisLine: {
            lineStyle: {
                color: '#999'
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        },
        splitArea: {
            show: true,
            areaStyle: {
                color: ['rgba(250,250,250,0.1)', 'rgba(0,0,0,0.01)']
            }
        }
    }],

    // Add series
    series: [
        
        {
            data: [, , 200, 200, 200, 196, 193, 190, 186, 183, 180, 176],
            showSymbol: false,
            type: 'line',
            tooltip: {
                show: false
            },
            lineStyle: {
                normal: {
                    color: 'red',
                    width: 1,
                    type: 'dashed'
                }
            }
        },
        {
            data: [, , 165, 165, 165, 161, 158, 155, 152, 149, 146, 143],
            showSymbol: false,
            type: 'line',
            tooltip: {
                show: false
            },
            lineStyle: {
                normal: {
                    color: 'green',
                    width: 1,
                    type: 'dashed'
                }
            }
        },
        {
            name: 'Temperatura',
            data: [110, 100, 180, 185, 190, 195, 185, 170, 170, 185, 160, 175,180,176],
            showSymbol: false,
            type: 'line',
            smooth: 0.6,
            lineStyle: {
                normal: {
                    color: 'gray',
                    width: 1,
                }
            }
        }
    ]
};
var Grafica_3Wet = echarts.init(Grafica_3Wet_element);



var Grafica_ECoat_element = document.getElementById('Grafica_ECoat');
var Grafica_ECoat_options = {

    // Define colors

    // Global text styles
    textStyle: {
        fontFamily: 'Roboto, Arial, Verdana, sans-serif',
        fontSize: 13
    },

    // Chart animation duration
    animationDuration: 750,

    // Setup grid
    grid: {
        left: 0,
        right: 40,
        top: 35,
        bottom: 0,
        containLabel: true
    },

    // Add legend
    legend: {
        data: ['Maximum', 'Minimum'],
        itemHeight: 8,
        itemGap: 20
    },

    // Add tooltip
    tooltip: {
        trigger: 'axis',
        backgroundColor: 'rgba(0,0,0,0.75)',
        padding: [10, 15],
        textStyle: {
            fontSize: 13,
            fontFamily: 'Roboto, sans-serif'
        }
    },

    // Horizontal axis
    xAxis: [{
        type: 'category',
        boundaryGap: false,
        data: ['0 Min', '5 Min', '10 Min', '15 Min', '20 Min', '25 Min', '35 Min', '40 Min', '45 Min', '50 Min', '55 Min', '60 Min', '65 Min', '70 Min'],
        axisLabel: {
            color: '#333'
        },
        axisLine: {
            lineStyle: {
                color: '#999'
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        }
    }],

    // Vertical axis
    yAxis: [{
        type: 'value',
        min: 100,

        axisLabel: {
            formatter: '{value} °C',
            color: '#333'
        },
        axisLine: {
            lineStyle: {
                color: '#999'
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        },
        splitArea: {
            show: true,
            areaStyle: {
                color: ['rgba(250,250,250,0.1)', 'rgba(0,0,0,0.01)']
            }
        }
    }],

    // Add series
    series: [

        {
            data: [, , 200, 200, 200, 196, 193, 190, 186, 183, 180, 176],
            showSymbol: false,
            type: 'line',
            tooltip: {
                show: false
            },
            lineStyle: {
                normal: {
                    color: 'red',
                    width: 1,
                    type: 'dashed'
                }
            }
        },
        {
            data: [, , 165, 165, 165, 161, 158, 155, 152, 149, 146, 143],
            showSymbol: false,
            type: 'line',
            tooltip: {
                show: false
            },
            lineStyle: {
                normal: {
                    color: 'green',
                    width: 1,
                    type: 'dashed'
                }
            }
        },
        {
            name: 'Temperatura',
            data: [110, 100, 180, 185, 190, 195, 185, 170, 170, 185, 160, 175, 180, 176],
            showSymbol: false,
            type: 'line',
            smooth: 0.6,
            lineStyle: {
                normal: {
                    color: 'gray',
                    width: 1,
                }
            }
        }
    ]
};
var Grafica_ECoat = echarts.init(Grafica_ECoat_element);

var Grafica_Horno2Tonos_element = document.getElementById('Grafica_Horno2Tonos');
var Grafica_Horno2Tonos_options = {

    // Define colors

    // Global text styles
    textStyle: {
        fontFamily: 'Roboto, Arial, Verdana, sans-serif',
        fontSize: 13
    },

    // Chart animation duration
    animationDuration: 750,

    // Setup grid
    grid: {
        left: 0,
        right: 40,
        top: 35,
        bottom: 0,
        containLabel: true
    },

    // Add legend
    legend: {
        data: ['Maximum', 'Minimum'],
        itemHeight: 8,
        itemGap: 20
    },

    // Add tooltip
    tooltip: {
        trigger: 'axis',
        backgroundColor: 'rgba(0,0,0,0.75)',
        padding: [10, 15],
        textStyle: {
            fontSize: 13,
            fontFamily: 'Roboto, sans-serif'
        }
    },

    // Horizontal axis
    xAxis: [{
        type: 'category',
        boundaryGap: false,
        data: ['0 Min', '5 Min', '10 Min', '15 Min', '20 Min', '25 Min', '35 Min', '40 Min', '45 Min', '50 Min', '55 Min', '60 Min', '65 Min', '70 Min'],
        axisLabel: {
            color: '#333'
        },
        axisLine: {
            lineStyle: {
                color: '#999'
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        }
    }],

    // Vertical axis
    yAxis: [{
        type: 'value',
        min: 100,

        axisLabel: {
            formatter: '{value} °C',
            color: '#333'
        },
        axisLine: {
            lineStyle: {
                color: '#999'
            }
        },
        splitLine: {
            lineStyle: {
                color: ['#eee']
            }
        },
        splitArea: {
            show: true,
            areaStyle: {
                color: ['rgba(250,250,250,0.1)', 'rgba(0,0,0,0.01)']
            }
        }
    }],

    // Add series
    series: [

        {
            data: [, , 200, 200, 200, 196, 193, 190, 186, 183, 180, 176],
            showSymbol: false,
            type: 'line',
            tooltip: {
                show: false
            },
            lineStyle: {
                normal: {
                    color: 'red',
                    width: 1,
                    type: 'dashed'
                }
            }
        },
        {
            data: [, , 165, 165, 165, 161, 158, 155, 152, 149, 146, 143],
            showSymbol: false,
            type: 'line',
            tooltip: {
                show: false
            },
            lineStyle: {
                normal: {
                    color: 'green',
                    width: 1,
                    type: 'dashed'
                }
            }
        },
        {
            name: 'Temperatura',
            data: [110, 100, 180, 185, 190, 195, 185, 170, 170, 185, 160, 175, 180, 176],
            showSymbol: false,
            type: 'line',
            smooth: 0.6,
            lineStyle: {
                normal: {
                    color: 'gray',
                    width: 1,
                }
            }
        }
    ]
};
var Grafica_Horno2Tonos = echarts.init(Grafica_Horno2Tonos_element);


// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    DatatableBasic.init();
    //Grafica_3Wet.setOption(Grafica_3Wet_options, true);


    //Grafica_ECoat.setOption(Grafica_3Wet_options, true);
    //Grafica_Horno2Tonos.setOption(Grafica_3Wet_options, true);
    dashboard_wet();
    dashboard_ecoat();
    dashboard_2tonos();

    dashboard_temp_wet();
    dashboard_temp_ecoat();
    dashboard_temp_2tonos();


    horno_ecoat();
    horno_dos_tonos();
    horno_wet();


    const interval = setInterval(function () {
        dashboard_wet();
        dashboard_ecoat();
        dashboard_2tonos();

        dashboard_temp_wet();
        dashboard_temp_ecoat();
        dashboard_temp_2tonos();

    }, 3000);

    

});




function dashboard_temp_wet() {

    $.ajax({

        type: "GET",
        url: "/Home/dashboard_temp_wet_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                console.log(response[0].temp_1);

                document.getElementById("temperatura_wet").textContent = response[0].temp_1 + ' C°';


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
function dashboard_temp_ecoat() {

    $.ajax({

        type: "GET",
        url: "/Home/dashboard_temp_ecoat_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                console.log(response);

                document.getElementById("temperatura_ecoat").textContent = response[0].temp_1 + ' C°';



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
function dashboard_temp_2tonos() {

    $.ajax({

        type: "GET",
        url: "/Home/dashboard_temp_2tonos_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                document.getElementById("temperatura_dos_tonos").textContent = response[0].temp_1 + ' C°';


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

function dashboard_wet() {

    $.ajax({

        type: "GET",
        url: "/Home/dashboard_wet_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                console.log(response[0].temp_1);

                document.getElementById("3Wet").textContent = 'Vin: ' + response[0].vin;



                Grafica_3Wet_options.series[2].data[0] =  response[0].temp_1;
                Grafica_3Wet_options.series[2].data[1] =  response[0].temp_2;
                Grafica_3Wet_options.series[2].data[2] =  response[0].temp_3;
                Grafica_3Wet_options.series[2].data[3] =  response[0].temp_4;
                Grafica_3Wet_options.series[2].data[4] =  response[0].temp_5;
                Grafica_3Wet_options.series[2].data[5] =  response[0].temp_6;
                Grafica_3Wet_options.series[2].data[6] =  response[0].temp_7;
                Grafica_3Wet_options.series[2].data[7] =  response[0].temp_8;
                Grafica_3Wet_options.series[2].data[8] =  response[0].temp_9;
                Grafica_3Wet_options.series[2].data[9] =  response[0].temp_10;
                Grafica_3Wet_options.series[2].data[10] = response[0].temp_11;
                Grafica_3Wet_options.series[2].data[11] = response[0].temp_12;
                Grafica_3Wet_options.series[2].data[12] = response[0].temp_13;
                Grafica_3Wet_options.series[2].data[13] = response[0].temp_14;
                Grafica_3Wet_options.series[2].data[14] = response[0].temp_15;

                Grafica_3Wet_options.series[0].data[0] = '';
                Grafica_3Wet_options.series[0].data[1] = '';
                Grafica_3Wet_options.series[0].data[2] =  response[1].temp_3;
                Grafica_3Wet_options.series[0].data[3] =  response[1].temp_4;
                Grafica_3Wet_options.series[0].data[4] =  response[1].temp_5;
                Grafica_3Wet_options.series[0].data[5] =  response[1].temp_6;
                Grafica_3Wet_options.series[0].data[6] =  response[1].temp_7;
                Grafica_3Wet_options.series[0].data[7] =  response[1].temp_8;
                Grafica_3Wet_options.series[0].data[8] =  response[1].temp_9;
                Grafica_3Wet_options.series[0].data[9] =  response[1].temp_10;
                Grafica_3Wet_options.series[0].data[10] = response[1].temp_11;
                Grafica_3Wet_options.series[0].data[11] = response[1].temp_12;
                Grafica_3Wet_options.series[0].data[12] = '';
                Grafica_3Wet_options.series[0].data[13] = '';
                Grafica_3Wet_options.series[0].data[14] = '';

                Grafica_3Wet_options.series[1].data[0] = '';
                Grafica_3Wet_options.series[1].data[1] = '';
                Grafica_3Wet_options.series[1].data[2] =  response[2].temp_3;
                Grafica_3Wet_options.series[1].data[3] =  response[2].temp_4;
                Grafica_3Wet_options.series[1].data[4] =  response[2].temp_5;
                Grafica_3Wet_options.series[1].data[5] =  response[2].temp_6;
                Grafica_3Wet_options.series[1].data[6] =  response[2].temp_7;
                Grafica_3Wet_options.series[1].data[7] =  response[2].temp_8;
                Grafica_3Wet_options.series[1].data[8] =  response[2].temp_9;
                Grafica_3Wet_options.series[1].data[9] =  response[2].temp_10;
                Grafica_3Wet_options.series[1].data[10] = response[2].temp_11;
                Grafica_3Wet_options.series[1].data[11] = response[2].temp_12;
                Grafica_3Wet_options.series[1].data[12] = '';
                Grafica_3Wet_options.series[1].data[13] = '';
                Grafica_3Wet_options.series[1].data[14] = '';


                Grafica_3Wet.setOption(Grafica_3Wet_options, true);



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

function dashboard_ecoat() {

    $.ajax({

        type: "GET",
        url: "/Home/dashboard_ecoat_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                console.log(response);

                document.getElementById("ECoat").textContent = 'Vin: ' + response[0].vin;

                Grafica_ECoat_options.series[2].data[0] =  response[0].temp_1;
                Grafica_ECoat_options.series[2].data[1] =  response[0].temp_2;
                Grafica_ECoat_options.series[2].data[2] =  response[0].temp_3;
                Grafica_ECoat_options.series[2].data[3] =  response[0].temp_4;
                Grafica_ECoat_options.series[2].data[4] =  response[0].temp_5;
                Grafica_ECoat_options.series[2].data[5] =  response[0].temp_6;
                Grafica_ECoat_options.series[2].data[6] =  response[0].temp_7;
                Grafica_ECoat_options.series[2].data[7] =  response[0].temp_8;
                Grafica_ECoat_options.series[2].data[8] =  response[0].temp_9;
                Grafica_ECoat_options.series[2].data[9] =  response[0].temp_10;
                Grafica_ECoat_options.series[2].data[10] = response[0].temp_11;
                Grafica_ECoat_options.series[2].data[11] = response[0].temp_12;
                Grafica_ECoat_options.series[2].data[12] = response[0].temp_13;
                Grafica_ECoat_options.series[2].data[13] = response[0].temp_14;
                Grafica_ECoat_options.series[2].data[14] = response[0].temp_15;

                Grafica_ECoat_options.series[0].data[0] = '';
                Grafica_ECoat_options.series[0].data[1] = '';
                Grafica_ECoat_options.series[0].data[2] =  response[1].temp_3;
                Grafica_ECoat_options.series[0].data[3] =  response[1].temp_4;
                Grafica_ECoat_options.series[0].data[4] =  response[1].temp_5;
                Grafica_ECoat_options.series[0].data[5] =  response[1].temp_6;
                Grafica_ECoat_options.series[0].data[6] =  response[1].temp_7;
                Grafica_ECoat_options.series[0].data[7] =  response[1].temp_8;
                Grafica_ECoat_options.series[0].data[8] =  response[1].temp_9;
                Grafica_ECoat_options.series[0].data[9] =  response[1].temp_10;
                Grafica_ECoat_options.series[0].data[10] = response[1].temp_11;
                Grafica_ECoat_options.series[0].data[11] = response[1].temp_12;
                Grafica_ECoat_options.series[0].data[12] = '';
                Grafica_ECoat_options.series[0].data[13] = '';
                Grafica_ECoat_options.series[0].data[14] = '';

                Grafica_ECoat_options.series[1].data[0] = '';
                Grafica_ECoat_options.series[1].data[1] = '';
                Grafica_ECoat_options.series[1].data[2] =  response[2].temp_3;
                Grafica_ECoat_options.series[1].data[3] =  response[2].temp_4;
                Grafica_ECoat_options.series[1].data[4] =  response[2].temp_5;
                Grafica_ECoat_options.series[1].data[5] =  response[2].temp_6;
                Grafica_ECoat_options.series[1].data[6] =  response[2].temp_7;
                Grafica_ECoat_options.series[1].data[7] =  response[2].temp_8;
                Grafica_ECoat_options.series[1].data[8] =  response[2].temp_9;
                Grafica_ECoat_options.series[1].data[9] =  response[2].temp_10;
                Grafica_ECoat_options.series[1].data[10] = response[2].temp_11;
                Grafica_ECoat_options.series[1].data[11] = response[2].temp_12;
                Grafica_ECoat_options.series[1].data[12] = '';
                Grafica_ECoat_options.series[1].data[13] = '';
                Grafica_ECoat_options.series[1].data[14] = '';

                Grafica_ECoat.setOption(Grafica_ECoat_options, true);


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
function dashboard_2tonos() {

    $.ajax({

        type: "GET",
        url: "/Home/dashboard_2tonos_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {


                document.getElementById("Horno2Tonos").textContent = 'Vin: ' + response[0].vin;

                Grafica_Horno2Tonos_options.series[2].data[0] =  response[0].temp_1;
                Grafica_Horno2Tonos_options.series[2].data[1] =  response[0].temp_2;
                Grafica_Horno2Tonos_options.series[2].data[2] =  response[0].temp_3;
                Grafica_Horno2Tonos_options.series[2].data[3] =  response[0].temp_4;
                Grafica_Horno2Tonos_options.series[2].data[4] =  response[0].temp_5;
                Grafica_Horno2Tonos_options.series[2].data[5] =  response[0].temp_6;
                Grafica_Horno2Tonos_options.series[2].data[6] =  response[0].temp_7;
                Grafica_Horno2Tonos_options.series[2].data[7] =  response[0].temp_8;
                Grafica_Horno2Tonos_options.series[2].data[8] =  response[0].temp_9;
                Grafica_Horno2Tonos_options.series[2].data[9] =  response[0].temp_10;
                Grafica_Horno2Tonos_options.series[2].data[10] = response[0].temp_11;
                Grafica_Horno2Tonos_options.series[2].data[11] = response[0].temp_12;
                Grafica_Horno2Tonos_options.series[2].data[12] = response[0].temp_13;
                Grafica_Horno2Tonos_options.series[2].data[13] = response[0].temp_14;
                Grafica_Horno2Tonos_options.series[2].data[14] = response[0].temp_15;

                Grafica_Horno2Tonos_options.series[0].data[0] = '';
                Grafica_Horno2Tonos_options.series[0].data[1] = '';
                Grafica_Horno2Tonos_options.series[0].data[2] =  response[1].temp_3;
                Grafica_Horno2Tonos_options.series[0].data[3] =  response[1].temp_4;
                Grafica_Horno2Tonos_options.series[0].data[4] =  response[1].temp_5;
                Grafica_Horno2Tonos_options.series[0].data[5] =  response[1].temp_6;
                Grafica_Horno2Tonos_options.series[0].data[6] =  response[1].temp_7;
                Grafica_Horno2Tonos_options.series[0].data[7] =  response[1].temp_8;
                Grafica_Horno2Tonos_options.series[0].data[8] =  response[1].temp_9;
                Grafica_Horno2Tonos_options.series[0].data[9] =  response[1].temp_10;
                Grafica_Horno2Tonos_options.series[0].data[10] = response[1].temp_11;
                Grafica_Horno2Tonos_options.series[0].data[11] = response[1].temp_12;
                Grafica_Horno2Tonos_options.series[0].data[12] = '';
                Grafica_Horno2Tonos_options.series[0].data[13] = '';
                Grafica_Horno2Tonos_options.series[0].data[14] = '';

                Grafica_Horno2Tonos_options.series[1].data[0] = '';
                Grafica_Horno2Tonos_options.series[1].data[1] = '';
                Grafica_Horno2Tonos_options.series[1].data[2] =  response[2].temp_3;
                Grafica_Horno2Tonos_options.series[1].data[3] =  response[2].temp_4;
                Grafica_Horno2Tonos_options.series[1].data[4] =  response[2].temp_5;
                Grafica_Horno2Tonos_options.series[1].data[5] =  response[2].temp_6;
                Grafica_Horno2Tonos_options.series[1].data[6] =  response[2].temp_7;
                Grafica_Horno2Tonos_options.series[1].data[7] =  response[2].temp_8;
                Grafica_Horno2Tonos_options.series[1].data[8] =  response[2].temp_9;
                Grafica_Horno2Tonos_options.series[1].data[9] =  response[2].temp_10;
                Grafica_Horno2Tonos_options.series[1].data[10] = response[2].temp_11;
                Grafica_Horno2Tonos_options.series[1].data[11] = response[2].temp_12;
                Grafica_Horno2Tonos_options.series[1].data[12] = '';
                Grafica_Horno2Tonos_options.series[1].data[13] = '';
                Grafica_Horno2Tonos_options.series[1].data[14] = '';

                Grafica_Horno2Tonos.setOption(Grafica_Horno2Tonos_options, true);


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



function horno_dos_tonos() {

    $.ajax({

        type: "GET",
        url: "/Home/fifo_2tonos_ng_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                LoadDosTonos(response);

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

function LoadDosTonos(oResults) {

    $('#tabla_dos_tonos').DataTable().destroy();

    // tableTorques.clear().draw();

    $('#tabla_dos_tonos').dataTable({

        "order": [],


        data: oResults,
        "columns": [
            { "data": "posicion" },
            { "data": "vin" },
            { "data": "sobrehorneado" },
            { "data": "subhorneado" },

        ]
    });



}


function horno_wet() {

    $.ajax({

        type: "GET",
        url: "/Home/fifo_wet_ng_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                LoadWet(response);

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

function LoadWet(oResults) {

    $('#tabla_wet').DataTable().destroy();

    // tableTorques.clear().draw();

    $('#tabla_wet').dataTable({

        "order": [],


        data: oResults,
        "columns": [
            { "data": "posicion" },
            { "data": "vin" },
            { "data": "sobrehorneado" },
            { "data": "subhorneado" },

        ]
    });



}


function horno_ecoat() {

    $.ajax({

        type: "GET",
        url: "/Home/fifo_ecoat_ng_Json",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != null) {

                LoadEcoat(response);

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

function LoadEcoat(oResults) {

    $('#tabla_ecoat').DataTable().destroy();

    // tableTorques.clear().draw();

    $('#tabla_ecoat').dataTable({

        "order": [],


        data: oResults,
        "columns": [
            { "data": "posicion" },
            { "data": "vin" },
            { "data": "sobrehorneado" },
            { "data": "subhorneado" },

        ]
    });



}
