(function () {

    var _$quoteTable = $("#QuoteTable")
    var _$quoteSynTable = $("#QuoteSynTable")
    var _quotesService = abp.services.app.quotesService
    var _quoteSynService = abp.services.app.quotesSynthesise
    var _quoteRelService = abp.services.app.quotesRelationshipService
    var ArraryQuoteId = new Array()

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Quote/CreateItems',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Quote/_CreateModal.js',
        modalClass: 'CreatDelModal',
        modalType: 'modal-xl'

    });
    $('#Add').on('click', function () {
        _createModal.open();
    })
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
                    fodata = el.files[0];
                    if (fodata != null) {
                        loadExcel();
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
    ////
    /* DEFINE TABLE */
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
                //className: 'dt-body-center text-center',
                //render: function (data, type, row, meta) {
                //    return '<input type="checkbox" name="" value="' + row.id + '">';
                //}
                className: "dt-control",
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
        "initComplete": function (settings, json) {
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


    $('#QuoteTable tbody').on('click', 'td.dt-control', function () {
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);
        var ItemsId = tr[0].firstElementChild.firstElementChild.dataset.objid;
        tr.attr('data-objid', ItemsId);
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('dt-hasChild shown');
            //   tr.css({ "padding": "", "color": "", "background-color": "", "box-shadow": "" })
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
                            data = new Date();
                            creationTime = new Date(row.creationTime)
                            if (((creationTime.getTime() - data.getTime()) / 60000) > 1) {

                                return '<input type="radio" name="' + $(meta.settings.nTable).attr('id') + '" value="' + row.id + '" data-items=' + row.itemId + '>';

                            } else {
                                return '<input type="radio" name="' + $(meta.settings.nTable).attr('id') + '" value="' + row.id + '" data-items=' + row.itemId + '>';
                            }

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
                "createdRow": function (row, data, index) {
                    datenow = new Date();
                    creationTime = new Date(data.creationTime)
                    if (((datenow.getTime() - creationTime.getTime()) / 60000) < 40) {
                        $(row).css("background-color", "#4c95fb");
                    }



                },
                "initComplete": function (settings, json) {

                    var kc = $(settings.nTable).find('tbody tr td input');
                    $.each(kc, function (index, value) {
                        $(this).click(function (e) {
                            var idchecked = $(settings.nTable).find('tbody tr td input:checked').val();
                            ArraryQuoteId.push(idchecked)
                            console.log(ArraryQuoteId)
                            var idcheck = new Array();
                            $(settings.nTable).find('tbody tr td input').not(':checked').each(function (index, value) {
                                idcheck.push($(value).val())
                                ArraryQuoteId = ArraryQuoteId.filter(function (item) {
                                    return item != $(value).val();
                                })
                            });
                            var checked = $(this).attr('checked');
                            if (checked) {
                                $(this).prop('checked', false)
                            } else {
                                $(this).prop('checked', true)
                            }
                            var checked = $(this).attr('checked', $(this).prop('checked'));
                        
                        });
                    })


                },
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
            });
        }
    })

    function ShowRowTbl(d, id) {
        return `<table id="ContractsTbl` + id + `" class=" ContractsTbl table table-bordered table-hover table-striped w-100" style="width: 100%;">` +
            `</table > `;
    }

    $('#Search').click(function (e) {
        e.preventDefault();
        getquoteTable();
    });
    function getquoteTable() {
        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });



    /////////////////////////////////////////////////////////////////////////////////////
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().trim();
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
            ajaxFunction: _quoteSynService.getAllByCreator,
            inputFilter: getFilter
        },
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return '<input type="checkbox" name="" value="' + row.id + '">';
                }
            },
            {
                orderable: true,
                targets: 1,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "code",
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <a class='doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > ` + row.code + ` </a>
                            </div>`;
                }
            },
            {
                orderable: false,
                targets: 3,
                data: null,
                render: function (data, type, row, meta) {
                    var html = "";
                    for (var i = 0; i < row.suppliersNames.length; i++) {
                        html += `<span>` + row.suppliersNames[i] + `</span><br>`
                    }
                    return html;
                }
            },
            {

                targets: 4,
                orderable: false,
                autoWidth: false,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }

            },
            {

                targets: 5,
                data: 'status',
                orderable: false,
                autoWidth: false,
                render: function (status) {
                    if (status == 0) {
                        return `<span class="span_status span-defaut"> Bản nháp </span>`
                    } else if (status == 2) {
                        return `<span class="span_status span-approve"> Đã Phê Duyệt </span>`
                    } else if (status == 3) {
                        return `<span class="span_status span-reject"> Từ chối </span>`
                    } else if (status == 1) {
                        return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                    }
                    else {
                        return `<span class="span_status span-contract"> Đã tạo hợp đồng </span>`
                    }
                }

            },

        ]
    });


    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = SyndataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#QuoteSynTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#QuoteSynTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#QuoteSynTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }

        if (!this.checked) {
            var el = $('#example-select-all').get(0);
            // If "Select all" control is checked and has 'indeterminate' property
            if (el && el.checked && ('indeterminate' in el)) {
                // Set visual state of "Select all" control
                // as 'indeterminate'
                el.indeterminate = true;
            }
        }
    });

    SyndataTable.$('input[type="checkbox"]').each(function (index, value) {
        if (value > 0) {
            $('#DeleteAll').removeAttr('disabled');
        }
    })

    $('#DeleteAll').on('click', function (e) {
        abp.message.confirm(
            app.localize('Xóa trình báo giá'),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    // Iterate over all checkboxes in the table
                    SyndataTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            _quoteSynService.delete(
                                $(value).val()
                            ).done(function () {
                                // getUsers(true);
                                $('#example-select-all').prop('checked', false);
                                abp.notify.success(app.localize('Xóa trình báo thành công'));
                                SyndataTable.ajax.reload();
                            });
                        }

                    }
                    );

                }
            });
    })

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {
        dataTable.ajax.reload();
        SyndataTable.ajax.reload();
    }

    $('#QuoteSynTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Quote/CompareDetail?SynthesiseId=" + results.id;
            },
        });
    });

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });



    $('#Choose_btn').on('click', function () {

        //$('#QuoteTable tbody input[type="radio"]:checked').each(function () {
        //    ArraryQuoteId.push($(this).val())
        //});

        var kc = $('#QuoteTable').find('tbody input:checked');
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize('Bạn có chắc chắn muốn chọn giá'),
            app.localize('Tổng hợp giá'),

            function (isConfirmed) {
                if (isConfirmed) {
                    console.log(ArraryQuoteId)
                    debugger
                    abp.services.app.quotesSynthesise.create({}).done(function (resultSynthesise) {
                        $.each(ArraryQuoteId, function (index, value) {
                            debugger

                            _quotesService.get({ id: value }).done(function (result) {
                                data = result;
                                data.id = 0;
                                data.quantity = result.QuantityQuote;
                                data.quotesSynthesiseId = resultSynthesise;
                                data.quoteId = value;
                                abp.services.app.quotesRequestsService.create(data).done(function () {
                                    SyndataTable.ajax.reload();
                                });
                            })
                        })
                        abp.notify.success(app.localize('Tổng hợp trình báo giá thành công'));

                        getDocs();

                    })

                }
            });

    })

})(jQuery);