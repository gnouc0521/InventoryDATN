using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.UploadFileProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.Storage.FileSystem;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.UploadFileProfiles
{
    public class UploadFileProfileAppService : ApplicationService, IUploadFileProfileAppService
    {
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        private readonly IRepository<bbk.netcore.mdl.PersonalProfile.Core.Entities.UploadFile> _uploadfileRepository;
        public UploadFileProfileAppService(FileSystemBlobProvider fileSystemBlobProvider, IRepository<bbk.netcore.mdl.PersonalProfile.Core.Entities.UploadFile> uploadfileRepository)
        {
            _fileSystemBlobProvider = fileSystemBlobProvider;
            _uploadfileRepository = uploadfileRepository;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<UploadFileProfileDto>> GetFile(int personId)
        {
            try
            {
                var list = await _uploadfileRepository.GetAll().Where(n=>n.PersonId==personId).OrderByDescending(n => n.CreationTime).ToListAsync();
                return new List<UploadFileProfileDto>(ObjectMapper.Map<List<UploadFileProfileDto>>(list));
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task AddFile(UploadFileProfileDto uploadFileProfileDto)
        {
            try
            {
                var check = await _uploadfileRepository.GetAll().Where(n => n.PersonId == uploadFileProfileDto.PersonId && n.Title.ToLower() == uploadFileProfileDto.Title.ToLower()).ToListAsync();
                if (check.Count > 0)
                {
                    throw new UserFriendlyException("Tên tài liệu đã tồn tại!");

                }
                var entity = ObjectMapper.Map<UploadFile>(uploadFileProfileDto);
                await _uploadfileRepository.InsertAsync(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
           
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteFileDto deleteFileDto)
        {
            try
            {
                int id = deleteFileDto.Id;
                int personId = deleteFileDto.PersonId;
                if (deleteFileDto.Id != null && deleteFileDto.PersonId != null)
                {
                    var check = await _uploadfileRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                    if (check != null)
                    {
                        string filepath = check.FilePath;
                        await _uploadfileRepository.DeleteAsync(id);
                        await _fileSystemBlobProvider.DeleteAsync(filepath);
                        return;
                    }
                    else
                        throw new UserFriendlyException("Xóa file thất bại!");
                }
                throw new UserFriendlyException("Xóa file thất bại!");
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }


        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task UpdateFile(UploadFile uploadFile)
        {
            try
            {
                var check = await _uploadfileRepository.GetAll().Where(n => n.PersonId == uploadFile.PersonId && n.Title.ToLower() == uploadFile.Title.ToLower()).ToListAsync();
                if (check.Count > 0)
                {
                    throw new UserFriendlyException("Tên tài liệu đã tồn tại!");

                }
                await _uploadfileRepository.UpdateAsync(uploadFile);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
           
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<UploadFile> CheckFile(int PersonalId, int Id)
        {
            try
            {
                var entity = await _uploadfileRepository.FirstOrDefaultAsync(x => x.PersonId == PersonalId && x.Id == Id);
                return entity;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
            
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<UploadFile> GetFileEdit(int id)
        {
            try
            {
                var file = await _uploadfileRepository.FirstOrDefaultAsync(n => n.Id == id);
                if (file == null)
                {
                    throw new UserFriendlyException("File sửa không tồn tại!");
                }
                return file;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }

        }
    }

}
     