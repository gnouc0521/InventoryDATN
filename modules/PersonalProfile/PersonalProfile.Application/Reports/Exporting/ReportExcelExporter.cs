using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Timing.Timezone;
using Abp.UI;
using bbk.netcore.DataExporting.Excel.EpPlus;
using bbk.netcore.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Statistics.Dto;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.Net.MimeTypes;
using bbk.netcore.Storage;
using bbk.netcore.Storage.FileSystem;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Vml;
using OfficeOpenXml.Style;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Exporting
{
    public class ReportExcelExporter : EpPlusExcelExporterBase, IReportExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;


        public ReportExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            IFileSystemBlobProvider fileSystemBlobProvider,
            ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        public FileDto ExportToFile(List<BM03FilterDto> bM03ListDto)
        {
            //Create a stream of .xlsx file contained within my project using reflection
            //Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EPPlusTest.templates.VendorTemplate.xlsx");

            //ExcelPackage has a constructor that only requires a stream.
            //ExcelPackage pck = new OfficeOpenXml.ExcelPackage(stream);

            return CreateExcelPackage(
                "AuditLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditLogs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Time"),
                        L("UserName"),
                        L("Service"),
                        L("Action"),
                        L("Parameters"),
                        L("Duration"),
                        L("IpAddress"),
                        L("Client"),
                        L("Browser"),
                        L("ErrorState")
                    );

                    AddObjects(
                        sheet, 2, bM03ListDto,
                        _ => _.Title
                        //_ => _timeZoneConverter.Convert(_.ExecutionTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        //_ => _.UserName,
                        //_ => _.ServiceName,
                        //_ => _.MethodName,
                        //_ => _.Parameters,
                        //_ => _.ExecutionDuration,
                        //_ => _.ClientIpAddress,
                        //_ => _.ClientName,
                        //_ => _.BrowserInfo,
                        //_ => _.Exception.IsNullOrEmpty() ? L("Success") : _.Exception
                        );

                    //Formatting cells

                    var timeColumn = sheet.Column(1);
                    timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 10; i++)
                    {
                        if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }

        /// <summary>
        /// Đây là ví dụ về addObjects, nếu mọi người ko dùng lại thì có thể viết riêng nhé
        /// </summary>
        //protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        //{
        //    if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
        //    {
        //        return;
        //    }

        //    for (var i = 0; i < items.Count; i++)
        //    {
        //        for (var j = 0; j < propertySelectors.Length; j++)
        //        {
        //            sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
        //        }
        //    }
        //}

        public async Task<FileDto> ExportBM01ToFile(List<BM01Dto> bm01Dtos, DateTime fromDate, DateTime toDate, string orgName)
        {
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM01.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM01-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[4, 1].Value = $"THỐNG KÊ SỐ LIỆU QUẢN LÝ BIÊN CHẾ NĂM {Clock.Normalize(toDate).Year}";
                sheet.Cells[5, 1].Value = $"Tính từ {Clock.Normalize(fromDate).ToShortDateString()} đến ngày {Clock.Normalize(toDate).ToShortDateString()}";
                AddObjects(
                        sheet, 10, bm01Dtos,
                        _ => _.STT,
                        _ => _.OrganizationUnit,
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => _.PresentTo.Total,
                        _ => _.PresentTo.PayrollByLeadershipPositions,
                        _ => _.PresentTo.PayrollByProfessionalTitle,
                        _ => _.PresentTo.Constract,
                        _ => _.PayrollIncreaseOrDecrease.Total,
                        _ => _.PayrollIncreaseOrDecrease.PayrollByLeadershipPositions,
                        _ => _.PayrollIncreaseOrDecrease.PayrollByProfessionalTitle,
                        _ => _.PayrollIncreaseOrDecrease.Constract,
                        _ => "",
                        _ => _.Total,
                        _ => _.OtherConstractPresentTo
                        );
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM01aToFile(List<BM01aDto> bm01aDtos, int year, string orgName)
        {
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM01a.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM01a-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[1, 8].Value = $"Biểu mẫu 01a/{ year }";
                sheet.Cells[2, 1].Value = $"{ orgName.ToUpper() }";
                sheet.Cells[3, 1].Value = $"DANH SÁCH HỢP ĐỒNG LAO ĐỘNG KHÁC NĂM { year }";
                AddObjects(
                        sheet, 7, bm01aDtos,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => _.OrganizationUnit,
                        _ => _.DateNumber,
                        _ => _.ConstractDuration.HasValue ? $"{Clock.Normalize(_.ConstractDuration.Value).ToShortDateString()}" : " ",
                        _ => _.Work,
                        _ => _.TitleOfConstractSigner,
                        _ => "",
                        _ => _.TotalContract
                        );
                MakeBorder(sheet, 7, 1, 7 + bm01aDtos.Count, 9);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM02ToFile(List<Bm02Dto> bm02Dtos, DateTime fromDate, DateTime toDate, string org)
        {
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM02.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM02-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[2, 1].Value = org.ToUpper();
                sheet.Cells[5, 1].Value = $"Tính từ {Clock.Normalize(fromDate).ToShortDateString()} đến ngày {Clock.Normalize(toDate).ToShortDateString()}";

                AddObjects(
                        sheet, 10, bm02Dtos,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => _.MaleDateTime.HasValue ? $"{Clock.Normalize(_.MaleDateTime.Value).ToShortDateString()}" : " ",
                        _ => _.FeMaleDateTime.HasValue ? $"{Clock.Normalize(_.FeMaleDateTime.Value).ToShortDateString()}" : " ",
                        _ => _.Year.HasValue ? $"{_.Year.Value.Year}" : " ",
                        _ => (!string.IsNullOrEmpty(_.currentCoefficientsSalaryDetail.WorkingTitle) ?  $"{ _.currentCoefficientsSalaryDetail.WorkingTitle}" : " ") + (!string.IsNullOrEmpty(_.currentCoefficientsSalaryDetail.OrganName) ?  $", { _.currentCoefficientsSalaryDetail.OrganName}" : " ") + (!string.IsNullOrEmpty(_.currentCoefficientsSalaryDetail.JobPosition) ? $", { _.currentCoefficientsSalaryDetail.JobPosition}": " "),
                        _ => _.currentCoefficientsSalaryDetail.Glone,
                        _ => $"{_.currentCoefficientsSalaryDetail.PayRate}, {_.currentCoefficientsSalaryDetail.CoefficientsSalary}, {Clock.Normalize(_.currentCoefficientsSalaryDetail.FromDate).ToShortDateString()}",
                        _ => "",
                        _ => _.currentCoefficientsSalaryDetail.PositionAllowance,
                        _ => _.currentCoefficientsSalaryDetail.OtherAllowance,
                        _ => "",
                        _ => "",
                        _ => _.oldCoefficientsSalaryDetail!=null ? (!string.IsNullOrEmpty(_.oldCoefficientsSalaryDetail.WorkingTitle) ? $"{ _.oldCoefficientsSalaryDetail.WorkingTitle}" : " ") + (!string.IsNullOrEmpty(_.oldCoefficientsSalaryDetail.OrganName) ? $", {_.oldCoefficientsSalaryDetail.OrganName}" : " ") + (!string.IsNullOrEmpty(_.oldCoefficientsSalaryDetail.JobPosition) ? $", { _.oldCoefficientsSalaryDetail.JobPosition}" : " ") : " ",
                        _ => _.oldCoefficientsSalaryDetail != null ? _.oldCoefficientsSalaryDetail.Glone : " ",
                        _ => _.oldCoefficientsSalaryDetail != null ? $"{_.oldCoefficientsSalaryDetail.PayRate}, {_.oldCoefficientsSalaryDetail.CoefficientsSalary}, {Clock.Normalize(_.oldCoefficientsSalaryDetail.FromDate).ToShortDateString()}" : " ",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => ""
                        );

                MakeBorder(sheet, 10,1,10 + bm02Dtos.Count,20);

                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM03ToFile(List<BM03Dto> bm03Dto, DateTime fromDate, DateTime toDate, string orgName)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM03.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM03-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[5, 1].Value = $"Tính từ {Clock.Normalize(fromDate).ToShortDateString()} đến ngày {Clock.Normalize(toDate).ToShortDateString()}";
                sheet.Cells[2, 1].Value = $"{ orgName.ToUpper() }";
                AddObjects(
                        sheet, 11, bm03Dto,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => _.ServantCode,
                        _ => Clock.Normalize(_.IssuedDate.Value).ToShortDateString(),
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => _.MeetQualificationRequirement.Specialize,
                        _ => _.MeetQualificationRequirement.StateManagement,
                        _ => _.MeetQualificationRequirement.ForeignLanguage,
                        _ => _.MeetQualificationRequirement.InformationTechnology,
                        _ => _.MeetQualificationRequirement.PoliticsTheoReticalLevel,
                        _ => "",
                        _ => _.MeetQualificationRequirement.TimeKeepOldServant,
                        _ => "",
                        _ => "",
                        _ => _.Note
                        );
                MakeBorder(sheet, 11, 1, 11 + bm03Dto.Count, 19);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM05ToFile(List<BM05Dto> bm05Dto, DateTime fromDate, DateTime toDate, string orgName)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM05.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM05-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[5, 1].Value = $"GIỮ CHỨC VỤ LÃNH ĐẠO, QUẢN LÝ TỪ {Clock.Normalize(fromDate).ToShortDateString()} ĐẾN {Clock.Normalize(toDate).ToShortDateString()}";
                sheet.Cells[2, 1].Value = $"{ orgName.ToUpper() }";
                AddObjects(
                        sheet, 11, bm05Dto,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => _.Male,
                        _ => _.FeMale,
                        _ => _.Position,
                        _ => _.DecisionAppointDate.HasValue ? Clock.Normalize(_.DecisionAppointDate.Value).ToShortDateString() : "",
                        _ => _.DecisionReAppointDate.HasValue ? Clock.Normalize(_.DecisionReAppointDate.Value).ToShortDateString() : "",
                        _ => _.DecisionMaker,
                        _ => _.CurriculumVitae,
                        _ => _.PropertyDeclaration,
                        _ => "",
                        _ => "",
                        _ => _.Qualification,
                        _ => _.PoliticsTheoReticalLevel,
                        _ => _.StateManagement,
                        _ => _.ForeignLanguage,
                        _ => _.InformationTechnology
                        );
                MakeBorder(sheet, 11, 1, 11 + bm05Dto.Count, 19);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM04ToFile(List<BM04Dto> bm04ListDto, DateTime fromDate, DateTime toDate, string org)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM04.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM04-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[6, 1].Value = $"LUÂN CHUYỂN, BIỆT PHÁI; CHUYỂN ĐỔI VỊ TRÍ CÔNG TÁC TỪ {Clock.Normalize(fromDate).ToShortDateString()} ĐẾN {Clock.Normalize(toDate).ToShortDateString()}";
                sheet.Cells[4, 1].Value = org.ToUpper();

                AddObjects(
                        sheet, 12, bm04ListDto,
                        _ => _.STT,
                        _ => "",
                        _ => _.FullName,
                        _ => "",
                        _ => "",
                        _ => _.OldJobPosition,
                        _ => _.TimeHoldOldJobPosition,
                        _ => _.NewWorkingTitle,
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => _.SecondedTime,
                        _ => _.DecisionYear,
                        _ => _.DecisionMaker,
                        _ => _.Glone,
                        _ => _.OldAndNewWorkiingTitle,
                        _ => _.Allowance
                        );
                MakeBorder(sheet, 12, 1, 12 + bm04ListDto.Count, 20);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM07ToFile(List<BM07Dto> bm07ListDto, DateTime fromDate, DateTime toDate, string org)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM07.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM07-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[4, 1].Value = $"TỪ {Clock.Normalize(fromDate).ToShortDateString()} ĐẾN {Clock.Normalize(toDate).ToShortDateString()}";
                sheet.Cells[2, 1].Value = org.ToUpper();

                AddObjects(
                        sheet, 9, bm07ListDto,
                        _ => _.STT,
                        _ => "",
                        _ => _.FullName,
                        _ => _.OldWorkingTitleAndOrg,
                        _ => _.TimeHoldWorkingTitle,
                        _ => Clock.Normalize(_.IssuedDate).ToShortDateString(),
                        _ => _.DecisionMaker,
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => _.WorkingTitleAndOrg
                        );
                MakeBorder(sheet, 9, 1, 9 + bm07ListDto.Count, 16);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM09ToFile(List<BM09Dto> bm09Dto, int fromDate, int toDate, string orgName)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM09.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM09-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[2, 1].Value = $"{ orgName.ToUpper() }";
                sheet.Cells[4, 1].Value = $"DANH SÁCH CÔNG CHỨC, VIÊN CHỨC ĐƯỢC ĐÁNH GIÁ, XẾP LOẠI NĂM {fromDate} ĐẾN {toDate}";
                sheet.Cells[7, 3].Value = $"Năm {fromDate}";
                sheet.Cells[7, 9].Value = $"Năm {toDate}";
                AddObjects(
                        sheet, 10, bm09Dto,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => _.Evaluate1 != null ? _.Evaluate1.Position : "",
                        _ => _.Evaluate1 != null ? _.Evaluate1.SelfAssessment : "",
                        _ => _.Evaluate1 != null ? _.Evaluate1.LeaderAssessment : "",
                        _ => _.Evaluate1 != null ? _.Evaluate1.GroupAssessment : "",
                        _ => _.Evaluate1 != null ? _.Evaluate1.CmpetentPersonsAssessment : "",
                        _ => _.Evaluate1 != null ? _.Evaluate1.Result : "",
                        _ => _.Evaluate2 != null ? _.Evaluate2.Position : "",
                        _ => _.Evaluate2 != null ? _.Evaluate2.SelfAssessment : "",
                        _ => _.Evaluate2 != null ? _.Evaluate2.LeaderAssessment : "",
                        _ => _.Evaluate2 != null ? _.Evaluate2.GroupAssessment : "",
                        _ => _.Evaluate2 != null ? _.Evaluate2.CmpetentPersonsAssessment : "",
                        _ => _.Evaluate2 != null ? _.Evaluate2.Result : "",
                        _ => ""
                        );
                MakeBorder(sheet, 10, 1, 10 + bm09Dto.Count, 15);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM08ToFile(List<BM08Dto> bm08ListDto, DateTime fromDate, DateTime toDate, string org)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM08.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM08-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[4, 1].Value = $"TỪ {Clock.Normalize(fromDate).ToShortDateString()} ĐẾN {Clock.Normalize(toDate).ToShortDateString()}";
                sheet.Cells[2, 1].Value = org.ToUpper();
                var fromRow = 9;
                var toRowBorder = 0;
                foreach (var item in bm08ListDto)
                {
                    toRowBorder = toRowBorder + item.TrainningDetails.Count;
                    var row = fromRow;
                    sheet.Cells[row, 1].Value = item.STT;
                    sheet.Cells[row, 2].Value = "";
                    sheet.Cells[row, 3].Value = item.FullName;
                    sheet.Cells[row, 4].Value = item.WorkingTitle;
                    sheet.Cells[row, 5].Value = item.Org;
                    sheet.Cells[row, 6].Value = item.Glone;
                    foreach (var detail in item.TrainningDetails)
                    {
                        var col = 7;
                        sheet.Cells[row, col].Value = detail.TrainningContent;
                        col++;
                        sheet.Cells[row, col].Value = detail.TrainningType;
                        col++;
                        sheet.Cells[row, col].Value = detail.TrainningTime;
                        row++;
                    }
                    for(var i = 1; i <= 6; i++)
                    {
                        sheet.Cells[fromRow, i, row - 1, i].Merge = true;
                    }
                    fromRow = row;
                }
                MakeBorder(sheet, 9, 1, 9 + toRowBorder, 10);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM10ToFile(List<BM10Dto> bm10ListDto,int count1, int count2, int count3, string org)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM10.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM10-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[2, 5].Value = $"DANH SÁCH CÔNG CHỨC, VIÊN CHỨC ĐƯỢC KHEN THƯỞNG \n NĂM {Clock.Now.Year - 2}, {Clock.Now.Year-1}, {Clock.Now.Year}";
                sheet.Cells[2, 1].Value = $"Đơn vị: {org.ToUpper()}";
                sheet.Cells[5, 4].Value = $"THI ĐUA KHEN THƯỞNG NĂM {Clock.Now.Year - 2}";
                sheet.Cells[5, 7].Value = $"THI ĐUA KHEN THƯỞNG NĂM {Clock.Now.Year - 1}";
                sheet.Cells[5, 10].Value = $"THI ĐUA KHEN THƯỞNG NĂM {Clock.Now.Year}";

                AddObjects(
                        sheet, 8, bm10ListDto,
                        _ => _.STT,
                        _ => "",
                        _ => _.FullName,
                        _ => _.CommendationTitle3,
                        _ => "",
                        _ => "",
                        _ => _.CommendationTitle2,
                        _ => "",
                        _ => "",
                        _ => _.CommendationTitle1,
                        _ => "",
                        _ => ""
                        );
                sheet.Cells[8+bm10ListDto.Count + 2,1].Value = $"Tổng số người được khen thưởng năm {Clock.Now.Year - 2}: {count3} người";
                sheet.Cells[8+bm10ListDto.Count + 3,1].Value = $"Tổng số người được khen thưởng năm {Clock.Now.Year - 1}: {count2} người";
                sheet.Cells[8+bm10ListDto.Count + 4,1].Value = $"Tổng số người được khen thưởng năm {Clock.Now.Year}: {count1} người";
                MakeBorder(sheet, 8, 1, 8 + bm10ListDto.Count, 13);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM12ToFile(List<BM12Dto> bm12ListDto, DateTime fromDate, DateTime toDate, string orgName)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM12.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM12-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[3, 1].Value = $"{ orgName.ToUpper() }";
                sheet.Cells[6, 1].Value = $" KÉO DÀI THỜI GIAN CÔNG TÁC, THÔI VIỆC TỪ {Clock.Normalize(fromDate).ToShortDateString()} ĐẾN {Clock.Normalize(toDate).ToShortDateString()}";

                AddObjects(
                        sheet, 12, bm12ListDto,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => _.Male.HasValue ? Clock.Normalize(_.Male.Value).ToShortDateString() : "",
                        _ => _.FeMale.HasValue ? Clock.Normalize(_.FeMale.Value).ToShortDateString() : "",
                        _ => _.Position,
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => _.DecisionDate.HasValue ? Clock.Normalize(_.DecisionDate.Value).ToShortDateString() : "",
                        _ => _.Retirement.ToRetirementAge,
                        _ => "",
                        _ => _.Retirement.NoticeDate.HasValue ? Clock.Normalize(_.Retirement.NoticeDate.Value).ToShortDateString() : "",
                        _ => Clock.Normalize(_.Retirement.RetirementTimeInNotice).ToShortDateString(),
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => "",
                        _ => _.Decision.DecisionDate.HasValue ? Clock.Normalize((DateTime)_.Decision.DecisionDate).ToShortDateString() : "",
                        _ => _.Decision.RetirementTime.HasValue ? Clock.Normalize((DateTime)_.Decision.RetirementTime).ToShortDateString() : "",
                        _ => _.Decision.TitlePersonDecision,
                        _ => ""
                        );
                MakeBorder(sheet, 12, 1, 12 + bm12ListDto.Count, 21);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM15ToFile(List<BM15Dto> bm15Dto, string orgName)
        {
            // Find template for VEA-Reports
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM15.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM15-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                // get first in 1-base index
                var sheet = excelPackage.Workbook.Worksheets[1];
                sheet.Cells[2, 1].Value = $"Đơn vị: { orgName.ToUpper() }";
                AddObjects(
                        sheet, 7, bm15Dto,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => _.StaffBackground,
                        _ => _.CurriculumVitae,
                        _ => _.AdditionalStaffBackground,
                        _ => _.BriefBiography,
                        _ => _.BirthCertificate,
                        _ => _.HealthCertificate,
                        _ => _.PersonalIdentityDcuments,
                        _ => _.TrainingQualificationCV,
                        _ => _.RecruitmentDecision,
                        _ => _.DocumentsAppointingPosition,
                        _ => _.SelfAssessment,
                        _ => _.EvaluationComment,
                        _ => _.PropertyDeclaration,
                        _ => _.HandlingDocument,
                        _ => _.WorkProcessDocument,
                        _ => _.RequestFormToResearchRecord,
                        _ => _.DocumentObjective,
                        _ => _.DocumentClipCover,
                        _ => _.ProfileCover,
                        _ => ""
                        );
                MakeBorder(sheet, 7, 1, 7 + bm15Dto.Count, 22);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportBM13ToFile(List<BM13Dto> bM13Dtos, DateTime fromDate, DateTime toDate, string org)
        {
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\BM13.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Reports + "\\BM13-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            //ExcelPackage has a constructor that only requires a stream.
            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                var sheet = excelPackage.Workbook.Worksheets[1];
                // get first in 1-base index
                sheet.Cells[2, 9].Value = $"DANH SÁCH CÔNG CHỨC, VIÊN CHỨC ĐƯỢC TUYỂN DỤNG CỦA CƠ QUAN, ĐƠN VỊ QUA HÌNH THỨC THI \nTUYỂN, XÉT TUYỂN TỪ {Clock.Normalize(fromDate).ToShortDateString()} đến ngày {Clock.Normalize(toDate).ToShortDateString()}";
                sheet.Cells[2, 1].Value = $"Đơn vị: {org.ToUpper()}";
                AddObjects(
                        sheet, 8, bM13Dtos,
                        _ => _.STT,
                        _=>"",
                        _ => _.FullName,
                        _ => _.DonDuTuyen,
                        _ => "",
                        _ => _.LyLichCoXacNhan,
                        _ => _.BanSaoKhaiSinh,
                        _ => _.GiayChungNhanSucKhoe,
                        _ => _.ChungNhanDoiTuongUuTien,
                        _ => _.KhongBiKyLuat,
                        _ => _.ChuyenMon,
                        _ => _.NgoaiNgu,
                        _ => _.TinHoc,
                        _ => "",
                        _ => _.ThoiGianThongBaoKetQua,
                        _ => _.ThoiGianRaQuyetDinh,
                        _ => _.DonViVaViTriCongTac,
                        _ => _.ThoiGianDenNhanViec,
                        _ => _.QuyetDinhHuongDanTapSu,
                        _ => _.TapSuCheDoCuaNguoiTapSu,
                        _ => _.CheDoCuaNguoiHuongDan,
                        _ => _.MienTapSu
                        );
                MakeBorder(sheet, 8, 1, 8 + bM13Dtos.Count, 23);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportStatisticToFile(List<StatisticDto> staffs)
        {
            string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\Statistic.template.xlsx"));

            FileInfo fileInfo = new FileInfo(templateFile);

            FileDto fileDto = new FileDto(PersonalProfileCoreConsts.Statistics + "\\Statistics-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (ExcelPackage excelPackage = new OfficeOpenXml.ExcelPackage(fileInfo))
            {
                var sheet = excelPackage.Workbook.Worksheets[1];
                AddObjects(
                        sheet, 3, staffs,
                        _ => _.STT,
                        _ => _.FullName,
                        _ => Clock.Normalize(_.DateOfBirth).ToShortDateString(),
                        _ => _.NativePlace,
                        _ => _.Position,
                        _ => _.CivilServantCode,
                        _ => _.CoefficientsSalary,
                        _ => _.PositionAllowance,
                        _ => _.OtherAllowance,
                        _ => _.TimeIncreaseSalary.HasValue ? Clock.Normalize(_.TimeIncreaseSalary.Value).ToShortDateString() : "",
                        _ => _.HighestAcademicLevel,
                        _ => _.Specialized
                        );
                MakeBorder(sheet, 3, 1, 3 + staffs.Count, 12);
                MemoryStream memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, memoryStream));
            }

            return fileDto;
        }

        public async Task<FileDto> ExportHS02(ExportHS02Dto person)
        {
            try
            {
                string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ReportTemplates + @"\\HS02.template.doc"));
                string fullName = person.FullName.Replace(" ", "-");
                FileDto fileDto = new FileDto(PersonalProfileCoreConsts.HS02s + "\\" + fullName + "-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".doc", MimeTypeNames.ApplicationMsword);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document doc = new Document(templateFile);
                    doc.Replace("{OrganizationUnit}", person.OrganizationUnit, false, true);
                    doc.Replace("{FullName}", person.FullName.ToUpper(), false, true);
                    doc.Replace("{OtherName}", person.OtherName, false, true);
                    doc.Replace("{Gender}", EnumExtensions.GetDisplayName(person.Gender), false, true);
                    doc.Replace("{D}", person.DateOfBirth.Day.ToString(), false, true);
                    doc.Replace("{M}", person.DateOfBirth.Month.ToString(), false, true);
                    doc.Replace("{Y}", person.DateOfBirth.Year.ToString(), false, true);
                    doc.Replace("{Ethnic}", person.Ethnic, false, true);
                    doc.Replace("{Religion}", person.Religion, false, true);
                    doc.Replace("{AddressOfBirth}", person.VillageOfBirth + ", " + person.DistrictOfBirth + ", " + person.ProvinceOfBirth, false, true);
                    doc.Replace("{AddressOfNativePlace}", person.VillageOfNativePlace + ", " + person.DistrictOfNativePlace + ", " + person.ProvinceOfNativePlace, false, true);
                    doc.Replace("{PermanentPlace}", person.PermanentPlace, false, true);
                    doc.Replace("{ResidentialPlace}", person.ResidentialPlace, false, true);
                    doc.Replace("{Occupation}", person.Occupation, false, true);
                    doc.Replace("{CurrentPosition}", person.CurrentPosition, false, true);
                    doc.Replace("{RecruitmentDate}", person.RecruitmentDate.ToString("dd/MM/yyyy"), false, true);
                    doc.Replace("{RecruitmentUnit}", person.RecruitmentUnit, false, true);
                    doc.Replace("{ConcurrentPosition}", person.ConcurrentPosition, false, true);
                    doc.Replace("{KeyResponsibilities}", person.KeyResponsibilities, false, true);
                    doc.Replace("{CivilServantSector}", person.CivilServantSector, false, true);
                    doc.Replace("{CivilServantSectorCode}", person.CivilServantSectorCode, false, true);
                    doc.Replace("{Coefficientssalary}", person.Coefficientssalary, false, true);
                    doc.Replace("{PayRate}", person.PayRate, false, true);
                    doc.Replace("{PayDay}", person.PayDay.ToString("dd/MM/yyyy"), false, true);
                    doc.Replace("{PositionAllowance}", person.PositionAllowance, false, true);
                    doc.Replace("{OtherAllowance}", person.OtherAllowance, false, true);
                    doc.Replace("{HighSchoolEducationalLevel}", person.HighSchoolEducationalLevel, false, true);
                    doc.Replace("{HighestAcademicLevel}", person.HighestAcademicLevel, false, true);
                    doc.Replace("{PoliticsTheoReticalLevel}", person.PoliticsTheoReticalLevel, false, true);
                    doc.Replace("{StateManagement}", person.StateManagement, false, true);
                    doc.Replace("{ForeignLanguage}", person.ForeignLanguage, false, true);
                    doc.Replace("{InfomationTechnology}", person.InfomationTechnology, false, true);
                    doc.Replace("{HighestAwardedTitle}", person.HighestAwardedTitle, false, true);
                    doc.Replace("{AcademicTitle}", person.AcademicTitle, false, true);
                    doc.Replace("{Specialized}", person.Specialized, false, true);
                    doc.Replace("{Awards}", person.Awards, false, true);
                    doc.Replace("{Discipline}", person.Discipline, false, true);
                    if (!string.IsNullOrEmpty(person.HealthStatus))
                    {
                        doc.Replace("{HealthStatus}", person.HealthStatus, false, true);
                    }
                    else
                    {
                        doc.Replace("{HealthStatus}", "……………………", false, true);
                        TextSelection[] txtHealthStatus = doc.FindAllString("……………………", false, true);
                        foreach (TextSelection seletion in txtHealthStatus)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (!string.IsNullOrEmpty(person.Weight))
                    {
                        doc.Replace("{Weight}", person.Weight, false, true);
                    }
                    else
                    {
                        doc.Replace("{Weight}", "………", false, true);
                        TextSelection[] txtWeight = doc.FindAllString("………", false, true);
                        foreach (TextSelection seletion in txtWeight)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (!string.IsNullOrEmpty(person.Height))
                    {
                        doc.Replace("{Height}", person.Height, false, true);
                    }
                    else
                    {
                        doc.Replace("{Height}", "………", false, true);
                        TextSelection[] txtHeight = doc.FindAllString("………", false, true);
                        foreach (TextSelection seletion in txtHeight)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (!string.IsNullOrEmpty(person.BloodGroup))
                    {
                        doc.Replace("{BloodGroup}", person.BloodGroup, false, true);
                    }
                    else
                    {
                        doc.Replace("{BloodGroup}", "………", false, true);
                        TextSelection[] txtBloodGroup = doc.FindAllString("………", false, true);
                        foreach (TextSelection seletion in txtBloodGroup)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (!string.IsNullOrEmpty(person.WoundedSoldierRank))
                    {
                        doc.Replace("{WoundedSoldierRank}", person.WoundedSoldierRank, false, true);
                    }
                    else
                    {
                        doc.Replace("{WoundedSoldierRank}", "……./…….", false, true);
                        TextSelection[] txtWoundedSoldierRank = doc.FindAllString("……./…….", false, true);
                        foreach (TextSelection seletion in txtWoundedSoldierRank)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (!string.IsNullOrEmpty(person.SonOfFamilyPolicy))
                    {
                        doc.Replace("{SonOfFamilyPolicy}", person.SonOfFamilyPolicy, false, true);
                    }
                    else
                    {
                        doc.Replace("{SonOfFamilyPolicy}", "………………………...", false, true);
                        TextSelection[] txtSonOfFamilyPolicy = doc.FindAllString("………………………...", false, true);
                        foreach (TextSelection seletion in txtSonOfFamilyPolicy)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    doc.Replace("{IdentityCardNo}", person.IdentityCardNo, false, true);
                    doc.Replace("{DateOfIssuanceIdentityCard}", person.DateOfIssuanceIdentityCard.ToString("dd/MM/yyyy"), false, true);
                    doc.Replace("{SocialInsuranceBookNo}", person.SocialInsuranceBookNo, false, true);
                    doc.Replace("{DateOfIssuanceIdentityCard}", person.DateOfIssuanceIdentityCard.ToString("dd/MM/yyyy"), false, true);
                    if (person.DateOfJoiningInVNCommunistParty.HasValue)
                    {
                        doc.Replace("{DateOfJoiningInVNCommunistParty}", person.DateOfJoiningInVNCommunistParty.Value.ToString("dd/MM/yyyy"), false, true);
                    }
                    else
                    {
                        doc.Replace("{DateOfJoiningInVNCommunistParty}", "…./……/……", false, true);
                        TextSelection[] txtDateJoinVNComPar = doc.FindAllString("…./……/……", false, true);
                        foreach (TextSelection seletion in txtDateJoinVNComPar)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (person.OfficialDateOfJoiningInVNCommunistParty.HasValue)
                    {
                        doc.Replace("{OfficialDateOfJoiningInVNCommunistParty}", person.OfficialDateOfJoiningInVNCommunistParty.Value.ToString("dd/MM/yyyy"), false, true);
                    }
                    else
                    {
                        doc.Replace("{OfficialDateOfJoiningInVNCommunistParty}", "....../……/……", false, true);
                        TextSelection[] txtOfficialDateJoinVNComPar = doc.FindAllString("....../……/……", false, true);
                        foreach (TextSelection seletion in txtOfficialDateJoinVNComPar)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (!string.IsNullOrEmpty(person.DateOfJoiningSocialPoliticalAssosiations))
                    {
                        doc.Replace("{DateOfJoiningSocialPoliticalAssosiations}", person.DateOfJoiningSocialPoliticalAssosiations, false, true);
                    }
                    else
                    {
                        doc.Replace("{DateOfJoiningSocialPoliticalAssosiations}", "..……………………………………………", false, true);
                        TextSelection[] txtDateJoinSocial = doc.FindAllString("..……………………………………………", false, true);
                        foreach (TextSelection seletion in txtDateJoinSocial)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    doc.Replace("{DateOfJoiningSocialPoliticalAssosiations}", person.DateOfJoiningSocialPoliticalAssosiations, false, true);
                    if (person.DateOfEnlistment.HasValue)
                    {
                        doc.Replace("{DateOfEnlistment}", person.DateOfEnlistment.Value.ToString("dd/MM/yyyy"), false, true);
                    }
                    else
                    {
                        doc.Replace("{DateOfEnlistment}", "…/…/…", false, true);
                        TextSelection[] txtDateOfEnlist = doc.FindAllString("…/…/…", false, true);
                        foreach (TextSelection seletion in txtDateOfEnlist)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (person.DateOfDisCharge.HasValue)
                    {
                        doc.Replace("{DateOfDisCharge}", person.DateOfDisCharge.Value.ToString("dd/MM/yyyy"), false, true);
                    }
                    else
                    {
                        doc.Replace("{DateOfDisCharge}", "…/…/…", false, true);
                        TextSelection[] txtDateOfDisCharge = doc.FindAllString("…/…/…", false, true);
                        foreach (TextSelection seletion in txtDateOfDisCharge)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    if (!string.IsNullOrEmpty(person.HighestArmyRank))
                    {
                        doc.Replace("{HighestArmyRank}", person.HighestArmyRank, false, true);
                    }
                    else
                    {
                        doc.Replace("{HighestArmyRank}", "………………..", false, true);
                        TextSelection[] txtHighestArmyRank = doc.FindAllString("………………..", false, true);
                        foreach (TextSelection seletion in txtHighestArmyRank)
                        {
                            seletion.GetAsOneRange().CharacterFormat.TextColor = Color.Black;
                        }
                    }
                    doc.Replace("{Imprisoned}", person.Imprisoned, false, true);
                    doc.Replace("{OrganizationAbroad}", person.OrganizationAbroad, false, true);
                    doc.Replace("{RelativesAbroad}", person.RelativesAbroad, false, true);
                    doc.Replace("{Year}", DateTime.Today.Year.ToString(), false, true);
                    TextSelection[] txtNone = doc.FindAllString("Không", false, true);
                    foreach (TextSelection seletion in txtNone)
                    {
                        seletion.GetAsOneRange().CharacterFormat.Italic = true;
                    }
                    if (!string.IsNullOrEmpty(person.Evaluate.Trim()))
                    {
                        doc.Replace("{Evaluate}", person.Evaluate, false, true);
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            person.Evaluate += "……………………………………………………………………………………………………………";
                        }
                        doc.Replace("{Evaluate}", person.Evaluate, false, true);
                    }
                    Section s = doc.Sections[0];

                    Table tblTrainning = (Table)s.Tables[3];

                    if (person.TrainningInfos != null)
                    {
                        String[][] trainnings = new String[person.TrainningInfos.Count][];
                        for (int i = 0; i < person.TrainningInfos.Count; i++)
                        {
                            trainnings[i] = new String[5] { person.TrainningInfos[i].SchoolName, person.TrainningInfos[i].MajoringName, person.TrainningInfos[i].FromDate.ToString("dd/MM/yyyy") + "-" + person.TrainningInfos[i].ToDate.ToString("dd/MM/yyyy"), person.TrainningInfos[i].TrainningType, person.TrainningInfos[i].Diploma };
                        }

                        CreateWordTable(tblTrainning, trainnings, Color.Blue);
                    }

                    Table tblWorkingProcess = (Table)s.Tables[4];

                    if (person.WorkingProcesses != null)
                    {
                        String[][] workingProcesses = new String[person.WorkingProcesses.Count][];
                        for (int i = 0; i < person.WorkingProcesses.Count; i++)
                        {
                            workingProcesses[i] = new String[2] { person.WorkingProcesses[i].FromDate.ToString("dd/MM/yyyy") + " đến " + (person.WorkingProcesses[i].ToDate.HasValue ? person.WorkingProcesses[i].ToDate.Value.ToString("dd/MM/yyyy") : " nay"), person.WorkingProcesses[i].WorkingTitle + ", " + person.WorkingProcesses[i].OrganName };
                        }

                        CreateWordTable(tblWorkingProcess, workingProcesses, Color.Blue);
                    }

                    Table tblRelationSelf = (Table)s.Tables[5];

                    if (person.RelationSelf != null)
                    {
                        String[][] relationSelfs = new String[person.RelationSelf.Count][];
                        for (int i = 0; i < person.RelationSelf.Count; i++)
                        {
                            relationSelfs[i] = new String[4] { person.RelationSelf[i].RelationName, person.RelationSelf[i].FullName, person.RelationSelf[i].YearBirth.ToString(), person.RelationSelf[i].Info };
                        }

                        CreateWordTable(tblRelationSelf, relationSelfs, Color.Blue);
                    }

                    Table tblRelationOther = (Table)s.Tables[6];

                    if (person.RelationOther != null)
                    {
                        String[][] relationOthers = new String[person.RelationOther.Count][];
                        for (int i = 0; i < person.RelationOther.Count; i++)
                        {
                            relationOthers[i] = new String[4] { person.RelationOther[i].RelationName, person.RelationOther[i].FullName, person.RelationOther[i].YearBirth.ToString(), person.RelationOther[i].Info };
                        }

                        CreateWordTable(tblRelationOther, relationOthers, Color.Blue);
                    }


                    if (person.SalaryProcesses != null)
                    {
                        var salaryProcessesCol1 = new List<String> { "Tháng/năm" };
                        var salaryProcessesCol2 = new List<String> { "Mã số" };
                        var salaryProcessesCol3 = new List<String> { "Bậc lương" };
                        var salaryProcessesCol4 = new List<String> { "Hệ số lương" };
                        for (int i = person.SalaryProcesses.Count - 1; i >= 0; i--)
                        {
                            salaryProcessesCol1.Add(person.SalaryProcesses[i].SalaryIncreaseTime.ToString("MM/yyyy"));
                            salaryProcessesCol2.Add(person.SalaryProcesses[i].GloneCode);
                            salaryProcessesCol3.Add(person.SalaryProcesses[i].PayRate);
                            salaryProcessesCol4.Add(person.SalaryProcesses[i].CoefficientsSalary);
                        }
                        String[][] salaryProcesses =
                        {
                            salaryProcessesCol2.ToArray(),
                            salaryProcessesCol3.ToArray(),
                            salaryProcessesCol4.ToArray()
                        };
                        Table tblSalary = s.AddTable(true);
                        tblSalary.ResetCells(1, salaryProcesses[0].Length);
                        TableRow FRow = tblSalary.Rows[0];
                        FRow.IsHeader = true;
                        for (int i = 0; i < salaryProcessesCol1.ToArray().Length; i++)
                        {
                            Paragraph p = FRow.Cells[i].AddParagraph();
                            FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                            p.Format.HorizontalAlignment = HorizontalAlignment.Left;
                            TextRange TR = p.AppendText(salaryProcessesCol1.ToArray()[i]);
                            TR.CharacterFormat.FontName = "Times New Roman";
                            TR.CharacterFormat.FontSize = 13;
                            TR.CharacterFormat.TextColor = Color.Black;
                            TR.CharacterFormat.Bold = false;
                        }
                        s.Paragraphs[51].AppendBookmarkStart("Spire");
                        s.Paragraphs[52].AppendBookmarkEnd("Spire");
                        BookmarksNavigator bn = new BookmarksNavigator(doc);
                        bn.MoveToBookmark("Spire", true, true);
                        Section section0 = doc.AddSection();
                        Paragraph paragraph = section0.AddParagraph();

                        bn.InsertTable(tblSalary);
                        doc.Sections.Remove(section0);
                        CreateWordTable(tblSalary, salaryProcesses, Color.Black);
                    }
                    
                    doc.SaveToStream(memoryStream, FileFormat.Doc);

                    byte[] docBytes = memoryStream.ToArray();
                    MemoryStream inStream = new MemoryStream(docBytes);
                    await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, inStream));
                }
                return fileDto;
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private void CreateWordTable(Table table, String[][] data, Color color)
        {
            for (int r = 0; r < data.Length; r++)
            {
                TableRow DataRow = table.AddRow();
                table.Rows.Insert(r + 1, DataRow);

                DataRow.Height = 20;

                for (int c = 0; c < data[r].Length; c++)
                {
                    DataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    Paragraph p2 = DataRow.Cells[c].AddParagraph();
                    TextRange TR2 = p2.AppendText(data[r][c]);
                    p2.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    TR2.CharacterFormat.FontName = "Times New Roman";
                    TR2.CharacterFormat.FontSize = 13;
                    TR2.CharacterFormat.TextColor = color;
                }
            }
            table.TableFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Hairline;
            table.TableFormat.Borders.Color = Color.Black;
        }

        private void MakeBorder(ExcelWorksheet sheet, int fromRow, int fromCol, int toRow, int toCol)
        {
            if(toRow > fromRow)
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
