using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequests
{
    [AbpAuthorize]
    public class ImportRequestAppService : ApplicationService, IImportRequestAppService
    {
        private readonly IRepository<ImportRequest> _importRequest;
        private readonly IRepository<ImportRequestDetail> _importRequestdetail;
        private readonly IRepository<Transfer,long> _transfer;
        private readonly IRepository<ImportRequestSubsidiary> _importRequestSubsidiary;
        private readonly IRepository<Warehouse> _wareHouse;
        private readonly IRepository<User, long> _user;
        public ImportRequestAppService(IRepository<ImportRequest> importRequest, IRepository<ImportRequestDetail> importRequestdetail, 
            IRepository<Transfer, long> transfer, IRepository<Warehouse> wareHouse, IRepository<ImportRequestSubsidiary> importRequestSubsidiary, IRepository<User, long> user)
        {
            _importRequest = importRequest;
            _importRequestdetail = importRequestdetail;
            _transfer = transfer;
            _wareHouse = wareHouse;
            _importRequestSubsidiary = importRequestSubsidiary;
            _user = user;
        }

        public async Task<PagedResultDto<ImportRequestListDto>> GetAll(GetImportRequestInput input)
        {
            try
            {
                if(input.Status == 1)
                {
                    var tranfer = _transfer.GetAll();
                    var importS = _importRequestSubsidiary.GetAll();
                    var query = _importRequest
                          .GetAll().Where(x => x.ImportStatus != ImportResquestEnum.ImportResquestStatus.Draft && x.ImportStatus != ImportResquestEnum.ImportResquestStatus.Approve)
                          .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Code.Contains(input.SearchTerm))
                          .WhereIf(!string.IsNullOrEmpty(input.ResquestDate), u => u.RequestDate == DateTime.ParseExact(input.ResquestDate, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                          .WhereIf(input.WarehouseDestinationId != 0, x => x.WarehouseDestinationId == input.WarehouseDestinationId)
                          .WhereIf(input.CreatorById != 0, x => x.CreatorUserId == input.CreatorById)
                          //.WhereIf(input.Status != 100, x => x.Status == input.Status)
                          .OrderBy(x => x.Id);


                    var warehoue = _wareHouse.GetAll();
                    var user = _user.GetAll().ToList();
                    var results = (from imp in query
                                   join u in user on imp.CreatorUserId equals u.Id
                                   join tr in tranfer on imp.TransferId equals tr.Id into t
                                   from tr in t.DefaultIfEmpty()
                                   join ims in importS on imp.ImportRequestSubsidiaryId equals ims.Id into z
                                   from ims in z.DefaultIfEmpty()
                                   join wh in warehoue on imp.WarehouseDestinationId equals wh.Id
                                   select new ImportRequestListDto
                                   {
                                       Id = imp.Id,
                                       Code = imp.Code,
                                       CreatedBy = u.Surname + " " + u.Name,
                                       RequestDate = imp.RequestDate,
                                       NameWareHouse = wh.Name,
                                       ImportStatus = imp.ImportStatus,
                                       WarehouseDestinationId = imp.WarehouseDestinationId,
                                       TransferId = imp.TransferId,
                                       ImportRequestSubsidiaryId = imp.ImportRequestSubsidiaryId,
                                       TranferCode = tr?.TransferCode,
                                       YcnkCode = ims?.Code,

                                   }).OrderByDescending(x => x.Id);

                    return new PagedResultDto<ImportRequestListDto>(
                      results.Distinct().Count(),
                      results.Distinct().ToList()
                      );
                }
                else
                {
                    var tranfer = _transfer.GetAll();
                    var importS = _importRequestSubsidiary.GetAll();
                    var query = _importRequest
                          .GetAll()
                          .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Code.Contains(input.SearchTerm))
                          .WhereIf(!string.IsNullOrEmpty(input.ResquestDate), u => u.RequestDate == DateTime.ParseExact(input.ResquestDate, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                          .WhereIf(input.WarehouseDestinationId != 0, x => x.WarehouseDestinationId == input.WarehouseDestinationId)
                          .WhereIf(input.CreatorById != 0, x => x.CreatorUserId == input.CreatorById)
                          //.WhereIf(input.Status != 100, x => x.Status == input.Status)
                          .OrderBy(x => x.Id);
                    var warehoue = _wareHouse.GetAll();
                    var user = _user.GetAll().ToList();
                    var results = (from imp in query
                                   join u in user on imp.CreatorUserId equals u.Id
                                   join tr in tranfer on imp.TransferId equals tr.Id into t
                                   from tr in t.DefaultIfEmpty()
                                   join ims in importS on imp.ImportRequestSubsidiaryId equals ims.Id into z
                                   from ims in z.DefaultIfEmpty()
                                   join wh in warehoue on imp.WarehouseDestinationId equals wh.Id
                                   select new ImportRequestListDto
                                   {
                                       Id = imp.Id,
                                       Code = imp.Code,
                                       CreatedBy = u.Surname + " " + u.Name,
                                       RequestDate = imp.RequestDate,
                                       NameWareHouse = wh.Name,
                                       ImportStatus = imp.ImportStatus,
                                       WarehouseDestinationId = imp.WarehouseDestinationId,
                                       TransferId = imp.TransferId,
                                       ImportRequestSubsidiaryId = imp.ImportRequestSubsidiaryId,
                                       TranferCode = tr?.TransferCode,
                                       YcnkCode = ims?.Code,

                                   }).OrderByDescending(x => x.Id);

                    return new PagedResultDto<ImportRequestListDto>(
                      results.Distinct().Count(),
                      results.Distinct().ToList()
                      );
                }
                
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Create(ImportRequestCreateDto input)
        {
            try
            {
                var query = await _importRequest.GetAll().ToListAsync();
                string sinhma(string ma)
                {
                    string s = ma.Substring(9, ma.Length - 9);

                    string Thang;

                    if (DateTime.Now.Month >= 10)
                    {
                        Thang = DateTime.Now.Month.ToString();
                    }
                    else
                    {
                        Thang = "0" + DateTime.Now.Month;
                    }

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "PN" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "PN" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "PN" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
                    else
                        return "PN" + Thang + DateTime.Now.Year + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _importRequest.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                input.Code = sinhma(ma.ToString());
                input.ImportStatus = ImportResquestEnum.ImportResquestStatus.Draft;
                ImportRequest newItemId = ObjectMapper.Map<ImportRequest>(input);
                var newId = await _importRequest.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        public async Task<long> Update(ImportRequestListDto input)
        {
            ImportRequest IMP = await _importRequest.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.CreationTime = IMP.CreationTime;
            input.Code = IMP.Code;
            input.CreatorUserId = IMP.CreatorUserId;
            ObjectMapper.Map(input, IMP);
            await _importRequest.UpdateAsync(IMP);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _importRequest.DeleteAsync(id);
                //await _importRequestdetail.DeleteAsync(x=>x.ImportRequestId == id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ImportRequestListDto> GetAsync(EntityDto itemId)
        {
            var item = _importRequest.Get(itemId.Id);
            ImportRequestListDto newItem = ObjectMapper.Map<ImportRequestListDto>(item);
            return newItem;
        }


        //code Ha
        public async Task<ImportRequestListDto> CreateToTranssfer(ImportRequestCreateDto input)
        {
            try
            {
                var query = await _importRequest.GetAll().ToListAsync();
                string sinhma(string ma)
                {
                    string s = ma.Substring(9, ma.Length - 9);

                    string Thang;

                    if (DateTime.Now.Month >= 10)
                    {
                        Thang = DateTime.Now.Month.ToString();
                    }
                    else
                    {
                        Thang = "0" + DateTime.Now.Month;
                    }

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "PN" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "PN" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "PN" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
                    else
                        return "PN" + Thang + DateTime.Now.Year + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _importRequest.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                input.Code = sinhma(ma.ToString());
                input.ImportStatus = ImportResquestEnum.ImportResquestStatus.Draft;
                ImportRequest newItemId = ObjectMapper.Map<ImportRequest>(input);
                var newId = await _importRequest.InsertAndGetIdAsync(newItemId);
                return new ImportRequestListDto
                {
                    Id = newItemId.Id,
                    WarehouseDestinationId = newItemId.WarehouseDestinationId,
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateStatusImport(ImportRequestListDto input)
        {
            ImportRequest IMP = await _importRequest.FirstOrDefaultAsync(x => x.Id == input.Id);
            IMP.ImportStatus = input.ImportStatus;
            await _importRequest.UpdateAsync(IMP);
            return input.Id;
        }

        public async Task<long> UpdateSunmit(ImportRequestListDto input)
        {
            ImportRequest IMP = await _importRequest.FirstOrDefaultAsync(x => x.Id == input.Id);
            IMP.ImportStatus = input.ImportStatus;
            IMP.ImportRequestSubsidiaryId = input.ImportRequestSubsidiaryId;
            IMP.ShipperName = input.ShipperName;
            IMP.ShipperPhone = input.ShipperPhone;

            await _importRequest.UpdateAsync(IMP);
            return input.Id;
        }
    }
}
