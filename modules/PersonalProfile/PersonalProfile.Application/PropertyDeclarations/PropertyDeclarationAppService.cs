using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.Storage.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations
{
    public class PropertyDeclarationAppService : ApplicationService, IPropertyDeclarationAppService
    {
        private readonly IRepository<PropertyDeclaration> _propertyDeclarationRepository;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;

        public PropertyDeclarationAppService(IRepository<PropertyDeclaration> propertyDeclarationRepository, IFileSystemBlobProvider fileSystemBlobProvider)
        {
            _propertyDeclarationRepository = propertyDeclarationRepository;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Create(PropertyDeclarationDto propertyDeclarationDto)
        {
            try
            {
                if(!propertyDeclarationDto.IsExist.HasValue || !Enum.IsDefined(typeof(PropertyDeclarationBoolConst),propertyDeclarationDto?.IsExist))
                {
                    propertyDeclarationDto.IsExist = PropertyDeclarationBoolConst.No;
                }
                var checkExist = await _propertyDeclarationRepository.FirstOrDefaultAsync(x => x.PersonId == propertyDeclarationDto.PersonId && x.Year == propertyDeclarationDto.Year);
                if (checkExist != null)
                {
                    throw new Exception($"Năm {propertyDeclarationDto.Year} đã có trong danh sách kê khai");
                }
                var entity = ObjectMapper.Map<PropertyDeclaration>(propertyDeclarationDto);
                int id = await _propertyDeclarationRepository.InsertAndGetIdAsync(entity);
                return id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Update(PropertyDeclarationDto propertyDeclarationDto)
        {
            try
            {
                var id = propertyDeclarationDto.Id;
                var personId = propertyDeclarationDto.PersonId;
                var check = await _propertyDeclarationRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin kê khai tài sản không đúng!");
                }
                var checkExist = await _propertyDeclarationRepository.FirstOrDefaultAsync(x => x.PersonId == propertyDeclarationDto.PersonId && x.Year == propertyDeclarationDto.Year && x.Id != id);
                if (checkExist != null)
                {
                    throw new Exception($"Năm {propertyDeclarationDto.Year} đã có trong danh sách kê khai");
                }
                if (!propertyDeclarationDto.IsExist.HasValue || !Enum.IsDefined(typeof(PropertyDeclarationBoolConst), propertyDeclarationDto?.IsExist))
                {
                    propertyDeclarationDto.IsExist = PropertyDeclarationBoolConst.No;
                }
                propertyDeclarationDto.FilePath = check.FilePath;
                propertyDeclarationDto.FileUrl = check.FileUrl;
                ObjectMapper.Map(propertyDeclarationDto, check);
                return check.Id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<PropertyDeclarationDto>> GetAll(GetAllFilter filter)
        {
            try
            {
                int personId = filter.PersonId ?? default(int);
                var result = await _propertyDeclarationRepository.GetAllListAsync();
                result = result.WhereIf(filter.PersonId.HasValue, x => x.PersonId == personId).OrderBy(x=>x.Year).ToList();
                List<PropertyDeclarationDto> dtos = new List<PropertyDeclarationDto>();
                ObjectMapper.Map(result, dtos);
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<PropertyDeclarationDto> GetById(int id)
        {
            try
            {
                var entity = await _propertyDeclarationRepository.FirstOrDefaultAsync(x=>x.Id == id);
                if(entity == null)
                {
                    throw new Exception("Thông tin kê khai tài sản không đúng!");
                }
                var result = ObjectMapper.Map<PropertyDeclarationDto>(entity);
                return result;
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeletePropertyDeclarationDto deletePropertyDeclarationDto)
        {
            try
            {
                int id = deletePropertyDeclarationDto.Id;
                int personId = deletePropertyDeclarationDto.PersonId;
                var check = await _propertyDeclarationRepository.FirstOrDefaultAsync(x =>x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin kê khai tài sản không đúng!");
                }
                await _propertyDeclarationRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task UploadFile(UploadFileDto uploadFileDto)
        {
            try
            {
                int id = uploadFileDto.Id;
                var check = await _propertyDeclarationRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (check == null)
                {
                    throw new Exception("Thông tin kê khai tài sản không đúng!");
                }
                if (!string.IsNullOrEmpty(check.FilePath))
                {
                    _ = _fileSystemBlobProvider.DeleteAsync(check.FilePath);
                }
                check.FileUrl = uploadFileDto.FileUrl;
                check.FilePath = uploadFileDto.FilePath;

                await _propertyDeclarationRepository.UpdateAsync(check);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteFile(int id)
        {
            string filePath = "";
            try
            {
                var checkTrainningInfo = await _propertyDeclarationRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (checkTrainningInfo == null)
                {
                    return;
                }
                filePath = checkTrainningInfo.FilePath;
                checkTrainningInfo.FileUrl = string.Empty;
                checkTrainningInfo.FilePath = string.Empty;

                await _propertyDeclarationRepository.UpdateAsync(checkTrainningInfo);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            if (!string.IsNullOrEmpty(filePath))
            {
                _ = _fileSystemBlobProvider.DeleteAsync(filePath);
            }
        }
    }
}
