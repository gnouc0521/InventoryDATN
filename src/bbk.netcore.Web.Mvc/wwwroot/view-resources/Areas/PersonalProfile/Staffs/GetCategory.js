(function ($) {
    //serializeFormToObject plugin for jQuery
    $.fn.serializeFormToObject = function (camelCased = false) {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        if (camelCased && camelCased === true) {
            return convertToCamelCasedObject(obj);
        }

        return obj;
    };
    var _categoryService = abp.services.app.category,
        _$table = $('#CategoryTable');
    var _$categorysTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ajax: function (data, callback, settings) {
            
            var filter = {};
            filter.keyword = data.search.value;
            if (data.length == "-1") {
                filter.maxResultCount = 100;
            }
            else {
                filter.maxResultCount = data.length;
            }
            filter.skipCount = data.start;
            filter.categoryType = $("#CategoryType option:selected").val();
            abp.ui.setBusy(_$table);
            _categoryService.getList(filter).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            }).always(function () {
                abp.ui.clearBusy(_$table);
            });
        },
        rowId: "id",
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$categorysTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [

            {
                targets: 0,
                data: 'translate',
                sortable: false
            },
            {
                targets: 1,
                data: 'title',
                sortable: false
            },


        ]
    });
    $(document).on('change', '#CategoryType', function () {
       
        _$categorysTable.ajax.reload();
    });
    $('#CategoryTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$categorysTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    abp.event.on('create', (data) => {
        _$categorysTable.ajax.reload();
    });
    abp.event.on('upload', (data) => {
        _$categorysTable.ajax.reload();
    });

    $(document).on('click', '#EditCategory', function (e) {
        var id = _$categorysTable.row('.selected').id();
        abp.ajax({
            url: abp.appPath + `PersonalProfile/Staffs/EditCategoryModal?id=${id}`,
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                $('#EditCategoryModal div.modal-dialog').html(content);
                $('#EditCategoryModal').modal('toggle');
            },
            error: function (e) {
                $('#EditCategoryModal div.modal-dialog').html(null);
                abp.message.error('Chọn tên loại danh mục trước khi sửa!', 'Lỗi:');
                
            }
        });
    });
    $('#DeleteCategoryType').click(function () {
        var categoryTypeId = _$categorysTable.row('.selected').id();
        if (!categoryTypeId) {
            //abp.notify.error("Bạn phải chọn danh mục trước khi xóa!");
            abp.message.error('Chọn tên loại danh mục trước khi sửa!', 'Lỗi:');
            return;
        }

            abp.message.confirm(
                abp.utils.formatString("Bạn có chắc chắn muốn xóa!"),
                null,
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$table);
                        _categoryService.deleteById(categoryTypeId)
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                _$categorysTable.ajax.reload();
                            }).always(function () {
                                abp.ui.clearBusy(_$table);
                            });
                    }
                }
            );
        
        
    });

    //$(document).on('click', '.editCategory', function (e) {
    //    var id = _$categorysTable.row('.selected').id();
    //    if (id) {
    //        abp.ajax({
    //            url: abp.appPath + `PersonalProfile/Staffs/?id=${id}`,
    //            type: 'Get',
    //            dataType: 'html',
    //            success: function (content) {
    //                $('#WorkingProcessModal div.modal-dialog').html(content);
    //                $('#WorkingProcessModal').modal('toggle');
    //            },
    //            error: function (e) {
    //                $('#WorkingProcessModal div.modal-dialog').html(null);
    //            }
    //        });
    //    }
    //});
})(jQuery);

