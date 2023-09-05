(function ($) {

    app.modals.ExportRequirementCreateModal = function () {


        var _itemsServiceService = abp.services.app.itemsService;
        var _exportRequests = abp.services.app.exportRequests;
        var _exportRequestsdetail = abp.services.app.exportRequestDetails;
        var _inventory = abp.services.app.inventoryService;
        var _warehouseLocationItemService = abp.services.app.warehouseLocationItemService;
        var _unitService = abp.services.app.unitService;
        var _modalManager;
        var _frmDelivery = null;


        this.init = function (modalManager) {
   

            $("#Uploadfile").attr('disabled',true);

            $('#WarehouseDestinationId').change( function () { 
                $("#Uploadfile").removeAttr('disabled');

                table
                    .rows()
                    .remove()
                    .draw();

            })

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
                            console.log(data)
                            return ` <span class="text-center total-item item_` + data[3] + `" data-codeItem="` + data[3] + `"></span>`;
                        }
                    },
                    {
                        targets: 4,
                        className: "TotalTK",
                        render: function (data, type, row, meta) {
                            return ` <input class="numberDC" type="number" value=` + data + ` min="0"/>`;
                        }
                    }, {
                        targets: 5,
                        className: "DVT",
                    }
                ],
                "initComplete": function (settings, json) {
                    ViewTotalItem();

                },

            });

            var ViewTotalItem = function (e) {

                //Get Total Item
                var html = "";
                var dataFilter = {};
                dataFilter.wareHouseId = $("#WarehouseDestinationId").val();
                for (var i = 0; i < $(".total-item").length; i++) {
                    dataFilter.searchTerm = $(".total-item")[i].getAttribute("data-codeItem");
                    _inventory.getViewItemInWare(dataFilter).done(function (result1) {
                        if (result1.quantity == 0) {
                            html = result1.quantity;
                            var DomTr = $(".item_" + result1.codeItem).parents("tr")
                            table.row($(DomTr)).remove().draw();
                          //  $(".item_" + result1.codeItem).parents("tr").remove();
                          
                        }
                        else {
                            html = result1.quantity;
                            $(".item_" + result1.codeItem).text(html);
                            html = "";
                        }
                    });
                }

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

                                columns.splice(3, 0, columns[1].split("-")[0]);

                                debugger
                                table.rows.add([columns]).draw();
                                getDocs();
                            }
                        });

                    };
                    reader.onerror = function (ex) {
                    };

                    reader.readAsBinaryString(file);
                };
            };

            /// upload file 
            function onClickHandler(ev) {
                var el = window._protected_reference = document.createElement("INPUT");
                el.type = "file";
                el.accept = "file/*";
                el.multiple = "multiple"; // remove to have a single file selection

                // (cancel will not trigger 'change')
                el.addEventListener('change', function (ev2) {
                    // access el.files[] to do something with it (test its length!)

                    // add first image, if available
                    if (el.files.length) {
                        // document.getElementById('out').src = URL.createObjectURL(el.files[0]);
                        var ext = el.files[0].name.split('.').pop().toLowerCase();
                        if ($.inArray(ext, ['xlsx', 'xls']) == -1) {
                            // alert('File không hợp lệ \nVui lòng nhập lại file');
                            abp.message.error('Vui lòng chọn lại file ', 'File không hợp lệ');
                        } else {
                            debugger
                            fodata = el.files[0];
                            if (fodata != null) {
                                //loadExcel();
                                handleFileSelect(fodata)
                            }
                        }
                    }
                    // test some async handling
                    new Promise(function (resolve) {
                        setTimeout(function () { resolve(); }, 1000);
                    })
                        .then(function () {
                            // clear / free reference
                            el = window._protected_reference = undefined;
                        });

                });

                el.click(); // open
            }

            $("#Uploadfile").on("click", function () {
              
                onClickHandler()
            })

            function getDocs() {
                table.ajax.reload(ViewTotalItem);

            }

            function handleFileSelect(evt) {
               // var files = evt.target.files; // FileList object
                var xl2json = new ExcelToJSON();
                xl2json.parseExcel(evt);
            }


            $(document).ready(function () {
                $('#fileupload').change(handleFileSelect);
            });
            //document.getElementById('fileupload').addEventListener('change', handleFileSelect, false);
            //$('#fileupload').on('change', function () {
            //    handleFileSelect
            //    /*$('#ItemTable').DataTable().destroy();*/
            //})


            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

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
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            var data = _frmDelivery.serializeFormToObject();
            _modalManager.setBusy(true);
            data.WarehouseDestinationId = $('#WarehouseDestinationId').find(':selected').val()
            _exportRequests.create(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới phiếu yêu cầu xuất kho thành công!');
                    abp.event.trigger('app.reloadDocTable');
                    $('#ItemTable tbody tr').each(function (index, value) {
                        var TotalInWare = parseInt($(value).children('td.TotalDc').find("span.total-item").text());
                        var Quantity = parseInt($(value).children('td.TotalTK').find('input').val());
                        var ExportPrice = parseInt($(value).children('td.Price').text());
                        _itemsServiceService.getItemByCode($(value).children('td.nameItem').text().split("-")[0]).done(function (result3) {
                            _unitService.getUnitByText($(value).children('td.DVT').text()).done(function (unit) {
                                data.ExportRequestId = result;
                                data.ItemId = result3.id;
                                data.Quantity = Quantity ;
                                data.ExportPrice = ExportPrice;
                                if ($(value).children('th').find('.BlockId option:selected').val() != "") {
                                    data.BlockId = $(value).children('th').find('.BlockId option:selected').val();

                                }
                                data.FloorId = $(value).children('th').find('.FloorId option:selected').val();
                                data.ShelfId = $(value).children('th').find('.ShelfId option:selected').val();
                                data.UnitId = unit.id;
                                data.UnitName = $(value).children('td.DVT').text();
                                debugger
                                _exportRequestsdetail.create(data)

                            })
                        })


                        //data.ExportRequestId = result;
                        //data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                        //data.Quantity = $(value).children('th').find('.Quantity ').val();
                        //data.ExportPrice = $(value).children('th').find('.ExportPrice ').val();
                        //if ($(value).children('th').find('.BlockId option:selected').val() != "") {
                        //    data.BlockId = $(value).children('th').find('.BlockId option:selected').val();

                        //}
                        //data.FloorId = $(value).children('th').find('.FloorId option:selected').val();
                        //data.ShelfId = $(value).children('th').find('.ShelfId option:selected').val();
                        //data.UnitId = $(value).children('th').find('.UnitId option:selected').val()
                        //_exportRequestsdetail.create(data)

                    })
                }).always(function () {
                    _modalManager.setBusy(false);
                });

        }
    };
})(jQuery);


