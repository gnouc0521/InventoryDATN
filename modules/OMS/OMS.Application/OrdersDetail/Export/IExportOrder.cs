using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Export.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.OrdersDetail.Export
{
    public interface IExportOrder
    {
        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="PoDtos">Danh sách các hàng hóa đơn hàng </param>
        /// <param name="fromDate">  </param>
        /// <param name="totalorder">tổng giá trị đơn hàng </param>
        /// <param name="SupplierName"> Tên nhà cung cấp</param>
        /// <returns>Export file  </returns>
        Task<FileDto> ExportPOToFile(List<OrdersDetailListDto> PoDtos, DateTime fromDate , decimal totalorder , string SupplierName);
    }
}
