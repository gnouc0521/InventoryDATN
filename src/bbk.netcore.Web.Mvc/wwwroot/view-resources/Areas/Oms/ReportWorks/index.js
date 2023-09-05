(function () {
    var _workService = abp.services.app.work;
    moment.locale(abp.localization.currentLanguage.name);
 
    $('.date-picker').datepicker({
        rtl: false,
        dateFormat: 'dd-mm--yy',
        orientation: "bottom left",
        autoclose: true,
        language: abp.localization.currentLanguage.name
    });

    var getFilter = function () {
        let dataFilter = {};

        $("#FromDate").datepicker();
        $("#ToDate").datepicker();

        fromDate = $("#FromDate").datepicker("getDate");
        toDate = $("#ToDate").datepicker("getDate");
        
        if (fromDate != null) {
            dateStart = moment(fromDate).format('L');
            dataFilter.fromDate = dateStart
        }
        if (toDate != null) {
            dateEnd = moment(toDate).format('L');
            dataFilter.toDate = dateEnd;
        }
        
        return dataFilter;
    }

   

    //-------------------------------------Chart JS---------------------------------------------
    
    var Userid = $("#UserId").val();

    //Khởi tạo các biểu đồ
    var chartLabels = ['Hoàn thành', 'Đang xử lý', 'Chờ xử lý', 'Quá hạn'];


    //------------------------------ Biểu đồ được giao --------------------------

    var assignedChart = document.getElementById("assignedChart").getContext('2d');

    var chart = new Chart(assignedChart, LoadChart(chartLabels, 0));
    var myLegendContainer = document.getElementById("legend");
    // chú thích của Biểu đồ được giao
    myLegendContainer.innerHTML = chart.generateLegend();
    // bind onClick event to all LI-tags of the legend
    var legendItems = myLegendContainer.getElementsByTagName('li');
    for (var i = 0; i < legendItems.length; i += 1) {
        legendItems[i].addEventListener("click", legendClickCallback, false);
    }

    //------------------------------ / Biểu đồ được giao --------------------------

    //------------------------------- Biểu đồ đã giao -----------------------------

    var deliveredChart = document.getElementById("deliveredChart").getContext('2d');
    var chartDelivered = new Chart(deliveredChart, LoadChart(chartLabels, 1));
    var legendDelivered = document.getElementById("legendDelivered");
    // chú thích của Biểu đồ đã giao
    legendDelivered.innerHTML = chartDelivered.generateLegend();
    // bind onClick event to all LI-tags of the legend
    var legendItems = legendDelivered.getElementsByTagName('li');
    for (var i = 0; i < legendItems.length; i += 1) {
        legendItems[i].addEventListener("click", legendClickCallback, false);
    }

    //------------------------------- / Biểu đồ đã giao -----------------------------


    //-------------------------------- Biểu đồ phối hợp ------------------------------
    var combinationChart = document.getElementById("combinationChart").getContext('2d');
    var chartDataCombination = [0, 0, 1, 1];
    var chartCombination = new Chart(combinationChart, LoadChart(chartLabels, 2));
    var legendCombination = document.getElementById("combinationDelivered");
    // chú thích của Biểu đồ phối hợp
    legendCombination.innerHTML = chartCombination.generateLegend();
    // bind onClick event to all LI-tags of the legend
    var legendItems = legendCombination.getElementsByTagName('li');
    for (var i = 0; i < legendItems.length; i += 1) {
        legendItems[i].addEventListener("click", legendClickCallback, false);
    }

    //-------------------------------- / Biểu đồ phối hợp ------------------------------


    //---------------------------------- Biểu đồ theo dõi ------------------------------
    var followChart = document.getElementById("followChart").getContext('2d');
    
    var chartFollow = new Chart(followChart, LoadChart(chartLabels, 3));
    var legendFollow = document.getElementById("followDelivered");
    // chú thích của Biểu đồ theo dõi
    legendFollow.innerHTML = chartFollow.generateLegend();
    // bind onClick event to all LI-tags of the legend
    var legendItems = legendFollow.getElementsByTagName('li');
    for (var i = 0; i < legendItems.length; i += 1) {
        legendItems[i].addEventListener("click", legendClickCallback, false);
    }

    //---------------------------------- / Biểu đồ theo dõi ------------------------------



    //Function Load Chart
    function LoadChart(chartlable, index) {
        var config = {
            type: 'doughnut',
            data: {
                labels: chartlable,
                datasets: [{
                    backgroundColor: [
                        "#24c7a0",
                        "#058515",
                        "#0094f2",
                        "#ec3b48",
                    ],
                    data: [0,0,0,0],
                    datalabels: {
                        labels: {
                            value: {
                                color: '#000'
                            }
                        }
                    }
                }]
            },
            options: {
                plugins: {
                    datalabels: {
                        formatter: (value) => {
                            if (value == 0) {
                                return '';
                            }
                        },
                        display: true,
                        align: 'center',
                        borderRadius: 3,
                        font: {
                            size: 16,
                        }
                    },
                },
                legend: {
                    display: false
                },
                legendCallback: function (chart) {
                    var text = [];
                    text.push('<ul class="' + index + '-legend">');
                    var ds = chart.data.datasets[0];
                    var sum = ds.data.reduce(function add(a, b) { return a + b; }, 0);
                    for (var i = 0; i < ds.data.length; i++) {
                        text.push('<li>');
                        var perc = Math.round(100 * ds.data[i] / sum, 0);
                        text.push('<input type="checkbox" class="check-input" checked name="check' + i + '"/><span style="background-color:' + ds.backgroundColor[i] + '">' + '</span>' + chart.data.labels[i]);
                        text.push('</li>');
                    }
                    text.push('</ul>');
                    return text.join("");
                }
            }
        };
        return config;
    }
    //Function when click legendItems
    function legendClickCallback(event) {
        event = event || window.event;
        var target = event.target || event.srcElement;
        while (target.nodeName !== 'LI') {
            target = target.parentElement;
        }
        var parent = target.parentElement;
        var chartId = parseInt(parent.classList[0].split("-")[0], 10);
        var chart = Chart.instances[chartId];
        var index = Array.prototype.slice.call(parent.children).indexOf(target);
        var meta = chart.getDatasetMeta(0);

        var item = meta.data[index];
        console.log(index);

        if (item.hidden === null || item.hidden === false) {
            item.hidden = true;
            target.classList.add('hidden');
            $(this).find('input[name="check' + index + '"]').prop("checked", false);
            /*$('input[name="check' + index + '"]').prop("checked", false);*/
        } else {
            target.classList.remove('hidden');
            item.hidden = null;
            /*$('input[name="check' + index + '"]').prop("checked", true);*/
            $(this).find('input[name="check' + index + '"]').prop("checked", true);
        }
        chart.update();
    }



    //Hàm load lại biểu đồ
    function LoadDataOfChart() {
        _workService.getAllByTime(getFilter()).done(function (result) {

            var tongdg = 0;
            var tonghoanthanhdg = 0;
            var tongdxldg = 0;
            var tongchoxldg = 0;
            var tongquahandg = 0;

            var tongdag = 0;
            var tonghoanthanhdag = 0;
            var tongdxldag = 0;
            var tongchoxldag = 0;
            var tongquahandag = 0;

            var tongph = 0;
            var tonghoanthanhph = 0;
            var tongdxlph = 0;
            var tongchoxlph = 0;
            var tongquahanph = 0;

            for (var i = 0; i < result.items.length; i++) {
                //đã giao
                if (result.items[i].ownerStatusEnum == 4) {
                    if (result.items[i].status == 2) {
                        tonghoanthanhdag += 1;
                        tongdag += 1;
                    }
                    if (result.items[i].status == 1) {
                        tongdxldag += 1;
                        tongdag += 1;
                    }
                    if (result.items[i].status == 3) {
                        tongquahandag += 1;
                        tongdag += 1;
                    }
                    if (result.items[i].status == 0) {
                        tongchoxldag += 1;
                        tongdag += 1;
                    }

                }
                //được giao
                if (result.items[i].ownerStatusEnum == 1) {
                    if (result.items[i].status == 2) {
                        tonghoanthanhdg += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].status == 1) {
                        tongdxldg += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].status == 3) {
                        tongquahandg += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].status == 0) {
                        tongchoxldg += 1;
                        tongdg += 1;
                    }

                }
                //phối hợp
                if (result.items[i].ownerStatusEnum == 2) {
                    if (result.items[i].status == 2) {
                        tonghoanthanhph += 1;
                        tongph += 1;
                    }
                    if (result.items[i].status == 1) {
                        tongdxlph += 1;
                        tongph += 1;
                    }
                    if (result.items[i].status == 3) {
                        tongquahanph += 1;
                        tongph += 1;
                    }
                    if (result.items[i].status == 0) {
                        tongchoxlph += 1;
                        tongph += 1;
                    }
                }
            }
            //update data
            chart.config.data.datasets[0].data = [tonghoanthanhdg, tongdxldg, tongchoxldg, tongquahandg];
            chartDelivered.data.datasets[0].data = [tonghoanthanhdag, tongdxldag, tongchoxldag, tongquahandag];
            chartCombination.data.datasets[0].data = [tonghoanthanhph, tongdxlph, tongchoxlph, tongquahanph];

            $("#sumdg").html(tongdg);
            $("#sumdag").html(tongdag);
            $("#sumph").html(tongph);
            $("#sumtg").html(0);

            //update lại biểu đồ
            chart.update();
            chartDelivered.update();
            chartCombination.update();

        })
    }
    LoadDataOfChart();


    $('#Search').click(function () {
        LoadDataOfChart();
    });


    //-------------------------------------/ Chart JS---------------------------------------------


})(jQuery);