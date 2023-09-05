using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears.Dtos;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using Abp.Authorization;
using bbk.netcore.Authorization;

namespace bbk.netcore.mdl.PersonalProfile.Application.AssessedByYears
{
    public class AssessedByYearAppService : ApplicationService, IAssessedByYearAppService
    {
        private readonly IRepository<AssessedByYear> _assessedByYearRepository;

        public AssessedByYearAppService(IRepository<AssessedByYear> assessedByYearRepository)
        {
            _assessedByYearRepository = assessedByYearRepository;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetAssessedByYearDto>> GetAll(AssessFilter filter)
        {
            try
            {
                var AssessedByYear = await _assessedByYearRepository
               .GetAll()
               .Include(x => x.SelfAssessment)
               .Include(x => x.ResultsOfClassification)
               .Where(u => u.PersonId == filter.PersonId && u.AssessedByYearType == filter.Type)
               .OrderByDescending(t => t.Id)
               .ToListAsync();

                List<GetAssessedByYearDto> getAssessedByYearDtos = new List<GetAssessedByYearDto>();
                foreach (var r in AssessedByYear)
                {
                    GetAssessedByYearDto getAssessedByYearDto = new GetAssessedByYearDto();
                    getAssessedByYearDto.Id = r.Id;
                    getAssessedByYearDto.PersonId = r.PersonId;
                    getAssessedByYearDto.Year = r.Year;
                    getAssessedByYearDto.SelfAssessment = r.SelfAssessment.Title;
                    getAssessedByYearDto.AssessmentByLeader = r.AssessmentByLeader;
                    getAssessedByYearDto.CollectiveFeedback = r.CollectiveFeedback;
                    getAssessedByYearDto.EvaluationOfAuthorizedPerson = r.EvaluationOfAuthorizedPerson;
                    getAssessedByYearDto.ResultsOfClassification = r.ResultsOfClassification.Title;
                    getAssessedByYearDto.AssessedByYearType = EnumExtensions.GetDisplayName(r.AssessedByYearType);
                    getAssessedByYearDtos.Add(getAssessedByYearDto);
                }
                return getAssessedByYearDtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
           

        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]

        public async Task Create(AssessedByYearDto input)
        {
            try
            {
                var entity = ObjectMapper.Map<AssessedByYear>(input);
                await _assessedByYearRepository.InsertAndGetIdAsync(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(AssessedByYearDto assessedByYearDto)
        {
            try
            {

                int id = assessedByYearDto.Id;

                var checkAssessedByYear = await _assessedByYearRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (checkAssessedByYear == null)
                {
                    throw new Exception("Chi tiết đánh giá không đúng!");
                }
                ObjectMapper.Map(assessedByYearDto, checkAssessedByYear);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(int Id)
        {
            try
            {
                var assessedByYear =  _assessedByYearRepository.Get(Id);
                if (assessedByYear == null)
                {
                    throw new Exception("ID không tồn tại!");
                }
                await _assessedByYearRepository.DeleteAsync(assessedByYear);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<AssessedByYearDto> GetById(int id)
        {
            try
            {
                var entity = await _assessedByYearRepository.FirstOrDefaultAsync(id);
                if (entity == null)
                {
                    throw new Exception("Thông tin đào tạo không đúng!");
                }
                AssessedByYearDto assessedByYearDto = ObjectMapper.Map<AssessedByYearDto>(entity);
                return assessedByYearDto;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
       
    }
}
