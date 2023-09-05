(async function ($) {
    var _personService = abp.services.app.personalProfile;
    var _reportService = abp.services.app.report;

    var coverImage = null;
    var coverImage = null;
    let userAvatar = null;

    var _civilServantService = abp.services.app.civilServant,
        _$select = $('#CivilServantSector');

    _civilServantService.getAllCivilServant().done(function (result) {
        _$select.empty();
        var csId = _$select.attr('data-cs-id');
        for (let i = 0; i < result.length; i++) {
            _$select.append(`<option value="${result[i].id}" ${result[i].id == csId ? 'selected' : ''}>${result[i].name}</option>`);
        }
        onLoadCivilServant();
    });


    var selectProvinceOfBirth = '#selectProvinceOfBirth',
        selectDistrictOfBirth = '#selectDistrictOfBirth',
        selectVillageOfBirth = '#selectVillageOfBirth',
        selectProvinceOfNativePlace = '#selectProvinceOfNativePlace',
        selectDistrictOfNativePlace = '#selectDistrictOfNativePlace',
        selectVillageOfNativePlace = '#selectVillageOfNativePlace',
        filePathDistrict = 'district.json',
        filePathVillage = 'village.json';
    loadAddress(selectDistrictOfBirth, filePathDistrict, $(selectProvinceOfBirth).val(), selectVillageOfBirth, filePathVillage);
    loadAddress(selectDistrictOfNativePlace, filePathDistrict, $(selectProvinceOfNativePlace).val(), selectVillageOfNativePlace, filePathVillage);

    function loadAddress(selectDistrict, filePathDistrict, provinceId, selectVillage, filePathVillage) {
        if (provinceId != null) {
            _personService.getAddress(filePathDistrict, provinceId).done(function (districts) {
                for (let i = 0; i < districts.addresses.length; i++) {
                    $(selectDistrict).append(`<option value="${districts.addresses[i].id}" ${checkSelected(selectDistrict, districts.addresses[i].name)}>${districts.addresses[i].name}</option>`);
                }
                if ($(selectDistrict).val() != null) {
                    loadVillage(selectDistrict, selectVillage, filePathVillage);
                }
            })
        }
    }

    function loadVillage(selectDistrict, selectVillage, filePathVillage) {
        _personService.getAddress(filePathVillage, $(selectDistrict).val()).done(function (villages) {
            for (let i = 0; i < villages.addresses.length; i++) {
                $(selectVillage).append(`<option value="${villages.addresses[i].id}" ${checkSelected(selectVillage, villages.addresses[i].name)}>${villages.addresses[i].name}</option>`);
            }
        });
    }

    function checkSelected(idSelect, value) {
        if ($(idSelect).attr('data-select-id') != null && value != null) {
            var txt = $(idSelect).attr('data-select-id').trim().toLowerCase();
            var valCheck = value.trim().toLowerCase();
            if (txt.split(' ')[txt.split(' ').length - 1] == valCheck.split(' ')[valCheck.split(' ').length - 1]
                && txt.split(' ')[txt.split(' ').length - 2] == valCheck.split(' ')[valCheck.split(' ').length - 2]) {
                return 'selected';
            }
        }
        return '';
    }

    function changeStatusSelect(idSelect, txt, stt) {
        $(idSelect).empty();
        $(idSelect).prop('disabled', stt);
        $(idSelect).append(`<option value="" selected disabled>` + txt + `</option>`);
    }

    $(document).on('change', selectProvinceOfBirth, function () {
        changeStatusSelect(selectDistrictOfBirth, "Chọn quận/huyện", false);
        loadAddress(selectDistrictOfBirth, filePathDistrict, $(selectProvinceOfBirth).val(), selectVillageOfBirth, filePathVillage);
        changeStatusSelect(selectVillageOfBirth, "Chọn xã/phường", true);
    });
    $(document).on('change', selectDistrictOfBirth, function () {
        loadVillage(selectDistrictOfBirth, selectVillageOfBirth, filePathVillage);
        changeStatusSelect(selectVillageOfBirth, "Chọn xã/phường", false);
    });
    $(document).on('change', selectProvinceOfNativePlace, function () {
        changeStatusSelect(selectDistrictOfNativePlace, "Chọn quận/huyện", false);
        loadAddress(selectDistrictOfNativePlace, filePathDistrict, $(selectProvinceOfNativePlace).val(), selectVillageOfNativePlace, filePathVillage);
        changeStatusSelect(selectVillageOfNativePlace, "Chọn xã/phường", true);
    });
    $(document).on('change', selectDistrictOfNativePlace, function () {
        loadVillage(selectDistrictOfNativePlace, selectVillageOfNativePlace, filePathVillage);
        changeStatusSelect(selectVillageOfNativePlace, "Chọn xã/phường", false);
    });

    $("#file").bind("change", function () {
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

            let formData = new FormData();
            formData.append("avatar", fileData);
            formData.append("id", $('#PersonId').val());
            userAvatar = formData;
        } else {
            alert("Trình duyệt không hỗ trợ đọc file.");
        }
    });

    function onLoadCivilServant() {
        _civilServantService.getCivilServant(_$select.val()).done(function (result) {
            $('#CivilServantSectorCode').val(result.code);

            _civilServantService.getAllSalaryLevelByGroup(result.group).done(function (rs2) {
                $('#PayRate').empty();
                var sId = $('#PayRate').attr('data-salary-id');
                for (let i = 0; i < rs2.length; i++) {
                    $('#PayRate').append(`<option value="${rs2[i].id}" ${rs2[i].id == sId ? 'selected' : ''}>${rs2[i].level}</option>`);
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

    $(document).on('change', '#CivilServantSector', function (e) {
        onLoadCivilServant();
    });

    $(document).on('change', '#PayRate', function (e) {
        onLoadSalary();
    });

    $(document).on('click', '.update-person', function (e) {
        e.preventDefault();
        Save();
    });

    $(document).on('click', '.save-statistic-profile', function () {
        loadFormStatisticProfile();
    });
    $(document).on('click', '#upload_image', function () {

        $(".file_img_avatar").click();
    });

    function Save() {
        _$form = $('#profile-form');
        _$form.validateForm();
        if (!_$form.valid()) {
            return;
        }
        var input = _$form.serializeFormToObject();
        input.ProvinceOfBirth = $(selectProvinceOfBirth + ' option:selected').text();
        input.DistrictOfBirth = $(selectDistrictOfBirth + ' option:selected').text();
        input.VillageOfBirth = $(selectVillageOfBirth + ' option:selected').text();
        input.ProvinceOfNativePlace = $(selectProvinceOfNativePlace + ' option:selected').text();
        input.DistrictOfNativePlace = $(selectDistrictOfNativePlace + ' option:selected').text();
        input.VillageOfNativePlace = $(selectVillageOfNativePlace + ' option:selected').text();
        abp.ui.setBusy(_$form);
        _personService.update(input).done(function () {
            if (userAvatar != null) {

                $.ajax({
                    url: abp.appPath +'PersonalProfile/Staffs/UpdateAvatar',
                    type: "post",
                    cache: false,
                    contentType: false,
                    processData: false,
                    data: userAvatar,
                    success: function (result) {
                    },
                    error: function (error) {
                        abp.message.error('Cập nhật ảnh không thành công!', 'Lỗi:');
                        return;
                    }
                });
            }
            abp.notify.info('Cập nhật thành công');
        }).always(function () {
            abp.ui.clearBusy(_$form);
        }); 

    }

    function loadFormStatisticProfile() {
        $('#statistic-profile-body').empty();
        var formData = $('#profile-form').serializeFormToObject();
        $('#EditProfileStatisticModal').modal('hide');
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

    $(document).on('click', '.export-hs02', function (e) {
        e.preventDefault();
        _$form = $('#profile-form');
        abp.ui.setBusy(_$form);
        var input = _$form.serializeFormToObject();
        input.currentPosition = $('#CurrentPosition option:selected').text();
        input.organizationUnit = $('#OrganizationUnit option:selected').text();
        input.civilServantSector = $('#CivilServantSector option:selected').text();
        input.payRate = $('#PayRate option:selected').text();
        input.highestAcademicLevel = $('#HighestAcademicLevel option:selected').text();
        input.politicsTheoReticalLevel = $('#PoliticsTheoReticalLevel option:selected').text();
        input.woundedSoldierRank = $('#WoundedSoldierRank option:selected').text();
        input.ProvinceOfBirth = $('#selectProvinceOfBirth' + ' option:selected').text();
        input.DistrictOfBirth = $('#selectDistrictOfBirth' + ' option:selected').text();
        input.VillageOfBirth = $('#selectVillageOfBirth' + ' option:selected').text();
        input.ProvinceOfNativePlace = $('#selectProvinceOfNativePlace' + ' option:selected').text();
        input.DistrictOfNativePlace = $('#selectDistrictOfNativePlace' + ' option:selected').text();
        input.VillageOfNativePlace = $('#selectVillageOfNativePlace' + ' option:selected').text();
        input.Evaluate = $('#Evaluate').val();
        _reportService.exportHS02(input)
            .done(function (fileResult) {
                location.href = abp.appPath + 'File/DownloadReportFile?fileType=' + fileResult.fileType + '&fileToken=' + fileResult.fileToken + '&fileName=' + fileResult.fileName;
            }).always(function () {
                abp.ui.clearBusy(_$form);
            });
    });
})(jQuery);