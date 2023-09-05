(function ($) {
    var _propertyDeclarationService = abp.services.app.propertyDeclaration,
        _$modal = $('#Modal');

    var oldlabletext;
    abp.event.on('propertydeclaration.resetLabel', (data) => {
        oldlabletext = "Chọn tài liệu";
        $(".custom-file-label").text(oldlabletext);
    });
    let file = null;

    $(document).on("change", "#PropertyDeclarationFile", function () {
        let fileData = $(this).prop("files")[0];
        if (fileData) {
            $(".custom-file-label").text(fileData.name);
            file = fileData;
        } else {
            $(".custom-file-label").text(oldlabletext);
            file = null;
        }
    });

    $('#Modal').on('show.bs.modal', () => {
        file = null;
        oldlabletext = $(".custom-file-label").text();
    });

    $('#Modal').on('hide.bs.modal', () => {
        file = null;
    });

    function uploadFile(id) {
        if (file) {
            let formData = new FormData();
            formData.set("File", file);
            formData.set("Id", id);
            abp.ajax({
                url: abp.appPath + 'PersonalProfile/Staffs/UploadPropertyDeclarationFile',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false
            }).done(function () {
                abp.event.trigger("propertyDeclaration.updated");
            });
        }
    }

    function update() {
        var _$form = _$modal.find('form');
        _$form.validatePropertyDeclaration();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.personId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _propertyDeclarationService.update(data)
                .done(function (result) {
                    uploadFile(result);
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("propertyDeclaration.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function create() {
        var _$form = _$modal.find('form');
        _$form.validatePropertyDeclaration();
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _propertyDeclarationService.create(data) 
                .done(function (result) {
                    uploadFile(result);
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("propertyDeclaration.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '#Modal .create-propertyDeclaration #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '#Modal .edit-propertyDeclaration #SaveButton', function (e) {
        update();
    });

    $.fn.validatePropertyDeclaration = function () {
        $(this).addClass("was-validated");
        
        validatorRelationShip = $(this).validate({
            invalidHandler: function (form, validator) {
                validator.errorList[0].element.focus();
            },
            errorClass: "invalid",
            validClass: "valid",
            rules: {
                "Year": {
                    required: true
                },
                "IsExist": {
                    required: true
                }
            },
            messages: {
                "Year": {
                    required: "Không được để trống trường này"
                },
                "IsExist": {
                    required: "Không được để trống trường này"
                }
            }
        });
    }

})(jQuery);
