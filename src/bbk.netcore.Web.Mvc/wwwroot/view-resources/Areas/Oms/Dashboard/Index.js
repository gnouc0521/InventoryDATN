(function () {
    "use strict";
    $(function () {
        var _dashboardService = abp.services.app.dashboardService;
        var _$StaffTable = $('#StaffTable');
        var _PurchasesSyn = abp.services.app.purchasesSynthesise;
        var _purchaseAssignment = abp.services.app.purchaseAssignmentService;
        moment.locale(abp.localization.currentLanguage.name);


        var dataTable = _$StaffTable.DataTable({
            paging: false,
            serverSide: true,
            processing: true,
            "searching": false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không tìm thấy dữ liệu",
                searchPlaceholder: "Tìm kiếm"
            },
            "bInfo": false,
            "bLengthChange": false,
            //"dom": 'Rltip',
            lengthMenu: [
                [5, 10, 25, 50, -1],
                [5, 10, 25, 50, 'Tất cả'],
            ],

            pageLength: 5,
            listAction: {
                ajaxFunction: _PurchasesSyn.getItemByStaffD,
            },

            columnDefs: [
                {
                    orderable: false,
                    targets: 0,
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                {
                    orderable: false,
                    targets: 1,
                    data: 'purchasesSynthesiseCode',
                    render: function (data, type, row, meta) {
                        return `
                        <a class="purchasesSynthesiseCode" data-objid='` + row.purchasesSynthesiseId + `' href='javascript:void(0); '>` + row.purchasesSynthesiseCode + `</a>
                    `
                    }
                },
                {
                    orderable: false,
                    targets: 2,
                    data: "getPriceStatus",
                    className: "align-middle text-center",
                    render: function (data, type, row, meta) {
                        if (row.getPriceStatus == 2) {
                            return `<span class="span_status span-defaut"> Mới </span>`
                        } else if (row.getPriceStatus == 0) {
                            debugger
                            if (row.creatorUserId != abp.session.userId)
                            {
                                return `<span class="span_status span-approve"> Hoàn thành </span>`
                            }
                            else
                            {
                                return `<span class="span_status span-approve"> NCC Xác nhận </span>`
                            }
                            
                        } else if (row.getPriceStatus == 3) {
                            return `<span class="span_status span-reject"> Từ chối </span>`
                        }
                    }
                },
            ],
        });

        $('#StaffTable').on('click', '.purchasesSynthesiseCode', function (e) {
            var btnClick = $(this);
            var dataFilter = { purchasesSynthesiseId: btnClick[0].dataset.objid };
            debugger
            abp.ajax({
                contentType: "application/x-www-form-urlencoded",
                url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.purchasesSynthesiseId,
                success: function (results) {
                    debugger
                    window.location.href =
                        "/Inventorys/MyWork/Update?id=" + results.id;
                },
            });
        });

       


        //------------------------------------------------------------ Chart Js ---------------------------------------------------------------
        var getFilter = function () {
            let dataFilter = {};
            dataFilter.numInTime = $(".dataDgSearch.active").attr("data-objid");
            return dataFilter;
        }

        var getFilterDelivered = function () {
            let dataFilter = {};
            dataFilter.numInTime = $(".dataDagSearch.active").attr("data-objid");
            return dataFilter;
        }

        //Khởi tạo các biểu đồ
        var chartLabels = ['Hoàn thành', 'Săp đến hạn', 'Chưa xử lý', 'Quá hạn'];

        var assiChart = ['Hoàn thành', 'Săp đến hạn', 'Chưa xử lý', 'Quá hạn'];

        //------------------------------ Biểu đồ được giao --------------------------

        var assignedChart = document.getElementById("assignedChart").getContext('2d');

        var chart = new Chart(assignedChart, LoadChart1(chartLabels, 0));
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
        var chartDelivered = new Chart(deliveredChart, LoadChart1(chartLabels, 1));
        var legendDelivered = document.getElementById("legendDelivered");
        // chú thích của Biểu đồ đã giao
        legendDelivered.innerHTML = chartDelivered.generateLegend();
        // bind onClick event to all LI-tags of the legend
        var legendItems = legendDelivered.getElementsByTagName('li');
        for (var i = 0; i < legendItems.length; i += 1) {
            legendItems[i].addEventListener("click", legendClickCallback, false);
        }


        //Function Load Chart
        function LoadChart1(chartLabels, index) {
            var config = {
                type: 'doughnut',
                data: {
                    labels: chartLabels,
                    datasets: [{
                        backgroundColor: [
                            "#0094f2",
                            "#058515",
                            "#24c7a0",
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
        ////Hàm load lại biểu đồ chung
        //function LoadDataOfChart() {
        //    _dashboardService.getAllInDashBoard(getFilter()).done(function (result) {
        //        var tongdg = 0;
        //        var tonghoanthanhdg = 0;
        //        var tongdxldg = 0;
        //        var tongchoxldg = 0;
        //        var tongquahandg = 0;

        //        var tongdag = 0;
        //        var tonghoanthanhdag = 0;
        //        var tongdxldag = 0;
        //        var tongchoxldag = 0;
        //        var tongquahandag = 0;

        //        for (var i = 0; i < result.items.length; i++) {
        //            //da giao
        //            if (result.items[i].ownerStatus == 1) {
        //                if (result.items[i].workStatus == 0) {
        //                    tonghoanthanhdag += 1;
        //                    tongdag += 1;
        //                }
        //                if (result.items[i].workStatus == 1) {
        //                    tongdxldag += 1;
        //                    tongdag += 1;
        //                }
        //                if (result.items[i].workStatus == 3) {
        //                    tongquahandag += 1;
        //                    tongdag += 1;
        //                }
        //                if (result.items[i].workStatus == 2) {
        //                    tongchoxldag += 1;
        //                    tongdag += 1;
        //                }


        //            }
        //            //được giao
        //            if (result.items[i].ownerStatus == 2) {
        //                if (result.items[i].workStatus == 0) {
        //                    tonghoanthanhdg += 1;
        //                    tongdg += 1;
        //                }
        //                if (result.items[i].workStatus == 1) {
        //                    tongdxldg += 1;
        //                    tongdg += 1;
        //                }
        //                if (result.items[i].workStatus == 3) {
        //                    tongquahandg += 1;
        //                    tongdg += 1;
        //                }
        //                if (result.items[i].workStatus == 2) {
        //                    tongchoxldg += 1;
        //                    tongdg += 1;
        //                }
                       
                     
        //            }
        //        }
        //        //update data
        //        chart.config.data.datasets[0].data = [tonghoanthanhdg, tongdxldg, tongchoxldg, tongquahandg];
        //        chartDelivered.data.datasets[0].data = [tonghoanthanhdag, tongdxldag, tongchoxldag, tongquahandag];
        //        $("#sumdg").html(tongdg);
        //        $("#sumdag").html(tongdag);
        //        $("#sumtg").html(0);
               
        //        //update lại biểu đồ
        //        chart.update();
        //        chartDelivered.update();
        //        // chartCombination.update();

        //    })
        //}
        //LoadDataOfChart();


        //function LoadDataOfDelivered() {
        //    _dashboardService.getAllInDashBoard(getFilterDelivered()).done(function (result) {

        //        var tongdag = 0;
        //        var tonghoanthanhdag = 0;
        //        var tongdxldag = 0;
        //        var tongchoxldag = 0;
        //        var tongquahandag = 0;

        //        for (var i = 0; i < result.items.length; i++) {
        //            //đã giao
        //            if (result.items[i].ownerStatus == 1) {
        //                if (result.items[i].workStatus == 0) {
        //                    tonghoanthanhdag += 1;
        //                    tongdag += 1;
        //                }
        //                if (result.items[i].workStatus == 1) {
        //                    tongdxldag += 1;
        //                    tongdag += 1;
        //                }
        //                if (result.items[i].workStatus == 3) {
        //                    tongquahandag += 1;
        //                    tongdag += 1;
        //                }
        //                if (result.items[i].workStatus == 2) {
        //                    tongchoxldag += 1;
        //                    tongdag += 1;
        //                }

        //            }
        //        }
        //        chartDelivered.data.datasets[0].data = [tonghoanthanhdag, tongdxldag, tongchoxldag, tongquahandag];
        //        chartDelivered.update();
        //    })
        //}

        ////Hàm load lại biểu đồ công việc được giao
        //function LoadDataOfAssigned() {
        //    _dashboardService.getAllInDashBoard(getFilter()).done(function (result) {
        //        var tongdg = 0;
        //        var tonghoanthanhdg = 0;
        //        var tongdxldg = 0;
        //        var tongchoxldg = 0;
        //        var tongquahandg = 0;

        //        for (var i = 0; i < result.items.length; i++) {
        //            if (result.items[i].ownerStatus == 2) {
        //                if (result.items[i].workStatus == 0) {
        //                    tonghoanthanhdg += 1;
        //                    tongdg += 1;
        //                }
        //                if (result.items[i].workStatus == 1) {
        //                    tongdxldg += 1;
        //                    tongdg += 1;
        //                }
        //                if (result.items[i].workStatus == 3) {
        //                    tongquahandg += 1;
        //                    tongdg += 1;
        //                }
        //                if (result.items[i].workStatus == 2) {
        //                    tongchoxldg += 1;
        //                    tongdg += 1;
        //                }

        //            }


        //        }
        //        chart.config.data.datasets[0].data = [tonghoanthanhdg, tongdxldg, tongchoxldg, tongquahandg];
        //        chart.update();

        //    })
        //}

        //$("#chart-dg").on("click", ".dataDgSearch", function () {
        //    $(".dataDgSearch.active").removeClass("active");
        //    $(this).addClass("active");
        //    LoadDataOfAssigned();
        //})


        //$("#chart-dag").on("click", ".dataDagSearch", function () {
        //    $(".dataDagSearch.active").removeClass("active");
        //    $(this).addClass("active");
        //    LoadDataOfDelivered();
        //})
    });
})();

