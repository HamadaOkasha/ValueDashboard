/* ------------------------------------------------------------------------------
 *
 *  # Google Visualization - columns
 *
 *  Google Visualization column chart demonstration
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var GoogleColumnBasic = function() {


    //
    // Setup module components
    //

    // Column chart
    var _googleColumnBasic = function() {
        if (typeof google == 'undefined') {
            console.warn('Warning - Google Charts library is not loaded.');
            return;
        }

        // Initialize chart
        google.charts.load('current', {
            callback: function () {
                // Draw chart
                drawColumn();
                // Resize on sidebar width change
                $(document).on('click', '.sidebar-control', drawColumn);
                // Resize on window resize
                var resizeColumn;
                $(window).on('resize', function() {
                    clearTimeout(resizeColumn);
                    resizeColumn = setTimeout(function () {
                        drawColumn();
                    }, 200);
                });
            },
            packages: ['corechart']
        });

        // Chart settings
        function drawColumn() {

            // Define charts element
            var line_chart_element = document.getElementById('orders-visits-column');

            // Data
            var data = google.visualization.arrayToDataTable([
                ['Month', 'عدد طلبات الخدمات', 'عدد الزيارات'],
                ['يناير',  125,      360],
                ['فبراير',  160,      260],
                ['مارس',  150,       120],
                ['إبريل',  100,      340]                ,
                ['مايو',  123,      200],
                ['يونيو',  113,      260],
                ['يوليو',  178,       120],
                ['أغسطس',  190,      160],
                ['سبتمبر',  105,     120],
                ['أكتوبر',  160,      260],
                ['نوفمبر',  70,       220],
                ['ديسمبر',  110,      340]
            ]);


            // Options
            var options_column = {
                fontName: 'Roboto',
                height: 400,
                fontSize: 12,
                chartArea: {
                    left: '5%',
                    width: '94%',
                    height: 350
                },
                tooltip: {
                    textStyle: {
                        fontName: 'Roboto',
                        fontSize: 13
                    }
                },
                vAxis: {
                    title: 'طلبات الخدمات و الزيارات',
                    titleTextStyle: {
                        fontSize: 13,
                        italic: false
                    },
                    gridlines:{
                        color: '#e5e5e5',
                        count: 10
                    },
                    minValue: 0
                },
                colors: ['#ffa726','#00838f'],
                legend: {
                    position: 'top',
                    alignment: 'center',
                    textStyle: {
                        fontSize: 12
                    }
                }
            };

            // Draw chart
            var column = new google.visualization.ColumnChart(line_chart_element);
            column.draw(data, options_column);
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _googleColumnBasic();
        }
    }
}();


// Initialize module
// ------------------------------

GoogleColumnBasic.init();
