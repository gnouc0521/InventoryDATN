using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.CivilServants.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.CivilServants
{
    [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]

    public class CivilServantAppService : ApplicationService, ICivilServantAppService
    {
        private readonly IRepository<CivilServant> _civilServantRepository;
        private readonly IRepository<SalaryLevel> _salaryLevelRepository;

        public CivilServantAppService(IRepository<CivilServant> civilServantRepository, IRepository<SalaryLevel> salaryLevelRepository)
        {
            _civilServantRepository = civilServantRepository;
            _salaryLevelRepository = salaryLevelRepository;
        }
        public async Task<List<CivilServantDto>> GetAllCivilServant()
        {
            var list = await _civilServantRepository.GetAll()
                .OrderBy(u => u.Name)
                .ToListAsync();
            return ObjectMapper.Map<List<CivilServantDto>>(list);
        }
        public async Task<CivilServantDto> GetCivilServant(int id)
        {
            var obj = await _civilServantRepository.GetAsync(id);
            return ObjectMapper.Map<CivilServantDto>(obj);
        }
        public async Task<List<SalaryLevelDto>> GetAllSalaryLevelByGroup(CivilServantGroup group)
        {
            var list = _salaryLevelRepository.GetAll()
                .Where(u => u.Group == group).ToList().ToList()
                .OrderBy(u => Convert.ToInt32(u.Level.Split('/').First()));
            return await Task.FromResult(ObjectMapper.Map<List<SalaryLevelDto>>(list));
        }
        public async Task<SalaryLevelDto> GetSalaryLevel(int id)
        {
            var obj = await _salaryLevelRepository.GetAsync(id);
            return ObjectMapper.Map<SalaryLevelDto>(obj);
        }
        public async Task<List<SalaryLevelDto>> GetAllSalaryLevel()
        {
            var list = await _salaryLevelRepository.GetAll().ToListAsync();
            return ObjectMapper.Map<List<SalaryLevelDto>>(list);
        }
    }
}
