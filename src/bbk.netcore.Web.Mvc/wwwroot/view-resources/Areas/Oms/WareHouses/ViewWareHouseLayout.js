(function () {

    var _warehouseItemsService = abp.services.app.warehouseItem;
    var _warehouseLocationItemService = abp.services.app.warehouseLocationItemService;
    var _$ArrangeItemsTables = $('#ImportRequestTable');
    var idwwarehouse = $("#IdWareHouse").val();

    

    //color
    $("#cp2").colorpicker();

    _warehouseItemsService.getAllItemOutLayout(idwwarehouse).done(function (result) {
        var html = "";
        $.each(result.items, function (index, value) {
            html = `<option value=` + value.id + ` data-value=` + value.id + ` class="itemwh_` + value.id + `" ><span>` + value.name + `</span></option>`;

            $("#selectWareHouseItem").append(html);

        });
    });

    var widthWare = $("#WareWidth").val();
    var lengWare = $("#WareLength").val();

    //Thu vien Konva
    let stage = new Konva.Stage({
        height: widthWare,
        width: lengWare,
        container: "layout",
        draggable: true,
    });

    let layer = new Konva.Layer();
    stage.add(layer);

    //Load ra Layout
    function LoadWidget() {
        _warehouseItemsService.getAllItemInLayout(idwwarehouse).done(function (result) {

            var xPo = 1;
            var yPo = 1;

            var htmlLi = "";
            
            $.each(result.items, function (index, value) {

                let reat = new Konva.Rect({
                    x: xPo*value.positionX,
                    y: yPo*value.positionY,
                    fill: value.color,
                    width: value.width,
                    height: value.lenght,
                    draggable: true,
                    name: value.name,
                    value: value.id,
                });

                htmlLi = `<li class="d-flex align-items-baseline mb-1"><div class="mr-2" style="height: 12px; width: 12px; background: ` + value.color + `"><a href="#ViewInforLocation"></a></div> - <span class="ml-1">` + value.name + `</span></li>`;
                $("#listBock").append(htmlLi);

                // add widget to the layer
                layer.add(reat);
            });

            //let reat1 = new Konva.Rect({
            //    x: 50,
            //    y: 50,
            //    fill: "black",
            //    width: 50,
            //    height: 50,
            //    draggable: true,
            //    name: "ssds",
            //    value: 109,
            //});

            //layer.add(reat1);

        });
    };

    // Them moi widget
    $("#add-widget").click(function () {
        var selectVal = $("#selectWareHouseItem").val();

        var elememtSelect = $("#selectWareHouseItem").children();
        $(`#selectWareHouseItem option[value='` + selectVal + `']`).remove();
        var color = $("#color-widget").val();

        var dataFilter = { id: selectVal };
        var htmlLi = "";
        _warehouseItemsService.getById(dataFilter).done(function (result1) {
            
            let reat = new Konva.Rect({
                x: 10,
                y: 10,
                fill: color,
                width: result1.width,
                height: result1.lenght,
                draggable: true,
                name: result1.name,
                value: result1.id,
            });

            // add widget to the layer
            layer.add(reat);

            htmlLi = `<li class="d-flex align-items-baseline mb-1"><div class="mr-2" style="height: 12px; width: 12px; background: ` + color + `"></div> - <span class="ml-1">` + result1.name + `</span></li>`;
            $("#listBock").append(htmlLi);
        })
    });

    //Luu Layout
    $("#SaveWidget").click(function () {
        var data = {};
        for (var i = 0; i < layer.children.length; i++) {
            data.Id = layer.children[i].attrs.value;
            data.positionX = (layer.children[i].attrs.x).toFixed();
            data.positionY = (layer.children[i].attrs.y).toFixed();
            data.Color = layer.children[i].attrs.fill;

            _warehouseItemsService.saveLayOut(data).done(function (result2) {

            });
        }

        abp.notify.info('Cập nhật thành công!');
    });

   


    //Click view table
    layer.on('click', function (evt) {
        $("#viewTable").empty();
        // get the shape that was clicked on
        var shape = evt.target;
        var dataText = {};
        dataText.id = shape.value();

        //Hiện đến chi tiết vị trí khi nhấn vào layout
        document.body.scrollTop = 2000;
        document.documentElement.scrollTop = 2000;
        //Text View
        _warehouseItemsService.getById(dataText).done(function (result2) {
            $("#CodeWidget").text(result2.code);
            $("#NameWidget").text(result2.name);
        });

        /* DEFINE TABLE */
        var getFilter = function () {
            let dataFilter = {};
            dataFilter.warehouseId = $("#IdWareHouse").val();
            dataFilter.block = shape.value();
            return dataFilter;
        }

        var htmltable = `
                       <table id="ImportRequestTable" class="table table-bordered table-hover table-striped w-100">
                            <thead class="bg-primary-600">
                                <tr>
                                    <th>Mã phiếu nhập kho</th>
                                    <th>Mã Vị trí</th>
                                    <th>Tên Vị trí</th>
                                    <th>Mã hàng hóa</th>
                                    <th>Tên hàng hóa</th>
                                    <th>Đơn vị tính</th>
                                    <th>Số lượng ở vị trí </th>
                                    <th>Số lượng Thực tế </th>
                                    
                                    <th>Trạng thái</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                `;

         $("#viewTable").append(htmltable);

        //bảng chi tiết
        var dataTables = $('#ImportRequestTable').DataTable({
            paging: false,
            serverSide: true,
            processing: true,
            "searching": false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
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
                ajaxFunction: _warehouseLocationItemService.getByIdBlock,
                inputFilter: getFilter
            },

            columnDefs: [
                {
                    orderable: true,
                    targets: 0,
                    width: "8%",
                    data: 'importRequestCode',
                    render: function (data, type, row, meta) {
                        console.log(row);
                        return `<span class="importRequestCode" data-ImportRequestDetailId='` + row.importRequestDetailId + `' data-importRequestId="` + row.importRequestId + `">` + row.importRequestCode + ` </span>`
                    }

                },
                {
                    orderable: false,
                    width: "10%",
                    targets: 1,
                    data: "blockName",
                },
                {
                    orderable: false,
                    width: "10%",
                    targets: 2,
                    data: "locationName",
                },
                {
                    orderable: false,
                    targets: 3,
                    width: "10%",
                    data: "itemCode",
                    render: function (data, type, row, meta) {
                        return `<span class="WarehouseId" data-warehouseId='` + row.warehouseId + `'>` + row.itemCode + ` </span>`
                    }
                },
                {
                    targets: 4,
                    data: "itemsName",
                    width: "10%",
                    render: function (data, type, row, meta) {
                        return `<span class="itemCode" data-ItemsId='` + row.id + `' data-ItemCode="` + row.itemCode + `">` + row.itemsName + ` </span>`
                    }
                },
                {
                    orderable: true,
                    targets: 5,
                    width: "8%",
                    data: "unitName",
                    render: function (row) {
                        return `<span>` + row + ` </span>`
                    }
                },
                {
                    orderable: true,
                    targets: 6,
                    width: "5%",
                    className: "text-center",
                    data: "quantity",
                    //render: function (data, type, row, meta) {
                    //    return `<input type="number" class="form-control Quantity " name="Quantity" value="` + row.quantity + `" max="` + row.quantity + `" min="1" required disabled >`
                    //}
                },
                {
                    orderable: false,
                    width: "5%",
                    targets: 7,
                    className: "text-center",
                    data: "quantityReality",
                },
                //{
                //    targets: 8,
                //    data: "expireDate",
                //    render: function (creationTime) {
                //        return moment(creationTime).format('L');
                //    }
                //},
                {
                    targets: 8,
                    width: "15%",
                    render: function (data, type, row, meta) {
                        var html = "";
                        if (row.importStatus == 5 && row.isItems == true) {
                            html = `<span class="span_status span-approve">Đã có hàng trong vị trí</span>`
                        }
                        else {
                            html = `<span class="span_status span-defaut">Hàng chờ</span>`
                        }
                        return html;
                    }
                },

            ],
            "initComplete": function (settings, json) {
               // SelectLocationss()

            }
        });

        //var SelectLocationss = function () {
        //    var locationCode = $('#ImportRequestTable tbody td span.locationCode')
        //    $.each(locationCode, function (index, value) {
        //        var dataBlock = $(value).attr('data-block');
        //        _warehouseItemsService.getCodeLocationBin(id = dataBlock).done(function (result) {
        //            $(value).text(result)
        //        })
        //    })
        //}
    });

    //Move amd Zoom Block
    var scaleBy = 1.01;
    stage.on('wheel', (e) => {
        // stop default scrolling
        e.evt.preventDefault();

        var oldScale = stage.scaleX();
        var pointer = stage.getPointerPosition();

        var mousePointTo = {
            x: (pointer.x - stage.x()) / oldScale,
            y: (pointer.y - stage.y()) / oldScale,
        };

        // how to scale? Zoom in? Or zoom out?
        let direction = e.evt.deltaY > 0 ? 1 : -1;

        // when we zoom on trackpad, e.evt.ctrlKey is true
        // in that case lets revert direction
        if (e.evt.ctrlKey) {
            direction = -direction;
        }

        var newScale = direction > 0 ? oldScale * scaleBy : oldScale / scaleBy;

        stage.scale({ x: newScale, y: newScale });

        var newPos = {
            x: pointer.x - mousePointTo.x * newScale,
            y: pointer.y - mousePointTo.y * newScale,
        };
        stage.position(newPos);
    });

    LoadWidget();
})(jQuery);