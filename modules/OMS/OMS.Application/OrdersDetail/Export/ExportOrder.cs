using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using bbk.netcore.DataExporting.Excel.EpPlus;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Exporting;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Export.Dto;
using Abp.UI;
using Abp.Timing;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.Net.MimeTypes;
using OfficeOpenXml;
using System.IO;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Security.Policy;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Linq;

namespace bbk.netcore.mdl.OMS.Application.OrdersDetail.Export
{
    public class ExportOrder : EpPlusExcelExporterBase, IExportOrder
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;


        public ExportOrder(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            IFileSystemBlobProvider fileSystemBlobProvider,
            ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        public async Task<FileDto> ExportPOToFile(List<OrdersDetailListDto> PoDtos, DateTime fromDate, decimal totalorder, string SupplierName)
        {
            try
            {
                string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ExportPO + @"\\POExport.xlsx"));

                FileInfo fileInfo = new FileInfo(templateFile);

                FileDto fileDto = new FileDto(PersonalProfileCoreConsts.ExportPO + "\\PO-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

                //ExcelPackage has a constructor that only requires a stream.
                using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
                {
                    // get first in 1-base index

                    int Podtosleght = PoDtos.Count;
                    var sheet = excelPackage.Workbook.Worksheets[1];
                    var refName = sheet.Names.FirstOrDefault(x => x.Name == "nameOfNameManager");
                    if (refName != null)
                    {
                        sheet.Names.Remove("nameOfNameManager");
                    }

                  //  sheet.InsertRow(13, PoDtos.Count);

                    sheet.Cells[5, 2].Value = $"Số:{PoDtos[0].OrderCode} ";
                    sheet.Cells[9, 1].Value = $"Kính gửi : \t {SupplierName} ";
                    AddObjects(
                            sheet, 13, PoDtos,
                            _ => _.ContractCode,
                            _ => _.Itemcode,
                            _ => _.ItemName,
                            _ => _.Specifications,
                            _ => _.SupplierName,
                            _ => _.UnitName,
                            _ => _.Quantity,
                            _ => _.OrderPrice,
                            _ => _.TotalPrice,
                            _ => "",
                            _ => _.Note
                            );

                    MakeBorder(sheet, 13, 1, 13 + Podtosleght, 11);

                    sheet.Cells[13 + Podtosleght, 9].Value = totalorder;
                    sheet.Cells[14 + Podtosleght, 9].Value = (totalorder * 10 / 100);
                    sheet.Cells[15 + Podtosleght, 9].Value = totalorder + (totalorder * 10 / 100);



                    //sheet.Cells[16 + Podtosleght, 1].Value = $"Bằng chữ:";
                    //sheet.Cells[17 + Podtosleght, 1].Value = $"Ghi chú:";
                    //sheet.Cells[18 + Podtosleght, 1].Value = $"Điều kiện thanh toán:";
                    //sheet.Cells[19 + Podtosleght, 1].Value = $"Chất lượng yêu cầu:";
                    //sheet.Cells[20 + Podtosleght, 1].Value = $"Thời gian giao hàng:";
                    //sheet.Cells[21 + Podtosleght, 1].Value = $"Địa điểm giao hàng:";
                    //sheet.Cells[22 + Podtosleght, 1].Value = $"Thông tin người nhận hàng:";
                    //sheet.Cells[23 + Podtosleght, 1].Value = $"Tài khoản NCC:";
                    //sheet.Cells[24 + Podtosleght, 1].Value = $"Thông tin liên hệ NCC:";

                    //sheet.Cells[26 + Podtosleght, 2].Value = $"XÁC NHẬN CỦA NHÀ CUNG CẤP";
                    //sheet.Cells[25 + Podtosleght, 5].Value = $"Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm{DateTime.Now.Year}";
                    //sheet.Cells[26 + Podtosleght, 5].Value = $"GIÁM ĐỐC";


                    MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                    await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
                }

                return fileDto;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        private void MakeBorder(ExcelWorksheet sheet, int fromRow, int fromCol, int toRow, int toCol)
        {
            if (toRow > fromRow)
            {
                using (ExcelRange Rng = sheet.Cells[fromRow, fromCol, toRow - 1, toCol])
                {
                    Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Top.Color.SetColor(Color.Black);
                    Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Left.Color.SetColor(Color.Black);
                    Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Right.Color.SetColor(Color.Black);
                    Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Rng.Style.Border.Bottom.Color.SetColor(Color.Black);
                }
            }

        }
    }
}
