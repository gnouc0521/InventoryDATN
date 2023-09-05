(function () {
    var _producerService = abp.services.app.producer;
    moment.locale(abp.localization.currentLanguage.name);


  

    //------------------------- Load Name Address -----------------
    fullPathProvince = 'province.json';
    fullPathDistrict = 'district.json';
    fullPathVillage = 'village.json';

    function LoadAddress(filePath, idAddress, idSet, divview) {
        _producerService.getAddress(filePath, idAddress).done((result) => {
            for (let i = 0; i < result.addresses.length; i++) {
                if (result.addresses[i].id == idSet) {
                    $(divview).html(result.addresses[i].name);
                }
            }
        })
    }

    LoadAddress(fullPathProvince, "", $("#CityId").val(), "#CityName");
    LoadAddress(fullPathDistrict, $("#CityId").val(), $("#DistrictId").val(), "#DistrictName");
    LoadAddress(fullPathVillage, $("#DistrictId").val(), $("#WardsId").val(), "#VillageName");

})(jQuery);