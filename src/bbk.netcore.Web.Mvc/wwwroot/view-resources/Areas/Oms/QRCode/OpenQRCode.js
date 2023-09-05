(function () {
    
    moment.locale(abp.localization.currentLanguage.name);

    var html5QrcodeScanner = new Html5QrcodeScanner(
        "reader", { fps: 10, qrbox: 250 });
    html5QrcodeScanner.render(onScanSuccess);

    function onScanSuccess(decodedText, decodedResult) {
        // Handle on success condition with the decoded text or result.
        console.log(`Scan result: ${decodedText}`, decodedResult);

        html5QrcodeScanner.clear();
        $("#reader").remove();
    }
})(jQuery);