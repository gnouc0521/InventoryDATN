using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.UploadFileProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.UploadFileProfiles
{
    public interface IUploadFileProfileAppService : IApplicationService
    {
        Task<List<UploadFileProfileDto>> GetFile(int personId);
        Task AddFile(UploadFileProfileDto uploadFileProfileDto);
 
        Task<UploadFile> GetFileEdit(int id);
        Task<UploadFile> CheckFile(int PesonaId, int Id);
        Task UpdateFile(UploadFile uploadFile);
    }
}
