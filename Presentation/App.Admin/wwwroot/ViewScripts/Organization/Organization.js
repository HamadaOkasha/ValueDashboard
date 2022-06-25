function LoadData() {
    var table = $('.table').DataTable(
        {
            "searching": false,
            "aaSorting": [],
            "buttons": [
                {
                    "extend": 'excelHtml5', "title": Resources.SharedResource.Organization, "text": '<span>' + Resources.SharedResource.exportexcel + ' <i class="icon-file-excel"></i></span>', "className": 'dt-button buttons-print btn btn-light', "exportOptions":
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
                "url": "/Organization/LoadData?Search=" + $("#Name").val()
                    + "&StatusId=" + $("#StatusId").val(),
                "type": "Post",
                "dataType": "JSON",
            },
            "columns": [
                { "data": "Name" },
                { "data": "WebsiteUrl" },
               
                {
                    "mData": null,
                    "bSortable": false,
                    "mRender": function (o) {
                        if (o.IsPublish) {
                            return '<span class="badge bg-success">' + Resources.SharedResource.published + '</span>';
                        }
                        else {
                            return '<span class="badge bg-danger">' + Resources.SharedResource.notpublished + '</span>';
                        }
                    }
                },
                {
                    "mData": null,
                    "bSortable": false,
                    "mRender": function (o) {
                        return '<div class="list-icons">'
                            + '<a href="/Organization/AddEdit/' + o.Id + '" class="list-icons-item text-primary-600" data-popup="tooltip" title="' + Resources.SharedResource.edit + '"><i class="icon-pencil"></i></a> &nbsp;'
                            + (o.IsPublish ? ' <a href = "javascript:Status(' + o.Id + ',false);" class="list-icons-item text-danger-600" data - popup="tooltip" data-container="body" title="' + Resources.SharedResource.notpublished + '" > <i class="icon-blocked"></i></a > ' : ' <a href="javascript:Status(' + o.Id + ',true);" class="list-icons-item text-success-600" data-popup="tooltip" data-container="body" title="' + Resources.SharedResource.published + '"> <i class="icon-checkmark"></i></a>')
                            + '&nbsp;<a href="javascript:Delete(\'' + o.Id + '\');" class="list-icons-item text-danger-600" data-popup="tooltip" title="' + Resources.SharedResource.delete + '"><i class="icon-cross2"></i></a></div > ';
                    }
                }
            ]
        });

}
function RefreshTable() {
    var table = $('.table').DataTable();
    table.destroy();
    LoadData();
    $(window).scrollTop($(document).height() / 2);
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
function Delete(id) {
    swal({
        title: "", text: Resources.SharedResource.deleteconfirm, type: "warning",
        showCancelButton: true, confirmButtonColor: "#DD6B55",
        confirmButtonText: Resources.SharedResource.ok,
        cancelButtonText: Resources.SharedResource.cancel,
        closeOnCancel: true, closeOnConfirm: false
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: '/Organization/Delete/' + id,
                    type: 'Post',
                    cache: false,
                    success: function (data) {
                        if (data == true) {
                            swal({
                                title: '',
                                text: Resources.SharedResource.ok,
                                type: 'success',
                                confirmButtonText: Resources.SharedResource.ok,
                            }, function () {
                                RefreshTable();
                            });
                        }
                        else {
                            swal({ title: '', text: data, type: "error" }, function () {
                                console.log('------------------Error In Delete Data---------------')
                            });
                        }
                    }
                });
            }
        });
}

function Status(id, Status) {
    $.post('/Organization/Status?id=' + id + "&status=" + Status, function (response) {
        if (response == true) {
            swal({ title: '', text: Resources.SharedResource.ok, type: "success", confirmButtonText: Resources.SharedResource.ok, timer: 1000 }, function () {
                RefreshTable();
            });
        }
        else {
            swal({ title: '', text: response, type: "error" }, function () {
                console.log('------------------Error In Delete Data---------------')
            });
        }
    });
}