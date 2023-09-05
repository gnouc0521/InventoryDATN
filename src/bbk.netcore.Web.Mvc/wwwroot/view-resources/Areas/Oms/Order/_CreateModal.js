(function ($) {

    app.modals.OrderCreateModal = function () {

        var _$itemTable = $('#ItemTable');
        var _orderService = abp.services.app.order
        var _expertService = abp.services.app.expert;
        var _Contract = abp.services.app.contract;
        var _modalManager;
        var _frmIMP = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');

            $('#date-picker').datepicker({
                rtl: false,
                dateFormat: 'dd-mm--yy',
                orientation: "left",
                autoclose: true,
                language: abp.localization.currentLanguage.name,

            });

            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;

            var getFilterQuoteApprove = function () {
                let dataFilter = {};
                dataFilter.id = $('#Id').val();
                return dataFilter;
            }
            function inputchange() {
                $("#ItemTable tbody tr input").change(function () {
                 
                    var tdTotal = $(this).parents('tr').children('td:eq(5)');
                    var tdPrice = parseInt($(this).parents('tr').children('td:eq(4)').text());
                    tdTotal.text(numFormat(tdPrice * parseInt($(this).val())));
                });
                $('input[type="number"]').on('keyup', function () { if ($(this).val() > $(this).attr('max') * 1) { $(this).val($(this).attr('max')); } });
            }

            function addrow() {
                var tr = $('ItemTable tbody tr');

            }

            function tbodytr() {
                return html = `
                    <th><select class="form-control  selectUsers UserId " id="selectUsers" style="width:120px" required>
                    <option value="" title="" selected disabled > Chọn chuyên viên </option>
                </select></th>`;
            }
            function adduserselect() {
                var select = $('.selectUsers')
                $.each(select, function (index1, value1) {
                    var userid = $(value1).parents('tr').find('.selectUserId').attr('data-userid')
                    //  console.log($(value1).parents('tr').find('.selectUserId'))
                    _expertService.getAllUserALL().done(function (result1) {
                        $.each(result1.items, function (index, value) {
                            $(value1).append($('<option>', {
                                value: value.userId,
                                text: value.name
                            }));
                        })
                        $(value1).val(userid);
                    })
                })

            }

            var ApprovedataTable = _$itemTable.DataTable({
                paging: true,
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
                    ajaxFunction: _orderService.getAssignment,
                    inputFilter: getFilterQuoteApprove
                },
                order: [[0, 'asc']],
                columnDefs: [
                    {
                        orderable: false,
                        class: "Specifications",
                        targets: 0,
                        render: function (data, type, row, meta) {
                            return `<span data-specifications="` + row.specifications + `">` + (meta.row + 1 )+ `<\span>`;
                        }
                    },
                    //{
                    //    orderable: false,
                    //    targets: 1,
                    //    "render": function (data, type, row) {
                    //        return tbodytr()
                    //    }
                    //},
                    {
                        orderable: false,
                        targets: 1,
                        render: function (data, type, row, meta) {
                            return `<span class="selectUserId" data-UnitId=` + row.unitId + ` data-unitName="` + row.unitName + `"  data-itemId=` + row.itemId + ` data-quoteId=` + row.id + ` data-userid=` + row.userId + `>` + row.itemName + `</span>`
                        }
                    },
                    {
                        targets: 2,
                        orderable: false,
                        autoWidth: false,
                        data: 'unitName'
                    },
                    {
                        targets: 3,
                        orderable: false,
                        autoWidth: false,
                        data: 'quantityQuote',
                        render: function (data, type, row, meta) {
                            return `<input type='number' class='form-control Quantity custom-hiden-arrows'  value='` + row.quantityQuote + `' max="` + row.quantityQuote + `" min="1">`;
                        }

                    },

                    {

                        orderable: false,
                        targets: 4,
                        data: 'quotePrice',
                        class: 'quotePrice',

                    },
                    {

                        orderable: false,
                        targets: 5,
                        data: null,
                        render: function (data, type, row, meta) {
                            return numFormat(parseInt(row.quantityQuote, 10) * parseInt(row.quotePrice, 10));
                        }

                    },

                    {

                        orderable: false,
                        targets: 6,
                        data: 'note',
                        class: 'note'


                    },

                    {

                        orderable: false,
                        targets: 7,
                        data: null,
                        class:"text-center",
                        render: function (data, type, row, meta) {
                            return `<a class="delete_row" href="javascript:void(0);"><i class="fal fa-trash-alt  align-bottom "></i></a>`
                        }

                    },
                ],
                "initComplete": function (settings, json) {
                    // adduserselect();
                    // inputchange()
                    inputchange();
                },
            });
            $('#ItemTable').on('click', '.delete_row', function () {
                ApprovedataTable
                    .row($(this).parents('tr'))
                    .remove()
                    .draw();
            });


        }

       
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



            _frmIMP.addClass('was-validated');
            if (_frmIMP[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

          var  contractId = $('#Id').val()

            var data = {};
            _modalManager.setBusy(true);
            var select = $('#ItemTable').find('td .selectUserId');
            var total = $(select).length

            data.userid = abp.session.userId
            data.contractId = $('#Id').val();
            data.status = 1;
            _orderService.create(data)
                .done(function (result) {
                    abp.notify.info('Tạo đơn đặt hàng thành công!');
                 
                    _modalManager.close();
                    $.each(select, function (indexselect, valueselect) {
                        var input = {}
                        input.OrderId = result
                        input.ItemId = $(valueselect).parents('tr').find('td .selectUserId').attr('data-itemId');
                        input.UnitName = $(valueselect).parents('tr').find('td .selectUserId').attr('data-unitName');
                        input.Quantity = $(valueselect).parents('tr').find('td .Quantity').val();
                        input.UnitId = $(valueselect).parents('tr').find('td .selectUserId').attr('data-UnitId');
                        input.Note = $(valueselect).parents('tr').find('td .note').text().trim();
                        input.OrderPrice = $(valueselect).parents('tr').find('td.quotePrice').text();
                        input.Specifications = $(valueselect).parents('tr').find('td.Specifications').attr('data-specifications');
                        abp.services.app.ordersDetail.create(input).done(function (resultDetail) {
                            _orderService.checkContract({ id: contractId }).done(function (result) {
                                if (result == true) {
                                    _Contract.updateExportStatus({ id: contractId, exportStatus: 1 }).done(function () {
                                        abp.event.trigger('app.reloadgetDocContact');
                                    })
                                } else {
                                    abp.event.trigger('app.reloadgetDocContact');
                                }
                            })
                        })

                    })
                }).always(function () {
                    _modalManager.setBusy(false);
                });







            _modalManager.close()


        };
    };
})(jQuery);