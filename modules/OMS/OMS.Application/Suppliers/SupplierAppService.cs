using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using bbk.netcore.mdl.OMS.Application.Producers;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.mdl.PersonalProfile.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;

namespace bbk.netcore.mdl.OMS.Application.Suppliers
{
    public class SupplierAppService : ApplicationService, ISupplierAppService
    {
        private readonly IRepository<Supplier> _supplierrepository;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        public SupplierAppService(IRepository<Supplier> supplierrepository , IFileSystemBlobProvider fileSystemBlobProvider)
        {
            _supplierrepository = supplierrepository;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        public async Task<int> Create(SupplierCreateDto input)
        {
            try
            {
                string sinhma(string ma)
                {
                    string s = ma.Substring(4, ma.Length - 4);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "NCC-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "NCC-" + "0" + Convert.ToString(i);
                    else return "NCC-" + Convert.ToString(i);

                }
                string ma;
                var query = await _supplierrepository.GetAll().ToListAsync();
                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _supplierrepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();


                }



                input.Code = sinhma(ma.ToString());
                Supplier newItemId = ObjectMapper.Map<Supplier>(input);
                var newId = await _supplierrepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<SupplierListDto>> GetAll(GetSupplierInput input)
        {
            try
            {
                var query = _supplierrepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Name.Contains(input.SearchTerm))
                      .OrderBy(x => x.Id);
                var ProducerCount = query.Count();


                var ProducerListDto = ObjectMapper.Map<List<SupplierListDto>>(query);
                return new PagedResultDto<SupplierListDto>(
                  ProducerCount,
                  ProducerListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<long> Update(SupplierListDto input)
        {
            Supplier supplier = await _supplierrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.Code = supplier.Code;
            ObjectMapper.Map(input, supplier);
            await _supplierrepository.UpdateAsync(supplier);
            return input.Id;
        }

        public async Task<Address> GetAddress(string filePath, string superiorId)
        {
            string file = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.DataAddress + @"\\" + filePath));
            var data = ReadJson<Address>.ConvertJsonToObject(file);
            Address result = new Address
            {
                Addresses = data.Addresses.WhereIf(!string.IsNullOrEmpty(superiorId), u => u.SuperiorId == superiorId).OrderBy(u => u.Name).ToList()
            };
            return await Task.FromResult(result);
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _supplierrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<SupplierListDto> GetAsync(EntityDto itemId)
        {
            var item = _supplierrepository.Get(itemId.Id);
            SupplierListDto newItem = ObjectMapper.Map<SupplierListDto>(item);
            return newItem;
        }

        public async Task<List<SupplierListDto>> GetSupplierList()
        {
            try
            {
                var query = _supplierrepository.GetAll().OrderBy(x => x.Id);

                var supplierList = ObjectMapper.Map<List<SupplierListDto>>(query);
                return supplierList;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<SupplierListDto>> GetSupplier(GetSupplierInput input)
        {
            try
            {
                var query = _supplierrepository.GetAll().Where(x=>x.Id == input.supplierId);

                var supplierList = ObjectMapper.Map<List<SupplierListDto>>(query);
                return supplierList;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<SupplierListDto> GetSupByCode(string itemCode)
        {
            var item = _supplierrepository.GetAll().Where(x => x.Code.Contains(itemCode.Trim())).ToList();
            var newItem = ObjectMapper.Map<SupplierListDto>(item[0]);
            return newItem;
        }

        public async Task<PagedResultDto<SupplierListDto>> GetAllSupplier()
        {
            try
            {
                var query = _supplierrepository
                      .GetAll()
                      .OrderBy(x => x.Id);
                var ProducerCount = query.Count();


                var ProducerListDto = ObjectMapper.Map<List<SupplierListDto>>(query);
                return new PagedResultDto<SupplierListDto>(
                  ProducerCount,
                  ProducerListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
