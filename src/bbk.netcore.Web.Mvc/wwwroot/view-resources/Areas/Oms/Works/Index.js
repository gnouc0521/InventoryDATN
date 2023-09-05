(function () {
    var _$workTable = $('#workTable');
    var _workService = abp.services.app.work;
    var dateStart = "";
    var dateEnd = "";
    var start;
    var end;
    moment.locale(abp.localization.currentLanguage.name);

    var _permissions = {
        create: abp.auth.hasPermission('PersonalProfile.Work.Create'),
        edit: abp.auth.hasPermission('PersonalProfile.Work.Edit'),
        calender: abp.auth.hasPermission('PersonalProfile.Work.Calender'),
    };
    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Work/CreateWork',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Works/_CreateModal.js',
        modalClass: 'CreatDelModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Work/EditWorkModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Works/_EditModal.js',
        modalClass: 'DeliveryEditModal',
        modalType: 'modal-xl'
    });


    //Hiển thị view Công việc
    var _ViewDetails = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Work/ViewDetails',
        /*scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Work/_uploadFile.js',*/
        modalClass: 'ViewModal',
        modalType: 'modal-xl'
    });


    //Code Hà
    //Modal đặt lịch
    var _createCalendarModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ScheduleWork/CreateScheduleWork',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ScheduleWorks/_CreateModal.js',
        modalClass: 'ScheduleWorkCreateModal',
        modalType: 'modal-xl'
    });

    //Modal chọn người để chuyển tiếp
    var _selectUser = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Work/SelectUser',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Works/_SelectUser.js',
        modalClass: 'UserModal'
    });


    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });


    $('.date-picker').datepicker({
        rtl: false,
        dateFormat: 'dd-mm--yy',
        orientation: "left bottom",
        autoclose: true,
        language: abp.localization.currentLanguage.name
    });
  

    $("#StartDay").datepicker({
        todayBtn: 1,
        autoclose: true,
    }).on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        $('#EndDay').datepicker('setStartDate', minDate);
    });

    $("#EndDay").datepicker()
        .on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#StartDay').datepicker('setEndDate', maxDate);
        });



    /* DEFINE TABLE */
    var getFilter = function () {
        let data = {};
        let isPriorty = document.getElementById("Priorty");
        data.searchTerm = $('#SearchTerm').val();
        data.statustest = $("#StatusTypeId").val();
        data.priority = isPriorty.value;
        if (start != null) {
            dateStart = moment(start).format('L');
            data.dateStart = dateStart
        } if (end != null) {
            dateEnd = moment(end).format('L');
            data.dateEnd = dateEnd;
        }
        
        return data;
    }

    if ($("#StatusTypeId").val() == 2) {
        var dataTable = _$workTable.DataTable({
            paging: true,
            serverSide: false,
            processing: true,
            "searching": false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
            },
            "bInfo": false,
            "bLengthChange": true,
            //"dom": 'Rltip',
            lengthMenu: [
                [5, 10, 25, 50, -1],
                [5, 10, 25, 50, 'Tất cả'],
            ],
            pageLength: 5,
            listAction: {
                ajaxFunction: _workService.getAllByStatus,
                inputFilter: getFilter
            },
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    className: "dt-control",
                    render: function (data, type, row, meta) {
                        return `<span class='bindofferid' selected='false' data-objid='` + row.id + `'></span>`;
                    }
                },
                {

                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    render: function (data, type, row, meta) {
                        return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                            		<a class='dropdown-item docaddcalendar'data-objid='` + row.id + `'href='javascript:void(0); ' > Đặt lịch </a>
                                    <a class='dropdown-item doctransfer'data-objid='` + row.id + `'href='javascript:void(0); ' > Chuyển tiếp </a>
                            	</div>
                            </div>`;
                    }
                },
                {
                    orderable: true,
                    targets: 2,
                    data: "title",
                    render: function (data, type, row, meta) {
                        return `
                        <a class="viewwork" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.title + `</a>
                    `
                    }
                },
                {
                    orderable: false,
                    targets: 3,
                    data: "startDate",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },
                {
                    targets: 4,
                    data: "endDate",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },
                {
                    targets: 5,
                    data: "completionTime",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    },
                },
                {
                    orderable: true,
                    targets: 6,
                    data: "priority",
                    render: data => `<span>${data == 0 ? 'Thấp' : data == 1 ? 'Trung bình' : 'Cao'}</span>`
                },
                {
                    orderable: true,
                    targets: 7,
                    data: "status",
                    render: data => `<span>${data == 0 ? 'Chờ xử lý' : data == 1 ? 'Đang xử lý' : data == 2 ? 'Hoàn thành' : 'Quá hạn'}</span >`
                },
            ]
        });
    }
    else {
        //Load Table Work
        var dataTable = _$workTable.DataTable({
            paging: true,
            serverSide: false,
            processing: true,
            "searching": false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
            },
            "bInfo": false,
            "bLengthChange": true,
            //"dom": 'Rltip',
            lengthMenu: [
                [5, 10, 25, 50, -1],
                [5, 10, 25, 50, 'Tất cả'],
            ],
            pageLength: 5,
            listAction: {
                ajaxFunction: _workService.getAllByStatus,
                inputFilter: getFilter
            },
            columnDefs: [
                {
                    targets: 0,
                    orderable: false,
                    className: "dt-control",
                    render: function (data, type, row, meta) {
                        return `<span class='bindofferid' selected='false' data-objid='` + row.id + `'></span>`;
                    }
                },
                {

                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    render: function (data, type, row, meta) {
                        return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                            		<a class='dropdown-item docaddcalendar'data-objid='` + row.id + `'href='javascript:void(0); ' > Đặt lịch </a>
                                    <a class='dropdown-item doctransfer'data-objid='` + row.id + `'href='javascript:void(0); ' > Chuyển tiếp </a>
                            	</div>
                            </div>`;
                    }
                },
                {
                    orderable: true,
                    targets: 2,
                    data: "title",
                    render: function (data, type, row, meta) {
                        return `
                        <a class="viewwork" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.title + `</a>
                    `
                    }
                },
                {
                    orderable: false,
                    targets: 3,
                    data: "startDate",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },
                {
                    targets: 4,
                    data: "endDate",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },
                {
                    orderable: true,
                    targets: 5,
                    data: "priority",
                    render: data => `<span>${data == 0 ? 'Thấp' : data == 1 ? 'Trung bình' : 'Cao'}</span>`
                },
                {
                    orderable: true,
                    targets: 6,
                    data: "status",
                    render: data => `<span>${data == 0 ? 'Chờ xử lý' : data == 1 ? 'Đang thực hiện' : data == 2 ? 'Hoàn thành' : 'Quá hạn'}</span >`
                },
            ]
        });

    }

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    // Thêm sự kiện đóng mở bản con
    $('#workTable tbody').on('click', 'td.dt-control', function () {
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);
        var offerid = tr[0].firstElementChild.firstElementChild.dataset.objid;
        tr.attr('data-objid', offerid);
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('dt-hasChild shown');
        }
        else {
            // Open this row
            row.child(ShowRowTbl(row.data(), offerid)).show();
            tr.addClass('dt-hasChild shown');
            //Khai bao table
            var _$ContractTaskTable = $('#ContractsTbl' + offerid);
            //Khai bao gia tri nhap vao
            var getFilter = function () {
                let dataFilter = {};
                if (offerid != null) {
                    dataFilter.offerId = offerid;
                }
                if (taskstatus != null) {
                    dataFilter.taskStatus = taskstatus;
                }
                if (ContractStatus != null) {
                    dataFilter.contractStatus = ContractStatus;
                }
                return dataFilter;
            }
            //Bảng con
            datacontractTable = _$ContractTaskTable.DataTable({
                paging: false,
                serverSide: true,
                processing: false,
                ordering: false,
                info: false,
                "searching": false,
                "language": {
                    "info": "Hiển thị _START_ đến _END_ của _TOTAL_ kết quả tìm thấy",
                    "infoEmpty": "",
                    "emptyTable": "Không tìm thấy hợp đồng!"
                },
                "bLengthChange": false,
                pageLength: 10,
                //listAction: {
                //    ajaxFunction: _offerService.getContractById,
                //    inputFilter: getFilter
                //},
                columnDefs: [
                    {
                        orderable: false,
                        title: "ID",
                        targets: 0,
                        className: "text-center",
                        data: "id"
                    },
                    {
                        orderable: false,
                        title: "Tên Công việc",
                        targets: 1,
                        className: "text-center",
                        data: "contractName",
                    },
                    {
                        orderable: false,
                        title: "Ngày tạo",
                        targets: 2,
                        className: "text-center",
                        data: "creationTime",
                        render: function (creationTime) {
                            return moment(creationTime).format('L');
                        }
                    },
                    {
                        orderable: false,
                        title: "Quy trình xử lý",
                        targets: 3,
                        className: "text-center approvalworkflowth",
                        data: "id",
                        render: function (data) {
                            return `<span class='approvalworkflow' data-contractid=` + data + ` ></span>`
                        }
                    },
                    {
                        orderable: false,
                        title: "Chức năng",
                        targets: 4,
                        className: "text-center",
                        data: "offerId",
                        render: function (data, type, row, meta) {
                            return `
                            <button type="button" style="color: #D4090e" class="btn btn-xs viewcontractfunc" data-offerid='`+ data + `' data-contractid='` + row.id + `' />
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-fill" viewBox="0 0 16 16">
                            <path d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708l-3-3zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207l6.5-6.5zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.499.499 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11l.178-.178z"/>
                            </svg>
                            </button >
                                <button selected='false' type="button" class="btn btn-xs viewtaskhistory" style="color: red" data-offerid='` + data + `' data-objid='` + row.id + `'>
                                <svg xmlns="http://www.w3.org/2000/svg" width="15" height="15" fill="currentColor" class="bi bi-clock-history" viewBox="0 0 16 16">
                                <path d="M8.515 1.019A7 7 0 0 0 8 1V0a8 8 0 0 1 .589.022l-.074.997zm2.004.45a7.003 7.003 0 0 0-.985-.299l.219-.976c.383.086.76.2 1.126.342l-.36.933zm1.37.71a7.01 7.01 0 0 0-.439-.27l.493-.87a8.025 8.025 0 0 1 .979.654l-.615.789a6.996 6.996 0 0 0-.418-.302zm1.834 1.79a6.99 6.99 0 0 0-.653-.796l.724-.69c.27.285.52.59.747.91l-.818.576zm.744 1.352a7.08 7.08 0 0 0-.214-.468l.893-.45a7.976 7.976 0 0 1 .45 1.088l-.95.313a7.023 7.023 0 0 0-.179-.483zm.53 2.507a6.991 6.991 0 0 0-.1-1.025l.985-.17c.067.386.106.778.116 1.17l-1 .025zm-.131 1.538c.033-.17.06-.339.081-.51l.993.123a7.957 7.957 0 0 1-.23 1.155l-.964-.267c.046-.165.086-.332.12-.501zm-.952 2.379c.184-.29.346-.594.486-.908l.914.405c-.16.36-.345.706-.555 1.038l-.845-.535zm-.964 1.205c.122-.122.239-.248.35-.378l.758.653a8.073 8.073 0 0 1-.401.432l-.707-.707z"></path>
                                <path d="M8 1a7 7 0 1 0 4.95 11.95l.707.707A8.001 8.001 0 1 1 8 0v1z"></path>
                                <path d="M7.5 3a.5.5 0 0 1 .5.5v5.21l3.248 1.856a.5.5 0 0 1-.496.868l-3.5-2A.5.5 0 0 1 7 9V3.5a.5.5 0 0 1 .5-.5z"></path>
                                <span selected='false' style='color: red' class='viewtaskhistory' data-offerid='` + data + `' data-objid='` + row.id + `'></span>
                                </svg>
                            </button>`
                        }
                    }
                ],
                "initComplete": function (row) {
                    $(".approvalworkflowth").css("width", "500px");
                    for (var i = 0; i < row.aoData.length; i++) {
                        GetProcessContract(row.aoData[i]._aData.id);
                    }
                }
            });
        }
    });

    //----------------------------
    // Hiển thị hnagf của bảng con
    function ShowRowTbl(d, id) {
        return `<table id="ContractsTbl` + id + `" cellpadding="5" cellspacing="0" class="table table-bordered" style="width: 100%;">` +
            `<tr class="bg-fusion-50">` +
            `<th class="text-center"></th>` +
            `<th class="text-center"></th>` +
            `<th class="text-center"></th>` +
            `<th class="text-center"></th>` +
            `<th class="text-center"></th>` +
            `</tr>` +
            `</table>`;
    }




    $('#workTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        if (_permissions.edit) {
            var dataFilter = { id: btnClick[0].dataset.objid, StatusId: $("#StatusTypeId").val() };
            _EditModal.open(dataFilter);
        }
        else {
            abp.notify.error('Bạn không có quyền');
        }

    });

    $('#workTable').on('click', '.viewwork', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _ViewDetails.open(dataFilter);
    });


    $('#workTable').on('click', '.docaddcalendar', function (e) {
        //var btnClick = $(this);
        //var dataFilter = { id: btnClick[0].dataset.objid };
        if (_permissions.calender) {
            _createCalendarModal.open();
        }
        else {
            abp.notify.error('Bạn không có quyền');
        }
    });

    //Hiển thị Modal chuyển tiếp công việc
    $('#workTable').on('click', '.doctransfer', function (e) {
        //var btnClick = $(this);
        //var dataFilter = { id: btnClick[0].dataset.objid };
        _selectUser.open();
    });

    $('#Search').click(function (e) {
        e.preventDefault();
        $("#StartDay").datepicker();
        $("#EndDay").datepicker();
        start = $("#StartDay").datepicker("getDate");
        end = $("#EndDay").datepicker("getDate");
        getDocs();
    });

    function getDocs() {

        dataTable.ajax.reload();
    }
 

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });

    var StatusIdss = $("#StatusIdss").val();
    $('#ListStatus').val(StatusIdss).trigger('change');

    $(document).ready(function () {
        jQuery("#ListStatus").change(function () {
            var statusID = jQuery(this).children(":selected").attr("value");
            statusID = $("#ListStatus").val();
            $('#ListStatus option').removeAttr('selected');
            $("#ListStatus > [value =" + statusID + "]").attr("selected", "true");
            $.ajax({
                url: '/Inventorys/Work/Index',
                type: 'GET',
                data: {
                    StatusId: statusID
                },
                success: function (results) {
                    window.location.href = "/Inventorys/Work?StatusId=" + statusID;
                },
                error: function (xhr) {
                    alert('error');
                }
            });

        });
    });




})(jQuery);