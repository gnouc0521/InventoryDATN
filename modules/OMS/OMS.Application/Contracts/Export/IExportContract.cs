using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Contracts.Export
{
    public interface IExportContract
    {
        Task<FileDto> ExportPOToFile(ContractListDto ContractDtos, decimal totalorder, SupplierListDto SupplierDto, List<QuoteListDto> quoteListDto);
    }
}
