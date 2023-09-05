using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.DayOffs.Dto;
using bbk.netcore.mdl.OMS.Application.UploadFileCVs.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.UploadFileCVs
{
    public class UploadFileCVService :ApplicationService , IUploadFileCVService
    {
        private readonly IRepository<UploadFileCV> _uploadFileRepository;
        private readonly IRepository<Work> _workRepository;
        private readonly IUserAppService _userAppService;
        public UploadFileCVService(IRepository<UploadFileCV> uploadFileRepository, IRepository<Work> workRepository, IUserAppService userAppService)
        {
            _uploadFileRepository = uploadFileRepository;
            _workRepository = workRepository;
            _userAppService = userAppService;
        }


        public async Task<int> CreateUploadFile(UploadFileListDto input)
        {
            //Map theo Dto
            UploadFileCV newItemId = ObjectMapper.Map<UploadFileCV>(input);
            newItemId = await _uploadFileRepository.InsertAsync(newItemId);

            return newItemId.Id;
        }

        public async Task<string> DeleteUploadFile(DelUploadFileDto input)
        {
            var getWorkId = _workRepository.GetAll().ToList();
            var getAllUpload = _uploadFileRepository.GetAll().ToList();
            GetUsersInput getUsersInput = new GetUsersInput();
            var querya = _userAppService.GetUsers(getUsersInput).Result;
            string fullname = "";
            for (int i = 0; i < querya.TotalCount; i++)
            {
                foreach (var item in getWorkId)
                {
                    if (querya.Items[i].Id == item.CreatorUserId)
                    {
                        fullname = querya.Items[i].Name + " " + querya.Items[i].Surname;
                        break;
                    }
                }

            }
            var query = (from upload in getAllUpload
                         join work in getWorkId on upload.WorkId equals work.Id
                         where upload.WorkId == work.Id 
                         orderby upload.CreationTime
                         select new DelUploadFileDto()
                         {
                             Id = upload.Id,
                             WorkId = work.Id,
                             FileName = upload.FileName,
                             CreateBy = fullname,
                             CreateTime = upload.CreationTime,
                             FilePath = upload.FilePath,
                             FileUrl = upload.FileUrl
                         });

            var results = query.ToList();
            if (results != null)
            {
                foreach(var item in results)
                {
                    if(input.FileName == item.FileName)
                    {
                        var AttactItem = await _uploadFileRepository.GetAsync(item.Id);
                        await _uploadFileRepository.HardDeleteAsync(AttactItem);
                    }
                    
                }
            }

            return input.FileName;
        }

        public async Task<PagedResultDto<UploadFileListDto>> GetAllById(UploadFileListDto input)
        {
            try
            {
                var getWorkId = _workRepository.GetAll().Where(x=>x.Id == input.WorkId).ToList();
                var getAllUpload = _uploadFileRepository.GetAll().ToList();
                GetUsersInput getUsersInput = new GetUsersInput();
                var querya = _userAppService.GetUsers(getUsersInput).Result;
                string fullname = "";
                for (int i = 0; i < querya.TotalCount; i++)
                {
                    foreach(var item in getWorkId)
                    {
                        if (querya.Items[i].Id == item.CreatorUserId)
                        {
                            fullname = querya.Items[i].Name + " " + querya.Items[i].Surname;
                            break;
                        }
                    }
                   
                }
                var query = (from upload in getAllUpload
                             join work in getWorkId on upload.WorkId equals work.Id
                             where upload.WorkId == work.Id
                             orderby upload.CreationTime
                             select new UploadFileListDto()
                             {
                                WorkId = work.Id,
                                FileName = upload.FileName,
                                CreateBy = fullname,
                                CreateTime = upload.CreationTime,
                                FilePath = upload.FilePath,
                                FileUrl = upload.FileUrl
                             });

                var results = query.ToList();

                return new PagedResultDto<UploadFileListDto>(
                    results.Distinct().Count(),
                    results.Distinct().ToList()
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

    }
}
