(function () {

    var _$quoteSynTable = $("#QuoteSynTable");
    var _$quoteTable = $("#QuoteTable")
    var _quoteSynService = abp.services.app.quotesSynthesise;
    var _quoteRelService = abp.services.app.quotesRelationshipService;
    var _quotesRequestsService = abp.services.app.quotesRequestsService;
    var _quotesService = abp.services.app.quotesService


    var ArraryQuoteId = new Array()
    var ArraryQuoteIdLocal = new Array()
    var ListitemId = [];
    var listQuoteId = [];
    var IdQuoteSyn = $('#Id').val();


    /////////////////////////////////////////////////////////////////////////////////////


    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });



    var getFilter = function () {
        let dataFilter = {};
        dataFilter.id = $('#SynthesiseId').val();
        return dataFilter;
    }


    var SyndataTable = _$quoteSynTable.DataTable({
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

        pageLength: 10,
        listAction: {
            ajaxFunction: _quoteSynService.detail,
            inputFilter: getFilter
        },
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    ListitemId.push(row.itemId)
                    listQuoteId.push(row.id)
                    ArraryQuoteId.push(row.id);
                    return meta.row + 1;
                }
            },
            {
                orderable: true,
                targets: 1,
                data: 'itemCode',
                render: function (data, type, row, meta) {
                    return `<span class="ItemsId" data-itemId='` + row.itemId + `'> ` + row.itemCode + `<span>`
                }

            },
            {
                orderable: false,
                targets: 2,
                data: "itemName",

            },
            {
                orderable: false,
                targets: 3,
                data: "supplierName"
            },
            {

                targets: 4,
                orderable: false,
                autoWidth: false,
                data: 'unitName',

            },
            {

                targets: 5,
                data: 'quotePrice',
                orderable: false,
                autoWidth: false,


            }, {

                targets: 6,
                data: 'quantityQuote',
                orderable: false,
                autoWidth: false,


            },
            {

                targets: 7,
                data: 'note',
                orderable: false,
                autoWidth: false,


            },

        ]
    });



    $(".btn-browse").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.status = 2;
        dataFilter.id = btnClick[0].dataset.quo;
        dataFilter.link = window.location.href;
        _quoteSynService.update(dataFilter).done(function () {
            abp.notify.success(app.localize('Phê duyệt trình báo giá thành công'));
            getDocs();
            $(".btn-browse").prop("disabled", true);
            $(".btn-reject").prop("disabled", true);
        });
    })

    $(".btn-reject").click(function () {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quo };
        _createModalReject.open(dataFilter);
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
        $(".btn-browse").prop("disabled", true);
        $(".btn-reject").prop("disabled", true);
    });

    abp.event.on('app.reloadDocTableSyn', function () {
        getSyndataTable();
    });

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {
        ListitemId = unique(ListitemId);
        listQuoteId = unique(listQuoteId);
        debugger
        dataTable.ajax.reload(loadoptionselected(ListitemId, listQuoteId));
    }
    function getSyndataTable() {
        SyndataTable.ajax.reload();
    }
    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });


    // ham xoa phan tu giong nhau
    function unique(arr) {
        var newArr = []
        for (var i = 0; i < arr.length; i++) {
            if (newArr.indexOf(arr[i]) === -1) {
                newArr.push(arr[i])
            }
        }
        return newArr
    }
    // Premission update Quote
   
    /////////////////////////////////////////////////////////////////////////////////////
    $('#Update').click(function () {
        $('.Quote-Update').toggle();

    });
    $('#SendTo').click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.status = 1;
        dataFilter.id = btnClick[0].dataset.obj;
        dataFilter.email = $("#email").val();
        dataFilter.link = window.location.href;
        dataFilter.name = $("#name").val();
        _quoteSynService.update(dataFilter).done(function () {
            abp.notify.success(app.localize('Gửi trình báo giá thành công'));
            $("#SendTo").attr("hidden", true);
            $("#Update").attr("hidden", true);
            $('.Quote-Update').hide();
        });
    });


    var getFilter = function () {
        let dataFilter = {};
        //dataFilter.searchTerm = $('#SearchTerm').val();
        return dataFilter;
    }

    var dataTable = _$quoteTable.DataTable({
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

        pageLength: 10,
        listAction: {
            ajaxFunction: _quotesService.getAll,
            inputFilter: getFilter
        },
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: "dt-control",
                data: 'name',
                render: function (data, type, row, meta) {
                    return `
                        <span class='bindofferid_`+ row.id + ` itemsId' selected='false' data-objid='` + row.id + `'></span>
                                <span class="ml-auto">
                            <span class="collapsed-reveal" style="display:none">
                                <i class="fal fa-minus-circle text-danger"></i>
                            </span>
                            <span class="collapsed-hidden">
                                <i class="fal fa-plus-circle text-success"></i>
                            </span>
                            `+ row.name + `
                        </span>
                        `;
                }
            },
            {
                orderable: true,
                targets: 1,
                data: 'name',
                render: function (data, type, row, meta) {
                    return `
                        <span class='bindofferid_`+ row.id + `' selected='false' data-objid='` + row.id + `'></span>
                                <span class="ml-auto">
                            <span class="collapsed-reveal" style="display:none">
                                <i class="fal fa-minus-circle text-danger"></i>
                            </span>
                            <span class="collapsed-hidden">
                                <i class="fal fa-plus-circle text-success"></i>
                            </span>
                        </span>
                        `;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "name"
            },
            {
                orderable: false,
                targets: 3,
                data: "name"
            },
            {
                orderable: false,
                targets: 4,
                data: "name"
            },

            {

                targets: 5,
                data: 'id',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `<div class='text-right'> 
                                <a class='doceditfunc text-right' data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                            </div>`;
                }
            }, {

                targets: 6,
                data: 'id',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `<div class='text-right'> 
                                <a class='doceditfunc text-right' data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                            </div>`;
                }
            }

        ],
        "drawCallback": function (settings) {
            ListitemId = unique(ListitemId);
            listQuoteId = unique(listQuoteId);
            loadoptionselected(ListitemId, listQuoteId)
            ArraryQuoteId = unique(ArraryQuoteId)
            console.log(ArraryQuoteId)
        },
        "initComplete": function () {
            ArraryQuoteIdLocal = ArraryQuoteId
        },
        createdRow: function (row, data, dataIndex) {
            // If name is "Ashton Cox"
            // Add COLSPAN attribute
            $('td:eq(0)', row).attr('colspan', 8);

            // Hide required number of columns
            // next to the cell with COLSPAN attribute
            $('td:eq(1)', row).css('display', 'none');
            $('td:eq(2)', row).css('display', 'none');
            $('td:eq(3)', row).css('display', 'none');
            $('td:eq(4)', row).css('display', 'none');
            $('td:eq(5)', row).css('display', 'none');
            $('td:eq(6)', row).css('display', 'none');
            $('td:eq(7)', row).css('display', 'none');

            datenow = new Date();
            creationTime = new Date(data.creationTime)
            if (((datenow.getTime() - creationTime.getTime()) / 60000) < 40) {
                $(row).css("background-color", "#4c95fb");
            }
            // Update cell data
            this.api().cell($('td:eq(1)', row)).data("N/A");
        }
    });






    function loadoptionselected(ListitemId, listQuoteId) {
        debugger
        var td = $('#QuoteTable tbody td.dt-control span.itemsId')
        $.each(td, function (index, value) {
            for (var i = 0; i < ListitemId.length; i++) {
                if (ListitemId[i] == $(value).attr('data-objid')) {
                    var tr = $(value).parents('td.dt-control')
                    var tr = $(this).closest('tr');
                    var row = dataTable.row(tr);
                    var ItemsId = tr[0].firstElementChild.firstElementChild.dataset.objid;
                    tr.attr('data-objid', ItemsId);
                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                        tr.removeClass('dt-hasChild shown');
                        $(this).closest('tr').find('.collapsed-reveal').hide()
                        $(this).closest('tr').find(".collapsed-hidden").show()
                    }
                    else {
                        // Open this row
                        row.child(ShowRowTbl(row.data(), ItemsId)).show();
                        tr.addClass('dt-hasChild shown');
                        $(this).closest('tr').find('.collapsed-reveal').show()
                        $(this).closest('tr').find(".collapsed-hidden").hide()
                        //Khai bao table
                        var _$ContractTaskTable = $('#ContractsTbl' + ItemsId);
                        //Khai bao gia tri nhap vao
                        var getFilter = function () {
                            let dataFilter = {};
                            if (ItemsId != null) {
                                dataFilter.itemsId = ItemsId;
                            }

                            return dataFilter;
                        }

                        datacontractTable = _$ContractTaskTable.DataTable({
                            paging: false,
                            serverSide: false,
                            processing: true,
                            ordering: true,
                            info: false,
                            "searching": false,
                            language: {
                                emptyTable: "Không tìm thấy dữ liệu",
                                lengthMenu: "Hiển thị _MENU_ bản ghi",
                            },
                            "bLengthChange": false,
                            pageLength: 10,
                            listAction: {
                                ajaxFunction: _quotesService.getQuotebyItemsid,
                                inputFilter: getFilter
                            },
                            order: [[3, 'desc']],
                            columnDefs: [
                                {
                                    title: "NCC",
                                    targets: 1,
                                    className: "text-center",
                                    data: 'supplierName',
                                },
                                {

                                    targets: 0,
                                    data: null,
                                    defaultContent: '',
                                    orderable: false,
                                    mRender: function (data, type, row, meta) {
                                        var html = "";
                                        if (listQuoteId.indexOf(row.id) != -1) {
                                            ArraryQuoteId = ArraryQuoteId.filter(function (item) {
                                                return item != row.id;
                                            })
                                            html = '<input type="radio"  name="' + $(meta.settings.nTable).attr('id') + '" value="' + row.id + '" check="checked" checked  >';
                                        }
                                        else {
                                            html = '<input type="radio"  name="' + $(meta.settings.nTable).attr('id') + '" value="' + row.id + '">';

                                        }
                                        return html;
                                    }
                                },
                                {
                                    orderable: true,
                                    title: "THÔNG SỐ KỸ THUẬT",
                                    targets: 2,
                                    className: "text-center",
                                    data: 'specifications',

                                },
                                {
                                    orderable: true,
                                    title: "ĐVT",
                                    targets: 3,
                                    className: "text-center",
                                    data: 'unitName',

                                },
                                {
                                    orderable: false,
                                    title: "ĐƠN GIÁ/ĐVT",
                                    targets: 4,
                                    className: "text-center ",
                                    data: 'quotePrice',

                                },
                                {
                                    orderable: false,
                                    title: "SỐ LƯỢNG",
                                    targets: 5,
                                    className: "text-center ",
                                    data: 'quantityQuote',

                                },
                                {
                                    orderable: false,
                                    title: "NGÀY BÁO GIÁO",
                                    targets: 6,
                                    className: "text-center ",
                                    data: "content",
                                    data: 'creationTime',
                                    render: function (creationTime) {
                                        return moment(creationTime).format('L');
                                    }

                                }, {
                                    orderable: false,
                                    title: "GHI CHÚ",
                                    targets: 7,
                                    className: "text-center ",
                                    data: "content",
                                    data: 'note',
                                },
                            ],
                            select: {
                                style: 'os',
                                selector: 'td:first-child'
                            },
                            "createdRow": function (row, data, index) {
                                datenow = new Date();
                                creationTime = new Date(data.creationTime)
                                if (((datenow.getTime() - creationTime.getTime()) / 60000) < 40) {
                                    $(row).css("background-color", "#4c95fb");
                                }

                            },
                            "initComplete": function (settings, json) {
                                var kc = $(settings.nTable).find('tbody tr td input');
                             //   ArraryQuoteIdLocal = unique(ArraryQuoteIdLocal);
                                debugger
                                $.each(kc, function (index, value) {
                                    $(this).click(function (e) {
                                        debugger
                                        var idchecked = $(settings.nTable).find('tbody tr td input:checked').val();
                                        ArraryQuoteIdLocal = ArraryQuoteIdLocal.filter(function (item) {
                                            return item != parseInt(idchecked);
                                        })
                                        ArraryQuoteIdLocal.push(parseInt(idchecked))
                                        var idcheck = new Array();
                                        $(settings.nTable).find('tbody tr td input').not(':checked').each(function (index, value) {
                                            idcheck.push(parseInt($(value).val()))
                                            ArraryQuoteIdLocal = ArraryQuoteIdLocal.filter(function (item) {
                                                return item != parseInt($(value).val());
                                            })
                                        });
                                        console.log(ArraryQuoteIdLocal)
                                        var checked = $(this).attr('checked');
                                        if (checked) {
                                            $(this).prop('checked', false)
                                        } else {
                                            $(this).prop('checked', true)
                                        }
                                        $(this).attr('checked', $(this).prop('checked'));
                                    });
                                })
                            }
                        });
                    }
                }
            }


        })

    }

    function ShowRowTbl(d, id) {
        return `<table id="ContractsTbl` + id + `" class=" ContractsTbl table table-bordered table-hover table-striped w-100" style="width: 100%;">` +
            `</table > `;
    }

    $('#QuoteTable tbody').on('click', 'td.dt-control', function () {
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);
        var ItemsId = tr[0].firstElementChild.firstElementChild.dataset.objid;
        tr.attr('data-objid', ItemsId);
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('dt-hasChild shown');
            $(this).closest('tr').find('.collapsed-reveal').hide()
            $(this).closest('tr').find(".collapsed-hidden").show()
        }
        else {
            //   tr.css({ "padding": "15px 25px", "color": "#fff", "background-color": "rgb(255 135 90 / 70%)", "box-shadow": "0 9px #999" })
            // Open this row
            row.child(ShowRowTbl(row.data(), ItemsId)).show();
            tr.addClass('dt-hasChild shown');
            $(this).closest('tr').find('.collapsed-reveal').show()
            $(this).closest('tr').find(".collapsed-hidden").hide()
            //Khai bao table
            var _$ContractTaskTable = $('#ContractsTbl' + ItemsId);
            //Khai bao gia tri nhap vao
            var getFilter = function () {
                let dataFilter = {};
                if (ItemsId != null) {
                    dataFilter.itemsId = ItemsId;
                }

                return dataFilter;
            }

            datacontractTable = _$ContractTaskTable.DataTable({
                paging: false,
                serverSide: false,
                processing: true,
                ordering: true,
                info: false,
                "searching": false,
                language: {
                    emptyTable: "Không tìm thấy dữ liệu",
                    lengthMenu: "Hiển thị _MENU_ bản ghi",
                },
                "bLengthChange": false,
                pageLength: 10,
                listAction: {
                    ajaxFunction: _quotesService.getQuotebyItemsid,
                    inputFilter: getFilter
                },
                order: [[3, 'desc']],
                columnDefs: [
                    {
                        title: "NCC",
                        targets: 1,
                        className: "text-center",
                        data: 'supplierName',
                    },
                    {

                        targets: 0,
                        data: null,
                        defaultContent: '',
                        orderable: false,
                        //className: 'select-checkbox',

                        mRender: function (data, type, row, meta) {
                            var html = "";
                            if (listQuoteId.indexOf(row.id) != -1) {
                                html = '<input type="radio"  name="' + $(meta.settings.nTable).attr('id') + '" value="' + row.id + '" checked>';
                            }
                            else {
                                html = '<input type="radio"  name="' + $(meta.settings.nTable).attr('id') + '" value="' + row.id + '">';

                            }
                            return html;
                        }
                    },
                    {
                        orderable: true,
                        title: "THÔNG SỐ KỸ THUẬT",
                        targets: 2,
                        className: "text-center",
                        data: 'specifications',

                    },
                    {
                        orderable: true,
                        title: "ĐVT",
                        targets: 3,
                        className: "text-center",
                        data: 'unitName',

                    },
                    {
                        orderable: false,
                        title: "ĐƠN GIÁ/ĐVT",
                        targets: 4,
                        className: "text-center ",
                        data: 'quotePrice',

                    },
                    {
                        orderable: false,
                        title: "SỐ LƯỢNG",
                        targets: 5,
                        className: "text-center ",
                        data: 'quantityQuote',

                    },
                    {
                        orderable: false,
                        title: "NGÀY BÁO GIÁO",
                        targets: 6,
                        className: "text-center ",
                        data: "content",
                        data: 'creationTime',
                        render: function (creationTime) {
                            return moment(creationTime).format('L');
                        }

                    },
                    {
                        orderable: false,
                        title: "GHI CHÚ",
                        targets: 7,
                        className: "text-center ",
                        data: "content",
                        data: 'note',
                    },
                ],
                "initComplete": function (settings, json) {
                    var kc = $(settings.nTable).find('tbody tr td input');
                    ArraryQuoteId = unique(ArraryQuoteId);
                    $.each(kc, function (index, value) {
                        $(this).click(function (e) {
                            var idchecked = $(settings.nTable).find('tbody tr td input:checked').val();
                            ArraryQuoteIdLocal = ArraryQuoteIdLocal.filter(function (item) {
                                return item != idchecked;
                            })
                            ArraryQuoteIdLocal.push(parseInt(idchecked))
                            var idcheck = new Array();
                            $(settings.nTable).find('tbody tr td input').not(':checked').each(function (index, value) {
                                idcheck.push(parseInt( $(value).val()))
                                ArraryQuoteIdLocal = ArraryQuoteIdLocal.filter(function (item) {
                                    return item != $(value).val();
                                })
                            });
                            var checked = $(this).attr('checked');
                            if (checked) {
                                $(this).prop('checked', false)
                            } else {
                                $(this).prop('checked', true)
                            }
                            $(this).attr('checked', $(this).prop('checked'));
                        });
                    })
                },

                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },

            })
        }

    })

    // event add file 
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
                    fodata = el.files[0];
                    if (fodata != null) {
                        loadExcel();
                    }
                }
            }
            // test some async handling
            new Promise(function (resolve) {
                setTimeout(function () {resolve(); }, 1000);
            })
                .then(function () {
                    // clear / free reference
                    el = window._protected_reference = undefined;
                });

        });

        el.click(); // open
    }
    $("#Import_btn").on("click", function () {
        onClickHandler()
    })
    function loadExcel() {
        let formData = new FormData();
        formData.append("file", fodata);
        abp.ajax({
            url: '/Inventorys/Quote/ImportExcel',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            dataType: "json",
            success: (function (response) {
            })
        }).done(function () {
            abp.notify.info('Cập nhật file báo giá thành công!');
            getDocs();
        })
    }

    // cập nhật lại giá

    $('#Choose_btn').on('click', function () {
        var kc = $('#QuoteTable').find('tbody input:checked');
        var listQuoteIdNew = [];
        $.each(kc, function (index, value) {
            listQuoteIdNew.push($(value).val());
        })
        listQuoteIdNew = listQuoteIdNew.map(i => Number(i));
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize('Bạn có chắc chắn muốn cập nhật lại giá'),
            app.localize('Tổng hợp giá'),

            function (isConfirmed) {
                if (isConfirmed) {
                    abp.services.app.quotesSynthesise.getQuoteId({ id: IdQuoteSyn }).done(function (result) {
                        let differenceAdd = ArraryQuoteIdLocal.filter(x => !listQuoteId.includes(x));
                        let differenceDelete = listQuoteId.filter(x => !ArraryQuoteIdLocal.includes(x));
                        let intersection = listQuoteId.filter(x => ArraryQuoteIdLocal.includes(x));

                        $.each(differenceAdd, function (index, value) {
                            _quotesService.get({ id: value }).done(function (result) {
                                data = result;
                                data.id = 0;
                                data.quantity = result.QuantityQuote;
                                data.quotesSynthesiseId = IdQuoteSyn;
                                data.quoteId = value;
                                abp.services.app.quotesRequestsService.create(data).done(function () {
                                    abp.event.trigger('app.reloadDocTableSyn');
                                  //  getDocs();
                                });
                            })
                        })

                        $.each(differenceDelete, function (index, value) {
                            _quotesRequestsService.deleteQuoteRequests({
                                quotesSynthesiseId: IdQuoteSyn, quoteId: value
                            }).done(function () {
                                abp.event.trigger('app.reloadDocTableSyn');
                              //  getDocs();
                            })

                        })
                      
                       
                        abp.notify.success(app.localize('Tổng hợp trình báo giá thành công'));
                    })
                }
            });
    })


})(jQuery);