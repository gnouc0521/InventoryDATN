(function () {
    var _$iNVENTable = $('#StockTable');
  
    var _inventory = abp.services.app.inventoryService;
    var _warehouse = abp.services.app.wareHouse;

    var getFilter = function () {
        let dataFilter = {};
        var Categoryvalue = document.getElementById("GetCategoryValue");
        var Groupvalue = document.getElementById("GroupValue");
        var Kindvalue = document.getElementById("KindValue");

        dataFilter.searchTerm = $('#SearchTerm').val().trim();
        dataFilter.category = Categoryvalue.options[Categoryvalue.selectedIndex].value;
        dataFilter.group = Groupvalue.options[Groupvalue.selectedIndex].value;
        dataFilter.kind = Kindvalue.options[Kindvalue.selectedIndex].value;

        return dataFilter;
    }

    var columnLengtt = $("#StockTable thead tr th.titlecolumn").length;
    var columnLengData = $("#StockTable thead tr th.warehouse").length;
    var columnElemt = document.getElementsByClassName("warehouse");

    $('#Search').on('click', function () {
        loadTable();
    }) 
  

    function loadTable() {
        var tr = $('#StockTable').find('tbody tr').remove();
        _inventory.getAllInStock(getFilter()).done(function (result) {
            $.each(result.items, function (index, value) {
                var htmtTrBody = `<tr class="row` + value.itemId + `"></tr>`;
                var sumAmount = 0;
                var number = index + 1;
                
                $("#StockTable tbody").append(htmtTrBody);

                for (var i = 0; i < columnLengtt; i++) {
                    if (i == 0) {
                        var htmlTdBody = `<td>` + number + `</td>`;
                    }
                    if (i == 1) {
                        var htmlTdBody = `<td>` + value.nameCode + `</td>`;
                    }
                    if (i == columnLengtt - 1) {
                        sumAmount += value.quantity;
                        var textsumAmount = sumAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                        var htmlTdBody = `<td class="text-right sum` + value.itemId + `">` + textsumAmount +`</td>`;
                    }

                    $(".row" + value.itemId).append(htmlTdBody);
                }
                for (var i = 0; i < columnLengData; i++) {
                    
                    if (columnElemt[i].getAttribute("data-wareId") == value.warehouseId) {
                        var textQuantity = value.quantity.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

                        var htmlTdBody = `<td class="text-right">` + textQuantity + `</td>`;
                    }
                    else {
                        var htmlTdBody = `<td class="text-right">0</td>`;
                    }

                    $(".row" + value.itemId).append(htmlTdBody);
                }
            })
        })
        //_warehouse.getWarehouseList().done(function (ressult) {
        //    $.each(ressult, function (index, value) {
        //        var htmlColumn = `<th class="title`+value.id+`">`+value.name+`</th>`
        //    })
        //})
    }
    loadTable();


})(jQuery);