function LoadData() {
    var table = $('.table').DataTable(
        {
            "searching": false,
            "aaSorting": [],
            "buttons": [
                {
                    "extend": 'excelHtml5', "title": Resources.SharedResource.language, "text": '<span>' + Resources.SharedResource.exportexcel + ' <i class="icon-file-excel"></i></span>', "className": 'dt-button buttons-print btn btn-light', "exportOptions":
                    {
                        "columns": ':visible'
                    }
                },
                {
                    "extend": 'colvis', "text": '<span>' + Resources.SharedResource.selectrows + '<i class="icon-three-bars"></i></span>', "className": 'dt-button buttons-print btn btn-light'
                }
            ],
            "columnDefs": [
                { "width": "30%" },
                { "className": "text-center custom-middle-align" },
            ],
            "order": [
                [0, "desc"]
            ],
            "serverSide": true,
            "ajax":
            {
                "url": "/Language/LoadDataLocaleStringResource?ResourceName=" + $("#ResourceModel_ResourceName").val()
                    + "&ResourceValue=" + $("#ResourceModel_ResourceValue").val() + "&languageId=" + $("#ResourceModel_LanguageId").val(),
                "type": "Post",
                "dataType": "JSON",
            },
            "columns": [
                { "data": "ResourceName" },
                { "data": "ResourceValue" },
                {
                    "mData": null,
                    "bSortable": false,
                    "mRender": function (o) {
                        return '<div class="list-icons">'
                            + '<a href="javascript:Edit(' + o.Id + ');" class="list-icons-item text-primary-600" data-popup="tooltip" title="' + Resources.SharedResource.edit + '"><i class="icon-pencil"></i></a>'
                            + '</div > ';
                    }
                }
            ]
        });

}
function RefreshTable() {
    var table = $('.table').DataTable();
    table.destroy();
    LoadData();
}
$(document).ready(function () {
    LoadData();
});
$('#txtSearch').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        RefreshTable();
    }
});
function SaveLocaleStringResource() {
    var $form1 = $('#frmAddEditstring');
    if ($form1.valid()) {
        $.ajax({
            url: '/Language/AddEditLocaleStringResource',
            type: 'Post',
            data: {
                Id: $("#ResourceModel_Id").val(),
                LanguageId: $("#ResourceModel_LanguageId").val(),
                ResourceName: $("#ResourceModel_ResourceName").val(),
                ResourceValue: $("#ResourceModel_ResourceValue").val(),
            },
            cache: false,
            success: function (data) {
                if (data == "") {
                    $("#ResourceModel_Id").val(0);
                    $("#ResourceModel_ResourceName").val("");
                    $("#ResourceModel_ResourceValue").val("");
                    RefreshTable();
                }
                else {
                    swal({ title: '', text: data, type: "error" }, function () {
                    });
                }
            }
        });
    }
 
}
function Edit(id) {
    $.ajax({
        url: '/Language/AddEditLocaleStringResource/' + id,
        type: 'Get',
        cache: false,
        success: function (data) {
            $("#ResourceModel_Id").val(data.Id);
            $("#ResourceModel_ResourceName").val(data.ResourceName);
            $("#ResourceModel_ResourceValue").val(data.ResourceValue);
        }
    });
}