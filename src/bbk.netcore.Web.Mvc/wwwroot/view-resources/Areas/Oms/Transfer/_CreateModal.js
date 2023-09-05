(function ($) {

    app.modals.TransferCreateModal = function () {

        var _$itemTable = $('#ItemTable');
        var _inventory = abp.services.app.inventoryService;
        var _transferService = abp.services.app.transfer;
        var _transferDetail = abp.services.app.transferDetail;
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var _warehouseService = abp.services.app.wareHouse;
        var _modalManager;
        var _frmIMP = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');


            $('.date-picker').datepicker({
                rtl: false,
                format: 'dd/mm/yyyy',
                orientation: "left",
                autoclose: true,
                language: abp.localization.currentLanguage.name,

            });


            var table = $('#ItemTable').DataTable({
                paging: true,
                serverSide: false,
                processing: false,
                "searching": false,
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
                columnDefs: [
                    { targets: 0 },
                    {
                        targets: 1,
                        className: "nameItem",
                    },
                    {
                        targets: 2,
                        className: "Price",
                    },
                    {
                        targets: 3,
                        className: "TotalDc",
                        data: null,
                        render: function (data, type, row, meta) {
                            return ` <input class="numberDC form-control" type="number" min="0" required/>`;
                        }
                    },
                    {
                        targets: 4,
                        className: "TotalTK",
                        render: function (data, type, row, meta) {
                            return ` <span class="text-center total-item item_` + data + `" data-codeItem="` + data + `"></span>`;
                        }
                    },
                    {
                        targets: 5,
                        className: "DVT",
                    },
                    {
                        targets: 6,
                        className: "WareImport",
                        data: null,
                        render: function (data, type, row, meta) {
                            return ` <select class="warehouseImport form-control">
                                        <option selected value="" disable>Chọn kho</option>
                                    </select>`;
                        }
                    },
                ],
                "initComplete": function (settings, json) {
                    ViewTotalItem();

                },

            });
            $('#ItemTable').on('page.dt', function () {
                ViewTotalItem()
            });


            var ViewTotalItem = function (e) {

                //Get Total Item
                var html = "";
                var dataFilter = {};
                dataFilter.wareHouseId = $("#WarehouseDestinationId").val();
               
                
                for (var i = 0; i < $(".total-item").length; i++) {
                    dataFilter.searchTerm = $(".total-item")[i].getAttribute("data-codeItem");
                    /*console.log("ssdsds", $(".total-item")[i].getAttribute("data-codeItem"));*/
                    _inventory.getViewItemInWare(dataFilter).done(function (result1) {
                        if (result1.quantity == 0) {
                            $(".item_" + result1.codeItem).parent("td").parent("tr").remove();
                        }
                        else {
                            html = result1.quantity;
                            $(".item_" + result1.codeItem).text(html);
                            html = "";

                            $(".item_" + result1.codeItem).parent("td").parent("tr").children("td.TotalDc").find("input").attr("max", result1.quantity);

                        }
                    });
                }
                $('input[type="number"]').on('keyup', function () { if ($(this).val() > $(this).attr('max') * 1) { $(this).val($(this).attr('max')); } });
    
            }



            var ExcelToJSON = function () {
                this.parseExcel = function (file) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var data = e.target.result;
                        var workbook = XLSX.read(data, {
                            type: 'binary'
                        });



                        workbook.SheetNames.forEach(function (sheetName) {
                            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName], { raw: false });
                            debugger
                            var productList = JSON.parse(JSON.stringify(XL_row_object));

                            

                            for (i = 0; i < productList.length; i++) {
                                var columns = Object.values(productList[i]);
                            
                                columns.splice(3, 0, "", columns[1].split("-")[0]);
                                
                                table.rows.add([columns]).draw();
                                getDocs();
                            }

                            $('#ItemTable tr td select').each(function (index, value1) {
                                _warehouseService.getAllExceptId({ searchTerm: "",id: $("#WarehouseDestinationId").val() }).done(function (result2) {
                                    var optionHtml = "";
                                    $.each(result2.items, function (index, value) {
                                        optionHtml = `<option value="` + value.id + `">` + value.name + `</option>`;
                                        $(value1).append(optionHtml);

                                    })
                                })
                            })

                            //$('#ItemTable tr td input').each(function (index, value1) {
                            //    _warehouseService.getAllExceptId({ searchTerm: "", id: $("#WarehouseDestinationId").val() }).done(function (result2) {
                            //        var optionHtml = "";
                            //        $.each(result2.items, function (index, value) {
                            //            optionHtml = `<option value="` + value.id + `">` + value.name + `</option>`;
                            //            $(value1).append(optionHtml);

                            //        })
                            //    })
                            //})
                        });
                        
                    };
                    reader.onerror = function (ex) {
                    };

                    reader.readAsBinaryString(file);
                };
            };

            function getDocs() {
                table.ajax.reload(ViewTotalItem);

            }

            function handleFileSelect(evt) {
                var files = evt.target.files; // FileList object
                var xl2json = new ExcelToJSON();
                xl2json.parseExcel(files[0]);
            }

            document.getElementById('fileupload').addEventListener('change', handleFileSelect, false);
            $('#fileupload').on('change', function () {
                /*$('#ItemTable').DataTable().destroy();*/
            })


        }
        //sự kiện khi đóng modal
        $('.cancel-work-button').click(function () {
            abp.libs.sweetAlert.config = {
                confirm: {
                    icon: 'warning',
                    buttons: ['Không', 'Có']
                }
            };

            abp.message.confirm(
                app.localize('Đóng phiếu nhập'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _modalManager.close();
                        return true;

                    }
                }
            );

        });
        this.save = function () {
            _frmIMP.addClass('was-validated');
            if (_frmIMP[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            var dataTransfer = {};
            var length = $('#ItemTable tbody tr').length


            if (length == 0) {
                abp.message.warn(app.localize('Chưa nhập dữ liệu bảng'));
                $('#ItemTable').DataTable().destroy();
            }
            else {
                var data = _frmIMP.serializeFormToObject();
                data.Status = 0;
                data.exportStatus = 0;
                data.CommentText = "";
                _modalManager.setBusy(true);
                
                _transferService.create(data).done(function (result6) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới Phiếu yêu cầu thành công!');
                    abp.event.trigger('app.reloadDocTable');

                    $('#ItemTable tbody tr').each(function (index, value) {
                        var TotalInWare = parseInt($(value).children('td.TotalTK').find("span.total-item").text());

                        var iteamNames = $(value).children('td.nameItem').text().split("-")[0];
                        var dvt = $(value).children('td.DVT').text();
                        debugger
                        _itemsServiceService.getItemByCode($(value).children('td.nameItem').text().split("-")[0]).done(function (result3) {
                            _unitService.getUnitByText($(value).children('td.DVT').text()).done(function (unit) {
                                dataTransfer.TransferId = result6;
                                dataTransfer.ItemName = $(value).children('td.nameItem').text().split("-")[1];
                                dataTransfer.ItemCode = $(value).children('td.nameItem').text().split("-")[0];
                                dataTransfer.QuotePrice = $(value).children('td.Price').text();
                                dataTransfer.QuantityInStock = $(value).children('td.TotalTK').find("span.total-item").text();
                                dataTransfer.QuantityTransfer = $(value).children('td.TotalDc').find("input").val();
                                dataTransfer.UnitName = $(value).children('td.DVT').text();
                                dataTransfer.IdWarehouseReceiving = $(value).children('td.WareImport').find(".warehouseImport option:selected").val();
                                dataTransfer.WarehouseReceivingName = $(value).children('td.WareImport').find(".warehouseImport option:selected").text();
                                dataTransfer.ItemId = result3.id;
                                dataTransfer.IdUnit = unit.id;

                                console.log("ádasasd",dataTransfer);
                              
                                _transferDetail.create(dataTransfer)
                            });
                        });

                    })
                }).always(function () {
                        _modalManager.setBusy(false);
                    });

                


                //_importRequestService.create(data)
                //    .done(function (result) {
                //        _modalManager.close();
                //        abp.notify.info('Thêm mới loại kho thành công!');
                //        abp.event.trigger('app.reloadDocTable');

                //        //if (document.getElementById("fileupload").files.length == 0) {
                //        //    $('#ItemTable tbody tr').each(function (index, value) {
                //        //        data.ImportRequestId = result;
                //        //        data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                //        //        data.ImportPrice = $(value).children('th').find('.ImportPrice ').val();
                //        //        data.Quantity = $(value).children('th').find('.Quantity ').val();
                //        //        data.UnitId = $(value).children('th').find('.UnitId option:selected').val();
                //        //        data.UnitName = $(value).children('th').find('.UnitId option:selected').text();
                //        //        data.MFG = $(value).children('th').find('.MFG ').val();
                //        //        data.ExpireDate = $(value).children('th').find('.ExpireDate ').val();
                //        //        _importRequestsdetail.create(data)
                //        //    })
                //        //}
                //        //else {

                //        //}
                //        var dataa = $('#ItemTable').DataTable().rows().data();
                //        dataa.each(function (value, index) {
                //            //get Item
                //            _itemsServiceService.getItemByCode(value[1]).done(function (result3) {
                //                _unitService.getUnitByText(value[4]).done(function (unit) {
                //                    data.ItemId = result3.id;
                //                    data.ImportRequestId = result;
                //                    data.ImportPrice = value[2];
                //                    data.Quantity = value[3];
                //                    data.UnitName = value[4];
                //                    data.UnitId = unit.id;
                //                    data.MFG = value[5];
                //                    data.ExpireDate = value[6]
                //                    _importRequestsdetail.create(data);
                //                })

                //            });
                //        });
                //    }).always(function () {
                //        _modalManager.setBusy(false);
                //    });
            };
        };
    };
})(jQuery);