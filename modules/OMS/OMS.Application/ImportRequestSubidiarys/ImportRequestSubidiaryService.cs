using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys
{
    [AbpAuthorize]
    public class ImportRequestSubidiaryService : ApplicationService, IImportRequestSubidiaryService
    {
        private readonly IRepository<ImportRequestSubsidiary> _impsubrepository;
        private readonly IRepository<Warehouse> _wareHouse;
        private readonly IRepository<User, long> _user;
        private readonly IRepository<Supplier> _supplier;
        private IHostingEnvironment _Environment;
        private readonly ISendMailService _sendMailService;
        public ImportRequestSubidiaryService(IRepository<ImportRequestSubsidiary> impsubrepository,
            IRepository<Warehouse> wareHouse,
            IRepository<Supplier> supplier,
            IHostingEnvironment Environment,
            ISendMailService sendMailService,
            IRepository<User, long> user)
        {
            _impsubrepository = impsubrepository;
            _wareHouse = wareHouse;
            _supplier = supplier;
            _Environment = Environment;
            _sendMailService = sendMailService;
            _user = user;
        }

        public async Task<PagedResultDto<ImportRequestSubListDto>> GetAll(GetInput input)
        {
            try
            {

                var query = _impsubrepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Code.Contains(input.SearchTerm)).OrderBy(x=>x.ImportStatus);
                     
                var warehoue = _wareHouse.GetAll();
                var supllier = _supplier.GetAll();
                var results = (from imp in query
                               join wh in warehoue on imp.WarehouseDestinationId equals wh.Id
                               join sup in supllier on imp.SuppilerId equals sup.Id
                               select new ImportRequestSubListDto
                               {
                                   Id = imp.Id,
                                   Code = imp.Code,
                                   RequestDate = imp.RequestDate,
                                   NameWareHouse = wh.Name,
                                   NameSup = sup.Name,
                                   ImportStatus = imp.ImportStatus,
                                   WarehouseDestinationId = imp.WarehouseDestinationId,
                                   Browsingtime = imp.Browsingtime,
                                   CreatorUserId = imp.CreatorUserId
                               }).OrderByDescending(x => x.Id);

                return new PagedResultDto<ImportRequestSubListDto>(
                  results.Distinct().Count(),
                  results.ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        public async Task<PagedResultDto<ImportRequestSubListDto>> GetAllDone(GetInput input)
        {
            try
            {

                var query = _impsubrepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Code.Contains(input.SearchTerm))
                      .OrderBy(x => x.Id);
                var warehoue = _wareHouse.GetAll();
                var supllier = _supplier.GetAll();
                var results = (from imp in query
                               join wh in warehoue on imp.WarehouseDestinationId equals wh.Id
                               join sup in supllier on imp.SuppilerId equals sup.Id
                               where imp.ImportStatus == PurchasesRequestEnum.YCNK.Approve
                               select new ImportRequestSubListDto
                               {
                                   Id = imp.Id,
                                   Code = imp.Code,
                                   RequestDate = imp.RequestDate,
                                   NameWareHouse = wh.Name,
                                   NameSup = sup.Name,
                                   ImportStatus = imp.ImportStatus,
                                   WarehouseDestinationId = imp.WarehouseDestinationId,
                                   Browsingtime = imp.Browsingtime,
                                   StatusImport = imp.StatusImport


                               }).OrderByDescending(x => x.Id);

                return new PagedResultDto<ImportRequestSubListDto>(
                  results.Distinct().Count(),
                  results.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Create(ImportRequestSubCreateDto input)
        {
            try
            {
                var query = await _impsubrepository.GetAll().ToListAsync();
                string sinhma(string ma)
                {
                    string s = ma.Substring(5, ma.Length - 5);
                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "YCNK"+"-"+ "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "YCNK" + "-" + "0" + Convert.ToString(i);
                    else
                        return "YCNK" + "-" + Convert.ToString(i);

                    }
                    string ma;

                    var count = query.Count;
                    if (count == 0)

                    {
                        ma = "0000000000";
                    }
                    else
                    {
                        ma = _impsubrepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                    }

                    input.Code = sinhma(ma.ToString());
                    input.ImportStatus = PurchasesRequestEnum.YCNK.Draft;
                    ImportRequestSubsidiary newItemId = ObjectMapper.Map<ImportRequestSubsidiary>(input);
                    var newId = await _impsubrepository.InsertAndGetIdAsync(newItemId);
                    return newId;
                }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        public async Task<long> Update(ImportRequestSubListDto input)
        {
            ImportRequestSubsidiary IMP = await _impsubrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.CreationTime = IMP.CreationTime;
            input.Code = IMP.Code;
            input.CreatorUserId = IMP.CreatorUserId;
            ObjectMapper.Map(input, IMP);
            await _impsubrepository.UpdateAsync(IMP);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _impsubrepository.DeleteAsync(id);
                //await _importRequestdetail.DeleteAsync(x=>x.ImportRequestId == id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ImportRequestSubListDto> GetAsync(EntityDto itemId)
        {
            try
            {
                var item = _impsubrepository.Get(itemId.Id);
                ImportRequestSubListDto newItem = ObjectMapper.Map<ImportRequestSubListDto>(item);
                return newItem;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task<long> UpdateStatus(ImportRequestSubListDto input)
        {
            ImportRequestSubsidiary items = await _impsubrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            items.ImportStatus = input.ImportStatus;
            items.Browsingtime = DateTime.Now;
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


            await _impsubrepository.UpdateAsync(items);
            return items.Id;
        }

        public async Task<long> UpdateStatusIMP(ImportRequestSubListDto input)
        {
            ImportRequestSubsidiary items = await _impsubrepository.FirstOrDefaultAsync(x => x.Id == input.ImportRequestSubsidiaryId);

            items.StatusImport = true;
          
            await _impsubrepository.UpdateAsync(items);
            return items.Id;
        }

    }
}
