using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Export
{
    public interface IExportPurchaseAssignment
    {
        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="PoDtos">Danh sách các hàng hóa đơn hàng </param>
        /// <param name="fromDate">  </param>
        /// <param name="totalorder">tổng giá trị đơn hàng </param>
        /// <param name="SupplierName"> Tên nhà cung cấp</param>
        /// <returns>Export file  </returns>
        Task<FileDto> ExportPOToFile(List<PurchasesSynthesisListDto> PoDtos, DateTime fromDate , string SupplierName);
    }
}
