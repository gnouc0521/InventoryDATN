(function () {

    var _$quoteHisTable = $("#QuoteHisTable")
    var _quoteSynService = abp.services.app.quotesSynthesise
    var _quoteRelService = abp.services.app.quotesRelationshipService
    var _quoteService = abp.services.app.quotesService
    /////////////////////////////////////////////////////////////////////////////////////
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.id = $('#QuoteId').val();
        return dataFilter;
    }


    var SyndataTable = _$quoteHisTable.DataTable({
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
            ajaxFunction: _quoteService.getHistoryDetail,
            inputFilter: getFilter
        },
        order: [[1, 'asc']],
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                data: 'creationTime',
                className: 'dt-body-center text-center',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: true,
                targets: 1,
                data:'itemName'
              
            },
            {
                orderable: false,
                targets: 2,
                data: "supplierName",
              
            },
            {
                orderable: false,
                targets: 3,
                data: "specifications"
            },
            {

                targets: 4,
                orderable: false,
                autoWidth: false,
                data: "unitName"
               
            },
            {

                targets: 5,
                data: 'quotePrice',
                orderable: false,
                autoWidth: false,

               
            },{

                targets: 6,
                data: 'note',
                orderable: false,
                autoWidth: false,

               
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
            console.log(el)
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
                            _quoteRelService.delete(
                                $(value).val()
                            ).done(function () {
                                // getUsers(true);
                                $('#example-select-all').prop('checked', false);
                                abp.notify.success(app.localize('Xóa trình báo thành công'));
                                getDocs();
                            });
                        }
                        // If checkbox doesn't exist in DOM
                        // If checkbox is checked
                        console.log($(value).val())

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

        SyndataTable.ajax.reload();
    }
    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });

})(jQuery);