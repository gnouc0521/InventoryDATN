(function () {
    
      $("#btn-qrCode").click(function () {
        var html = `<div style="width: 100vw" id="reader" class="qr-content"></div>`;
        var htmlcancel = `<div class="text-right"><button id="close-qr" class="btn"><i class="fal fa-times text-white fz-2"></i></button></div>`;
        $("#qrcode-view").css({ "position": "fixed", "top": "0", "left": "0", "right": "0", "bottom": "0", "z-index": "1003", "background": "rgba(0, 0, 0, 0.8)" });
        $("#qrcode-view").append(htmlcancel);
        $("#qrcode-view").append(html);

        var html5QrcodeScanner = new Html5QrcodeScanner(
            "reader", {
                fps: 10,
                qrbox: 250
        });
        html5QrcodeScanner.render(onScanSuccess);


        // Dóng quét mã QR
        $("#close-qr").click(function () {

          html5QrcodeScanner.clear();
          $("#qrcode-view").css({ "position": "", "top": "", "left": "", "right": "", "bottom": "", "z-index": "", "background": "" });
          $("#qrcode-view").empty();

        })


        function onScanSuccess(decodedText, decodedResult) {
            // Handle on success condition with the decoded text or result.
            console.log(`Scan result: ${decodedText}`, decodedResult.result.text);

            html5QrcodeScanner.clear();
            $("#qrcode-view").css({ "position": "", "top": "", "left": "", "right": "", "bottom": "", "z-index": "", "background": "" });
            $("#qrcode-view").empty();

            var url = "/Inventorys/Items/DetailItemByCode?itemcode=" + decodedResult.result.text;
            console.log(url);
            /*window.location.replace(url);*/
            //abp.ajax({
            //    contentType: "application/x-www-form-urlencoded",
            //    url: abp.appPath + "PersonalProfile/Items/OverViewByCode?ItemCode=" + dataFilter.id,
            //    success: function (results) {
            //        window.location.href =
            //            "/PersonalProfile/Items/DetailItemByCode?itemcode=" + decodedResult.result.text;
            //    },
            //});

         }



    })

  
})(jQuery);