(function ($) {

  app.modals.ExportRequestsCreateModal = function () {


    var _exportRequests = abp.services.app.exportRequests;
    var _exportRequestsdetail = abp.services.app.exportRequestDetails;
    var _modalManager;
    var _frmDelivery = null;
    var _$ExportTable = $('#ExportRequestModal')



    this.init = function (modalManager) {
      $("#WarehouseDestinationId").val($('#IdWarehouseExport').val())

      var getFilter = function () {
        let dataFilter = {};
        dataFilter.id = $('#Idtransfer').val();
        return dataFilter;
      }



      var dataTable = _$ExportTable.DataTable({
        paging: true,
        serverSide: false,
        processing: false,
        "searching": false,
        "language": {
          "emptyTable": "Không tìm thấy dữ liệu",
          "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,
        lengthMenu: [
          [5, 10, 25, 50, -1],
          [5, 10, 25, 50, 'Tất cả'],
        ],
        pageLength: 10,
        listAction: {
          ajaxFunction: _transferDetail.getAll,
          inputFilter: getFilter
        },
        order: [[1, 'asc']],
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
            data: 'itemName',
            class: 'ItemId',
            render: function (data, type, row, meta) {
              return `<div class=''> 
                                <a class='itemName' data-objid='` + row.itemId + `'href='javascript:void(0); ' > ` + row.itemName + ` </a>
                            </div>`;
            }

          },
          {
            orderable: false,
            targets: 2,
            data: 'quotePrice',
            class: 'ExportPrice',
            render: function (data, type, row, meta) {
              return `<div class=''> 
                                <a class='quotePrice' data-objid='` + row.quotePrice + `'href='javascript:void(0); ' > ` + row.quotePrice + ` </a>
                            </div>`;
            }
          },
          {
            orderable: false,
            class: 'text-center',
            targets: 3,
            data: 'quantityInStock',

          },
          {
            orderable: false,
            targets: 4,
            data: "quantityTransfer",
            class: 'Quantity',
            render: function (data, type, row, meta) {
              return `<div class=''> 
                                <span class='quantityTransfer' data-idWarehouseReceiving=`+ row.idWarehouseReceiving + ` data-objid='` + row.quantityTransfer + `'href='javascript:void(0); ' > ` + row.quantityTransfer + ` </span>
                            </div>`;
            }

          },
          {

            targets: 5,
            data: 'unitName',
            class: 'text-center',
            orderable: false,
            autoWidth: false,
            class: 'UnitId',
            render: function (data, type, row, meta) {
              return `<div class=''> 
                                <span class='UnitId' data-objid='` + row.idUnit + `'href='javascript:void(0); ' > ` + row.unitName + ` </span>
                            </div>`;
            }
          }, {

            targets: 6,
            data: 'warehouseReceivingName',
            class: 'text-center',
            orderable: false,
            autoWidth: false,
          },

        ]
      })



      _modalManager = modalManager;
      _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');
    }

    //sự kiện khi đóng modal
    $(".close-modal").on("click", function () {
      abp.libs.sweetAlert.config = {
        confirm: {
          icon: 'warning',
          buttons: ['Không', 'Có']
        }
      };
      abp.message.confirm(
        app.localize('Đóng'),
        app.localize('Bạn có chắc không'),
        function (isConfirmed) {
          if (isConfirmed) {
            _modalManager.close();
            return true;

          }
        }
      );
    })

    this.save = function () {
      _frmDelivery.addClass('was-validated');
      if (_frmDelivery[0].checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        return;
      }
      var data = _frmDelivery.serializeFormToObject();
      _modalManager.setBusy(true);
      _exportRequests.create(data)
        .done(function (result) {
          _modalManager.close();
          abp.notify.info('Thêm mới phiếu xuất thành công!');
          abp.event.trigger('app.reloadDocTable');
          $('#ExportRequestModal tbody tr').each(function (index, value) {
            data.WarehouseSourceId = $(value).children('td.Quantity').find('.quantityTransfer').attr('data-idWarehouseReceiving');
            data.ExportRequestId = result;
            data.ItemId = $(value).children('td.ItemId').find('.itemName').attr('data-objid');
            data.Quantity = $(value).children('td.Quantity').find('.quantityTransfer').attr('data-objid');
            data.ExportPrice = $(value).children('td.ExportPrice').find('.quotePrice').attr('data-objid');
            if ($(value).children('td').find('.BlockId option:selected').attr('data-objid') != "") {
              data.BlockId = $(value).children('td').find('.BlockId option:selected').attr('data-objid');
            }
            data.FloorId = $(value).children('td').find('.FloorId option:selected').attr('data-objid');
            data.ShelfId = $(value).children('td').find('.ShelfId option:selected').attr('data-objid');
            data.UnitId = $(value).children('td.UnitId').find('.UnitId').attr('data-objid')
            _exportRequestsdetail.create(data)
          })
        }).always(function () {
          _modalManager.setBusy(false);
        });

    }
  };
})(jQuery);


