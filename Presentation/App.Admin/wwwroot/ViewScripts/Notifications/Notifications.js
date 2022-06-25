function LoadData() {
    var table = $('.table').DataTable(
        {

            "searching": true,
            "aaSorting": [],
           
            "buttons": [
                {
                    "extend": 'excelHtml5', "title": Resources.SharedResource.Region, "text": '<span>' + Resources.SharedResource.exportexcel + ' <i class="icon-file-excel"></i></span>', "className": 'dt-button buttons-print btn btn-light', "exportOptions":
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
                "url": "/Notification/LoadData",
                "type": "Post",
                "dataType": "JSON",
            },
            "columns": [
                { "data": "CustomerName" },
                { "data": "MessageBody" },
                { "data": "Since" },
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