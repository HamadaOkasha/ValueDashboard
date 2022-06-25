/* ------------------------------------------------------------------------------
*
*  # Buttons extension for Datatables. Print examples
*
*  Demo JS code for datatable_extension_buttons_print.html page
*
* ---------------------------------------------------------------------------- */

document.addEventListener('DOMContentLoaded', function() {


    // Table setup
    // ------------------------------

    // Setting datatable defaults
    $.extend( $.fn.dataTable.defaults, {
        autoWidth: false,
        dom: '<"datatable-header"fBl><"datatable-scroll-wrap"t><"datatable-footer"ip>',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            searchPlaceholder: 'Type to filter...',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' }
        }
    });


    // Basic initialization
    $('.datatable-button-print-basic').DataTable({
        buttons: [
            {
                extend: 'print',
                text: '<i class="icon-printer position-left"></i> Print table',
                className: 'btn bg-info-800'
            }
        ]
    });


    // Disable auto print
    $('.datatable-button-print-disable').DataTable({
        buttons: [
            {
                extend: 'print',
                text: '<i class="icon-printer position-left"></i> Print table',
                className: 'btn bg-info-800',
                autoPrint: false
            }
        ]
    });


    // Export options - column selector
    

    // Export options - row selector
    $('.datatable-button-print-rows').DataTable({
        buttons: {
            buttons: [
                {
                    extend: 'print',
                    className: 'btn btn-light',
                    text: '<i class="icon-printer position-left"></i> Print all'
                },
                {
                    extend: 'print',
                    className: 'btn btn-light',
                    text: '<i class="icon-checkmark3 position-left"></i> Print selected',
                    exportOptions: {
                        modifier: {
                            selected: true
                        }
                    }
                }
            ],
        },
        select: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        stateSave: true,
        buttons: {            
            buttons: [
                {
                    extend: 'print',
                    className: 'btn btn-light',
                    text: '<i class="icon-printer position-left"></i> Print all'
                },
                {
                    extend: 'print',
                    className: 'btn btn-light',
                    text: '<i class="icon-checkmark3 position-left"></i> Print selected',
                    exportOptions: {
                        modifier: {
                            selected: true
                        }
                    }
                },
                {
                    extend: 'excelHtml5',
                    className: 'btn btn-light',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    className: 'btn btn-default',
                    exportOptions: {
                        columns: [0, 1, 2, 5]
                    }
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i> <span class="caret"></span>',
                    className: 'btn bg-info-800 btn-icon'
                },
                {
                    extend: 'excelHtml5',
                    className: 'btn btn-default',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
            ]
        },
        columnDefs: [{
            targets: -1, // Hide actions column
            visible: false
        }],
    });



    // External table additions
    // ------------------------------

    // Enable Select2 select for the length option
    $('.dataTables_length select').select2({
        minimumResultsForSearch: Infinity,
        width: 'auto'
    });
    
});
