(function ($) {
    app.modals.ExportRequirementEditModal = function () {

        var _$ExportRequestModal = $('#ItemTable');
        var _unitService = abp.services.app.unitService;
        var _exportRequests = abp.services.app.exportRequests;
        var _exportRequestsdetail = abp.services.app.exportRequestDetails;
        var _inventory = abp.services.app.inventoryService;
        var _modalManager;
        var _frmIMP = null;
        var arrdetails = [];
        var arrchange = [];

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=FrmEdit]');
           
            $("#WarehouseDestinationId").val($("#WarehouseDestinationId_hidden").val());
            $("#SubsidiaryId").val($("#SubsidiaryId_hidden").val());            


            var getFilter = function () {
                let dataFilter = {};
                dataFilter.exportRequestId = $('#Id').val()
                dataFilter.warehouseId = $('#WarehouseDestinationId').val()
                return dataFilter;
            }

            var dataTable = _$ExportRequestModal.DataTable({
                paging: false,
                serverSide: false,
                processing: false,
                "searching": false,
                "language": {
                    "emptyTable": "Không tìm thấy dữ liệu",
                    "lengthMenu": "Hiển thị _MENU_ bản ghi",
                },
                "bInfo": false,
                listAction: {
                    ajaxFunction: _exportRequestsdetail.getAll,
                    inputFilter: getFilter
                },
                columnDefs: [

                    {
                        targets: 0,
                        orderable: false,
                        className: 'dt-body-center text-center',
                        render: function (data, type, row, meta) {
                            var stt = meta.row + 1;
                            return `<span>` + stt + `</span>`;
                        }
                    },
                    {
                        orderable: false,
                        targets: 1,
                        data: 'itemsCode',
                        render: function (data, type, row, meta) {
                            return `<span>` + row.itemsCode + `-` + row.itemsName + `</span>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 2,
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<span>` + row.exportPrice + `</span>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 3,
                        data: 'quantityTotal',
                    },
                    {
                        orderable: false,
                        targets: 4,
                        data: 'quantityExport',
                    },
                     {
                        orderable: false,
                        targets: 5,
                         data: 'unitName',
                    },


                ],

            })

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
                            html = result1.quantity;
                            $(".item_" + result1.codeItem).text(html);
                            html = "";
                            $(".item_" + result1.codeItem).parent("td").parent("tr").css("color", "red");
                            $(".item_" + result1.codeItem).parent("td").parent("tr").find("input").attr('disabled', 'disabled');
                            $(".item_" + result1.codeItem).parent("td").parent("tr").find("select").attr('disabled', 'disabled');
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
            var data = _frmIMP.serializeFormToObject();
            _modalManager.setBusy(true);
            _exportRequests.update(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Sửa thành công!');
                    abp.event.trigger('app.reloadDocTable');
                    $('#ExportRequestModal tbody tr').each(function (index, value) {
                        data.Id = $(value).children('th:first').attr('data-id');
                        if (data.Id != undefined) {
                            arrchange.push(data.Id);
                            data.ExportRequestId = result;
                            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                            data.Quantity = $(value).children('th').find('.Quantity ').val();
                            data.ExportPrice = $(value).children('th').find('.ExportPrice ').val();
                            data.UnitId = $(value).children('th').find('.UnitId option:selected').val()
                            data.WarehouseSourceId = data.WarehouseDestinationId
                            if ($(value).children('th').find('.BlockId option:selected').val() != "") {
                                data.BlockId = $(value).children('th').find('.BlockId option:selected').val();

                            }
                            data.FloorId = $(value).children('th').find('.FloorId option:selected').val();
                            data.ShelfId = $(value).children('th').find('.ShelfId option:selected').val();
                            _exportRequestsdetail.update(data)
                        } else {
                            data.ExportRequestId = result;
                            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
                            data.Quantity = $(value).children('th').find('.Quantity ').val();
                            data.ExportPrice = $(value).children('th').find('.ExportPrice ').val();
                            data.WarehouseSourceId = data.WarehouseDestinationId
                            data.UnitId = $(value).children('th').find('.UnitId option:selected').val()
                            if ($(value).children('th').find('.BlockId option:selected').val() != "") {
                                data.BlockId = $(value).children('th').find('.BlockId option:selected').val();

                            }
                            data.FloorId = $(value).children('th').find('.FloorId option:selected').val();
                            data.ShelfId = $(value).children('th').find('.ShelfId option:selected').val();
                            _exportRequestsdetail.create(data)
                        }
                    })
                    let difference = arrdetails.filter(x => !arrchange.includes(x));
                    difference.forEach(myFunction);
                    function myFunction(value, index, array) {
                        _exportRequestsdetail.delete(value)
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);