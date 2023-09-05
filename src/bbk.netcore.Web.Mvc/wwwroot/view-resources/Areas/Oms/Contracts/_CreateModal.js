(function ($) {

    app.modals.ContractsCreateModal = function () {


        var _WarehouseTypeService = abp.services.app.warehouseTypesService;
        var _quotesSynService = abp.services.app.quotesSynthesise;
        var _$TableItem = $("#ItemTable");
        var _contractService = abp.services.app.contract;
        var _supplierServiceup = abp.services.app.supplier;
        var _modalManager;
        var _frmDelivery = null;
        var dataAllInput = [];
        
        var dataSupId = [];
        


        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

            //Get time current
            var currentdate = new Date();  

            

            $('#Select_sup').select2({
                width: "100%",
                dropdownParent: $('#ContractsCreateModal'),
                placeholder: 'Chọn nhà cung cấp',
                search: true
            }).on('select2:select', function (e) {
                var data = e.params.data;

                var btnClick = data.id;
                var dataFilter = {};
                dataFilter.id = btnClick;
                _supplierServiceup.get(dataFilter).done(function (result) {
                    $("#NameSup").text(result.name);
                    $("#AddressSeller").text(result.adress);
                    $("#CodeSeller").text(result.taxCode);
                    $("#PhoneSeller").text(result.phoneNumber);
                    $("#FaxSeller").text(result.fax);
                 
                });

                // ----------------------------------------------- Load Ra Table Gia -------------------------------------
                $("#TableItemContent").empty();
               

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

                var getFilter = function () {
                    let dataFilter = {};
                    dataFilter.supper = btnClick;
                    dataFilter.quoSyn = $("#QuotesSynId").val();
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
                            className: "text-right",
                            data: "totalNumber",
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

                // ----------------------------------------------- / Load Ra Table Gia -------------------------------------

                //------------------------------------------------ lay du lieu O Input --------------------------
                $("#MouthNumber").empty();
                $("#Price").empty();
                $("#sellerDate").empty();
                $("#Indemnify").empty();
                $("#BankName").empty();
                $("#STK").empty();
                $("#RepresentA").empty();
                $("#PositionA").empty();
                $("#RepresentB").empty();
                $("#PositionB").empty();

                if (dataSupId.indexOf(data.id) == -1) {


                    $("#MouthNumber").append(`<input type="number" class="inputMouthNumber_` + data.id + `" />`);
                    $("#Price").append(`<input type="number" class="inputPrice_` + data.id + `" />`);
                    $("#Indemnify").append(`<input type="number" class="inputIndemnify_` + data.id + `" />`);
                    $("#sellerDate").append(`<input type="date" class="inputsellerDate_` + data.id + `" />`);
                    $("#BankName").append(`<input type="text" class="inputBankName_` + data.id + `" />`);
                    $("#STK").append(`<input type="text" class="inputSTK_` + data.id + `" />`);
                    $("#RepresentA").append(`<input type="text" class="inputRepA_` + data.id + `" />`);
                    $("#PositionA").append(`<input type="text" class="inputPosiA_` + data.id + `" />`);
                    $("#RepresentB").append(`<input type="text" class="inputRepB_` + data.id + `" />`);
                    $("#PositionB").append(`<input type="text" class="inputPosiB_` + data.id + `" />`);

                    let datainput = {};
                    //datainput.supId = "";
                    //datainput.price = 0;
                    //datainput.mouthNumber = 0;
                    //datainput.indemnify = 0;

                    datainput.SupplierId = data.id;
                    datainput.QuoteSynId = $("#QuotesSynId").val();
                    datainput.Status = 0;

                    console.log(ItemTable);
                    $(".inputRepA_" + data.id).change(function () {
                        datainput.RepresentA = $(this).val();
                    });

                    $(".inputPosiA_" + data.id).change(function () {
                        datainput.PositionA = $(this).val();
                    });

                    $(".inputRepB_" + data.id).change(function () {
                        datainput.RepresentB = $(this).val();
                    });

                    $(".inputPosiB_" + data.id).change(function () {
                        datainput.PositionB = $(this).val();
                    });

                    $(".inputMouthNumber_" + data.id).change(function () {
                        datainput.MouthNumber = $(this).val();
                    });

                    $(".inputPrice_" + data.id).change(function () {
                        datainput.Price = $(this).val();
                    })

                    $(".inputIndemnify_" + data.id).change(function () {
                        datainput.Indemnify = $(this).val();
                    })

                    $(".inputsellerDate_" + data.id).change(function () {
                        datainput.SellerDate = $(this).val();
                    })

                    $(".inputBankName_" + data.id).change(function () {
                        datainput.BankName = $(this).val();
                    })

                    $(".inputSTK_" + data.id).change(function () {
                        datainput.Stk = $(this).val();
                    })

                    dataSupId.push(data.id);

                    dataAllInput.push(datainput);

                }
                else {
                    $.each(dataAllInput, function (index, value) {
                        if (value.SupplierId == data.id) {
                            $("#MouthNumber").append(`<input type="number" class="inputMouthNumber_` + data.id + `" />`);
                            $("#Price").append(`<input type="number" class="inputPrice_` + data.id + `" />`);
                            $("#Indemnify").append(`<input type="number" class="inputIndemnify_` + data.id + `" />`);
                            $("#sellerDate").append(`<input type="date" class="inputsellerDate_` + data.id + `" />`);
                            $("#BankName").append(`<input type="text" class="inputBankName_` + data.id + `" />`);
                            $("#STK").append(`<input type="text" class="inputSTK_` + data.id + `" />`);
                            $("#RepresentA").append(`<input type="text" class="inputRepA_` + data.id + `" />`);
                            $("#PositionA").append(`<input type="text" class="inputPosiA_` + data.id + `" />`);
                            $("#RepresentB").append(`<input type="text" class="inputRepB_` + data.id + `" />`);
                            $("#PositionB").append(`<input type="text" class="inputPosiB_` + data.id + `" />`);

                            let datainput = {};
                            //datainput.supId = "";
                            //datainput.price = 0;
                            //datainput.mouthNumber = 0;



                            $(".inputMouthNumber_" + data.id).val(value.MouthNumber);
                            $(".inputPrice_" + data.id).val(value.Price);
                            $(".inputIndemnify_" + data.id).val(value.Indemnify);
                            $(".inputsellerDate_" + data.id).val(value.SellerDate);
                            $(".inputBankName_" + data.id).val(value.BankName);
                            $(".inputSTK_" + data.id).val(value.Stk);
                            $(".inputRepA_" + data.id).val(value.RepresentA);
                            $(".inputPosiA_" + data.id).val(value.PositionA);
                            $(".inputRepB_" + data.id).val(value.RepresentB);
                            $(".inputPosiB_" + data.id).val(value.PositionB);

                            value.SupplierId = data.id;
                            value.QuoteSynId = $("#QuotesSynId").val();
                            value.Status = 0;


                            $(".inputRepA_" + data.id).change(function () {
                                value.RepresentA = $(this).val();
                            });

                            $(".inputPosiA_" + data.id).change(function () {
                                value.PositionA = $(this).val();
                            });

                            $(".inputRepB_" + data.id).change(function () {
                                value.RepresentB = $(this).val();
                            });

                            $(".inputPosiB_" + data.id).change(function () {
                                value.PositionB = $(this).val();
                            });

                            $(".inputMouthNumber_" + data.id).change(function () {
                                value.MouthNumber = $(this).val();
                            });

                            $(".inputPrice_" + data.id).change(function () {
                                value.Price = $(this).val();
                            })

                            $(".inputIndemnify_" + data.id).change(function () {
                                value.Indemnify = $(this).val();
                            })

                            $(".inputsellerDate_" + data.id).change(function () {
                                value.SellerDate = $(this).val();
                            })

                            $(".inputBankName_" + data.id).change(function () {
                                value.BankName = $(this).val();
                            })

                            $(".inputSTK_" + data.id).change(function () {
                                value.Stk = $(this).val();
                            })

                        }
                    });
                }

                //------------------------------------------------ / lay du lieu O Input --------------------------------
            });;

            $(".valueDate").text(currentdate.getDate());
            $(".valueMonth").text((currentdate.getMonth() + 1));
            $(".valueYear").text(currentdate.getFullYear());


        }

        //sự kiện khi đóng modal
        $(".close-modal").on("click", function () {
            abp.libs.sweetAlert.config = {
                confirm: {
                    icon: 'warning',
                    buttons: ['Không', 'Có']
                }
            };
            abp.message.confirm(
                app.localize('Đóng'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _modalManager.close();
                        return true;

                    }
                }
            );
        })

        this.save = function () {
            var data = {};
            var dataFilter = {};
            //var databuuton = [];
            //var buttons = $(".btn-supper");
            //for (var i = 0; i < buttons.length; i++) {

            //    databuuton.push(buttons[i].getAttribute("data-btn"));
            //}

        
            console.log(dataAllInput);

            //Tao moi hop dong
            //data.QuoteSynId = $("#QuotesSynId").val();
            //data.Status = 0;
            var setIf = dataAllInput.length;
            for (var i = 0; i < dataAllInput.length; i++) {
                /*data.SupplierId = databuuton[j];*/
                dataAllInput[i].Number = i + 1;
                _contractService.create(dataAllInput[i]).done(function (result) {
                    /*_contractService.createCode(result).done();*/
                });
            }
           


            dataFilter.Id = $("#QuotesSynId").val();
            dataFilter.Status = 4;
           
            _quotesSynService.updateStatus(dataFilter).done(function () {
                _modalManager.close();
                abp.notify.success(app.localize('Tạo hợp đồng thành công'));
                abp.event.trigger('app.reloadgetDocApprove');

                abp.event.trigger('app.reloadgetDocContact');
                //if (setIf > 1) {
                //    _contractService.updateCodeSame($("#QuotesSynId").val()).done(function () {
                //        abp.event.trigger('app.reloadgetDocContact');
                //    })
                //}
                //else {
                //    abp.event.trigger('app.reloadgetDocContact');
                //}
                
            }).always(function () {
                _modalManager.setBusy(false);
            });;



            //_quotesSynService.getQuoteApprove($("#QuotesSynId").val()).done(function (result) {
            //    /*console.log("dtaaa", result.supplierList);*/
              
            
            
          
                
                
            //    //for (var i = 0; i < result.supplierList.length; i++) {
            //    //    data.SupplierId = result.supplierList[i].supplierNameId;
            //    //    data.status = 0;

            //    //    console.log("dataLoca", data);
            //    //    _contractService.create(data).done(function () { });
            //    //}
            //    //
            

               
            //})
        };
    };
})(jQuery);