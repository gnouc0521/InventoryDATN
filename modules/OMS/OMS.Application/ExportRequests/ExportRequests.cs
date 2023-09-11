using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ExportRequests
{
    public class ExportRequests : ApplicationService, IExportRequests
    {
        private readonly IRepository<ExportRequest, long> _exportRequestsRepository;
        private readonly IRepository<ExportRequestDetail, long> _exportRequestsDetailRepository;
        private readonly IRepository<User, long> _user;
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IRepository<Subsidiary, long> _subsidiaryRepository;
      //  private readonly IRepository<Transfer, long> _tranferRepository;
        private ILogger<ExportRequest> logger;
        private IHostingEnvironment _Environment;
        private readonly ISendMailService _sendMailService;
        public ExportRequests(IRepository<ExportRequest, long> exportRequestsRepository,
           IRepository<ExportRequestDetail, long> exportRequestsDetailRepository,
           IRepository<Warehouse> warehouseRepository,
           IRepository<Subsidiary, long> subsidiaryRepository,
          // IRepository<Transfer, long> tranferRepository,
           ILogger<ExportRequest> _logger,
           IHostingEnvironment Environment,
           ISendMailService sendMailService,
           IRepository<User, long> user)
        {
            _exportRequestsRepository = exportRequestsRepository;
            _exportRequestsDetailRepository = exportRequestsDetailRepository;
            _warehouseRepository = warehouseRepository;
            _subsidiaryRepository = subsidiaryRepository;
           // _tranferRepository = tranferRepository;
            logger = _logger;
            _Environment = Environment;
            _sendMailService = sendMailService;
            _user = user;
        }

        public async Task<long> Create(ExportRequestsCreate input)
        {
            try
            {
                string sinhma(string ma)
                {
                    string s = ma.Substring(9, ma.Length - 9);

                    int i = int.Parse(s);
                    i++;
                    string Thang;
                    if (DateTime.Now.Month >= 10)
                    {
                        Thang = DateTime.Now.Month.ToString();
                    }
                    else
                    {
                        Thang = "0" + DateTime.Now.Month;
                    }

                    if (i < 10) return "PX" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "PX" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "PX" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
                    else
                        return "PX" + Thang + DateTime.Now.Year + Convert.ToString(i);

                }
                string sinhmaRequirement(string ma)
                {
                    logger.LogInformation("Create SendMailService 1" + ma);
                    string s = ma.Substring(5, ma.Length - 5);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "YCXK-000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "YCXK-00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "YCXK-0" + Convert.ToString(i);
                    else
                        return "YCXK-" + Convert.ToString(i);
                }
                string ma;
                string Requirement;
                var query = await _exportRequestsRepository.GetAll().ToListAsync();
                var queryRequirement = await _exportRequestsRepository.GetAll().Where(x => x.TransferId ==0 ).ToListAsync();

                logger.LogInformation("Create SendMailService 2" + queryRequirement);
                var count = query.Count;
                var countRequirement = queryRequirement.Count;
                logger.LogInformation("Create SendMailService 3" + countRequirement);
                if (count == 0)
                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _exportRequestsRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }
                if (countRequirement == 0)
                {
                    Requirement = "000000000";
                }
                else
                {
                    Requirement = _exportRequestsRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.CodeRequirement).ToList().First();
                }
                input.CodeRequirement = sinhmaRequirement(Requirement.ToString());
                input.Code = sinhma(ma.ToString());
                ExportRequest newItemId = ObjectMapper.Map<ExportRequest>(input);
                var newId = await _exportRequestsRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Delete(long id)
        {
            try
            {
                await _exportRequestsRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ExportRequestsListDto>> GetAll(ExportRequestsSearch input)
        {
            try
            {
                var query = _exportRequestsRepository.GetAll()
                                                     .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Code.ToLower().Contains(input.SearchTerm.ToLower()))
                                                      .WhereIf(!string.IsNullOrEmpty(input.RequestDate), u => u.RequestDate == DateTime.Parse(input.RequestDate))
                                                      .WhereIf(input.Status.HasValue, u => u.Status > input.Status)
                                                      .WhereIf(input.ExportStatus.HasValue, u => u.ExportStatus != input.ExportStatus)
                                                     .WhereIf(input.WarehouseDestinationId != null, x => x.WarehouseDestinationId == input.WarehouseDestinationId)
                                                     .ToList();
                var queryware = _warehouseRepository.GetAll();
                var queryexp = _exportRequestsRepository.GetAll().Select(x => x.Id);
                var querydetail = _exportRequestsDetailRepository.GetAll();
                //Authorization.Users.Dto.GetUsersInput getUsersInput = new GetUsersInput();
                //getUsersInput.MaxResultCount = 1000;
                var WareSourceName = (from q in querydetail
                                      join e in queryware on q.WarehouseSourceId equals e.Id into gj
                                      from subpet in gj.DefaultIfEmpty()
                                      select new { q.ExportRequestId, subpet.Name }).ToList();

                //var user = _userAppService.GetUsers(getUsersInput).Result.Items.ToList();
                var user = _user.GetAll().ToList();
                var querysubsidiary = _subsidiaryRepository.GetAll();
              //  var querytranfer = _tranferRepository.GetAll();
                var output = (from q in query
                              join exp in queryexp on q.Id equals exp
                              join u in user on q.CreatorUserId equals u.Id
                              join subsidiary in querysubsidiary on q.SubsidiaryId equals subsidiary.Id into g
                              // join tranfer in querytranfer on q. equals subsidiary.Id into g
                              join ware in queryware on q.WarehouseDestinationId equals ware.Id into gj
                              let ListName = (from z in WareSourceName where exp == z.ExportRequestId select z.Name).Distinct().ToList()
                              from subpet in gj.DefaultIfEmpty()
                              from SubsidiaryNull in g.DefaultIfEmpty()
                              select new ExportRequestsListDto
                              {
                                  Id = q.Id,
                                  Address = q.Address,
                                  Code = q.Code,
                                  ExportStatus = q.ExportStatus,
                                  CreateBy = u.Surname + " " + u.Name,
                                  Status = q.Status,
                                  RequestDate = q.RequestDate,
                                  WarehouseDestinationId = subpet.Id,
                                  TranferCode = q.CodeTransfer,
                                  CreationTime = q.CreationTime,
                                  CreatorUserId = q.CreatorUserId,
                                  ReceiverName = q.ReceiverName,
                                  WarehouseDestinationName = subpet == null ?   string.Empty : subpet.Name,
                                 // LastModificationTime = q.LastModificationTime.Value,
                                  ListWarehouseSourceName = ListName.ToList(),
                                  CodeRequirement = q.CodeRequirement,
                                  SubsidiaryName = (SubsidiaryNull == null || SubsidiaryNull.NameCompany == null ?  string.Empty : SubsidiaryNull.NameCompany),
                              }).OrderBy(x => x.Status).ToList();
                var itemscount = query.Count();
                var itemslist = ObjectMapper.Map<List<ExportRequestsListDto>>(output);
                return new PagedResultDto<ExportRequestsListDto>(
                     itemscount,
                     itemslist.OrderBy(x => x.Status).ToList()
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            };
        }

        public async Task<PagedResultDto<ExportRequestsListDto>> GetAllRequirement(ExportRequestsSearch input)
        {
            try
            {
                var query = _exportRequestsRepository.GetAll()
                                                       .Where(x => x.TransferId == 0)
                                                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Code.ToLower().Contains(input.SearchTerm.ToLower()))
                                                      .WhereIf(!string.IsNullOrEmpty(input.RequestDate), u => u.RequestDate == DateTime.Parse(input.RequestDate))
                                                      .WhereIf(input.UserIdCreate.HasValue, u => u.CreatorUserId == input.UserIdCreate)
                                                      .WhereIf(input.Status.HasValue, u => u.Status > input.Status)
                                                     .WhereIf(input.WarehouseDestinationId != null, x => x.WarehouseDestinationId == input.WarehouseDestinationId)
                                                     .ToList();
                var queryware = _warehouseRepository.GetAll();
                var queryexp = _exportRequestsRepository.GetAll().Select(x => x.Id);
                var querydetail = _exportRequestsDetailRepository.GetAll();
                //Authorization.Users.Dto.GetUsersInput getUsersInput = new GetUsersInput();
               // getUsersInput.MaxResultCount = 1000;
                var WareSourceName = (from q in querydetail
                                      join e in queryware on q.WarehouseSourceId equals e.Id into gj
                                      from subpet in gj.DefaultIfEmpty()
                                      select new { q.ExportRequestId, subpet.Name }).ToList();

                //var user = _userAppService.GetUsers(getUsersInput).Result.Items.ToList();
                var user = _user.GetAll().ToList();
                var querysubsidiary = _subsidiaryRepository.GetAll();
                var output = (from q in query
                              join exp in queryexp on q.Id equals exp
                              join u in user on q.CreatorUserId equals u.Id
                              join subsidiary in querysubsidiary on q.SubsidiaryId equals subsidiary.Id into g
                              join ware in queryware on q.WarehouseDestinationId equals ware.Id into gj
                              let ListName = (from z in WareSourceName where exp == z.ExportRequestId select z.Name).Distinct().ToList()
                              from subpet in gj.DefaultIfEmpty()
                              from SubsidiaryNull in g.DefaultIfEmpty()
                              select new ExportRequestsListDto
                              {
                                  Id = q.Id,
                                  Address = q.Address,
                                  Code = q.Code,
                                  CreateBy = u.Surname + " " + u.Name,
                                  Status = q.Status,
                                  RequestDate = q.RequestDate,
                                  WarehouseDestinationId = subpet.Id,
                                  CreationTime = q.CreationTime,
                                  CreatorUserId = q.CreatorUserId,
                                  ReceiverName = q.ReceiverName,
                                  WarehouseDestinationName = subpet.Name ?? string.Empty,
                                  LastModificationTime = q.LastModificationTime,
                                  ListWarehouseSourceName = ListName.ToList(),
                                  CodeRequirement = q.CodeRequirement,
                                 SubsidiaryName = SubsidiaryNull.NameCompany ?? string.Empty,
                              }).OrderBy(x => x.Status).ToList();
                var itemscount = query.Count();
                var itemslist = ObjectMapper.Map<List<ExportRequestsListDto>>(output);
                return new PagedResultDto<ExportRequestsListDto>(
                     itemscount,
                     itemslist.OrderBy(x => x.Status).ThenByDescending(x=>x.Id).ToList()
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            };
        }  public async Task<PagedResultDto<ExportRequestsListDto>> GetAllRequirementApprove(ExportRequestsSearch input)
        {
            try
            {
                var query = _exportRequestsRepository.GetAll()
                                                       .Where(x => x.CodeRequirement != null)
                                                     .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Code.ToLower().Contains(input.SearchTerm.ToLower()))
                                                      .WhereIf(!string.IsNullOrEmpty(input.RequestDate), u => u.RequestDate == DateTime.Parse(input.RequestDate))
                                                      .WhereIf(input.Status.HasValue, u => u.Status == ExportEnums.ExportStatus.Approve)
                                                      .WhereIf(input.Status.HasValue, u => u.ExportStatus == ExportEnums.Export.Draft)
                                                     .WhereIf(input.WarehouseDestinationId != null, x => x.WarehouseDestinationId == input.WarehouseDestinationId)
                                                     .ToList();
                var queryware = _warehouseRepository.GetAll();
                var queryexp = _exportRequestsRepository.GetAll().Select(x => x.Id);
                var querydetail = _exportRequestsDetailRepository.GetAll();
                //Authorization.Users.Dto.GetUsersInput getUsersInput = new GetUsersInput();
                //getUsersInput.MaxResultCount = 1000;
                var WareSourceName = (from q in querydetail
                                      join e in queryware on q.WarehouseSourceId equals e.Id into gj
                                      from subpet in gj.DefaultIfEmpty()
                                      select new { q.ExportRequestId, subpet.Name }).ToList();

                //var user = _userAppService.GetUsers(getUsersInput).Result.Items.ToList();
                var user = _user.GetAll().ToList();
                var querysubsidiary = _subsidiaryRepository.GetAll();
                var output = (from q in query
                              join exp in queryexp on q.Id equals exp
                              join u in user on q.CreatorUserId equals u.Id
                              join subsidiary in querysubsidiary on q.SubsidiaryId equals subsidiary.Id into g
                              join ware in queryware on q.WarehouseDestinationId equals ware.Id into gj
                              let ListName = (from z in WareSourceName where exp == z.ExportRequestId select z.Name).Distinct().ToList()
                              from subpet in gj.DefaultIfEmpty()
                              from SubsidiaryNull in g.DefaultIfEmpty()
                              select new ExportRequestsListDto
                              {
                                  Id = q.Id,
                                  Address = q.Address,
                                  Code = q.Code,
                                  CreateBy = u.Surname + " " + u.Name,
                                  Status = q.Status,
                                  RequestDate = q.RequestDate,
                                  WarehouseDestinationId = subpet.Id,
                                  CreationTime = q.CreationTime,
                                  CreatorUserId = q.CreatorUserId,
                                  ReceiverName = q.ReceiverName,
                                  WarehouseDestinationName = subpet.Name ?? string.Empty,
                                  LastModificationTime = q.LastModificationTime,
                                  ListWarehouseSourceName = ListName.ToList(),
                                  CodeRequirement = q.CodeRequirement,
                                  SubsidiaryName = SubsidiaryNull.NameCompany ?? string.Empty,
                              }).OrderBy(x => x.Status).ToList();
                var itemscount = query.Count();
                var itemslist = ObjectMapper.Map<List<ExportRequestsListDto>>(output);
                return new PagedResultDto<ExportRequestsListDto>(
                     itemscount,
                     itemslist.OrderBy(x => x.Status).ToList()
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            };
        }

        public async Task<ExportRequestsListDto> GetAsync(EntityDto<long> itemId)
        {
            try
            {
                var item = _exportRequestsRepository.Get(itemId.Id);
                ExportRequestsListDto newItem = ObjectMapper.Map<ExportRequestsListDto>(item);
                return newItem;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="input"></param>
        /// <returns> các phiếu xuất theo trạng thái </returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<ExportRequestsListDto>> GetAllExport(ExportRequestsSearch input)
        {
            try
            {
                var query = _exportRequestsRepository.GetAll()
                                                     .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Code.ToLower().Contains(input.SearchTerm.ToLower()))
                                                      .WhereIf(!string.IsNullOrEmpty(input.RequestDate), u => u.RequestDate == DateTime.Parse(input.RequestDate))
                                                      .WhereIf(input.Status.HasValue, u => u.Status == input.Status)
                                                      .WhereIf(input.ExportStatus.HasValue, u => u.ExportStatus != ExportEnums.Export.Draft)
                                                     .WhereIf(input.WarehouseDestinationId != null, x => x.WarehouseDestinationId == input.WarehouseDestinationId)
                                                     .ToList();
                var queryware = _warehouseRepository.GetAll();
                var queryexp = _exportRequestsRepository.GetAll().Select(x => x.Id);
                var querydetail = _exportRequestsDetailRepository.GetAll();
                //Authorization.Users.Dto.GetUsersInput getUsersInput = new GetUsersInput();
                //getUsersInput.MaxResultCount = 1000;
                var WareSourceName = (from q in querydetail
                                      join e in queryware on q.WarehouseSourceId equals e.Id into gj
                                      from subpet in gj.DefaultIfEmpty()
                                      select new { q.ExportRequestId, subpet.Name }).ToList();


                //var user = _userAppService.GetUsers(getUsersInput).Result.Items.ToList();
                var user = _user.GetAll().ToList();
                var querysubsidiary = _subsidiaryRepository.GetAll();
               // var querytranfer = _tranferRepository.GetAll();
                var output = (from q in query
                              join exp in queryexp on q.Id equals exp
                              join u in user on q.CreatorUserId equals u.Id
                              join subsidiary in querysubsidiary on q.SubsidiaryId equals subsidiary.Id into g
                              from SubsidiaryNull in g.DefaultIfEmpty()
                             // join tranfer in querytranfer on q.TransferId equals tranfer.Id  into tranferempty
                            //  from tranferNull in tranferempty.DefaultIfEmpty()
                              join ware in queryware on q.WarehouseDestinationId equals ware.Id into gj
                              let ListName = (from z in WareSourceName where exp == z.ExportRequestId select z.Name).Distinct().ToList()
                            //  let timexuatkho = (from z in WareSourceName where exp == z.ExportRequestId select z.LastModificationTime.Value).ToList()
                              from subpet in gj.DefaultIfEmpty()
                              select new ExportRequestsListDto
                              {
                                  Id = q.Id,
                                  Address = q.Address,
                                  Code = q.Code,
                                  CreateBy = u.Surname + " " + u.Name,
                                  Status = q.Status,
                                  RequestDate = q.RequestDate,
                                  WarehouseDestinationId = subpet.Id,
                                  CreationTime = q.CreationTime,
                                  CreatorUserId = q.CreatorUserId,
                                  ReceiverName = q.ReceiverName,
                                  WarehouseDestinationName = subpet.Name ?? string.Empty,
                                  LastModificationTime = q.LastModificationTime,
                                  ListWarehouseSourceName = ListName.ToList(),
                                  CodeRequirement = q.CodeRequirement,
                                  ExportStatus = q.ExportStatus,
                                 // ExportTime = timexuatkho.ToList(),
                                  SubsidiaryName = SubsidiaryNull == null ? String.Empty : SubsidiaryNull.NameCompany,
                                  //TranferCode = tranferNull == null ? String.Empty : tranferNull.TransferCode
                              }).OrderBy(x => x.ExportStatus).ToList();
                var itemscount = query.Count();
                var itemslist = ObjectMapper.Map<List<ExportRequestsListDto>>(output);
                return new PagedResultDto<ExportRequestsListDto>(
                     itemscount,
                     itemslist.OrderBy(x => x.ExportStatus).ToList()
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            };
        }
        public async Task<long> Update(ExportRequestsListDto input)
        {
            ExportRequest items = await _exportRequestsRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            input.CreationTime = items.CreationTime;
            input.Code = items.Code;
            input.CreatorUserId = items.CreatorUserId;
            ObjectMapper.Map(input, items);
            await _exportRequestsRepository.UpdateAsync(items);
            return items.Id;
        }

        public async Task<long> UpdateStatus(ExportRequestsListDto input)
        {
            ExportRequest items = await _exportRequestsRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if(input.Status == ExportEnums.ExportStatus.Approve)
            {
                if (items.LastModificationTime.HasValue)
                {
                    items.LastModificationTime = items.LastModificationTime.Value;

                }
            }
            items.Status = input.Status;
            items.Comment = input.Comment;

            var webRootPath = this._Environment.WebRootPath;
            string path = "";
            path = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "sendMailCV.html";
            path = File.ReadAllText(path);
            var passlinkEmail = input.Link;
            var email = input.Email;
            var name = input.Name;
            if (items.CreatorUserId == AbpSession.UserId)
            {
                email = input.Email;
                name = input.Name;
                path = path.Replace("{{TenNguoiNhan}}", name);
                path = path.Replace("{{LinkFromEmail}}", passlinkEmail);
                await _sendMailService.SendEmailCvAsync(email, "Bạn có công việc mới :", path);
            }
            else
            {
                var user = _user.GetAll().Where(x => x.Id == items.CreatorUserId).FirstOrDefault();
                email = user.EmailAddress;
                name = user.Name;
                path = path.Replace("{{TenNguoiNhan}}", name);
                path = path.Replace("{{LinkFromEmail}}", passlinkEmail);
                await _sendMailService.SendEmailCvAsync(email, "Xác nhận công việc :", path);
                //GetUsersInput getUsersInput = new GetUsersInput();
                //getUsersInput.MaxResultCount = 1000;
                //var querya = _userAppService.GetUsers(getUsersInput).Result;
                //List<UsersListDto> usersListDtos = new List<UsersListDto>((querya.TotalCount));
                //for (int i = 0; i < querya.TotalCount; i++)
                //{
                //    if (querya.Items[i].Id == items.CreatorUserId)
                //    {
                //        UsersListDto usersListdata = new UsersListDto();
                //        email = querya.Items[i].EmailAddress;
                //        name = querya.Items[i].Name;
                //        usersListdata.Email = email;
                //        usersListdata.FullName = name;
                //        usersListDtos.Add(usersListdata);
                //        path = path.Replace("{{TenNguoiNhan}}", name);
                //        path = path.Replace("{{LinkFromEmail}}", passlinkEmail);
                //        await _sendMailService.SendEmailCvAsync(email, "Xác nhận công việc :", path);
                //    }

                //}
            }

            await _exportRequestsRepository.UpdateAsync(items);
            return items.Id;
        } 
        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="input"></param>
        /// <returns>đổi trạng thái phiếu xuất đã xuất kho  </returns>
        public async Task<long> UpdateExportStatus(ExportRequestsListDto input)
        {
            ExportRequest items = await _exportRequestsRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            items.ExportStatus = input.ExportStatus;
            await _exportRequestsRepository.UpdateAsync(items);
            return items.Id;
        }

        public async Task<ExportRequestsListDto> CreateToTransfer(ExportRequestsCreate input)
        {
            try
            {
                string sinhma(string ma)
                {
                    string s = ma.Substring(9, ma.Length - 9);

                    int i = int.Parse(s);
                    i++;
                    string Thang;
                    if (DateTime.Now.Month >= 10)
                    {
                        Thang = DateTime.Now.Month.ToString();
                    }
                    else
                    {
                        Thang = "0" + DateTime.Now.Month;
                    }

                    if (i < 10) return "PX" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "PX" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "PX" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
                    else
                        return "PX" + Thang + DateTime.Now.Year + Convert.ToString(i);

                }
                string ma;
                var query = await _exportRequestsRepository.GetAll().ToListAsync();
                var count = query.Count;
                if (count == 0)
                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _exportRequestsRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }
                input.Code = sinhma(ma.ToString());
                ExportRequest newItemId = ObjectMapper.Map<ExportRequest>(input);
                newItemId.LastModificationTime = DateTime.Now;
                var newId = await _exportRequestsRepository.InsertAndGetIdAsync(newItemId);
                return new ExportRequestsListDto
                {
                    Id = newItemId.Id,
                    IdWarehouseReceiving = newItemId.IdWarehouseReceiving,
                };
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<int> UpdateCodeSame(int idTransfer)
        {
            var query = _exportRequestsRepository.GetAll().Where(x => x.TransferId == idTransfer).ToList();
            var OldCode = _exportRequestsRepository.GetAll().Where(x => x.TransferId == idTransfer).ToList().Select(x => x.Code).FirstOrDefault();

            for (int i = 1; i < query.Count(); i++)
            {
                var codeUpdate = "";
                string s = OldCode.Substring(9, query[i].Code.Length - 9);
                var code = int.Parse(s) + i;

                string Thang;
                if (DateTime.Now.Month >= 10)
                {
                    Thang = DateTime.Now.Month.ToString();
                }
                else
                {
                    Thang = "0" + DateTime.Now.Month;
                }

                if (code < 10) { codeUpdate = "PX" + Thang + DateTime.Now.Year + "000" + Convert.ToString(code); }

                else
                if (code >= 10 && code < 100) { codeUpdate = "PX" + Thang + DateTime.Now.Year + "00" + Convert.ToString(code); }
                else
                if (code >= 100 && code < 1000) { codeUpdate = "PX" + Thang + DateTime.Now.Year + "0" + Convert.ToString(code); }
                else
                { codeUpdate = "PX" + Thang + DateTime.Now.Year + Convert.ToString(code); }

                ExportRequest export = _exportRequestsRepository.FirstOrDefault(x => x.Id == query[i].Id);

                export.Code = codeUpdate;

                await _exportRequestsRepository.UpdateAsync(export);

            }

            return idTransfer;
        }
    }
}
