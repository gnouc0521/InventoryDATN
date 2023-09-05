using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears
{
    public interface IAssessedByYearAppService : IApplicationService
    {
        Task<List<GetAssessedByYearDto>> GetAll(AssessFilter filter);
        Task<AssessedByYearDto> GetById(int id);

        Task Create(AssessedByYearDto input);

        Task Update(AssessedByYearDto assessedByYearDto);

        Task DeleteById(int Id);
    }
}
