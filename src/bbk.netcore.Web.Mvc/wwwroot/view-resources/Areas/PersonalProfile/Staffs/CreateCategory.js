(function ($) {
    var _categoryService = abp.services.app.category,
        _$modal = $('#createCategory');

    function create() {
        var _$form = _$modal.find('form');
        var data = _$form.serializeFormToObject();
        var x = null;
        var categoryType = data.CategoryType;
        abp.ui.setBusy(_$form);
     
        _categoryService.create(data)
            .done(function () {
                abp.notify.info('Thêm mới thành công!');
                _$modal.modal('hide');
                //$(".modal-body input").val("");
                x = $("#CreateCategoryType").val();
                $("#CategoryType").val(x);
                abp.event.trigger("create");
            }).always(function () {
                abp.ui.clearBusy(_$form);
            });

    }


    $(document).on('click', '#addRowBtn', function () {
        //var x = $("#title").val();
        //if (!x) {
        //    document.getElementById("thongbao").innerHTML = "Vui lòng nhập ô này!";
        //    document.getElementById("title").style.borderColor = "red";
        //    return;
        //}
        var _$form = _$modal.find('form');
        _$form.validateForm();
        if (!_$form.valid()) {
            return;
        }
        create();
    });
    //$('#title').on('change', function () {
    //    var x = $("#title").val();
    //    if (!x) {
    //        document.getElementById("thongbao").innerHTML = "Vui lòng nhập ô này!";
    //        document.getElementById("title").style.borderColor = "red";
    //    }
    //    else
    //    {
    //        document.getElementById("thongbao").innerHTML = "";
    //        document.getElementById("title").style.borderColor = "purple";
    //    }
    //});
})(jQuery);
