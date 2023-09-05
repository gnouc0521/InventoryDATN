(function () {
    var _$SynTable = $('#SynTable');
    var _PurchasesSyn = abp.services.app.purchasesSynthesise;
    var _PurchasesDetail = abp.services.app.purchasesRequestDetailService;
    var _email = abp.services.app.sendMailService;
    moment.locale(abp.localization.currentLanguage.name);
    var dataOld = [];
    var dataNew = [];
    var flag = 0;

    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/PurchasesRequest/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/PurchasesRequests/_CreateModalContractReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    function _disableInput() {
        if ($("#Edit").is(":hidden")) {
            $('#SynTable tbody tr td input').removeAttr('disabled');
        }
    }

    var disable_buttons = function () {

      
        $("#Edit").unbind("click").click(function () {
          
            // var _datatable = dataTable.row().data();
            $('#SynTable tbody tr td input').removeAttr('disabled');
            dataTable.data().each(function (d) {
                dataOld.push(d.purchasesDetailId)
            });
            $(this).hide();
            $('#Save').show();
            $("#SendTo").attr('disabled', true);

            
            dataTable.column(9).visible(true);
            $('#Save').removeAttr('hidden');

            //$('#SynTable').on('page.dt', function () {
            //   // var info = table.page.info();
            //    //  $('#pageInfo').html('Showing page: ' + info.page + ' of ' + info.pages);
            //    alert("add")
            //});

            //$('#SynTable')
            //    .on('order.dt', function (e) {
            //        // console.log("adđ")
            //        eventFired(e.currentTarget)
            //    })
            //    .on('search.dt', function (e) {

            //        eventFired(e.currentTarget)
            //        //console.log("adđ")

            //    })
            //    .on('page.dt', function (e, setting) {

            //        eventFired(e.currentTarget)
            //    })


            $('#SynTable tbody tr td input').each(function (index, value) {
                var rowIndexes = [];
                $(value).change(function () {
                    var valueinput = $(this).val()
                    var somevalue = $(this).attr('data-objid')
                    dataTable.rows(function (idx, data, node) {
                        // console.log(data, somevalue)
                        if (data.purchasesDetailId === parseInt(somevalue)) {
                            var newData = data
                            newData.quantityItems = parseInt(valueinput);
                            rowIndexes.push(idx);
                            dataTable.row(idx).data(newData).draw()
                            //dataTable.ajax.reload();

                        }

                        return false;
                    });

                });

            })
            //dataTable.rows().data();
        })

    }

    // disable_buttons()

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.id = $('#SynthesiseId').val().trim();
        return dataFilter;
    }


    var dataTable = _$SynTable.DataTable({
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

        pageLength: 15,
        listAction: {
            ajaxFunction: _PurchasesSyn.getAllItems,
            inputFilter: getFilter
        },

        columnDefs: [

            {
                targets: 0,
                orderable: false,
                class: 'text-center',
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },

            {
                orderable: false,
                targets: 1,
                data: 'itemsName',
            },
            {
                targets: 2,
                data: 'subsidiariesName',
                orderable: false,
                autoWidth: false,

            },
            {
                targets: 3,
                data: 'supplierName',
                orderable: false,
                autoWidth: false,
            },
            {

                targets: 4,
                data: 'unitName',
                orderable: false,
                autoWidth: false,
            },
            {
                targets: 5,
                data: 'quantityItems',
                orderable: false,
                autoWidth: false,
                orderable: true,
                render: function (data, type, row, meta) {
                    return `<input class='form-control' data-objid="` + row.purchasesDetailId + `" value='` + row.quantityItems + `' disabled >`
                }
            },
            {
                targets: 6,
                data: 'purpose',
                orderable: false,
                autoWidth: false,
            },
            {
                targets: 7,
                data: 'dateTimeNeed',
                orderable: false,
                autoWidth: false,
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                targets: 8,
                data: 'note',
                orderable: false,
                autoWidth: false,

            },
            {
                targets: 9,
                data: null,
                "visible": false,
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `<a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a>`;
                }

            },

        ],
        "fnDrawCallback": function () {
            flag++;
            _disableInput();
            disable_buttons(flag);
        }
    });



    /// Chỉnh sửa thông tin phiếu tổng hợp///

    var eventFired = function (e) {
        var input = $($(e).find('tbody tr td input'));
        $.each(input, function (index, value) {
            $(value).removeAttr('disabled')
        })
    }

    $('#SynTable').on('click', '.delete_row', function () {
        //var table = $('#example').DataTable();
        dataTable
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });

    $('#Save').click(function () {
        var btnclick = $(this)
        var dataupdate = [];


        dataTable.data().each(function (d) {
            dataNew.push(d.purchasesDetailId)
            let data = {};
            data.id = d.purchasesDetailId;
            data.quantity = d.quantityItems
            dataupdate.push(data)
        });


        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };

        abp.message.confirm(
            app.localize('Lưu thay đổi'),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    dataTable.column(9).visible(false);
                    let datadelete = dataOld.filter(x => !dataNew.includes(x));
                    console.log(datadelete)
                    for (var i = 0; i < datadelete.length; i++) {
                        _PurchasesDetail.delete(datadelete[i]).done(function () {
                            getDocs()
                        })

                    }
                    //console.log(dataupdate)
                    for (var i = 0; i < dataupdate.length; i++) {
                        _PurchasesDetail.updateQuantity({ id: dataupdate[i].id, quantity: dataupdate[i].quantity }).done(function () {
                            getDocs()
                        })
                    }
                    btnclick.hide();
                    $('#Edit').show();
                    $("#SendTo").removeAttr('disabled')
                    abp.notify.info('Chỉnh sửa phiếu tổng hợp thành công!');
                }
            }
        );

    })

    //////////////////////////////////////////////////


    $("#SendTo").click(function () {
        var btnClick = $(this);
        let dataFilter = {};

        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };

        abp.message.confirm(
            app.localize('Gửi phiếu yêu cầu tổng hợp'),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    dataFilter.purchasesRequestStatus = 1;
                    dataFilter.purchasesSynthesiseId = btnClick[0].dataset.obj;
                    dataFilter.email = $("#email").val();
                    dataFilter.link = window.location.href;
                    dataFilter.name = $("#name").val();
                    _PurchasesSyn.updateStatus(dataFilter).done(function () {
                        abp.notify.info('Gửi phiếu tổng hợp thành công!');
                        btnClick.hide();
                        $('#Edit').hide();
                        $('#Save').hide();
                    })

                }
            }
        );

    })

    $(".btn-browse").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.purchasesRequestStatus = 2;
        dataFilter.purchasesSynthesiseId = btnClick[0].dataset.quo;
        dataFilter.link = window.location.href;
        _PurchasesSyn.updateStatus(dataFilter).done(function () {
            abp.notify.info('Phê duyệt phiếu tổng hợp thành công!');
            $(".btn-browse").prop("hidden", true);
            $(".btn-reject").prop("hidden", true);
        })
    })

    $(".btn-reject").click(function () {
        var btnClick = $(this);
        debugger
        var dataFilter = { purchasesSynthesiseId: btnClick[0].dataset.quo };
        dataFilter.link = window.location.href;
        _createModalReject.open(dataFilter, function (callback) {
            btnClick.hide();
            $(".btn-browse").hide()

        });

    });




    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    function getDocs() {
        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });




})(jQuery);