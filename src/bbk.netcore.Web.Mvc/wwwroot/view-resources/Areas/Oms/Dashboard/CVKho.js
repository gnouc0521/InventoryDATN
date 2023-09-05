(function () {
    "use strict";
    $(function () {
        var _dashboardService = abp.services.app.dashboardService;
        moment.locale(abp.localization.currentLanguage.name);


        //------------------------------------------------------------ Chart Js ---------------------------------------------------------------
        var getFilter = function () {
            let dataFilter = {};
            //dataFilter.fromDate = null;
            //dataFilter.toDate = null;
            dataFilter.numInTime = $(".dataDgSearch.active").attr("data-objid");
            return dataFilter;
        }

        var getFilterDelivered = function () {
            let dataFilter = {};
            dataFilter.numInTime = $(".dataDagSearch.active").attr("data-objid");
            return dataFilter;
        }

        //Khởi tạo các biểu đồ
        var chartLabelPN = ['Chưa xử lý', 'Hoàn thành'];


        //------------------------------- Biểu đồ pn -----------------------------

        var deliveredChart = document.getElementById("deliveredChart").getContext('2d');
        var chartDelivered = new Chart(deliveredChart, LoadChart(chartLabelPN, 1));
        var legendDelivered = document.getElementById("legendDelivered");
        // chú thích của Biểu đồ đã giao
        legendDelivered.innerHTML = chartDelivered.generateLegend();
        // bind onClick event to all LI-tags of the legend
        var legendItems = legendDelivered.getElementsByTagName('li');
        for (var i = 0; i < legendItems.length; i += 1) {
            legendItems[i].addEventListener("click", legendClickCallback, false);
        }

        //------------------------------- / Biểu đồ pn -----------------------------

        //Function Load Chart
        function LoadChart(chartlable, index) {
            var config = {
                type: 'doughnut',
                data: {
                    labels: chartlable,
                    datasets: [{
                        backgroundColor: [
                            "#ec3b48",
                            "#0094f2",
                        ],
                        data: [0, 0],
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
            } else {
                target.classList.remove('hidden');
                item.hidden = null;
                $(this).find('input[name="check' + index + '"]').prop("checked", true);
            }
            chart.update();
        }

        //------------------------------- / Chuyên viên mua hàng -----------------------------
        //Hàm load lại biểu đồ chung
       

        function LoadDataOfChartPn() {
            _dashboardService.getAllThuKhoPn(getFilter()).done(function (result) {
                var tongpn = 0;
                var tonghoanthanhpn = 0;
                var tongdxlpn = 0;
                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].importStatus == 0) {
                        tonghoanthanhpn += 1;
                        tongpn += 1;
                    }
                    if (result.items[i].importStatus == 2) {
                        tongdxlpn += 1;
                        tongpn += 1;
                    }
                }
                //update data
                chartDelivered.data.datasets[0].data = [tongdxlpn,tonghoanthanhpn];
                $("#sumdg").html(tongpn);
                $("#sumtg").html(0);

                //update lại biểu đồ
                chartDelivered.update();
            })
        }

        LoadDataOfChartPn();


        

       

        function LoadDataOfDelivered() {
            _dashboardService.getAllThuKhoPn(getFilter()).done(function (result) {
                var tongpn = 0;
                var tonghoanthanhpn = 0;
                var tongdxlpn = 0;
                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].importStatus == 0) {
                        tonghoanthanhpn += 1;
                        tongpn += 1;
                    }
                    if (result.items[i].importStatus == 2) {
                        tongdxlpn += 1;
                        tongpn += 1;
                    }
                }
                //update data
                chartDelivered.data.datasets[0].data = [tongdxlpn, tonghoanthanhpn];
                $("#sumdg").html(tongpn);
                $("#sumtg").html(0);

                //update lại biểu đồ
                chartDelivered.update();
            })
        }

       
        //Filter biểu đồ đã giao
        $("#chart-dag").on("click", ".dataDagSearch", function () {
            $(".dataDagSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfDelivered();
        })




        //------------------------------------------------------------ /Chart Js ---------------------------------------------------------------

    });
})();

