(function () {
    "use strict";
    $(function () {
        var _dashboardService = abp.services.app.dashboardService;
        moment.locale(abp.localization.currentLanguage.name);


        //------------------------------------------------------------ Chart Js ---------------------------------------------------------------
        var getFilter = function () {
            let dataFilter = {};
            dataFilter.numInTime = $(".dataDgSearch.active").attr("data-objid");
            return dataFilter;
        }

        //Khởi tạo các biểu đồ
        var chartTPKH = ['Chờ xử lý', 'Phê duyệt', 'Từ chối'];
        var chartLabelPN = ['Chờ xử lý', 'Phê duyệt', 'Từ chối'];

        //------------------------------ Biểu đồ th --------------------------

        var assignedChart = document.getElementById("assignedChartTPKH").getContext('2d');

        var chart = new Chart(assignedChart, LoadChart(chartTPKH, 0));
        var myLegendContainer = document.getElementById("legendTPKH");
        // chú thích của Biểu đồ được giao
        myLegendContainer.innerHTML = chart.generateLegend();
        // bind onClick event to all LI-tags of the legend
        var legendItems = myLegendContainer.getElementsByTagName('li');
        for (var i = 0; i < legendItems.length; i += 1) {
            legendItems[i].addEventListener("click", legendClickCallback, false);
        }

        //------------------------------ / Biểu đồ th --------------------------

        //------------------------------ Biểu đồ pn --------------------------

        debugger
        var PnChart = document.getElementById("YCDCChart").getContext('2d');

        var chartPN = new Chart(PnChart, LoadChart(chartLabelPN, 0));
        var myLegendContainer = document.getElementById("legendYCDC");
        // chú thích của Biểu đồ được giao
        myLegendContainer.innerHTML = chartPN.generateLegend();
        // bind onClick event to all LI-tags of the legend
        var legendItems = myLegendContainer.getElementsByTagName('li');
        for (var i = 0; i < legendItems.length; i += 1) {
            legendItems[i].addEventListener("click", legendClickCallback, false);
        }

        //------------------------------ / Biểu đồ pn --------------------------

        //Function Load Chart
        function LoadChart(chartTPKH, index) {
            var config = {
                type: 'doughnut',
                data: {
                    labels: chartTPKH,
                    datasets: [{
                        backgroundColor: [
                            "#24c7a0",
                            "#0094f2",
                            "#ec3b48",
                        ],
                        data: [0, 0, 0],
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

        //------------------------------- / PTH-----------------------------
        //Hàm load lại biểu đồ chung
        function LoadDataOfChart() {
            _dashboardService.getAllTPKHTH(getFilter()).done(function (result) {
                var tongdg = 0;
                var tongpheduyet = 0;
                var tongchoxuly = 0;
                var tongtuchoi = 0;
                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].purchasesRequestStatus == 2) {
                        tongpheduyet += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].purchasesRequestStatus == 3) {
                        tongtuchoi += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].purchasesRequestStatus == 1 || result.items[i].purchasesRequestStatus == 0) {
                        tongchoxuly += 1;
                        tongdg += 1;
                    }
                }
                //update data
                chart.config.data.datasets[0].data = [tongchoxuly, tongpheduyet, tongtuchoi];
                $("#sumdg").html(tongdg);
                $("#sumtg").html(0);

                //update lại biểu đồ
                chart.update();
                // chartCombination.update();

            })
        }
        LoadDataOfChart();

        //Hàm load lại biểu đồ công việc được giao
        function LoadDataOfAssigned() {
            _dashboardService.getAllTPKHTH(getFilter()).done(function (result) {
                var tongdg = 0;
                var tongpheduyet = 0;
                var tongchoxuly = 0;
                var tongtuchoi = 0;

                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].purchasesRequestStatus == 2) {
                        tongpheduyet += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].purchasesRequestStatus == 3) {
                        tongtuchoi += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].purchasesRequestStatus == 1 || result.items[i].purchasesRequestStatus == 0) {
                        tongchoxuly += 1;
                        tongdg += 1;
                    }
                }
                //update data
                chart.config.data.datasets[0].data = [tongchoxuly, tongpheduyet, tongtuchoi];

                //update lại biểu đồ
                chart.update();

            })
        }

        //Filter biểu đồ được giao
        $("#chart-dgTPKH").on("click", ".dataDgSearch", function () {
            $(".dataDgSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfAssigned();
        })
        //------------------------------------------------------------ /Chart Js ---------------------------------------------------------------






        ////------------------------------- /ycpn -----------------------------
        ////Hàm load lại biểu đồ chung
        function LoadDataOfChartPN() {
            _dashboardService.getAllTranfer(getFilter()).done(function (result) {

                var tongpn = 0;
                var tongpheduyet = 0;
                var tongchoxuly = 0;
                var tongtuchoi = 0;
                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].status == 3) {
                        tongpheduyet += 1;
                        tongpn += 1;
                    }
                    if (result.items[i].status == 1) {
                        tongtuchoi += 1;
                        tongpn += 1;
                    }
                    if (result.items[i].status == 0 || result.items[i].status == 2) {
                        tongchoxuly += 1;
                        tongpn += 1;
                    }
                }
                //update data
                chartPN.config.data.datasets[0].data = [tongchoxuly, tongpheduyet, tongtuchoi];
                $("#sumdg").html(tongpn);
                $("#sumtg").html(0);

                //update lại biểu đồ
                chartPN.update();
                // chartCombination.update();

            })
        }
        LoadDataOfChartPN();

        //Hàm load lại biểu đồ công việc được giao
        function LoadDataOfAssignedPN() {
            _dashboardService.getAllTranfer(getFilter()).done(function (result) {
                var tongpn = 0;
                var tongpheduyet = 0;
                var tongchoxuly = 0;
                var tongtuchoi = 0;
                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].status == 3) {
                        tongpheduyet += 1;
                        tongpn += 1;
                    }
                    if (result.items[i].status == 1) {
                        tongtuchoi += 1;
                        tongpn += 1;
                    }
                    if (result.items[i].status == 0 || result.items[i].status == 2) {
                        tongchoxuly += 1;
                        tongpn += 1;
                    }
                }
                //update data
                chartPN.config.data.datasets[0].data = [tongchoxuly, tongpheduyet, tongtuchoi];
                //update lại biểu đồ
                chartPN.update();
            })
        }

        //Filter biểu đồ được giao
        $("#chart-dgYCDC").on("click", ".dataDgSearch", function () {
            $(".dataDgSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfAssignedPN();
        })
        //------------------------------------------------------------ /Chart Js ---------------------------------------------------------------
    });
})();

