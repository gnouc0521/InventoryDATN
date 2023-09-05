using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.Storage.FileSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos
{
    public class TrainningInfoAppService : ApplicationService, ITrainningInfoAppService
    {
        private readonly IRepository<TrainningInfo> _trainningInfoRepository;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        public TrainningInfoAppService(IRepository<TrainningInfo> trainningInfoRepository, FileSystemBlobProvider fileSystemBlobProvider)
        {
            _trainningInfoRepository = trainningInfoRepository;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Create(TrainningInfoDto trainningInfoDto)
        {
            try
            {
                if(trainningInfoDto.FromDate > trainningInfoDto.ToDate)
                {
                    throw new Exception("Ngày bắt đầu đang muộn hơn ngày kết thúc, nhập lại");
                }
                var entity = ObjectMapper.Map<TrainningInfo>(trainningInfoDto);
                int id = await _trainningInfoRepository.InsertAndGetIdAsync(entity);
                return id;
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Update(TrainningInfoDto trainningInfoDto)
        {
            try
            {
                if (trainningInfoDto.FromDate > trainningInfoDto.ToDate)
                {
                    throw new Exception("Ngày bắt đầu đang muộn hơn ngày kết thúc, nhập lại");
                }
                int id = trainningInfoDto.Id;
                int personId = trainningInfoDto.PersonId;
                var checkTrainningInfo = await _trainningInfoRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (checkTrainningInfo == null)
                {
                    throw new Exception("Chi tiết đào tạo không đúng!");
                }
                trainningInfoDto.FilePath = checkTrainningInfo.FilePath;
                trainningInfoDto.FileUrl = checkTrainningInfo.FileUrl;
                ObjectMapper.Map(trainningInfoDto, checkTrainningInfo);
                return checkTrainningInfo.Id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteTrainningInfoDto deleteTrainningInfoDto)
        {
            try
            {
                int id = deleteTrainningInfoDto.Id;
                int personId = deleteTrainningInfoDto.PersonId;
                var checkTrainningInfo = await _trainningInfoRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if(checkTrainningInfo == null)
                {
                    throw new Exception("Chi tiết đào tạo không đúng!");
                }
                await _trainningInfoRepository.DeleteAsync(id);
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetTrainningInfoDto>> GetAll(GetAllFilter filter)
        {
            try
            {
                int personId = filter.PersonId;
                var result = _trainningInfoRepository.GetAll().Include(x => x.Diploma).Include(x => x.TrainningType).Where(x=>x.PersonId == personId).ToList();
                List<GetTrainningInfoDto> getTrainningInfoDtos = new List<GetTrainningInfoDto>();
                foreach(var r in result)
                {
                    GetTrainningInfoDto getTrainningInfoDto = new GetTrainningInfoDto();
                    getTrainningInfoDto.Id = r.Id;
                    getTrainningInfoDto.PersonId = r.PersonId;
                    getTrainningInfoDto.SchoolName = r.SchoolName;
                    getTrainningInfoDto.MajoringName = r.MajoringName;
                    getTrainningInfoDto.FromDate = r.FromDate;
                    getTrainningInfoDto.ToDate = r.ToDate;
                    getTrainningInfoDto.TrainningType = r.TrainningType.Title;
                    getTrainningInfoDto.Diploma = r.Diploma.Title;
                    getTrainningInfoDto.FilePath = r.FilePath;
                    getTrainningInfoDto.FileUrl = r.FileUrl;
                    getTrainningInfoDtos.Add(getTrainningInfoDto);
                }
                getTrainningInfoDtos = getTrainningInfoDtos.OrderBy(x => x.FromDate).ToList();
                return getTrainningInfoDtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<TrainningInfoDto> GetById(int id)
        {
            try
            {
                var entity = await _trainningInfoRepository.FirstOrDefaultAsync(id);
                if(entity == null)
                {
                    throw new Exception("Thông tin đào tạo không đúng!");
                }
                TrainningInfoDto trainningInfoDto = ObjectMapper.Map<TrainningInfoDto>(entity);
                return trainningInfoDto;
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetTrainningInfoDto>> GetAllList()
        {
            try
            {
                var result = _trainningInfoRepository.GetAll().Include(x => x.Diploma).Include(x => x.TrainningType).ToList();
                List<GetTrainningInfoDto> getTrainningInfoDtos = new List<GetTrainningInfoDto>();
                foreach (var r in result)
                {
                    GetTrainningInfoDto getTrainningInfoDto = new GetTrainningInfoDto();
                    getTrainningInfoDto.Id = r.Id;
                    getTrainningInfoDto.PersonId = r.PersonId;
                    getTrainningInfoDto.SchoolName = r.SchoolName;
                    getTrainningInfoDto.MajoringName = r.MajoringName;
                    getTrainningInfoDto.FromDate = r.FromDate;
                    getTrainningInfoDto.ToDate = r.ToDate;
                    getTrainningInfoDto.TrainningType = r.TrainningType.Title;
                    getTrainningInfoDto.Diploma = r.Diploma.Title;
                    getTrainningInfoDto.FileUrl = r.FileUrl;
                    getTrainningInfoDto.FilePath = r.FilePath;
                    getTrainningInfoDtos.Add(getTrainningInfoDto);
                }
                getTrainningInfoDtos = getTrainningInfoDtos.OrderBy(x => x.FromDate).ToList();
                return getTrainningInfoDtos;
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
                var checkTrainningInfo = await _trainningInfoRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (checkTrainningInfo == null)
                {
                    throw new Exception("Chi tiết đào tạo không đúng!");
                }
                if (!string.IsNullOrEmpty(checkTrainningInfo.FilePath))
                {
                    _fileSystemBlobProvider.DeleteAsync(checkTrainningInfo.FilePath);
                }
                checkTrainningInfo.FileUrl = uploadFileDto.FileUrl;
                checkTrainningInfo.FilePath = uploadFileDto.FilePath;

                await _trainningInfoRepository.UpdateAsync(checkTrainningInfo);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task DeleteDiplomaFile(int id)
        {
            string filePath = "";
            try
            {
                var checkTrainningInfo = await _trainningInfoRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (checkTrainningInfo == null)
                {
                    return;
                }
                filePath = checkTrainningInfo.FilePath;
                checkTrainningInfo.FileUrl = string.Empty;
                checkTrainningInfo.FilePath = string.Empty;

                await _trainningInfoRepository.UpdateAsync(checkTrainningInfo);
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
