(function ($) {
    var _personService = abp.services.app.personalProfile;
    var coverImage = null;
    var _personService = abp.services.app.personalProfile,
        $statisticTable = $('#ProfileStatisticsTable');
    var _civilServantService = abp.services.app.civilServant,
        _$select = $('#CivilServantSector');
    $(document).ready(function () {
        _civilServantService.getAllCivilServant().done(function (result) {
            _$select.empty();
            for (let i = 0; i < result.length; i++) {
                _$select.append(`<option value="${result[i].id}">${result[i].name}</option>`);
            }
            onLoadCivilServant();
        });
    });
    //
    $(".file_img_avatar").bind("change", function () {
        let fileData = $(this).prop("files")[0];
        let math = ["image/png", "image/jpg", "image/jpeg"];
        if (!fileData) {
            $("#output").attr("src", null);
            return false;
        }
        if ($.inArray(fileData.type, math) === -1) {
            alert("Kiểu file không hợp lệ, chỉ chấp nhận jpg & png");
            $(this).val(null);
            return false;
        }
        if (typeof (FileReader) != "undefined") {
            let imagePreview = $("#image-cover-thumbnail");
            imagePreview.empty();
            let fileReader = new FileReader();
            fileReader.onload = function (element) {
                $("<img>", {
                    "src": element.target.result,
                    "id": "output",
                    "alt": "cover image",
                    "style": "width:264.463px; height: 150px"
                }).appendTo(imagePreview);
            }
            imagePreview.show();
            fileReader.readAsDataURL(fileData);
            coverImage = fileData;
        } else {
            alert("Trình duyệt không hỗ trợ đọc file.");
        }
    });

    var selectProvinceOfBirth = '#selectProvinceOfBirth',
        selectDistrictOfBirth = '#selectDistrictOfBirth',
        selectVillageOfBirth = '#selectVillageOfBirth',
        selectProvinceOfNativePlace = '#selectProvinceOfNativePlace',
        selectDistrictOfNativePlace = '#selectDistrictOfNativePlace',
        selectVillageOfNativePlace = '#selectVillageOfNativePlace',
        filePathDistrict = 'district.json',
        filePathVillage = 'village.json';

    function loadSelectAddress(filePath, superiorId, idSelect, txt) {
        var selectedName = $(idSelect).attr('data-select-id');
        changeStatusSelect(idSelect, txt, false);
        _personService.getAddress(filePath, superiorId).done(function (result) {
            for (let i = 0; i < result.addresses.length; i++) {
                $(idSelect).append(`<option value="${result.addresses[i].name}" data-id="${result.addresses[i].id}"
                                        ${selectedName == result.addresses[i].name ? 'selected' : ''}>
                                        ${result.addresses[i].name}
                                    </option>`);
            }
        });
    }

    $(document).on('change', selectProvinceOfBirth, function () {
        changeStatusSelect(selectVillageOfBirth, 'Chọn phường/xã', true);
        loadSelectAddress(filePathDistrict, $(selectProvinceOfBirth).find(':selected').attr('data-id'), selectDistrictOfBirth, 'Chọn quận/huyện');
    })
    $(document).on('change', selectDistrictOfBirth, function () {
        loadSelectAddress(filePathVillage, $(selectDistrictOfBirth).find(':selected').attr('data-id'), selectVillageOfBirth, 'Chọn phường/xã');
    })

    $(document).on('change', selectProvinceOfNativePlace, function () {
        changeStatusSelect(selectVillageOfNativePlace, 'Chọn phường/xã', true);
        loadSelectAddress(filePathDistrict, $(selectProvinceOfNativePlace).find(':selected').attr('data-id'), selectDistrictOfNativePlace, 'Chọn quận/huyện');
    })
    $(document).on('change', selectDistrictOfNativePlace, function () {
        loadSelectAddress(filePathVillage, $(selectDistrictOfNativePlace).find(':selected').attr('data-id'), selectVillageOfNativePlace, 'Chọn phường/xã');
    })

    function changeStatusSelect(idSelect, txt, stt) {
        $(idSelect).empty();
        $(idSelect).prop('disabled', stt);
        $(idSelect).append(`<option value="" selected disabled>` + txt + `</option>`);
    }

    function onLoadCivilServant(){
        _civilServantService.getCivilServant(_$select.val()).done(function (result) {
            $('#CivilServantSectorCode').val(result.code);

            _civilServantService.getAllSalaryLevelByGroup(result.group).done(function (rs2) {
                $('#PayRate').empty();
                for (let i = 0; i < rs2.length; i++) {
                    $('#PayRate').append(`<option value="${rs2[i].id}">${rs2[i].level}</option>`);
                }
                onLoadSalary();
            });
        });
    }

    function onLoadSalary() {
        _civilServantService.getSalaryLevel($('#PayRate').val()).done(function (result) {
            $('#Coefficientssalary').val(result.coefficientsSalary);
        });
    }

    $(document).on('click', '.create-person', function (e) {
        e.preventDefault();
        Save();
    });

    $(document).on('change', '#CivilServantSector', function (e) {
        onLoadCivilServant();
    });

    $(document).on('change', '#PayRate', function (e) {
        onLoadSalary();
    });

    $(document).on('click', '.save-statistic-profile', function () {
        loadFormStatisticProfile();
    });
    //
    function Save() {
        _$form = $('#create-profile-form');
        var input = _$form.serializeFormToObject();
        _$form.validateForm();
        if (!_$form.valid()) {
            return;
        }
        abp.ui.setBusy(_$form);
        _personService.create(input).done(function (result) {
            let formData = new FormData();
            formData.append("avatar", coverImage);
            formData.append("id", result);
            abp.ajax({
                url: abp.appPath + 'PersonalProfile/Staffs/UpdateAvatar',
                type: "post",
                cache: false,
                contentType: false,
                processData: false,
                data: formData,
            }).done(function () {
            }).always(function () {
            });
            abp.notify.info('Thêm mới thành công');
            setTimeout(function () { window.location.href = '/PersonalProfile/Staffs/Detail/' + result; }, 500);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });

    }
    $(document).on('click', '#upload_image', function () {

        $(".file_img_avatar").click();
    });

    function loadFormStatisticProfile() {
        $('#statistic-profile-body').empty();
        var formData = $('#create-profile-form').serializeFormToObject();
        $('#CreateProfileStatisticModal').modal('hide');
        $('#statistic-profile-body').append('<tr><td>' + getBoolEnum(formData.StaffBackground) + '</td><td>' + getBoolEnum(formData.CurriculumVitae) + '</td>'
            + '<td>' + getBoolEnum(formData.AdditionalStaffBackground) + '</td><td>' + getBoolEnum(formData.BriefBiography) + '</td>'
            + '<td>' + getBoolEnum(formData.BirthCertificate) + '</td><td>' + getBoolEnum(formData.HealthCertificate) + '</td>'
            + '<td>' + getBoolEnum(formData.PersonalIdentityDcuments) + '</td><td>' + getBoolEnum(formData.TrainingQualificationCV) + '</td>'
            + '<td>' + getBoolEnum(formData.RecruitmentDecision) + '</td><td>' + getBoolEnum(formData.DocumentsAppointingPosition) + '</td>'
            + '<td>' + getBoolEnum(formData.SelfAssessment) + '</td><td>' + getBoolEnum(formData.EvaluationComment) + '</td>'
            + '<td>' + getBoolEnum(formData.HandlingDocument) + '</td><td>' + getBoolEnum(formData.WorkProcessDocument) + '</td>'
            + '<td>' + getBoolEnum(formData.RequestFormToResearchRecord) + '</td><td>' + getBoolEnum(formData.DocumentObjective) + '</td>'
            + '<td>' + getBoolEnum(formData.DocumentClipCover) + '</td><td>' + getBoolEnum(formData.ProfileCover) + '</td>'
            + '<td>' + formData.Note + '</td></tr > ');
    }

    function getBoolEnum(val) {
        switch (val) {
            case "0":
                return "0";
                break;
            case "1":
                return "x";
                break;
            default:
                return "";
                break;
        }
    }
})(jQuery);