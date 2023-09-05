using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.mdl.PersonalProfile.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;

namespace bbk.netcore.mdl.OMS.Application.Subsidiaries
{
    public class SubsidiaryService : ApplicationService,  ISubsidiaryService
    {
        private readonly IRepository<Subsidiary, long> _subsidiaryRepository;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        public SubsidiaryService(IRepository<Subsidiary, long> subsidiaryRepository, IFileSystemBlobProvider fileSystemBlobProvider)
        {
            _subsidiaryRepository= subsidiaryRepository;
            _fileSystemBlobProvider= fileSystemBlobProvider;
        }


        /// <summary>
        /// hàm để lấy ra danh sách các bản ghi cua bang đơn vị yêu cầu
        /// createdby: Kiên
        /// </summary>
        /// <param name="input">gia tri de tim kiem</param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<SubsidiaryListDto>> GetAll(SubsidiarySearch input)
        {
            try
            {
                var query = _subsidiaryRepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.NameCompany.Contains(input.SearchTerm))
                      .OrderBy(x => x.Id);
                var subsidiaryCount = query.Count();
                var subsidiarytListDto = ObjectMapper.Map<List<SubsidiaryListDto>>(query);
                return new PagedResultDto<SubsidiaryListDto>(
                  subsidiaryCount,
                  subsidiarytListDto
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<SubsidiaryListDto>> GetAllSub()
        {
            try
            {
                var query = _subsidiaryRepository
                      .GetAll()
                      .OrderBy(x => x.Id);
                var subsidiaryCount = query.Count();
                var subsidiarytListDto = ObjectMapper.Map<List<SubsidiaryListDto>>(query);
                return new PagedResultDto<SubsidiaryListDto>(
                  subsidiaryCount,
                  subsidiarytListDto
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }


        /// <summary>
		/// Hàm để tạo mới bản ghi bảng cua bang đơn vị yêu cầu
		/// created : Kiên
		/// </summary>
		/// <param name="input">Đầu vào là dữ liệu các trường thông tin trong bảng </param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public async Task<long> Create(SubsidiaryCreateDto input)
        {
            try
            {
                Subsidiary newItemId = ObjectMapper.Map<Subsidiary>(input);
                var newId = await _subsidiaryRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// Hàm tạo ra để sửa bản ghi bảng cua bang đơn vị yêu cầu
        /// CreatedBy : Kiên
        /// </summary>
        /// <param name="input">Id bản ghi cần sửa</param>
        /// <returns></returns>
        public async Task<long> Update(SubsidiaryListDto input)
        {
            Subsidiary purchasesRequest = await _subsidiaryRepository.FirstOrDefaultAsync(input.Id);
            ObjectMapper.Map(input, purchasesRequest);
            await _subsidiaryRepository.UpdateAsync(purchasesRequest);
            return input.Id;
        }

        /// <summary>
		/// Hàm lấy Id bản ghi phục vụ cho mục dích sửa hoặc lấy ra bản ghi cần dùng
		/// created : Kiên
		/// </summary>
		/// <param name="itemId">id bản ghi cần lấy </param>
		/// <returns></returns>
		public async Task<SubsidiaryListDto> GetAsync(EntityDto itemId)
        {
            var item = _subsidiaryRepository.Get(itemId.Id);
            SubsidiaryListDto newItem = ObjectMapper.Map<SubsidiaryListDto>(item);
            return newItem;
        }


        /// <summary>
        /// Hàm để xoá bản ghi bảng cua bang đơn vị yêu cầu
        /// createdby : Kiên
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<int> Delete(int id)
        {
            try
            {
                await _subsidiaryRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }


        /// <summary>
        /// Lấy api địa chỉ các tỉnh thành trong file json
        /// created : Kiên
        /// </summary>
        /// <param name="filePath">đường dẫn file</param>
        /// <param name="superiorId">id từng địa chỉ</param>
        /// <returns></returns>
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


        public async Task<List<SubsidiaryListDto>> GetSubsidiaryList()
        {
            try
            {
                var query = _subsidiaryRepository.GetAll().OrderBy(x => x.Id);

                var List = ObjectMapper.Map<List<SubsidiaryListDto>>(query);
                return List;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<SubsidiaryListDto>> GetAllPurchese(SubsidiarySearch input)
        {
            try
            {
                var query = _subsidiaryRepository
                      .GetAll()
                      .Where(x=>x.Id == input.SubsidiaryCompanyId)
                      .OrderBy(x => x.Id);
                var subsidiaryCount = query.Count();
                var subsidiarytListDto = ObjectMapper.Map<List<SubsidiaryListDto>>(query);
                return new PagedResultDto<SubsidiaryListDto>(
                  subsidiaryCount,
                  subsidiarytListDto
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
