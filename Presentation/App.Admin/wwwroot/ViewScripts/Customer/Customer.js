function LoadData() {
    var table = $('#table').DataTable(
        {
            "searching": false,
            "aaSorting": [],
            "buttons": [
                {
                    "extend": 'excelHtml5', "title": Resources.SharedResource.Users, "text": '<span>' + Resources.SharedResource.exportexcel + ' <i class="icon-file-excel"></i></span>', "className": 'dt-button buttons-print btn btn-light', "exportOptions":
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
                "url": "/Customer/LoadData?FullName=" + $("#FullName").val()
                    + "&Email=" + $("#Email").val()
                    + "&StatusId=" + $("#StatusId").val()
                    + "&PhoneNumber=" + $("#PhoneNumber").val()
                    + "&RoleName=" + $("#RoleName").val(),
                "type": "Post",
                "dataType": "JSON",
            },
            "columns": [
                { "data": "FullName" },
                { "data": "Email" },
                { "data": "PhoneNumber" },
                { "data": "RoleName", "bSortable": false },
                {
                    "mData": null,
                    "bSortable": false,
                    "mRender": function (o) {
                        if (o.LockoutEnabled == false) {
                            return '<span class="badge bg-success">' + Resources.SharedResource.published + '</span>';
                        }
                        else {
                            return '<span class="badge bg-danger">' + Resources.SharedResource.notpublished + '</span>';
                        }
                    }
                }
                , {
                    "mData": null,
                    "bSortable": false,
                    "mRender": function (o) {
                        return '<div class="list-icons">'
                            + '<a href="/Customer/AddEdit/' + o.Id + '" class="list-icons-item text-primary-600" data-popup="tooltip" title="' + Resources.SharedResource.edit + '"><i class="icon-pencil"></i></a> &nbsp;'
                            + '<a href="/Customer/ChangePassword/' + o.Id + '" class="list-icons-item text-primary-600" data-popup="tooltip" title="' + Resources.SharedResource.ChangePassword + '"><i class="icon-lock2 text-muted"></i></a> &nbsp;'
                            + (o.LockoutEnabled == false ? ' <a href = "javascript:Status(\'' + o.Id + '\',true);" class="list-icons-item text-danger-600" data - popup="tooltip" data-container="body" title="' + Resources.SharedResource.notpublished + '" > <i class="icon-blocked"></i></a > ' : ' <a href="javascript:Status(\'' + o.Id + '\',false);" class="list-icons-item text-success-600" data-popup="tooltip" data-container="body" title="' + Resources.SharedResource.published + '"> <i class="icon-checkmark"></i></a>')
                            + '&nbsp;</div > ';
                    }
                }
            ]
        });
}
function RefreshTable() {
    var table = $('#table').DataTable();
    table.destroy();
    LoadData();
}
function Status(id, status) {
    $.post('/Customer/Status?id=' + id + "&status=" + status, function (response) {
        if (response == true) {
            RefreshTable();
        }
        else {
            swal({ title: '', text: response, type: "error" }, function () {
                console.log('------------------Error In Delete Data---------------')
            });
        }
    });
}
$(document).ready(function () {
    LoadData();
});
$('#txtSearch').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code  ///
    {
        RefreshTable();
    }
});