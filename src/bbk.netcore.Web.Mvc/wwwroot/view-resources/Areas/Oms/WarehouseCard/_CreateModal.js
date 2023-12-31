(function ($) {

    app.modals.WarehouseCardCreateModal = function () {

        var _$itemTable = $('#ItemTable');
        var _warehouseCardService = abp.services.app.warehouseCard;
        var _importRequests = abp.services.app.importRequest;
        var _exportRequests = abp.services.app.exportRequest;
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var _modalManager;
        var _frmIMP = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');


          function tbodytr(length) {
            var stt = length + 1
            return html = `<tr>
                                <th>`+ stt + `</th>
                                <th><select size="1" id="row-1-office" class="form-control import ImportRequestId" name="ImportRequestId"  required >
                                <option value="" selected=""> Chọn Phiếu nhập </option>
                                </select></th>
                                 <th><select size="1" id="row-1-office" class="form-control export" name="ExportRequestId"  required >
                                <option value="" selected=""> Chọn phiếu xuất </option>
                                </select></th>
                                <th><input type="number" id="row-1-age" name="ImportQuantity" class="form-control ImportQuantity" value="" fdprocessedid="7ch0d" required></th>
                                <th><input type="text" autocomplete="off" name="ExportQuantity" class="form-control ExportQuantity" value="" required></th>
                                <th><input type="text" autocomplete="off" class="form-control date-picker ImportDate" value="" placeholder="Nhập ngày" id="ExpireDate" name="ImportDate" required></th>
                                <th><input type="text" autocomplete="off" class="form-control date-picker ExportDate" value="" placeholder="Nhập ngày" id="ExpireDate" name="ExportDate" required></th>
                                <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;
          }

          $('#addRow').click(function () {
            var length = $('#ItemTable tbody tr').length
            $('#ItemTable tbody ').append(tbodytr(length))

            _importRequests.getAllByItems({ warehouseDestinationId: $('#WarehouseDestinationId').val(), itemsId: $("#ItemId").val(), status: 1 }).done(function (results) {
              $.each(results.items, function (index, valueoption) {

                option = `<option value="` + valueoption.id + `"  data-date =` + valueoption.creationTime + ` data-quantity= ` + valueoption.quantityItems + `>` + valueoption.code + `</option>`
                $('.ImportRequestId option:last').after(option)
              })
              $('.ImportRequestId').on('change', function () {
                var selectquantity = $(this)

                $('.ImportQuantity').val($(this).find('option:selected').attr('data-quantity'))
                $('.ImportDate').val($(this).find('option:selected').attr('data-date'))

              })
              $('.ImportDate').on('change', function () {
              })

            })

            $('.date-picker').datepicker({
              rtl: false,
              format: 'dd/mm/yyyy',
              orientation: "left",
              autoclose: true,
              language: abp.localization.currentLanguage.name,

            });

            $("#MFG").datepicker({
              todayBtn: 1,
              autoclose: true,
            }).on('changeDate', function (selected) {
              var minDate = new Date(selected.date.valueOf());
              $('#ExpireDate').datepicker('setStartDate', minDate);

            });

            $("#ExpireDate").datepicker()
              .on('changeDate', function (selected) {
                var maxDate = new Date(selected.date.valueOf());
                $('#MFG').datepicker('setEndDate', maxDate);
              });
          })
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
            _frmIMP.addClass('was-validated');
            if (_frmIMP[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

          var data = _frmIMP.serializeFormToObject();
          console.log("data", data);
          debugger;
            _modalManager.setBusy(true);
            _warehouseCardService.create(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới loại kho thành công!');
                    abp.event.trigger('app.reloadTranferTable');
                   
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);