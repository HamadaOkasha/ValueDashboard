// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function bindBootstrapTabSelectEvent(tabsId, inputId) {
    $('#' + tabsId + ' > ul li a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tabName = $(e.target).attr("data-tab-name");
        $("#" + inputId).val(tabName);
    });
}