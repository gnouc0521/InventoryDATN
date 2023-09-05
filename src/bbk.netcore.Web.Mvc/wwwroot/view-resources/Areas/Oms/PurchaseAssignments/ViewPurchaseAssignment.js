(function ($) {
    app.modals.ViewModal = function () {

        var _$StaffTable = $('#StaffTable');
        var _PurchasesSyn = abp.services.app.purchasesSynthesise;
        var _purchaseAssignment = abp.services.app.purchaseAssignmentService;
        var _itemsServiceService = abp.services.app.itemsService;
        var _expertService = abp.services.app.expert;
        var _assignService = abp.services.app.assignments;
        var _modalManager;
        var _frmIMP = null;
        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');

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

            var getFilter = function () {
                let dataFilter = {};
                dataFilter.id = $('#SynthesiseId').val().trim();
                return dataFilter;
            }

            var dataTable = _$StaffTable.DataTable({
                paging: false,
                serverSide: true,
                processing: true,
                "searching": false,
                "language": {
                    "emptyTable": "Không tìm thấy dữ liệu",
                    "lengthMenu": "Hiển thị _MENU_ bản ghi",
                },
                "bInfo": false,
                "bLengthChange": false,
                //"dom": 'Rltip',
                lengthMenu: [
                    [5, 10, 25, 50, -1],
                    [5, 10, 25, 50, 'Tất cả'],
                ],

                pageLength: 10,
                listAction: {
                    ajaxFunction: _PurchasesSyn.getAllItemByExpert,
                    inputFilter: getFilter
                },
                columnDefs: [
                    {
                        orderable: false,
                        targets: 0,
                        render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },
                    {
                        orderable: false,
                        targets: 1,
                        data: 'nameStaff',
                        "render": function (data, type, row) {
                            return tbodytr()

                        }
                    },
                    {
                        orderable: false,
                        targets: 2,
                        data: null,
                        render: function (data, type, row, meta) {
                            return `<span class="selectUserId" data-userid=` + row.userId + `>` + row.itemcode + `</span>`
                        }
                    },
                    {
                        orderable: false,
                        targets: 3,
                        className: "itemsName",
                        data: "itemsName"
                    },
                    {
                        orderable: false,
                        targets: 4,
                        className: "supplierName",
                        data: "supplierName"
                    },
                    {
                        orderable: false,
                        targets: 5,
                        className: "unitName",
                        data: "unitName"
                    },
                    {
                        orderable: false,
                        targets: 6,
                        className: "quantityItems",
                        data: "quantityItems"
                    },
                    {
                        orderable: false,
                        targets: 7,
                        className: "timeNeeded",
                        data: "timeNeeded",
                        render: function (creationTime) {
                            return moment(creationTime).format('L');
                        }
                    },
                    {
                        orderable: false,
                        targets: 8,
                        data: "note",
                    },
                ],
                "initComplete": function (settings, json) {
                    adduserselect();
                },
            });

        }


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
            var data = _frmIMP.serializeFormToObject();
            _modalManager.setBusy(true);
            _modalManager.close();
            abp.notify.info('Phân công thành công');
            var dataa = $('#StaffTable').DataTable().rows().data();
            var QHT = $(".selectUsers");
            dataa.each(function (value, index) {
                if (value.nameStaff == null) {
                    _itemsServiceService.getItemByCode(value.itemcode).done(function (result3) {
                        debugger
                        data.PurchasesSynthesiseId = value.id;
                        data.ItemId = result3.id;
                        data.UserId = $($(QHT)[index]).val()
                        data.PurchasesSynthesiseCode = value.purchasesSynthesiseCode;
                        data.GetPriceStatus = 2;
                        data.status = 2;
                        data.price = 0;
                        data.link = window.location.href;
                        _purchaseAssignment.create(data).done(function () {
                            abp.event.trigger('app.reloadTable');
                            abp.event.trigger('app.reloadDocTable');

                        });
                        _PurchasesSyn.updateSyn(data).done(function () {
                            abp.event.trigger('app.reloadTable');
                            abp.event.trigger('app.reloadDocTable');

                        });
                        _assignService.create(data).done(function () {
                            abp.event.trigger('app.reloadTable');
                            abp.event.trigger('app.reloadDocTable');

                        });
                       
                    })
                }
                else {
                    _itemsServiceService.getItemByCode(value.itemcode).done(function (result3) {
                        data.PurchasesSynthesiseId = value.id;
                        data.ItemId = result3.id;
                        data.UserId = $($(QHT)[index]).val()
                        data.GetPriceStatus = 2;
                        data.status = 2;
                        data.price = 0;
                        data.link = window.location.href;
                        data.PurchasesSynthesiseCode = value.purchasesSynthesiseCode;
                        _purchaseAssignment.create(data).done(function () {
                            abp.event.trigger('app.reloadTable');
                            abp.event.trigger('app.reloadDocTable');

                        });
                        _PurchasesSyn.updateSyn(data).done(function () {
                            abp.event.trigger('app.reloadTable');
                            abp.event.trigger('app.reloadDocTable');

                        });
                        _assignService.update(data).done(function () {
                            abp.event.trigger('app.reloadTable');
                            abp.event.trigger('app.reloadDocTable');

                        });
                    })
                }
            })
        };
    };
})(jQuery);