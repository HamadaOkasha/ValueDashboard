function LoadData() {
    var table = $('.table').DataTable(
        {
            "searching": false,
            "aaSorting": [],
            "buttons": [
                {
                    "extend": 'excelHtml5', "title": Resources.SharedResource.Groups, "text": '<span>' + Resources.SharedResource.exportexcel + ' <i class="icon-file-excel"></i></span>', "className": 'dt-button buttons-print btn btn-light', "exportOptions":
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
                "url": "/Groups/LoadData",
                "type": "Post",
                "dataType": "JSON",
            },
            "columns": [
                { "data": "Name" },
                {
                    "mData": null,
                    "bSortable": false,
                    "mRender": function (o) {
                        return '<div class="list-icons">'
                            + '<a href="/Groups/AddEdit/' + o.Id + '" class="list-icons-item text-primary-600" data-popup="tooltip" title="' + Resources.SharedResource.edit + '"><i class="icon-pencil"></i></a> &nbsp; '
                            + '&nbsp;<a href="javascript:Delete(\'' + o.Id + '\');" class="list-icons-item text-danger-600" data-popup="tooltip" title="' + Resources.SharedResource.Delete + '"><i class="icon-cross2"></i></a></div > ';
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

function Delete(id) {
    swal({
        title: "", text: Resources.SharedResource.DeleteConfirm, type: "warning",
        showCancelButton: true, confirmButtonColor: "#DD6B55",
        confirmButtonText: Resources.SharedResource.Ok,
        cancelButtonText: Resources.SharedResource.Cancel,
        closeOnCancel: true, closeOnConfirm: false
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: '/Groups/Delete/' + id,
                    type: 'Post',
                    cache: false,
                    success: function (data) {
                        if (data == true) {
                            swal({
                                title: '',
                                text: Resources.SharedResource.Ok,
                                type: 'success',
                                confirmButtonText: Resources.SharedResource.Ok,
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