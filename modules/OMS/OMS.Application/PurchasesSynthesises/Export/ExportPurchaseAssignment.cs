using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Timing.Timezone;
using Abp.UI;
using bbk.netcore.DataExporting.Excel.EpPlus;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.Net.MimeTypes;
using bbk.netcore.Storage;
using bbk.netcore.Storage.FileSystem;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Export
{
    public class ExportPurchaseAssignment : EpPlusExcelExporterBase, IExportPurchaseAssignment
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;


        public ExportPurchaseAssignment(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            IFileSystemBlobProvider fileSystemBlobProvider,
            ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        public async Task<FileDto> ExportPOToFile(List<PurchasesSynthesisListDto> PoDtos, DateTime fromDate, string SupplierName)
        {
            try
            {
                string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.Templates + @"\\Baogia.xlsx"));

                FileInfo fileInfo = new FileInfo(templateFile);

                FileDto fileDto = new FileDto("\\Baogia-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

                //ExcelPackage has a constructor that only requires a stream.
                using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
                {
                    // get first in 1-base index

                    int Podtosleght = PoDtos.Count;
                    var sheet = excelPackage.Workbook.Worksheets[1];

                    sheet.InsertRow(13, PoDtos.Count);

                     sheet.Cells[10, 1].Value = $" Kính gửi: Qúy công ty : \t {SupplierName} ";
                    AddObjects(
                            sheet, 13, PoDtos,
                            _ => "",
                            _ => _.ItemsName,
                            _ => _.Itemcode,
                            _ => "",
                            _ => _.UnitName,
                            _ => _.Quantity,
                            _ => "",
                            _ => _.Note
                            );

                    MakeBorder(sheet, 13, 1, 13 + Podtosleght, 8);


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

        private ExcelRange GetLastContiguousCell(ExcelRange beginCell)
        {
            var worksheet = beginCell.Worksheet;
            var beginCellAddress = new ExcelCellAddress(beginCell.Start.Row, beginCell.Start.Column);
            var lastCellAddress = worksheet.Dimension.End;
            var bottomCell = worksheet.Cells[beginCellAddress.Row, beginCellAddress.Column, lastCellAddress.Row, beginCellAddress.Column]
                .First(cell => cell.Offset(1, 0).Value == null);
            var rightCell = worksheet.Cells[beginCellAddress.Row, beginCellAddress.Column, beginCellAddress.Row, lastCellAddress.Column]
                .First(cell => cell.Offset(0, 1).Value == null);
            return worksheet.Cells[bottomCell.Start.Row, rightCell.Start.Column];
        }
    }
}
