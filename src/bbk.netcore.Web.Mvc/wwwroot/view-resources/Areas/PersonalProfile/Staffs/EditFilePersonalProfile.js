(function ($) {
    var filename = $(".custom-file-label").val();
    var _uploadfileService = abp.services.app.uploadFileProfile;
    var _$modal = $('#Modal');
    var file;
    $(document).on("change", "#PersonalProfile_Edit_File", function () {
        let fileData = $(this).prop("files")[0];
        if (fileData) {
            $(".custom-file-label").text(fileData.name);
            if (fileData.size > 10485760) //10 MB
            {
                abp.message.warn(app.localize('Dung lượng file không được quá 10Mb!'));
                return false;
            }
            file = fileData;
        } else {
            $(".custom-file-label").text(filename);
            file = null;
        }
    });
    $(document).on('click', '#Modal .edit-file-personalprofile #SaveButton', function (e) {
        e.preventDefault();
        edit();
     
    });
    function edit() {
        let _$form = _$modal.find('form');
        _$form.validateForm();
        if (_$form.valid()) {
            var id = $("#id_file_edit").val();
            if (id) {
                let formData = new FormData();
                formData.set("File", file);
                formData.set("Id", id);
                formData.set("PersonalId", parseInt($("#PersonId").val()));
                formData.set("Title", $("#Title_File_Edit").val());
                abp.ui.setBusy(_$form);
                abp.ajax({
                    url: abp.appPath + 'PersonalProfile/Staffs/EditFilePersonalProfile',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false
                }).done(function () {
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("file.updated");
                });
                abp.ui.clearBusy(_$form);
            }
        }
    }
   
})(jQuery);