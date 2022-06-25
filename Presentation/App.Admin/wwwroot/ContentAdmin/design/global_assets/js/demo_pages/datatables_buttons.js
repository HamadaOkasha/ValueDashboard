/* ------------------------------------------------------------------------------
 *
 *  # Buttons extension for Datatables. HTML5 examples
 *
 *  Demo JS code for datatable_extension_buttons_html5.html page
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var DatatableButtonsHtml5 = function() {


    //
    // Setup module components
    //

    // Basic Datatable examples
    var _componentDatatableButtonsHtml5 = function() {
        if (!$().DataTable) {
            console.warn('Warning - datatables.min.js is not loaded.');
            return;
        }

        // Setting datatable defaults
        $.extend( $.fn.dataTable.defaults, {
            autoWidth: false,
            dom: '<"datatable-header"fBl><"datatable-scroll-wrap"t><"datatable-footer"ip>',
            language: {
                search: '<span>بحث:</span> _INPUT_',
                searchPlaceholder: ' كلمة البحث ...',
                lengthMenu: '<span>عرض:</span> _MENU_',
                paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' },
                info: 'إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل',
                Processing: "جارٍ التحميل...",
                LengthMen: "أظهر _MENU_ مدخلات",
                ZeroRecords: "لم يعثر على أية سجلات",
                InfoEmpty: "يعرض 0 إلى 0 من أصل 0 سجل",
                InfoFiltered: "(منتقاة من مجموع _MAX_ مُدخل)",

            }
        });


        // Basic initialization
        $('.datatable-button-html5-basic').DataTable({
            buttons: {            
                dom: {
                    button: {
                        className: 'btn btn-light',
                    }
                },
                buttons: [
                    // {
                    //     extend: 'copyHtml5',
                    //     text: 'نسخ <i class="icon-copy3"></i>',
                    // },
                    {
                        extend: 'excelHtml5',
                        text: 'تصدير أكسل <i class="icon-file-excel"></i>',
                    }/*,
                    {
                        extend: 'pdfHtml5',
                        text: 'تصدير PDF <i class="icon-file-pdf"></i>',
                    }*/
                ]
            }
        });

    };

    // Select2 for length menu styling
    var _componentSelect2 = function() {
        if (!$().select2) {
            console.warn('Warning - select2.min.js is not loaded.');
            return;
        }

        // Initialize
        $('.dataTables_length select').select2({
            minimumResultsForSearch: Infinity,
            dropdownAutoWidth: true,
            width: 'auto'
        });
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _componentDatatableButtonsHtml5();
            _componentSelect2();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    DatatableButtonsHtml5.init();
});
