using Abp.Application.Features;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.DocumentEnum;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.OMS.Application.Works
{
    [AbpAuthorize]
    public class WorkAppService : ApplicationService, IWorkAppService
    {
        private readonly IRepository<Work> _workRepository;
        private readonly IRepository<DayOff> _dayOffRepository;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<UserWork> _userWorkrepository;
        private readonly IRepository<UploadFileCV> _uploadFileRepository;

        public WorkAppService(IRepository<Work> workRepository, IRepository<DayOff> dayOffRepository, IUserAppService userAppService, IRepository<UserWork> userWorkrepository, IRepository<UploadFileCV> uploadFileRepository)
        {
            _workRepository = workRepository;
            _dayOffRepository = dayOffRepository;
            _userAppService = userAppService;
            _userWorkrepository = userWorkrepository;
            _uploadFileRepository = uploadFileRepository;


        }

        public async Task<PagedResultDto<WorkListDto>> GetAll(GetWorkInput input)
        {
            try
            {
                var userWorks = _userWorkrepository.GetAll();
                var works = _workRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Title.Contains(input.SearchTerm));
                var query = (from userWork in userWorks
                             join work in works on userWork.WorkId equals work.Id
                             where userWork.UserId == AbpSession.UserId
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

                var results = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                return new PagedResultDto<WorkListDto>(
                    results.Distinct().Count(),
                    results.Distinct().ToList()
                    );
            }


            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkListDto>> GetAllByCoWork()
        {

            var userWorks = _userWorkrepository.GetAll();
            var works = _workRepository.GetAll();
            var query = (from userWork in userWorks
                         join work in works on userWork.WorkId equals work.Id
                         where userWork.UserId == AbpSession.UserId
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

            var results = query.ToList();

            return new PagedResultDto<WorkListDto>(
                query.Distinct().Count(),
                query.Distinct().ToList()
                );
        }


        public async Task<WorkEditDto> GetAsync(EntityDto itemId)
        {
            var item = _workRepository.Get(itemId.Id);
            WorkEditDto newItem = ObjectMapper.Map<WorkEditDto>(item);
            return newItem;
        }

        public async Task<long> Create(WorkCreateDto input)
        {
            try
            {
                // TODO: Check start&end out of dayofflist
                Work newItemId = ObjectMapper.Map<Work>(input);
                var newId = await _workRepository.InsertAndGetIdAsync(newItemId);
                //var newItem = await _workRepository.InsertAsync(newItemId);

                if (newItemId.HostId == AbpSession.UserId)
                {
                    // Create particular user
                    UserWork userWork = new UserWork();
                    userWork.WorkId = newId;
                    //userWork.WorkId = newItem.Id;
                    userWork.Status = newItemId.Status;
                    userWork.UserId = newItemId.HostId;
                    userWork.OwnerStatus = WorkEnum.OwnerStatusEnum.Host;
                    await _userWorkrepository.InsertAsync(userWork);
                }
                else
                {
                    // Create particular user
                    UserWork userWork = new UserWork();
                    userWork.WorkId = newId;
                    //userWork.WorkId = newItem.Id;
                    userWork.Status = newItemId.Status;
                    userWork.UserId = newItemId.HostId;
                    userWork.OwnerStatus = WorkEnum.OwnerStatusEnum.Host;
                    await _userWorkrepository.InsertAsync(userWork);

                    UserWork userAssignWork = new UserWork();
                    userAssignWork.WorkId = newId;
                    //userAssignWork.WorkId = newItem.Id;
                    userAssignWork.Status = newItemId.Status;
                    userAssignWork.UserId = newItemId.CreatorUserId.Value;
                    userAssignWork.OwnerStatus = WorkEnum.OwnerStatusEnum.Assign;
                    await _userWorkrepository.InsertAsync(userAssignWork);
                }


             
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public bool CheckDayOff(DateTime startDate, DateTime endDate)
        {

            var dayOffs = _dayOffRepository.GetAll().ToList();

            foreach (var dayoff in dayOffs)
            {
                if (dayoff.StartDate == startDate || dayoff.EndDate == startDate)
                {
                    return false;
                }
                else if (startDate >= dayoff.StartDate && endDate <= dayoff.EndDate)
                {
                    return false;
                }
            }
            return true;
        }


        public bool CheckDayOffStart(DateTime startDate)
        {
            var dayOffs = _dayOffRepository.GetAll().ToList();
            foreach (var dayoff in dayOffs)
            {
                if (dayoff.StartDate == startDate || dayoff.EndDate == startDate)
                {
                    return false;
                }
                else if (startDate >= dayoff.StartDate && startDate <= dayoff.EndDate)
                {
                    
                    return false;
                }
            }
            return true;
        }


        public async Task<long?> Update(WorkEditDto input)
        {
            Work lecture = _workRepository.Get(((int)input.Id));
            ObjectMapper.Map(input, lecture);
            var changstatus2 = _userWorkrepository.GetAll().Where(x => x.WorkId == lecture.Id);
            foreach (var item in changstatus2)
            {
                item.Status = lecture.Status;
                await _userWorkrepository.UpdateAsync(item);
            }
           if(lecture.Status == WorkEnum.Status.Done)
            {
                lecture.CompletionTime = DateTime.Now;
            }    
            await _workRepository.UpdateAsync(lecture);
            return input.Id;
        }

        public async Task<long> Changstatus(WorkListDto input)
        {
            var changstatus = _workRepository.Get(((int)input.Id));
            changstatus.Status = WorkEnum.Status.Done;
            changstatus.CompletionTime = DateTime.Now;
            var changstatus2 = _userWorkrepository.GetAll().Where(x=>x.WorkId == changstatus.Id);
            foreach(var item in changstatus2)
            {
                item.Status = changstatus.Status;
                await _userWorkrepository.UpdateAsync(item);
            }
           
            return changstatus.Id;
        }




        public async Task<WorkListDto> GetById(long id)
        {
            try
            {
                var entity = await _workRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                {
                    throw new Exception("Not exist");
                }
                return ObjectMapper.Map<WorkListDto>(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

   
        //Code Hà thêm

        public async Task<PagedResultDto<UsersListDto>> GetAlUsers()
        {
            try
            {
                //Get all recode theo Session Id trong bảng ApprovalTask

                var sessionid = AbpSession.UserId;
                GetUsersInput getUsersInput = new GetUsersInput();
                var querya = _userAppService.GetUsers(getUsersInput).Result;
                int id = 0;
                string username = "";
                string fullname = "";
                string email = "";
                List<UsersListDto> usersListDtos = new List<UsersListDto>((querya.TotalCount) - 1);
                for (int i = 0; i < querya.TotalCount; i++)
                {
                    if (querya.Items[i].Id == sessionid)
                    {
                        continue;
                    }
                    else
                    {
                        UsersListDto usersListdata = new UsersListDto();
                        id = (Int32)querya.Items[i].Id;
                        username = querya.Items[i].UserName;
                        fullname = querya.Items[i].Name + " " + querya.Items[i].Surname;
                        email = querya.Items[i].EmailAddress;

                        usersListdata.Id = id;
                        usersListdata.UserName = username;
                        usersListdata.FullName = fullname;
                        usersListdata.Email = email;

                        usersListDtos.Add(usersListdata);
                    }
                }
                var offerCount = usersListDtos.Count();

                return new PagedResultDto<UsersListDto>(offerCount, usersListDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkListDto>> GetAllByStatus(GetWorkListNumOfType StatusId)
        {
            try
            {
                var requestStatus = (WorkEnum.Status)StatusId.Statustest;
                var userWorks = _userWorkrepository.GetAll();
                var works = _workRepository.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(StatusId.SearchTerm), u => u.Title.Contains(StatusId.SearchTerm))
                    .WhereIf(!string.IsNullOrEmpty(StatusId.DateStart), x => x.StartDate >= DateTime.Parse(StatusId.DateStart) && x.EndDate <= DateTime.Parse(StatusId.DateEnd))
                    .WhereIf(StatusId.priority != null, u => u.priority.Equals(StatusId.priority));
                var query = (from userWork in userWorks
                             join work in works on userWork.WorkId equals work.Id
                             where userWork.UserId == AbpSession.UserId && userWork.Status == requestStatus
                             orderby work.Id descending
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

                var results = query.ToList();
                return new PagedResultDto<WorkListDto>(
                    results.Distinct().Count(),
                    results.Distinct().ToList()
                    );
            }


            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<WorkListDto>> GetAllByTime(GetWorkListNumInput input)
        {
            if(input.fromDate == null && input.toDate == null){
                DateTime toTime = DateTime.Now;
                DateTime fromTime = toTime.AddMonths(-3);

                var userWorks = _userWorkrepository.GetAll();
                var works = _workRepository.GetAll().Where(x => x.StartDate > fromTime && x.StartDate < toTime);
                var query = (from userWork in userWorks
                             join work in works on userWork.WorkId equals work.Id
                             where userWork.UserId == AbpSession.UserId
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

                var results = query.ToList();

                return new PagedResultDto<WorkListDto>(
                    query.Distinct().Count(),
                    query.Distinct().ToList()
                    );

            }
            else
            {
                var userWorks = _userWorkrepository.GetAll();
                var works = _workRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.fromDate), x => x.StartDate > DateTime.Parse(input.fromDate) && x.EndDate < DateTime.Parse(input.toDate));
                var query = (from userWork in userWorks
                             join work in works on userWork.WorkId equals work.Id
                             where userWork.UserId == AbpSession.UserId
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

                var results = query.ToList();

                return new PagedResultDto<WorkListDto>(
                    query.Distinct().Count(),
                    query.Distinct().ToList()
                    );
            }
           
        }

        public async Task<PagedResultDto<WorkListDto>> GetAllByTimeInBox()
        {
            DateTime toTime = DateTime.Now;
            DateTime fromTime = toTime.AddMonths(-3);

            var userWorks = _userWorkrepository.GetAll();
            var works = _workRepository.GetAll().Where(x => x.StartDate > fromTime && x.StartDate < toTime);
            var query = (from userWork in userWorks
                         join work in works on userWork.WorkId equals work.Id
                         where userWork.UserId == AbpSession.UserId
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

            var results = query.ToList();

            return new PagedResultDto<WorkListDto>(
                query.Distinct().Count(),
                query.Distinct().ToList()
                );
        }

        public async Task<PagedResultDto<WorkListDto>> GetAllInDashBoard(GetWorkListNumInput input)
        {
            if (input.NumInTime == 0)
            {
                DateTime toTime = DateTime.Now;
                DateTime fromTime = toTime.AddMonths(-3);
                //mac dinh 3 thang trk

                var userWorks = _userWorkrepository.GetAll();
                var works = _workRepository.GetAll().Where(x => x.StartDate > fromTime && x.StartDate < toTime);
                var query = (from userWork in userWorks
                             join work in works on userWork.WorkId equals work.Id
                             where userWork.UserId == AbpSession.UserId
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

                var results = query.ToList();

                return new PagedResultDto<WorkListDto>(
                    query.Distinct().Count(),
                    query.Distinct().ToList()
                    );

            }
            else
            {
                if (input.NumInTime == 1)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-1);
                    // 1 thang trk
                    var userWorks = _userWorkrepository.GetAll();
                    var works = _workRepository.GetAll().Where(x => x.StartDate > fromTime && x.StartDate < toTime);
                    var query = (from userWork in userWorks
                                 join work in works on userWork.WorkId equals work.Id
                                 where userWork.UserId == AbpSession.UserId
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

                    var results = query.ToList();

                    return new PagedResultDto<WorkListDto>(
                        query.Distinct().Count(),
                        query.Distinct().ToList()
                        );
                }
                else if(input.NumInTime == 3)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    // 3 thang trl select
                    var userWorks = _userWorkrepository.GetAll();
                    var works = _workRepository.GetAll().Where(x => x.StartDate > fromTime && x.StartDate < toTime);
                    var query = (from userWork in userWorks
                                 join work in works on userWork.WorkId equals work.Id
                                 where userWork.UserId == AbpSession.UserId
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

                    var results = query.ToList();

                    return new PagedResultDto<WorkListDto>(
                        query.Distinct().Count(),
                        query.Distinct().ToList()
                        );
                }
                else 
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddYears(-1);
                    //1 nawm
                    var userWorks = _userWorkrepository.GetAll();
                    var works = _workRepository.GetAll().Where(x => x.StartDate > fromTime && x.StartDate < toTime);
                    var query = (from userWork in userWorks
                                 join work in works on userWork.WorkId equals work.Id
                                 where userWork.UserId == AbpSession.UserId
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

                    var results = query.ToList();

                    return new PagedResultDto<WorkListDto>(
                        query.Distinct().Count(),
                        query.Distinct().ToList()
                        );
                }
            }
        }
    }

}
