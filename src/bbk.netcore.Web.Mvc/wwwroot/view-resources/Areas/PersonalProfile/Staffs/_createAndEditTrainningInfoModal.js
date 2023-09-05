(function ($) {
    var oldlabletext;
    abp.event.on('trainningInfo.resetLabel', (data) => {
        oldlabletext = "Chọn tài liệu";
        $(".custom-file-label").text(oldlabletext);
    });
    let diplomaFile = null;
    var _trainningInfoService = abp.services.app.trainningInfo,
        _$modal = $('#Modal');
    $(document).on("change", "#DiplomaFile", function () {
        let fileData = $(this).prop("files")[0];
        if (fileData) {
            $(".custom-file-label").text(fileData.name);
            diplomaFile = fileData;
        } else {
            $(".custom-file-label").text(oldlabletext);
            diplomaFile = null;
        }
    });

    $('#Modal').on('show.bs.modal', () => {
        diplomaFile = null;
        oldlabletext = $(".custom-file-label").text();
    });

    $('#Modal').on('hide.bs.modal', () => {
        diplomaFile = null;
    });
    function uploadFile(id) {
        if (diplomaFile) {
            let formData = new FormData();
            formData.set("DiplomaFile", diplomaFile);
            formData.set("Id", id);
            abp.ajax({
                url: abp.appPath + 'PersonalProfile/Staffs/UploadDiplomaFile',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false
            }).done(function () {
                abp.event.trigger("trainningInfo.updated");
            });
        }
    }

    function create() {
        let _$form = _$modal.find('form');
        validateTrainningInfo(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _trainningInfoService.create(data)
                .done(function (result) {
                    uploadFile(result);
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("trainningInfo.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    function update() {
        var _$form = _$modal.find('form');
        validateTrainningInfo(_$form);
        if (_$form.valid()) {
            var data = _$form.serializeFormToObject();
            data.PersonId = parseInt($("#PersonId").val());
            abp.ui.setBusy(_$form);
            _trainningInfoService.update(data)
                .done(function (result) {
                    uploadFile(result);
                    abp.notify.info('Cập nhật thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("trainningInfo.updated");
                }).always(function () {
                    abp.ui.clearBusy(_$form);
                });
        }
    }

    $(document).on('click', '.create-trainningInfo #SaveButton', function (e) {
        create();
    });

    $(document).on('click', '.edit-trainningInfo #SaveButton', function (e) {
        update();
    });

    function validateTrainningInfo(form) {
        form.addClass("was-validated");
        form.validate({
            invalidHandler: function (form, validator) {
                validator.errorList[0].element.focus();
            },
            errorClass: "invalid",
            validClass: "valid",
            rules: {
                "SchoolName": {
                    required: true,
                    maxlength: 100
                },
                "MajoringName": {
                    required: true,
                    maxlength: 100
                },
                "TrainningType": {
                    required: true
                },
                "DiplomaId": {
                    required: true,
                    maxlength: 50
                },
                "FromDate": {
                    required: true,
                    date : false
                },
                "ToDate": {
                    required: true,
                    date: false,
                }
            },
            messages: {
                "SchoolName": {
                    required: "Bắt buộc nhập tên trường đào tạo",
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "MajoringName": {
                    required: "Bắt buộc nhập tên chuyên ngành đào tạo, bồi dưỡng",
                    maxlength: "Hãy nhập tối đa 100 ký tự"
                },
                "TrainningType": {
                    required: "Bắt buộc nhập loại hình đào tạo"
                },
                "DiplomaId": {
                    required: "Bắt buộc nhập văn bằng, chứng chỉ"
                },
                "FromDate": {
                    required: "Bắt buộc nhập ngày bắt đầu"
                },
                "ToDate": {
                    required: "Bắt buộc nhập ngày kết thúc",
                }
            }
        });
    }
})(jQuery);
