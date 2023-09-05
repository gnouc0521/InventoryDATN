(function () {
    "use strict";
    $(function () {
        var _$ContractTable = $("#ContractTable");
        var _dashboardService = abp.services.app.dashboardService;
        var _contractService = abp.services.app.contract;
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
        var chartgd = ['Chờ xử lý', 'Phê duyệt', 'Từ chối'];


        //------------------------------ Biểu đồ được giao --------------------------

        var assignedChart = document.getElementById("assignedChartGD").getContext('2d');

        var chart = new Chart(assignedChart, LoadChart(chartgd, 0));
        var myLegendContainer = document.getElementById("legendGD");
        // chú thích của Biểu đồ được giao
        myLegendContainer.innerHTML = chart.generateLegend();
        // bind onClick event to all LI-tags of the legend
        var legendItems = myLegendContainer.getElementsByTagName('li');
        for (var i = 0; i < legendItems.length; i += 1) {
            legendItems[i].addEventListener("click", legendClickCallback, false);
        }

        //------------------------------ / Biểu đồ được giao --------------------------




        //Function Load Chart
        function LoadChart(chartgd, index) {
            var config = {
                type: 'doughnut',
                data: {
                    labels: chartgd,
                    datasets: [{
                        backgroundColor: [
                            "#24c7a0",
                            "#0094f2",
                            "#ec3b48",
                        ],
                        data: [0, 0,0],
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
            _dashboardService.getAllManager(getFilter()).done(function (result) {

                var tongdg = 0;
                var tongpheduyet = 0;
                var tongchoxuly = 0;
                var tongtuchoi = 0;
                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].status == 1) {
                        tongpheduyet += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].status == 2) {
                        tongtuchoi += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].status == 0) {
                        tongchoxuly += 1;
                        tongdg += 1;
                    }
                }
                //update data
                chart.config.data.datasets[0].data = [tongpheduyet, tongchoxuly, tongtuchoi];
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
            _dashboardService.getAllManager(getFilter()).done(function (result) {
                debugger
                var tongdg = 0;
                var tongpheduyet = 0;
                var tongchoxuly = 0;
                var tongtuchoi = 0;

                for (var i = 0; i < result.items.length; i++) {
                    //được giao
                    if (result.items[i].status == 1) {
                        tongpheduyet += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].status == 2) {
                        tongtuchoi += 1;
                        tongdg += 1;
                    }
                    if (result.items[i].status == 0) {
                        tongchoxuly += 1;
                        tongdg += 1;
                    }
                }
                //update data
                chart.config.data.datasets[0].data = [tongpheduyet, tongchoxuly, tongtuchoi];

                //update lại biểu đồ
                chart.update();

            })
        }

        //Filter biểu đồ được giao
        $("#chart-dg").on("click", ".dataDgSearch", function () {
            $(".dataDgSearch.active").removeClass("active");
            $(this).addClass("active");
            LoadDataOfAssigned();
        })
        //------------------------------------------------------------ /Chart Js ---------------------------------------------------------------


        if (abp.auth.isGranted('Inventorys.Quote.Edit')) {
            var getFilterC = function () {
                let dataFilter = {};
                dataFilter.status = 1;
                return dataFilter;
            }
        }
        else {
            var getFilterC = function () {
                let dataFilterC = {};
                dataFilter.status = 2;
                return dataFilter;
            }
        }


        var ContractdataTable = _$ContractTable.DataTable({
            paging: false,
            serverSide: false,
            processing: false,
            searching: false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không tìm thấy dữ liệu",
                searchPlaceholder: "Tìm kiếm"
            },
            "bInfo": false,
            "bLengthChange": true,

            lengthMenu: [
                [5, 10, 25, 50, -1],
                [5, 10, 25, 50, 'Tất cả'],
            ],

            pageLength: 5,
            listAction: {
                ajaxFunction: _contractService.getAllInApprove,
                inputFilter: getFilterC
            },
            order: [[0, 'asc']],
            columnDefs: [
                {
                    orderable: false,
                    targets: 0,
                    width: "5%",
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                {
                    orderable: false,
                    targets: 1,
                    width: "10%",
                    data: "code",
                    render: function (data, type, row, meta) {
                        return `<div class=''> 
                                <a class='doceditfunc' data-objid='` + row.id + `' data-view="` + row.id + `" href='javascript:void(0); ' > ` + row.code + ` </a>
                            </div>`;
                    }
                },
                {
                    orderable: false,
                    targets: 2,
                    width: "15%",
                    className: "text-center",
                    render: function (data, type, row, meta) {

                        return `<span class="total" data-contact="` + row.id + `" data-sup="` + row.supplierId + `" data-quoSyn="` + row.quoteSynId + `"></span>
                            <span class="viewtotal`+ row.id + `"></span>`;
                    }
                },
                {

                    targets: 3,
                    data: 'status',
                    width: "20%",
                    orderable: false,
                    autoWidth: false,
                    className: "align-middle text-center",
                    render: function (status) {
                        if (abp.auth.isGranted('Inventorys.Quote.Edit')) {
                            if (status == 1) {
                                return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                            } else if (status == 2) {
                                return `<span class="span_status span-subcontract"> Đã gửi </span>`
                            } else if (status == 3 || status == 4) {
                                return `<span class="span_status span-reject"> Từ chối </span>`
                            }
                            else if (status == 6) {
                                return `<span class="span_status span-approve"> Đã kí hợp đồng</span>`
                            }
                        }
                        else {
                            if (status == 2) {
                                return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                            } else if (status == 5) {
                                return `<span class="span_status span-approve"> Đã duyệt </span>`
                            } else if (status == 3) {
                                return `<span class="span_status span-reject"> Từ chối </span>`
                            }
                            else if (status == 6) {
                                return `<span class="span_status span-approve"> Đã kí hợp đồng</span>`
                            }
                        }
                    }
                },
                {

                    orderable: false,
                    targets: 4,
                    data: null,
                    width: "5%",
                    render: function (data, type, row, meta) {
                        return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item view-detail' data-view='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xem chi tiết') + `</a>
                            	</div>
                            </div>`;

                    },
                }
            ],
            "initComplete": function (settings, json) {
                ToTalNumber();
            },
        });

    });
})();

