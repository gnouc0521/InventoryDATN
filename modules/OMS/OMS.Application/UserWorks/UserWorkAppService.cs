using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.DayOffs.Dto;
using bbk.netcore.mdl.OMS.Application.UserWorks.Dto;
using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WorkUsers
{
    [AbpAuthorize]
    public class UserWorkAppService : ApplicationService, IUserWorkAppService
    {
        //private readonly IRepository<WorkUser> _repository;
        private readonly IRepository<UserWork> _userWorkrepository;

        private readonly IRepository<Work> _workrepository;

        public UserWorkAppService(IRepository<UserWork> userWorkrepository, IRepository<Work> workrepository)
        {
            _userWorkrepository = userWorkrepository;
            _workrepository = workrepository;
        }


        public async Task<int> Create(UserWorkCreateDto input)
        {
            try
            {
                UserWork newItemId = ObjectMapper.Map<UserWork>(input);
                Work work = new Work();
                work.Id = newItemId.WorkId;
                work.Status = newItemId.Status;
                var newId = await _userWorkrepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<UserWorkListDto>> GetAll(GetUserWorkInput input)
        {
            try
            {
                var query = _userWorkrepository
                      .GetAll()
                      .OrderBy(x => x.Id);
                var worksCount = query.Count();


                var WorkListDto = ObjectMapper.Map<List<UserWorkListDto>>(query);
                return new PagedResultDto<UserWorkListDto>(
                  worksCount,
                  WorkListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        //public async Task<PagedResultDto<UserWorkListDto>> GetAllByWork()
        //{
        //    try
        //    {
        //        var query = _userWorkrepository.GetAll();
        //        var work = _workrepository.GetAll();
        //        var worksCount = query.Count();
        //        var WorkListDto = ObjectMapper.Map<List<UserWorkListDto>>(query);
        //        var userWork = ObjectMapper.Map<List<WorkListDto>>(work);

        //        var query1 = (from c in WorkListDto
        //                      join d in work on c.WorkId equals d.Id
        //                      select new UserWorkListDto
        //                      {
        //                          WorkId = c.WorkId,

        //                      }).ToList();

        //        return new PagedResultDto<UserWorkListDto>(
        //          worksCount,
        //          WorkListDto
        //          );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}



        public async Task<int> Delete(int userId)
        {
            try
            {
                var query = _userWorkrepository.GetAll().Where(x => x.UserId == userId);
                var idUserword = from c in query
                                 select c.Id;

                await _userWorkrepository.DeleteAsync(idUserword.ToArray()[0]);
                return idUserword.ToArray()[0];
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        //code ha
        public async Task<PagedResultDto<UserWorkListDto>> GetUserIdByWorkId(int workId)
        {
            try
            {
                var query = _userWorkrepository.GetAll().Where(x => x.WorkId == workId);
                var conut = await query.CountAsync();
                var WorkListDto = ObjectMapper.Map<List<UserWorkListDto>>(query);

                return new PagedResultDto<UserWorkListDto>(
                    conut, WorkListDto);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            
        }
    }
}
