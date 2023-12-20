(function () {
  var _$warehouseCardTable = $('#warehouseCardTable');
  var _warehouseCardService = abp.services.app.warehouseCard;
  moment.locale(abp.localization.currentLanguage.name);

  var _createModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Inventorys/WareHouseCard/Create',
    scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WarehouseCard/_CreateModal.js',
    modalClass: 'WarehouseCardCreateModal',
    modalType: 'modal-xl'

  });

  var _EditModal = new app.ModalManager({
    viewUrl: abp.appPath + 'Inventorys/WarehouseTypes/EditWarehouseTypeModal',
    scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WarehouseTypes/_EditModal.js',
    modalClass: 'WarehouseTypeEditModal',
    modalType: 'modal-xl'
  });

  $('#CreateNewButtonxx').click(function () {
    _createModal.open();
  });

  /* DEFINE TABLE */
  var getFilter = function () {
    let dataFilter = {};
    dataFilter.searchTerm = $('#SearchTerm').val();
    return dataFilter;
  }


  var dataTable = _$warehouseCardTable.DataTable({
    paging: true,
    serverSide: false,
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
      ajaxFunction: _warehouseCardService.getAll,
      inputFilter: getFilter
    },

    columnDefs: [

      {
        orderable: false,
        targets: 0,
        width: "5%",
        className: 'dt-body-center text-center',
        render: function (data, type, row, meta) {
          return '<input type="checkbox" name="" value="' + row.id + '">';
        }
      },
      {
        targets: 1,
        data: "code"
      },
      {

        targets: 2,
        data: 'id',
        orderable: false,
        autoWidth: false,
        render: function (data, type, row, meta) {
          console.log(row)
          return `<div class='text-center'> 
                                <a class='btn btn-warning text-white doceditfunc ' data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                            </div>`;
        }
      },

    ],
    order: [[1, 'asc']]
  });

  $('#example-select-all').on('click', function () {
    // Get all rows with search applied
    var rows = dataTable.rows({ 'search': 'applied' }).nodes();
    // Check/uncheck checkboxes for all rows in the table
    $('input[type="checkbox"]', rows).prop('checked', this.checked);
    $('#DeleteAll').removeAttr('disabled');
    var selected = new Array();
    $('#unitsTable tbody input[type="checkbox"]:checked').each(function () {
      selected.push($(this));
    });
    if (selected.length > 0) {
      $('#DeleteAll').removeAttr('disabled');
    } else {
      $('#DeleteAll').prop('disabled', true);
    }
  });

  // Handle click on checkbox to set state of "Select all" control
  $('#unitsTable tbody').on('change', 'input[type="checkbox"]', function () {
    // If checkbox is not checked
    var selected = new Array();
    $('#unitsTable tbody input[type="checkbox"]:checked').each(function () {
      selected.push($(this));
    });
    if (selected.length > 0) {
      $('#DeleteAll').removeAttr('disabled');
    } else {
      $('#DeleteAll').prop('disabled', true);
    }

    if (!this.checked) {
      var el = $('#example-select-all').get(0);
      console.log(el)
      // If "Select all" control is checked and has 'indeterminate' property
      if (el && el.checked && ('indeterminate' in el)) {
        // Set visual state of "Select all" control
        // as 'indeterminate'
        el.indeterminate = true;
      }
    }
  });

  dataTable.$('input[type="checkbox"]').each(function (index, value) {
    if (value > 0) {
      $('#DeleteAll').removeAttr('disabled');
    }
  })

  $('#DeleteAll').on('click', function (e) {
    abp.message.confirm(
      app.localize('Xóa đơn vị tính', "Đơn vị tính"),
      app.localize('Bạn có chắc không'),
      function (isConfirmed) {
        if (isConfirmed) {
          // Iterate over all checkboxes in the table
          dataTable.$('input[type="checkbox"]').each(function (index, value) {
            if ($(value).is(":checked")) {
              _warehouseTypesService.delete(
                $(value).val()
              ).done(function () {
                // getUsers(true);
                $('#example-select-all').prop('checked', false);
                abp.notify.success(app.localize('Xóa đơn vị tính thành công'));
                getDocs();
              });
            }
            // If checkbox doesn't exist in DOM
            // If checkbox is checked
            console.log($(value).val())

          }
          );

        }
      });
  })

  abp.event.on('app.reloadDocTable', function () {
    getDocs();
  });

  $('#unitsTable').on('click', '.doceditfunc', function (e) {
    var btnClick = $(this);
    var dataFilter = { id: btnClick[0].dataset.objid };
    _EditModal.open(dataFilter);
  });

  $('#unitsTable').on('click', '.docdeletefunc', function (e) {
    var btnClick = $(this);
    var dataFilter = btnClick[0].dataset.objid;

    deleteUnits(dataFilter);
  });

  function deleteUnits(UnitID) {
    abp.message.confirm(
      app.localize('Xóa đơn vị tính'),
      app.localize('Bạn có chắc không'),
      (isConfirmed) => {
        if (isConfirmed) {
          _warehouseTypesService
            .delete(UnitID)
            .done(() => {
              abp.notify.success(app.localize('Xóa đơn vị tính thành công'));
              getDocs();
            });
        }
      }
    );
  }

  $('#Search').click(function (e) {
    e.preventDefault();
    getDocs();
  });

  function getDocs() {

    dataTable.ajax.reload();
  }

  jQuery(document).ready(function () {
    $("#SearchTerm").focus();
  });


})(jQuery);