using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto;
using bbk.netcore.mdl.OMS.Application.UploadFileCVs;
using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ProfileWorks
{
    [AbpAuthorize]
    public class ProfileWorkAppService : ApplicationService , IProfileWorkAppService
    {
        private readonly IRepository<ProfileWork> _profileWorkRepository;
        private readonly IRepository<Work> _workRepository;
        private readonly IRepository<UserWork> _userWorkrepository;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<UploadFileCV> _uploadFileCVRepository; 

        public ProfileWorkAppService(IRepository<ProfileWork> profileWorkRepository, IRepository<Work> workRepository, IRepository<UserWork> userWorkrepository, 
            IUserAppService userAppService, IRepository<UploadFileCV> uploadFileCVRepository)
        {
            _profileWorkRepository = profileWorkRepository;
            _workRepository = workRepository;
            _userWorkrepository = userWorkrepository;
            _userAppService = userAppService;
            _uploadFileCVRepository = uploadFileCVRepository;
        }

        public async Task<PagedResultDto<ProfileWorkListDto>> GetAll(GetProfileWorkInput input)
        {
            try
            {
                var query = _profileWorkRepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Title.Contains(input.SearchTerm) || u.Title.Contains(input.SearchTerm))
                      .WhereIf(input.IdProfileWork != null, u => u.Id.Equals(input.IdProfileWork))
                      .OrderBy(x => x.Order);

                if (input.IdProfileWork == null)
                {
                    var queryRoot = query.Where(x => x.ParentId == null);
                    var worksCount = await queryRoot.CountAsync();
                    var ProfileWorkListDto = ObjectMapper.Map<List<ProfileWorkListDto>>(queryRoot);
                    return new PagedResultDto<ProfileWorkListDto>(
                      worksCount,
                      ProfileWorkListDto
                      );
                }
                else
                {
                    var test = query.Where(x => x.ParentId == null);
                    var isParentId = from c in test
                                     select c.Id;
                    if (test.Count() == 0)
                    {
                        var worksCount = query.Count();
                        var ProfileWorkListDto = ObjectMapper.Map<List<ProfileWorkListDto>>(query);
                        return new PagedResultDto<ProfileWorkListDto>(
                          worksCount,
                          ProfileWorkListDto
                          );
                    }
                    else
                    {
                        var query2 = _profileWorkRepository.GetAll().Where(x => x.ParentId == isParentId.ToArray()[0]);

                        var worksCount = query2.Count();
                        var ProfileWorkListDto = ObjectMapper.Map<List<ProfileWorkListDto>>(query2);
                        return new PagedResultDto<ProfileWorkListDto>(
                          worksCount,
                          ProfileWorkListDto
                          );
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ProfileWorkListDto>> GetAllList()
        {
            try
            {
                var query = _profileWorkRepository
                     .GetAll();
                var worksCount = query.Count();
                var ProfileWorkListDto = ObjectMapper.Map<List<ProfileWorkListDto>>(query);
                return new PagedResultDto<ProfileWorkListDto>(
                  worksCount,
                  ProfileWorkListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<ProfileWorkListDto>> GetAllListParent()
        {
            try
            {
                var query = _profileWorkRepository.GetAll().Where(x => x.ParentId == null);
                var query1 = _profileWorkRepository.GetAll().Where(x => x.ParentId != null);

                var query2 = (from i in query
                              let count = (from z in query1 where i.Id == z.ParentId select z.Id).Count()
                              select new ProfileWorkListDto
                              {
                                  Id = i.Id,
                                  Title = i.Title,
                                  Order = i.Order,
                                  NumItemsChild = count,
                                  ParentId = i.ParentId,
                                  WorkProfileLevel = i.WorkProfileLevel,
                              }).ToList();

                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<ProfileWorkListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ProfileWorkListDto>> GetAllListItem(int id)
        {
            try
            {
                var query = _profileWorkRepository.GetAll().Where(x => x.ParentId != null && x.ParentId == id);
                var query1 = _profileWorkRepository.GetAll().Where(x => x.ParentId != null);

                var query2 = (from i in query
                              let count = (from z in query1 where i.Id == z.ParentId select z.Id).Count()
                              select new ProfileWorkListDto
                              {
                                  Id = i.Id,
                                  Title = i.Title,
                                  Order = i.Order,
                                  NumItemsChild = count,
                                  ParentId = i.ParentId,
                                  WorkProfileLevel= i.WorkProfileLevel,
                              }).ToList();


                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<ProfileWorkListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
      


        public async Task<PagedResultDto<ProfileWorkListDto>> GetAsync(EntityDto itemId)
        {
            var item = _profileWorkRepository.GetAll().Where(x => x.Id == itemId.Id);
            var newItem = ObjectMapper.Map<List<ProfileWorkListDto>>(item);
            return new PagedResultDto<ProfileWorkListDto>(
                  newItem.Count(),
                  newItem
                  );
        }


        public async Task<int> Create(ProfileWorkCreateDto input)
        {
            try
            {
                ProfileWork newItemId = ObjectMapper.Map<ProfileWork>(input);
                var newId = await _profileWorkRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<long> Update(ProfileWorkEditDto input)
        {
            ProfileWork lecture = _profileWorkRepository.Get(((int)input.Id));
            ObjectMapper.Map(input, lecture);
            await _profileWorkRepository.UpdateAsync(lecture);
            return input.Id;
        }

        //Code Hà
        public async Task<PagedResultDto<WorkListDto>> GetAllWorkByProfileWorkId(GetProfileWorkInput input)
        {
            try
            {
                var query = _workRepository.GetAll().Where(x => x.IdProfileWork == input.IdProfileWork).WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Title.ToLower().Contains(input.SearchTerm.ToLower()));
                var fileList1 = _uploadFileCVRepository.GetAll();
                var fileList = _uploadFileCVRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.FileName.ToLower().Contains(input.SearchTerm.ToLower()));


                GetUsersInput getUsersInput = new GetUsersInput();
                var querya = _userAppService.GetUsers(getUsersInput).Result;
                string fullname = "";
                for (int i = 0; i < querya.TotalCount; i++)
                {
                    foreach (var item in query)
                    {
                        if (querya.Items[i].Id == item.HostId)
                        {
                            fullname = querya.Items[i].Surname + " " + querya.Items[i].Name;
                            break;
                        }
                    }

                }

                var query1 = (from work in query
                              join file in fileList on work.Id equals file.WorkId into ps
                              from ps2 in ps.DefaultIfEmpty()
                              let count = (from file in fileList1 where work.Id == file.WorkId select file.FileUrl).Count()
                              select new WorkListDto()
                              {
                                  Id = work.Id,
                                  Title = work.Title,
                                  Description = work.Description,
                                  StartDate = work.StartDate,
                                  EndDate = work.EndDate,
                                  CompletionTime = work.CompletionTime,
                                  Status = work.Status,
                                  priority = work.priority,
                                  HostName = fullname,
                                  jobLabel = work.jobLabel,
                                  fileNum = count == null ? 0 : count,
                                  HostId = work.HostId,

                              }).OrderByDescending(x => x.Id);

                return new PagedResultDto<WorkListDto>(
                        query1.Distinct().Count(),
                        query1.Distinct().ToList()
                        );

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkListDto>> GetAllWorkByProfileWorkIdInChart(GetProfileWorkInput input)
        {
            try
            {
                var query = _workRepository.GetAll().Where(x => x.IdProfileWork == input.IdProfileWork);
                var userWorks = _userWorkrepository.GetAll();

                var query1 = (from work in query
                              join userWork in userWorks on work.Id equals userWork.WorkId
                              select new WorkListDto()
                              {
                                  Id = work.Id,
                                  Title = work.Title,
                                  Description = work.Description,
                                  StartDate = work.StartDate,
                                  EndDate = work.EndDate,
                                  CompletionTime = work.CompletionTime,
                                  Status = work.Status,
                                  priority = work.priority,
                                  jobLabel = work.jobLabel,
                                  OwnerStatusEnum = userWork.OwnerStatus,
                                  UserId = userWork.UserId,
                                  FilePath = work.FilePath,
                                  FileUrl = work.FileUrl,
                                  HostId = work.HostId,

                              });

                var results = query1.ToList();

                return new PagedResultDto<WorkListDto>(
                    query1.Distinct().Count(),
                    query1.Distinct().ToList()
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkListDto>> GetAllFileByProfileWorkId(EntityDto itemId)
        {
            try
            {
                var query = _workRepository.GetAll().Where(x => x.IdProfileWork == itemId.Id && x.FileUrl != null);
                //var userWorks = _userWorkrepository.GetAll();

                //var query1 = (from work in query
                //              join userWork in userWorks on work.Id equals userWork.WorkId
                //              select new WorkListDto()
                //              {
                //                  Id = work.Id,
                //                  Title = work.Title,
                //                  Description = work.Description,
                //                  StartDate = work.StartDate,
                //                  EndDate = work.EndDate,
                //                  CompletionTime = work.CompletionTime,
                //                  Status = work.Status,
                //                  priority = work.priority,
                //                  jobLabel = work.jobLabel,
                //                  OwnerStatusEnum = userWork.OwnerStatus,
                //                  UserId = userWork.UserId,
                //                  FilePath = work.FilePath,
                //                  FileUrl = work.FileUrl,
                //                  HostId = work.HostId,

                //              });

                //var results = query1.Where(x => x.FileUrl != null);
                var result = ObjectMapper.Map<List<WorkListDto>>(query);

                return new PagedResultDto<WorkListDto>(
                    result.Count(),
                    result.ToList()
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _profileWorkRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
