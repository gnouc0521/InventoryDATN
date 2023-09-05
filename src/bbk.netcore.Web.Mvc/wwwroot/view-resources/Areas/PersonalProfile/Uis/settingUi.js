(function ($) {
    var _settingUi = abp.services.app.settingUI;

    var oldLogoUrl = $("#logo-url").attr("src");
    var oldBannerUrl = $("#banner-url").attr("src");
    let logo = null;
    let banner = null;
    $("#logo-file").bind("change", function () {
        let file_logo_Data = $(this).prop("files")[0];
        let math = ["image/png", "image/jpg", "image/jpeg"];
        if (!file_logo_Data) {
            $("#logo-url").attr("src", oldLogoUrl);
            $("#logo-label").text("Chọn ảnh");
            logo = null;
            return false;
        }
        if ($.inArray(file_logo_Data.type, math) === -1) {
            alert("Kiểu file không hợp lệ, chỉ chấp nhận jpg, jpeg & png");
            $(this).val(null);
            $("#logo-url").attr("src", oldLogoUrl);
            $("#logo-label").text("Chọn ảnh");
            logo = null;
            return false;
        }
        logo = file_logo_Data;
        $("#logo-label").text(file_logo_Data.name);
        if (typeof (FileReader) != "undefined") {
            let fileReader = new FileReader();
            fileReader.onload = function (element) {
                $("#logo-url").attr("src", element.target.result);
            }
            fileReader.readAsDataURL(file_logo_Data);
        } else {
            alert("Trình duyệt không hỗ trợ đọc file.");
        }
    });

    $("#banner-file").bind("change", function () {
        let file_banner_Data = $(this).prop("files")[0];
        let math = ["image/png", "image/jpg", "image/jpeg"];
        if (!file_banner_Data) {
            $("#banner-url").attr("src", oldBannerUrl);
            $("#banner-label").text("Chọn ảnh");
            banner = null;
            return false;
        }
        if ($.inArray(file_banner_Data.type, math) === -1) {
            alert("Kiểu file không hợp lệ, chỉ chấp nhận jpg, jpeg & png");
            $(this).val(null);
            $("#banner-url").attr("src", oldBannerUrl);
            $("#banner-label").text("Chọn ảnh");
            banner = null;
            return false;
        }
        banner = file_banner_Data;
        $("#banner-label").text(file_banner_Data.name);
        if (typeof (FileReader) != "undefined") {
            let fileReader = new FileReader();
            fileReader.onload = function (element) {
                $("#banner-url").attr("src", element.target.result);
            }
            fileReader.readAsDataURL(file_banner_Data);
        } else {
            alert("Trình duyệt không hỗ trợ đọc file.");
        }
    });

    $('#setting_default').click(function () {
        _settingUi.reset()
            .then(function () {
                abp.message.success("Đặt lại về mặc định thành công!");
                setTimeout(function () { window.location.reload(); }, 1200);
            });
    });

    $(document).on('click', '#update_setting', function (e) {
        e.preventDefault();
        Save();
    });

    function Save() {
        _$form = $('#setting-form');
        _$form.validateForm();
        if (!_$form.valid()) {
            return;
        }
        let formData = new FormData(_$form[0]);
        if (logo != null) formData.append("logo", logo);
        if (banner != null) formData.append("banner", banner);
        abp.ui.setBusy(_$form);
        $.ajax({
            url: abp.appPath + 'PersonalProfile/Settings/Index',
            type: "post",
            cache: false,
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                abp.message.success('Cập nhật thành công!');
                setTimeout(function () { window.location.reload(); }, 700);
            },
            error: function (error) {
                abp.message.error('Cập nhật ảnh không thành công!');
            },
        });
        abp.ui.clearBusy(_$form);
    };

})(jQuery);