(function () {
    "use strict";
    $(function () {
        /* GLOBAL PAGE Variables */
        var _workService = abp.services.app.work;
        moment.locale(abp.localization.currentLanguage.name);

        /* DOCUMENTS widget Variables */
        var _$newestDocTbl = $('#newestDocTbl');
        //var _docService = abp.services.app.document;
        /* DOCUMENTS: DEFINE PERMISSION
        //var _permissions = {
        //    create: abp.auth.hasPermission('Pages.Administration.docs.Create'),
        //    edit: abp.auth.hasPermission('Pages.Administration.docs.Edit'),
        //    changePermissions: abp.auth.hasPermission('Pages.Administration.docs.ChangePermissions'),
        //    impersonation: abp.auth.hasPermission('Pages.Administration.docs.Impersonation'),
        //    'delete': abp.auth.hasPermission('Pages.Administration.docs.Delete')
        //};
        */

        //var dataTable = _$newestDocTbl.DataTable({
        //    paging: false,
        //    serverSide: true,
        //    processing: true,
        //    "searching": false,
        //    "language": {
        //        "info": "",
        //        "infoEmpty": "",
        //        "emptyTable": "Không tìm thấy dữ liệu"
        //    },
        //    "bLengthChange": false,
        //    pageLength: 4,
        //    "order": [[1, "desc"]],
        //    "aoColumns": [{ "bSortable": false, "aTargets": [1] }],
        //    listAction: {
        //        ajaxFunction: _docService.getDashboardDocs
        //    },
        //    columnDefs: [
        //        {
        //            targets: 0,
        //            orderable: false,
        //            data: "decisionNumber",
        //            render: function (data, type, row) {
        //                //var $container = $("<span/>");
        //                if (row.fileUrl) {
        //                    return `<a style="border:none" href="` + row.fileUrlInfo.fileUrl + `" target="_blank" >
        //                        <i class="fal fa-file mr-1 color-danger-700"></i>
        //                    </a>` + data;
        //                } else {
        //                    return data;
        //                }
        //            }
        //        },
        //        {
        //            targets: 1,
        //            orderable: false,
        //            data: "issuedDate",
        //            render: function (creationTime) {
        //                return moment(creationTime).format('L');
        //            }
        //        },
        //        {
        //            orderable: false,
        //            targets: 2,
        //            className: "color-primary",
        //            data: "description",
        //            render: function (data, type, row) {
        //                return `<a style ="border:none; color: #006b2d" href="` + row.fileUrlInfo.fileUrl + `" target="_blank">${row.description}</a>`;
        //            }
        //        }
        //    ]
        //});

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

        var getFilterCombination = function () {
            let dataFilter = {};
            dataFilter.numInTime = $(".dataPhSearch.active").attr("data-objid");
            return dataFilter;
        }

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
                        data: [0, 0, 0, 0],
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


        
        //Hàm load lại biểu đồ chung
        function LoadDataOfChart() {
            _workService.getAllInDashBoard(getFilter()).done(function (result) {

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

        //Hàm load lại biểu đồ công việc được giao
        function LoadDataOfAssigned() {
            _workService.getAllInDashBoard(getFilter()).done(function (result) {

                var tongdg = 0;
                var tonghoanthanhdg = 0;
                var tongdxldg = 0;
                var tongchoxldg = 0;
                var tongquahandg = 0;

                for (var i = 0; i < result.items.length; i++) {
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
                }
                //update data
                chart.config.data.datasets[0].data = [tonghoanthanhdg, tongdxldg, tongchoxldg, tongquahandg];

                //update lại biểu đồ
                chart.update();

            })
        }

        //Hàm load lại biểu đồ công việc đã giao
        function LoadDataOfDelivered() {
            _workService.getAllInDashBoard(getFilterDelivered()).done(function (result) {

                var tongdag = 0;
                var tonghoanthanhdag = 0;
                var tongdxldag = 0;
                var tongchoxldag = 0;
                var tongquahandag = 0;

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
                }
                //update data
                chartDelivered.data.datasets[0].data = [tonghoanthanhdag, tongdxldag, tongchoxldag, tongquahandag];

                //update lại biểu đồ
                chartDelivered.update();
            })
        }

        //Hàm load lại biểu đồ công việc phối hợp
        function LoadDataOfCombination() {
            _workService.getAllInDashBoard(getFilterCombination()).done(function (result) {

                var tongph = 0;
                var tonghoanthanhph = 0;
                var tongdxlph = 0;
                var tongchoxlph = 0;
                var tongquahanph = 0;

                for (var i = 0; i < result.items.length; i++) {
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
                chartCombination.data.datasets[0].data = [tonghoanthanhph, tongdxlph, tongchoxlph, tongquahanph];

                //update lại biểu đồ
                chartCombination.update();

            })
        }

        //Filter biểu đồ được giao
        $("#chart-dg").on("click", ".dataDgSearch", function () {
            $(".dataDgSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfAssigned();
        })

        //Filter biểu đồ đã giao
        $("#chart-dag").on("click", ".dataDagSearch", function () {
            $(".dataDagSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfDelivered();
        })

        //Filter biểu đồ phối hợp
        $("#chart-ph").on("click", ".dataPhSearch", function () {
            $(".dataPhSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfCombination();
        })

       
//------------------------------------------------------------ /Chart Js ---------------------------------------------------------------

    });
})();


//var usersScript = function () {
//    return {
//        init: function () {
//            moment.locale(abp.localization.currentLanguage.name);
//            // handle here
//        },
//    };
//}();
//jQuery(document).ready(function () {
//    usersScript.init();
//});