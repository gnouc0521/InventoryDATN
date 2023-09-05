(function () {
    "use strict";
    $(function () {
        var _$YCNKTable = $('#YCNKTable');
        var _$ExportRequimentTable = $('#ExportRequimentTable');
        var _ExportService = abp.services.app.exportRequests;
        var _impsService = abp.services.app.importRequestSubidiaryService;
        var _dashboardService = abp.services.app.dashboardService;
        moment.locale(abp.localization.currentLanguage.name);

        var DataTable = _$YCNKTable.DataTable({
            paging: false,
            serverSide: false,
            processing: true,
            "searching": false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không tìm thấy dữ liệu",
                searchPlaceholder: "Tìm kiếm"
            },
            "bInfo": false,
            "bLengthChange": true,
            //"dom": 'Rltip',
            lengthMenu: [
                [5, 10, 25, 50, -1],
                [5, 10, 25, 50, 'Tất cả'],
            ],


            pageLength: 10,
            listAction: {
                ajaxFunction: _impsService.getAllDone,
            },
            order: [[0, 'asc']],
            columnDefs: [

                {
                    orderable: false,
                    className: 'dt-body-center text-center',
                    targets: 0,
                    data: null,
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                {
                    orderable: false,
                    targets: 1,
                    data: "code",
                    render: function (data, type, row, meta) {
                        return `
                        <a class="viewwork" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.code + `</a>
                    `
                    }
                },
             
                {
                    orderable: false,
                    targets: 2,
                    data: "requestDate",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },
                {
                    orderable: false,
                    targets: 3,
                    data: "browsingtime",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },

                {

                    targets: 4,
                    class: 'text-center',
                    orderable: false,
                    render: function (data, type, row, meta) {
                        return row.statusImport == true ?
                            "<a type='button' class='btn btn-primary btn-sm disabled' style='margin-left:5px'><i class='fal fa-check'></i> Tạo phiếu nhập</a>" :
                            ` <a type = 'button' class='btn btn-primary btn-sm impcreatefunc' data-objid='` + row.id + `'href = 'javascript:void(0);' style = 'margin-left:5px'><i class='fa fa-plus'></i> Tạo phiếu nhập</a>`;
                    }
                }
            ]
        });

       


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
        var chartLabels = ['Chưa xử lý', 'Hoàn thành'];
        var chartLabelPN = ['Chưa xử lý', 'Hoàn thành'];

        //------------------------------ Biểu đồ px --------------------------

        var assignedChart = document.getElementById("assignedChartTKPX").getContext('2d');

        var chart = new Chart(assignedChart, LoadChart(chartLabels, 0));
        var myLegendContainer = document.getElementById("legendTKPX");
        // chú thích của Biểu đồ được giao
        myLegendContainer.innerHTML = chart.generateLegend();
        // bind onClick event to all LI-tags of the legend
        var legendItems = myLegendContainer.getElementsByTagName('li');
        for (var i = 0; i < legendItems.length; i += 1) {
            legendItems[i].addEventListener("click", legendClickCallback, false);
        }

        //------------------------------ / Biểu đồ px --------------------------

        //------------------------------- Biểu đồ pn -----------------------------

        debugger
        var deliveredChart = document.getElementById("deliveredChartTKPN").getContext('2d');
        var chartDelivered = new Chart(deliveredChart, LoadChart(chartLabelPN, 1));
        var legendDelivered = document.getElementById("legendDeliveredTKPN");
        // chú thích của Biểu đồ đã giao
        legendDelivered.innerHTML = chartDelivered.generateLegend();
        // bind onClick event to all LI-tags of the legend
        var legendItems = legendDelivered.getElementsByTagName('li');
        for (var i = 0; i < legendItems.length; i += 1) {
            legendItems[i].addEventListener("click", legendClickCallback, false);
        }

        //------------------------------- / Biểu đồ pn -----------------------------

        //Function Load Chart
        function LoadChart(chartLabels, index) {
            var config = {
                type: 'doughnut',
                data: {
                    labels: chartLabels,
                    datasets: [{
                        backgroundColor: [
                            "#0094f2",
                            "#ec3b48"
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
        function LoadDataOfChart() {
            _dashboardService.getAllThuKhoPx(getFilter()).done(function (result) {
                var tongdg = 0;
                var tonghoanthanhpx = 0;
                var tongdxlpx = 0;

                for (var i = 0; i < result.items.length; i++) {
                    //được giao

                    if (result.items[i].exportStatus == 2) {
                        tonghoanthanhpx += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].exportStatus == 1) {
                        tongdxlpx += 1;
                        tongdg += 1;
                    }
                }
                //update data
                chart.config.data.datasets[0].data = [tongdxlpx,tonghoanthanhpx];
                $("#sumdg").html(tongdg);
                $("#sumtg").html(0);

                chart.update();

            })
        }

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
                chartDelivered.data.datasets[0].data = [tongdxlpn, tonghoanthanhpn];
                $("#sumdg").html(tongpn);
                $("#sumtg").html(0);

                //update lại biểu đồ
                chartDelivered.update();
            })
        }
        LoadDataOfChart();

        LoadDataOfChartPn();


        

        //Hàm load lại biểu đồ công việc được giao
        function LoadDataOfAssigned() {
            _dashboardService.getAllThuKhoPx(getFilter()).done(function (result) {
                var tongdg = 0;
                var tonghoanthanhpx = 0;
                var tongdxlpx = 0;

                for (var i = 0; i < result.items.length; i++) {
                    //được giao

                    if (result.items[i].exportStatus == 2) {
                        tonghoanthanhpx += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].exportStatus == 1) {
                        tongdxlpx += 1;
                        tongdg += 1;
                    }
                }
                //update data
                chart.config.data.datasets[0].data = [tongdxlpx, tonghoanthanhpx];

                //update lại biểu đồ
                chart.update();

            })
        }

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

        //Filter biểu đồ được giao
        $("#chart-dgtkpx").on("click", ".dataDgSearch", function () {
            $(".dataDgSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfAssigned();
        })

        //Filter biểu đồ đã giao
        $("#chart-dagtkpn").on("click", ".dataDagSearch", function () {
            $(".dataDagSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfDelivered();
        })



        ///px
        var getFilterRequiment = function () {
            let dataFilter = {};
            dataFilter.status = 2;
            dataFilter.exportStatus = 0;
            return dataFilter;
        }

        var RequimentTable = _$ExportRequimentTable.DataTable({
            paging: false,
            serverSide: false,
            processing: false,
            "searching": false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
            },
            "ordering": false,
            "bInfo": false,
            "bLengthChange": true,
            lengthMenu: [
                [5, 10, 25, 50, -1],
                [5, 10, 25, 50, 'Tất cả'],
            ],
            pageLength: 5,
            listAction: {
                ajaxFunction: _ExportService.getAllRequirementApprove,
                inputFilter: getFilterRequiment
            },
            columnDefs: [

                {
                    targets: 0,
                    orderable: false,
                    className: 'dt-body-center text-center',
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                {
                    orderable: true,
                    targets: 1,
                    data: 'codeRequirement',
                    render: function (data, type, row, meta) {
                        return `
                            <a class='Exportviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>` + row.codeRequirement + ` </a>`;
                    }
                },
                {
                    orderable: false,
                    targets: 2,
                    data: "creationTime",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },
                {
                    orderable: false,
                    targets: 3,
                    data: null,
                    render: function (data, type, row, meta) {
                        if (row.status == 3) {
                            return moment(row.lastModificationTime).format('L');
                        } else {
                            return `<span></span>`
                        }

                    }
                },

                {

                    targets: 4,
                    data: 'id',
                    class: 'text-center',
                    orderable: false,
                    autoWidth: false,
                    render: function (data, type, row, meta) {
                        return ` <button id="Create-Export" name="CreateExport" class="btn btn-sm btn-toolbar-full btn-primary ml-auto Create-Export" data-objid=` + row.id + `>
                                Tạo phiếu xuất
                                </button>`
                    }
                },

            ]
        })





        //------------------------------------------------------------ /Chart Js ---------------------------------------------------------------

    });
})();

