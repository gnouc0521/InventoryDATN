(function ($) {
    var _categoryService = abp.services.app.category,
        _$modal = $('#EditCategoryModal');
    function update() {
        var _$form = _$modal.find('form');
        var data = _$form.serializeFormToObject();
        var id = $("#id_update").val();
        var categorytype = null;
        _categoryService.getIdCategory(id).done(function (result) {
            categorytype = result;
        });
        abp.ui.setBusy(_$form);
        _categoryService.update(data)
            .done(function () {
                abp.notify.info('Cập nhật thành công!');
                _$modal.modal('hide');
                if (categorytype == null) {
                    categorytype = 100;
                }
                $("#CategoryType").val(categorytype);
                //$('#CategoryTable tbody').on('load', 'tr', function () {
                //    $(this).val($("#CategoryType"))
                //    $(this).addClass('selected');
                    
                //});
                
                abp.event.trigger("upload");
            }).always(function () {
                abp.ui.clearBusy(_$form);
            });
    }
    $(document).on('click', '#SaveButton', function (e) {
        //var x = $("#updatetitle").val();
        //if (!x) {
        //    document.getElementById("message").innerHTML = "Vui lòng nhập ô này!";
        //    document.getElementById("updatetitle").style.borderColor = "red";
        //    return;
        //}
        var _$form = _$modal.find('form');
        _$form.validateForm();
        if (!_$form.valid()) {
            return;
        }
        update();
    });
 
})(jQuery);
