(function ($) {
    var _quotesSynService = abp.services.app.quotesSynthesise;
    var _contractService = abp.services.app.contract;
    var _supplierServiceup = abp.services.app.supplier;

    var _createModalContractReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModalContractReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });


    var html = `<table id="ItemTable" class="table table-bordered table-hover table-striped w-100">
                                <thead class="bg-primary-600">
                                    <tr>
                                        <th>STT</th>
                                        <th>Tên hàng hóa</th>
                                        <th>Đơn vị tính</th>
                                        <th>Số lượng</th>
                                        <th>Đơn giá/ĐVT</th>
                                        <th class="text-center">Thành tiền</th>
                                    </tr>
                                </thead>
                                 <tfoot>
                                    <tr>
                                        <th colspan="5">Cộng giá trị tiền bảng</th>
                                        <th class="text-right"></th>
                                    </tr>
                                    <tr>
                                        <th colspan="5">Thuế GTGT: 10%</th>
                                        <th class="Tax text-right"></th>
                                    </tr>
                                    <tr>
                                        <th colspan="5">Tổng giá trị Hợp đồng</th>
                                        <th class="TotalLast text-right"></th>
                                    </tr>
                                </tfoot>
                                <tbody></tbody>
                            </table>`;

    $("#TableItemContent").append(html);

    var dataFilter = {};
    dataFilter.id = $("#SupplierId").val();
    _supplierServiceup.get(dataFilter).done(function (result) {
        $(".nameSup").text(result.name);
        $(".AddressSeller").text(result.adress);
        $(".CodeSeller").text(result.taxCode);
        $(".PhoneSeller").text(result.phoneNumber);
        $(".FaxSeller").text(result.fax);
    });

    var dataFilterSy = {};
    dataFilterSy.id = $("#QuoteSynId").val();
    _quotesSynService.get(dataFilterSy).done(function (result) {
        $("#codeSyn").text(result.code);
       
    });

    var getFilter = function () {
        let dataFilter = {};
        dataFilter.supper = $("#SupplierId").val();
        dataFilter.quoSyn = $("#QuoteSynId").val();
        return dataFilter;
    }

    //Load Table Item 
    var ItemTable = $("#ItemTable").DataTable({
        paging: false,
        serverSide: false,
        processing: true,
        "searching": false,
        searching: false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _contractService.allItemInContact,
            inputFilter: getFilter
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                orderable: false,
                targets: 0,
                width: "10%",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "itemName",
                width: "30%",
            },
            {
                orderable: false,
                targets: 2,
                data: "unitName",
                width: "10%",
            },
            {

                targets: 3,
                width: "10%",
                orderable: false,
                autoWidth: false,
                data: "quantityQuote",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                targets: 4,
                width: "10%",
                orderable: false,
                autoWidth: false,
                data: 'quotePrice',
                render: $.fn.dataTable.render.number(',', ',', '')

            },
            {

                targets: 5,
                width: "20%",
                data: "totalNumber",
                className: "text-right",
                orderable: false,
                autoWidth: false,
                render: $.fn.dataTable.render.number(',', ',', '')

            },
        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };


            // Total over this page
            pageTotal1 = api
                .column(5, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            /*$(api.column(4).footer()).html(numFormat(pageTotal));*/
            $(api.column(5).footer()).html(numFormat(pageTotal1));
            var taxvalue = (pageTotal1 * 10) / 100;
            $(".Tax").html(numFormat(taxvalue));

            var Tong = pageTotal1 + taxvalue;
            $(".TotalLast").html(numFormat(Tong));
            $(".TotalLast").attr("data-tong", Tong);
        },
    });

    //--------------------------------------------- Action Button ---------------------------------------------
    $("#btn-contract_Save").prop("disabled", true);
    $("#btn-contract_Save").css({ "opacity": "0.4", "cursor": "default" });
    $(".input-contract").prop("disabled", true);

    $("#btn-contract_Edit").on("click", function () {
        $("#btn-contract_Submit").prop("disabled", true);
        $("#btn-contract_Submit").css({ "opacity": "0.4", "cursor": "default" });

        $("#btn-contract_Edit").prop("disabled", true);
        $("#btn-contract_Edit").css({ "opacity": "0.4", "cursor": "default" });


        $("#btn-contract_Save").removeAttr('disabled');
        $("#btn-contract_Save").css({ "opacity": "1", "cursor": "pointer" });
        $(".input-contract").removeAttr('disabled');
    });

    $("#btn-contract_Save").on("click", function () {
        var data = {};
        data.Id = $("#Id").val();
        data.BankName = $("#BankName").val();
        data.Stk = $("#STK").val();
        data.MouthNumber = $("#MouthNumber").val();
        data.Price = $("#Price").val();
        data.Indemnify = $("#Indemnify").val();
        data.SellerDate = $("#sellerDate").val();
        data.RepresentA = $("#RepresentA").val();
        data.PositionA = $("#PositionA").val();
        data.RepresentB = $("#RepresentB").val();
        data.PositionB = $("#PositionB").val();

        _contractService.updateContract(data).done(function (result) {
            abp.message.success('Bạn đã cập nhật Hợp đồng thành công', 'Chúc mừng');
            window.setTimeout('location.reload()', 2500);
        })
        console.log(data);
    });


    //-- gửi
    $("#btn-contract_Submit").on("click", function () {
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize(''),
            app.localize('Bạn có chắc Gửi không?'),
            function (isConfirmed) {
                if (isConfirmed) {
                    var dataUpdate = {};
                    dataUpdate.Status = 1;
                    dataUpdate.Id = $("#Id").val();
                    dataUpdate.email = $("#email").val();
                    dataUpdate.link = window.location.href;
                    dataUpdate.name = $("#name").val();
                    _contractService.updateContractStatus(dataUpdate).done(function () {
                        abp.notify.success(app.localize('Đã gửi hợp đồng thành công!'));
                        window.setTimeout('location.reload()', 1000);
                    })
                }
            }
        );
    })

    //-------------------------------- In------------------------


    var htmlAction = `<table id="ItemTableAction" class="table table-bordered table-hover table-striped w-100">
                                <thead class="bg-primary-600">
                                    <tr>
                                        <th>STT</th>
                                        <th>Tên hàng hóa</th>
                                        <th>Đơn vị tính</th>
                                        <th>Số lượng</th>
                                        <th>Đơn giá/ĐVT</th>
                                        <th class="text-center">Thành tiền</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                                 <tfoot>
                                    <tr>
                                        <th colspan="5">Cộng giá trị tiền bảng</th>
                                        <th class="text-right"></th>
                                    </tr>
                                    <tr>
                                        <th colspan="5">Thuế GTGT: 10%</th>
                                        <th class="Tax text-right"></th>
                                    </tr>
                                    <tr>
                                        <th colspan="5">Tổng giá trị Hợp đồng</th>
                                        <th class="TotalLast text-right"></th>
                                    </tr>
                                </tfoot>
                                
                            </table>`;

    $("#TableItemContentAction").append(htmlAction);
    //Load Table Item 
    var ItemTableAction = $("#ItemTableAction").DataTable({
        paging: false,
        serverSide: false,
        processing: true,
        "searching": false,
        searching: false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _contractService.allItemInContact,
            inputFilter: getFilter
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                orderable: false,
                targets: 0,
                width: "10%",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "itemName",
                width: "30%",
            },
            {
                orderable: false,
                targets: 2,
                data: "unitName",
                width: "10%",
            },
            {

                targets: 3,
                width: "10%",
                orderable: false,
                autoWidth: false,
                data: "quantityQuote",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                targets: 4,
                width: "10%",
                orderable: false,
                autoWidth: false,
                data: 'quotePrice',
                render: $.fn.dataTable.render.number(',', ',', '')

            },
            {

                targets: 5,
                width: "20%",
                data: "totalNumber",
                className: "text-right",
                orderable: false,
                autoWidth: false,
                render: $.fn.dataTable.render.number(',', ',', '')

            },
        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };


            // Total over this page
            pageTotal1 = api
                .column(5, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            /*$(api.column(4).footer()).html(numFormat(pageTotal));*/
            $(api.column(5).footer()).html(numFormat(pageTotal1));
            var taxvalue = (pageTotal1 * 10) / 100;
            $(".Tax").html(numFormat(taxvalue));

            var Tong = pageTotal1 + taxvalue;
            $(".TotalLast").html(numFormat(Tong));
            $(".TotalLast").attr("data-tong", Tong);
        },
    });
    $("#Content-Contract").hide(); 


    $("#btnPrint").on("click", function () {
        $("#Content-Contract").show();
        $("#Content-Contract").css("opacity", "0");


        window.print();

        $("#Content-Contract").hide();
    })

    //-------------------------------- / In-----------------------


    //---Gửi duyệt
    $("#btn-contract_SubApp").on("click", function () {
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize(''),
            app.localize('Bạn có chắc Gửi duyệt không?'),
            function (isConfirmed) {
                if (isConfirmed) {
                    var dataUpdate = {};
                    dataUpdate.Status = 2;
                    dataUpdate.Id = $("#Id").val();
                    dataUpdate.email = $("#emailGD").val();
                    dataUpdate.link = window.location.href;
                    dataUpdate.name = $("#nameGD").val();
                    _contractService.updateContractStatus(dataUpdate).done(function () {
                        abp.notify.success(app.localize('Đã gửi hợp đồng thành công!'));
                        window.setTimeout('location.reload()', 1000);
                    })
                }
            }
        );
    })

    //--Duyệt
    $("#btn-contract_Approve").on("click", function () {
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize(''),
            app.localize('Bạn có chắc Duyệt không?'),
            function (isConfirmed) {
                if (isConfirmed) {
                    var dataUpdate = {};
                    dataUpdate.Status = 5;
                    dataUpdate.Id = $("#Id").val();
                    dataUpdate.link = window.location.href;
                    _contractService.updateContractStatus(dataUpdate).done(function () {
                        abp.notify.success(app.localize('Đã gửi hợp đồng thành công!'));
                        window.setTimeout('location.reload()', 1000);
                    })
                }
            }
        );
    })

    //-Từ chối
    $("#btn-contract_Reject").on("click", function () {
        var dataFilter = { id: $("#Id").val() };
        _createModalContractReject.open(dataFilter);
    });

    abp.event.on('app.reloadgetDocContact', function () {
        window.setTimeout('location.reload()', 1000);
    });

    //-Xác nhận NCC
    $("#btn-contract_Contract").on("click", function () {
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize(''),
            app.localize('Bạn có chắc Xác nhận không?'),
            function (isConfirmed) {
                if (isConfirmed) {
                    var dataUpdate = {};
                    dataUpdate.Status = 6;
                    dataUpdate.Id = $("#Id").val();

                    _contractService.updateContractStatus(dataUpdate).done(function () {
                        abp.notify.success(app.localize('Xác nhận thành công!'));
                        window.setTimeout('location.reload()', 1000);
                    })
                }
            }
        );
    })


    //----------------- Xuất Doc----------------------
    //function exportHTML() {
    //    $("#Content-Contract").show();
    //    var header = "<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
    //        "xmlns:w='urn:schemas-microsoft-com:office:word' " +
    //        "xmlns='http://www.w3.org/TR/REC-html40'>" +
    //        "<head><meta charset='utf-8'><title>Export HTML to Word Document with JavaScript</title></head><body>";
    //    var footer = "</body></html>";
    //    var sourceHTML = header + document.getElementById("Content-Contract").innerHTML + footer;

    //    var source = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHTML);
    //    var fileDownload = document.createElement("a");
    //    document.body.appendChild(fileDownload);
    //    fileDownload.href = source;
    //    fileDownload.download = 'HopDong.doc';
    //    fileDownload.click();
    //    document.body.removeChild(fileDownload);
    //}

    $("#btnDoc").on("click", function () {
        /*$("#Content-Contract").css("opacity", "0");*/
        //exportHTML();

        //$("#Content-Contract").hide();
        //  _contractService.getPOListDto($("#Id").val()).done();
        exportReport($("#Id").val())
    })


    function exportReport(reportEnum) {
        var filterObj = {
            id: reportEnum
        };
        _contractService.getPOListDto(reportEnum)
            .done(function (fileResult) {
                location.href = abp.appPath + 'File/DownloadReportFile?fileType=' + fileResult.fileType + '&fileToken=' + fileResult.fileToken + '&fileName=' + fileResult.fileName;
            });
    }

    //----------------- / Xuất Doc----------------------



    //--------------------------------------------- / Action Button ---------------------------------------------
})(jQuery);