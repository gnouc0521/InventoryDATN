(function ($) {
    var _recruitmentInfomationService = abp.services.app.recruitmentInfomation,
        _$modal = $('#Modal');

    function update() {
        var _$form = _$modal.find('form');
        validateRecruitment(_$form);
        if (!_$form.valid()) {
            return;
        }
        var data = _$form.serializeFormToObject();
        data.profileStaffId = parseInt($("#PersonId").val());
        abp.ui.setBusy(_$form);
        _recruitmentInfomationService.update(data)
            .done(function () {
                abp.notify.info('Cập nhật thành công!');
                _$modal.modal('hide');
                abp.event.trigger("recruitmentInfomation.updated");
            }).always(function () {
                abp.ui.clearBusy(_$form);
            });
    }
    function validateRecruitment(form) {
        form.addClass("was-validated");
        form.validate({
            invalidHandler: function (form, validator) {
                validator.errorList[0].element.focus();
            },
            errorClass: "invalid",
            validClass: "valid",
            rules: {
                "Expertise": {
                    required: true
                },
                "TimeElectNotice": {
                    required: true,
                    date: false
                },
                "WorkUnit": {
                    required: true

                },
                "TimeGetJob": {
                    required: true,
                    date: false
                },
                "RegulationsGuideProbation": {
                    required: true,

                },
                "TimeDecision": {
                    required: true,
                    date: false
                }

            },
            messages: {
                "Expertise": {
                    required: "Không được để trống chuyên môn"
                },
                "TimeElectNotice": {
                    required: "Không được để trống thời gian thông báo"
                },
                "WorkUnit": {
                    required: "Không được để trống đơn vị, vị trí công tác"
                },
                "TimeGetJob": {
                    required: "Không được để trống thời gian nhận việc"
                },
                "RegulationsGuideProbation": {
                    required: "Không được để trống quyết định hướng dẫn tập sự"
                },
                "TimeDecision": {
                    required: "Không được để trống thời gian ra quyết định"
                }

            }
        });
    }
    $(document).on('click', '.edit-recruitmentInfo #SaveButton', function (e) {
        update();
    });
})(jQuery);
