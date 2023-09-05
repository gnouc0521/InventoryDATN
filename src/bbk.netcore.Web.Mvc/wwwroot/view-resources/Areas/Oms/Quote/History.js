(function () {
    var _$quoteSynTable = $("#QuoteSynTable");
    var _quoteService = abp.services.app.quotesService;

    $('.date-picker').datepicker({});
    for (i = new Date().getFullYear(); i > 2000; i--) {
        $('#Year').append($('<option />').val(i).html(i));
    }

    function LoadTable() {
        var columnLengtt = $("#QuoteSynTable thead tr th.col-start").length;
        var getFilter = function () {
            let dataFilter = {};
            let year = document.getElementById("Year");
            let quy = document.getElementById("Quy");
            dataFilter.searchTerm = $('#SearchTerm').val();
            //if ($('#Year').val() != undefined) {
            //    dataFilter.year = $('#Year').val();
            //}
            dataFilter.year = year.options[year.selectedIndex].value;
            dataFilter.quy = quy.options[quy.selectedIndex].value;
            
            return dataFilter;
        }

        console.log(getFilter())

        var tr = $('#QuoteSynTable').find('tbody tr').remove();
        _quoteService.getHisByHa(getFilter()).done(function (result) {
            $.each(result.items, function (index, value) {
                var stt = index + 1;
                
                for (var i = 0; i < value.quoteHístoryList.length; i++) {
                    if (i == 0) {
                        var htmtTrBody = `<tr class="row` + value.quoteHístoryList[i].itemId +`_`+i+`">
                            <td rowspan="`+ value.quoteHístoryList.length + `" class="text-center">` + stt + `</td>
                            <td rowspan="`+ value.quoteHístoryList.length + `">` + value.itemCode + " - " + value.itemName + `</td>
                            <td>` + value.quoteHístoryList[i].supplierName + `</td>
                        </tr>`;
                        $("#QuoteSynTable tbody").append(htmtTrBody);
                        
                        for (var j = 0; j < value.quoteHístoryList[i].timeAndIds.length; j++) {
                            var htmltime = `<td>
                                                <a class='historyviewfunc text-right' data-objid='` + value.quoteHístoryList[i].timeAndIds[j].idQuotes + `' href='javascript:void(0); ' > ` + moment(value.quoteHístoryList[i].timeAndIds[j].timeCre).format('L') + ` </a>
                                            </td>`;
                            
                            $(".row" + value.quoteHístoryList[i].itemId + "_" + i).append(htmltime);
                        }
                    }
                    else {
                        var htmtTrBody = `<tr class="row` + value.quoteHístoryList[i].itemId + `_` + i +`">
                            <td>` + value.quoteHístoryList[i].supplierName + `</td>
                        </tr>`;
                        $("#QuoteSynTable tbody").append(htmtTrBody);

                        for (var j = 0; j < value.quoteHístoryList[i].timeAndIds.length; j++) {
                            var htmltime = `<td>
                                                <a class='historyviewfunc text-right' data-objid='` + value.quoteHístoryList[i].timeAndIds[j].idQuotes + `' href='javascript:void(0); ' > ` + moment(value.quoteHístoryList[i].timeAndIds[j].timeCre).format('L') + ` </a>
                                            </td>`;

                            $(".row" + value.quoteHístoryList[i].itemId +"_"+ i).append(htmltime);
                        }
                    }
                }

            })

            $('#QuoteSynTable').on('click', '.historyviewfunc', function (e) {
                var btnClick = $(this);
                var dataFilter = { id: btnClick[0].dataset.objid };
                abp.ajax({
                    contentType: "application/x-www-form-urlencoded",
                    url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
                    success: function (results) {
                        window.location.href =
                            "/Inventorys/Quote/HistoryQuoteDetail?QuoteId=" + results.id;
                    },
                });
            });
        })
        /*$("#QuoteSynTable ")*/
    }

    LoadTable();

    $("#Search").on("click", function () {
        LoadTable();
    })

    

})(jQuery);