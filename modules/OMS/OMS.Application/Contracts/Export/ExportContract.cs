using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Timing.Timezone;
using Abp.UI;
using bbk.netcore.DataExporting.Excel.EpPlus;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.Net.MimeTypes;
using bbk.netcore.Storage;
using bbk.netcore.Storage.FileSystem;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Contracts.Export
{
    public class ExportContract : EpPlusExcelExporterBase, IExportContract
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;

        public ExportContract(ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            IFileSystemBlobProvider fileSystemBlobProvider,
            ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        public async Task<FileDto> ExportPOToFile(ContractListDto ContractDtos, decimal totalorder, SupplierListDto SupplierDto, List<QuoteListDto> quoteListDto)
        {
            try
            {
                string templateFile = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.ContractPO + @"\\HopDong.docx"));

                FileInfo fileInfo = new FileInfo(templateFile);
                FileDto fileDto = new FileDto(PersonalProfileCoreConsts.ContractPO + "\\PO-" + Clock.Now.ToString("yyyyMMddhhmmss") + ".doc", MimeTypeNames.ApplicationMsword);

                using (MemoryStream memoryStream = new MemoryStream()) {
                    Document doc = new Document(templateFile);
                    doc.Replace("{ContractCode}", ContractDtos.Code, false, true);
                    doc.Replace("{DayCreate}", ContractDtos.CreationTime.Day.ToString(), false, true);
                    doc.Replace("{MouthCreate}", ContractDtos.CreationTime.Month.ToString(), false, true);
                    doc.Replace("{YearCreate}", ContractDtos.CreationTime.Year.ToString(), false, true);

                    if (!string.IsNullOrEmpty(SupplierDto.Name))
                    {
                        doc.Replace("{NameA}", SupplierDto.Name, false, true);
                    }
                    else
                    {
                        doc.Replace("{NameA}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(SupplierDto.Adress))
                    {
                        doc.Replace("{AddressA}", SupplierDto.Adress, false, true);
                    }
                    else
                    {
                        doc.Replace("{AddressA}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(SupplierDto.TaxCode))
                    {
                        doc.Replace("{Tax}", SupplierDto.TaxCode, false, true);
                    }
                    else
                    {
                        doc.Replace("{Tax}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(SupplierDto.PhoneNumber))
                    {
                        doc.Replace("{PhoneNumber}", SupplierDto.PhoneNumber, false, true);
                    }
                    else
                    {
                        doc.Replace("{PhoneNumber}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(SupplierDto.Fax))
                    {
                        doc.Replace("{Fax}", SupplierDto.Fax, false, true);
                    }
                    else
                    {
                        doc.Replace("{Fax}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.RepresentA))
                    {
                        doc.Replace("{RepresentA}", ContractDtos.RepresentA, false, true);
                    }
                    else
                    {
                        doc.Replace("{RepresentA}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.PositionA))
                    {
                        doc.Replace("{PositionA}", ContractDtos.PositionA, false, true);
                    }
                    else
                    {
                        doc.Replace("{RepresentA}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.RepresentB))
                    {
                        doc.Replace("{RepresentB}", ContractDtos.RepresentB, false, true);
                    }
                    else
                    {
                        doc.Replace("{RepresentB}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.PositionB))
                    {
                        doc.Replace("{PositionB}", ContractDtos.PositionB, false, true);
                    }
                    else
                    {
                        doc.Replace("{PositionB}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.BankName))
                    {
                        doc.Replace("{BankName}", ContractDtos.BankName, false, true);
                    }
                    else
                    {
                        doc.Replace("{BankName}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.Stk))
                    {
                        doc.Replace("{STK}", ContractDtos.Stk, false, true);
                    }
                    else
                    {
                        doc.Replace("{STK}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.SellerDate.ToString()))
                    {
                        doc.Replace("{SellerDate}", ContractDtos.SellerDate.ToString("dd/MM/yyyy"), false, true);
                    }
                    else
                    {
                        doc.Replace("{SellerDate}", "…./……/……", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.Price.ToString()))
                    {
                        doc.Replace("{Price}", ContractDtos.Price.ToString(), false, true);
                    }
                    else
                    {
                        doc.Replace("{Price}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.MouthNumber.ToString()))
                    {
                        doc.Replace("{MouthNumber}", ContractDtos.MouthNumber.ToString(), false, true);
                    }
                    else
                    {
                        doc.Replace("{MouthNumber}", "……………………", false, true);
                    }

                    if (!string.IsNullOrEmpty(ContractDtos.Indemnify.ToString()))
                    {
                        doc.Replace("{Indemnify}", ContractDtos.Indemnify.ToString(), false, true);
                    }
                    else
                    {
                        doc.Replace("{Indemnify}", "……………………", false, true);
                    }






                    
                    
                    

                    Section s = doc.Sections[0];
                    Table tblItem = (Table)s.Tables[0];

                    String[][] ListItem = new String[quoteListDto.Count][];
                    for (int i = 0; i < quoteListDto.Count; i++)
                    {
                        ListItem[i] = new String[6] { (i+1).ToString(), quoteListDto[i].ItemName, quoteListDto[i].UnitName, quoteListDto[i].QuantityQuote.ToString(), quoteListDto[i].QuotePrice.ToString(), quoteListDto[i].TotalNumber.ToString() };
                    }

                    CreateWordTable(tblItem, ListItem, Color.Blue);

                    doc.SaveToStream(memoryStream, FileFormat.Doc);

                    byte[] docBytes = memoryStream.ToArray();
                    MemoryStream inStream = new MemoryStream(docBytes);
                    await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, inStream));
                }

                return fileDto;
            }
            catch (Exception ex)
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
    }
}
