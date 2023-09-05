(function ($) {
    $("header #text_header").text(abp.setting.get("App.Ui.Header").toUpperCase());
    $("footer #text_footer").text(abp.setting.get("App.Ui.Footer").toUpperCase());
    $("header #image_header").attr("src", abp.setting.get("App.Ui.LogoUrl"));
    $("#banner_header").css("background-image", `url('${abp.setting.get("App.Ui.BannerUrl")}')`);
})(jQuery);

