(function () {
    var _warehouseLocationService = abp.services.app.warehouseLocationItemService;
    moment.locale(abp.localization.currentLanguage.name);


    console.log($("#ItemcodeText").text().trim());


    var textma = $("#ItemcodeText").text().trim();

    console.log("sâdasdasd");

    //Qr Code
    var qrcode = new QRCode(document.getElementById("id-qrcode"), {
        text: "textma",
        width: 100,
        height: 100,
        colorDark: "#000000",
        colorLight: "#ffffff",
        /*correctLevel: QRCode.CorrectLevel.M*/
    });

    qrcode.makeCode(textma);

    JsBarcode("#barcode", textma);

})(jQuery);