using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Suppliers
{
    public interface ISupplierAppService : IApplicationService
    {
        Task<int> Create(SupplierCreateDto input);
        Task<PagedResultDto<SupplierListDto>> GetAll(GetSupplierInput input);
        Task<long> Update(SupplierListDto input);
        Task<int> Delete(int id);
        Task<SupplierListDto> GetAsync(EntityDto itemId);
        Task<List<SupplierListDto>> GetSupplierList();
        Task<Address> GetAddress(string filePath, string superiorId);

        Task<SupplierListDto> GetSupByCode(string itemCode);
        Task<List<SupplierListDto>> GetSupplier(GetSupplierInput input);
        Task<PagedResultDto<SupplierListDto>> GetAllSupplier();

    }
}
