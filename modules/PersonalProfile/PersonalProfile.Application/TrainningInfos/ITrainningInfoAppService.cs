using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.PersonalProfiles.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.TrainningInfos
{
    public interface ITrainningInfoAppService : IApplicationService
    { 
        Task<int> Create(TrainningInfoDto trainningInfoDto);

        Task<int> Update(TrainningInfoDto trainningInfoDto);

        Task DeleteById(DeleteTrainningInfoDto deleteTrainningInfoDto);

        Task UploadFile(UploadFileDto uploadFileDto);

        Task DeleteDiplomaFile(int id);

        Task<List<GetTrainningInfoDto>> GetAll(GetAllFilter filter);

        Task<List<GetTrainningInfoDto>> GetAllList();

        Task<TrainningInfoDto> GetById(int id);
    }
}
