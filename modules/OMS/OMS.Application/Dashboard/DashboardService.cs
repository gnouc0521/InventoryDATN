using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.Dashboard.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.PurchaseAssignments.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Transfers.Dto;
using bbk.netcore.mdl.OMS.Application.UserWorkCounts.Dto;
using bbk.netcore.mdl.OMS.Application.UserWorks.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Dashboard
{
    public class DashboardService : ApplicationService, IDashboardService
    {

        private readonly IRepository<UserWorkCount> _userworkCount;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<ImportRequest> _imporTrequest;
        private readonly IRepository<ExportRequest, long> _exporTrequest;
        private readonly IRepository<PurchasesSynthesise,long> _purchasesSynthesise;
        private readonly IRepository<ImportRequestSubsidiary> _ImportRequestSubsidiary;
        private readonly IRepository<Transfer,long> _Transfer;
        public DashboardService(
            IRepository<UserWorkCount> userworkCount,
            IRepository<Contract> contractRepository,
            IRepository<ImportRequest> imporTrequest,
            IRepository<ExportRequest, long> exporTrequest,
            IRepository<PurchasesSynthesise, long> purchasesSynthesise,
           IRepository<ImportRequestSubsidiary> ImportRequestSubsidiary,
           IRepository<Transfer, long> transfer)
        {
            _userworkCount = userworkCount;
            _contractRepository = contractRepository;
            _imporTrequest = imporTrequest;
            _exporTrequest = exporTrequest;
            _purchasesSynthesise = purchasesSynthesise;
            _ImportRequestSubsidiary = ImportRequestSubsidiary;
            _Transfer = transfer;
        }




        /// <summary>
        /// load bieu do chuyen vien mua hang
        /// created by : Kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserWorkCountCreateDto>> GetAllInDashBoard(DashboardInput input)
        {
            if (input.NumInTime == 0)
            {
                DateTime toTime = DateTime.Now;
                DateTime fromTime = toTime.AddMonths(-3);
                //mac dinh 3 thang trk
                var userwork = _userworkCount.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.UserId == AbpSession.UserId);
                var query = (from uw in userwork
                             where uw.UserId == AbpSession.UserId
                             select new UserWorkCountCreateDto
                             {
                                 UserId = uw.UserId,
                                 OwnerStatus = uw.OwnerStatus,
                                 WorkStatus = uw.WorkStatus,
                                 PurchasesSynthesisesId = uw.PurchasesSynthesisesId,
                             });

                var CodePu = query.Select(x => x.PurchasesSynthesisesId).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let UserId = (from z in query where i == z.PurchasesSynthesisesId select z.UserId).ToArray()
                               let OwnerStatus = (from z in query where i == z.PurchasesSynthesisesId select z.OwnerStatus).Distinct().ToList()
                               let WorkStatus = (from z in query where i == z.PurchasesSynthesisesId select z.WorkStatus).ToArray()
                               select new UserWorkCountCreateDto
                               {
                                 PurchasesSynthesisesId = i,
                                 UserId = UserId[0],
                                 WorkStatus= WorkStatus[0],
                                 OwnerStatus = OwnerStatus[0]
                               });

                return new PagedResultDto<UserWorkCountCreateDto>(
                    Haquery.Distinct().Count(),
                    Haquery.Distinct().ToList()
                    );

            }
            else
            {
                if (input.NumInTime == 1)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-1);
                    // 1 thang trk
                    var userwork = _userworkCount.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.UserId == AbpSession.UserId);
                    var query = (from uw in userwork
                                 where uw.UserId == AbpSession.UserId
                                 select new UserWorkCountCreateDto
                                 {
                                     UserId = uw.UserId,
                                     OwnerStatus = uw.OwnerStatus,
                                     WorkStatus = uw.WorkStatus,
                                     PurchasesSynthesisesId = uw.PurchasesSynthesisesId,
                                 });

                    var CodePu = query.Select(x => x.PurchasesSynthesisesId).Distinct().ToArray();

                    var Haquery = (from i in CodePu
                                   let UserId = (from z in query where i == z.PurchasesSynthesisesId select z.UserId).ToArray()
                                   let OwnerStatus = (from z in query where i == z.PurchasesSynthesisesId select z.OwnerStatus).Distinct().ToList()
                                   let WorkStatus = (from z in query where i == z.PurchasesSynthesisesId select z.WorkStatus).ToArray()
                                   select new UserWorkCountCreateDto
                                   {
                                       PurchasesSynthesisesId = i,
                                       UserId = UserId[0],
                                       WorkStatus = WorkStatus[0],
                                       OwnerStatus = OwnerStatus[0]
                                   });

                    return new PagedResultDto<UserWorkCountCreateDto>(
                        Haquery.Distinct().Count(),
                        Haquery.Distinct().ToList()
                        );
                }
                else if (input.NumInTime == 3)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    // 3 thang trl select
                    var userwork = _userworkCount.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.UserId == AbpSession.UserId);
                    var query = (from uw in userwork
                                 where uw.UserId == AbpSession.UserId
                                 select new UserWorkCountCreateDto
                                 {
                                     UserId = uw.UserId,
                                     OwnerStatus = uw.OwnerStatus,
                                     WorkStatus = uw.WorkStatus,
                                     PurchasesSynthesisesId = uw.PurchasesSynthesisesId,
                                 });

                    var CodePu = query.Select(x => x.PurchasesSynthesisesId).Distinct().ToArray();

                    var Haquery = (from i in CodePu
                                   let UserId = (from z in query where i == z.PurchasesSynthesisesId select z.UserId).ToArray()
                                   let OwnerStatus = (from z in query where i == z.PurchasesSynthesisesId select z.OwnerStatus).Distinct().ToList()
                                   let WorkStatus = (from z in query where i == z.PurchasesSynthesisesId select z.WorkStatus).ToArray()
                                   select new UserWorkCountCreateDto
                                   {
                                       PurchasesSynthesisesId = i,
                                       UserId = UserId[0],
                                       WorkStatus = WorkStatus[0],
                                       OwnerStatus = OwnerStatus[0]
                                   });

                    return new PagedResultDto<UserWorkCountCreateDto>(
                        Haquery.Distinct().Count(),
                        Haquery.Distinct().ToList()
                        );
                }
                else
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddYears(-1);
                    //1 nawm
                    var userwork = _userworkCount.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.UserId == AbpSession.UserId);
                    var query = (from uw in userwork
                                 where uw.UserId == AbpSession.UserId
                                 select new UserWorkCountCreateDto
                                 {
                                     UserId = uw.UserId,
                                     OwnerStatus = uw.OwnerStatus,
                                     WorkStatus = uw.WorkStatus,
                                     PurchasesSynthesisesId = uw.PurchasesSynthesisesId,
                                 });

                    var CodePu = query.Select(x => x.PurchasesSynthesisesId).Distinct().ToArray();

                    var Haquery = (from i in CodePu
                                   let UserId = (from z in query where i == z.PurchasesSynthesisesId select z.UserId).ToArray()
                                   let OwnerStatus = (from z in query where i == z.PurchasesSynthesisesId select z.OwnerStatus).Distinct().ToList()
                                   let WorkStatus = (from z in query where i == z.PurchasesSynthesisesId select z.WorkStatus).ToArray()
                                   select new UserWorkCountCreateDto
                                   {
                                       PurchasesSynthesisesId = i,
                                       UserId = UserId[0],
                                       WorkStatus = WorkStatus[0],
                                       OwnerStatus = OwnerStatus[0]
                                   });

                    return new PagedResultDto<UserWorkCountCreateDto>(
                        Haquery.Distinct().Count(),
                        Haquery.Distinct().ToList()
                        );
                }
            }
        }


        ///kiem them tam de tao du lieu view giam doc 
        public async Task<PagedResultDto<ContractListDto>> GetAllManager(DashboardInput input)
        {
            try
            {
                if (input.NumInTime == 0)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    var query = _contractRepository.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                    var result = (from contract in query
                                  select new ContractListDto
                                  {
                                      Id = contract.Id,
                                      Code = contract.Code,
                                      Status = contract.Status,
                                  }).ToList();

                    return new PagedResultDto<ContractListDto>(
                        result.Count(),
                        result
                        );
                }
                else
                {
                    if (input.NumInTime == 1)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-1);
                        // 1 thang trk
                        var query = _contractRepository.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from contract in query
                                      select new ContractListDto
                                      {
                                          Id = contract.Id,
                                          Code = contract.Code,
                                          Status = contract.Status,
                                      }).ToList();

                        return new PagedResultDto<ContractListDto>(
                            result.Count(),
                            result
                            );
                    }
                    else if (input.NumInTime == 3)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-3);
                        // 3 thang trl select
                        var query = _contractRepository.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from contract in query
                                      select new ContractListDto
                                      {
                                          Id = contract.Id,
                                          Code = contract.Code,
                                          Status = contract.Status,
                                      }).ToList();

                        return new PagedResultDto<ContractListDto>(
                            result.Count(),
                            result
                            );
                    }
                    else
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddYears(-1);
                        //1 nawm
                        var query = _contractRepository.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from contract in query
                                      select new ContractListDto
                                      {
                                          Id = contract.Id,
                                          Code = contract.Code,
                                          Status = contract.Status,
                                      }).ToList();

                        return new PagedResultDto<ContractListDto>(
                            result.Count(),
                            result
                            );
                    }
                }


            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }



        /// view thu kho
        public async Task<PagedResultDto<ImportRequestListDto>> GetAllThuKhoPn(DashboardInput input)
        {
            try
            {
                if (input.NumInTime == 0)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    var query = _imporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                    var result = (from im in query
                                  select new ImportRequestListDto
                                  {
                                      Id = im.Id,
                                      Code = im.Code,
                                      ImportStatus = im.ImportStatus,
                                  }).ToList();

                    return new PagedResultDto<ImportRequestListDto>(
                        result.Count(),
                        result
                        );
                }
                else
                {
                    if (input.NumInTime == 1)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-1);
                        // 1 thang trk
                        var query = _imporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from im in query
                                      select new ImportRequestListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          ImportStatus = im.ImportStatus,
                                      }).ToList();

                        return new PagedResultDto<ImportRequestListDto>(
                            result.Count(),
                            result
                            );
                    }
                    else if (input.NumInTime == 3)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-3);
                        // 3 thang trl select
                        var query = _imporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from im in query
                                      select new ImportRequestListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          ImportStatus = im.ImportStatus,
                                      }).ToList();

                        return new PagedResultDto<ImportRequestListDto>(
                            result.Count(),
                            result
                            );
                    }
                    else
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddYears(-1);
                        //1 nawm
                        var query = _imporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from im in query
                                      select new ImportRequestListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          ImportStatus = im.ImportStatus,
                                      }).ToList();

                        return new PagedResultDto<ImportRequestListDto>(
                            result.Count(),
                            result
                            );
                    }
                }


            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ExportRequestsListDto>> GetAllThuKhoPx(DashboardInput input)
        {
            try
            {
                if (input.NumInTime == 0)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                    var result = (from im in query
                                  select new ExportRequestsListDto
                                  {
                                      Id = im.Id,
                                      Code = im.Code,
                                      Status = im.Status,
                                      ExportStatus= im.ExportStatus,
                                  }).ToList();

                    return new PagedResultDto<ExportRequestsListDto>(
                        result.Count(),
                        result
                        );
                }
                else
                {
                    if (input.NumInTime == 1)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-1);
                        // 1 thang trk
                        var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from im in query
                                      select new ExportRequestsListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          Status = im.Status,
                                          ExportStatus = im.ExportStatus,
                                      }).ToList();

                        return new PagedResultDto<ExportRequestsListDto>(
                            result.Count(),
                            result
                            );
                    }
                    else if (input.NumInTime == 3)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-3);
                        // 3 thang trl select
                        var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from im in query
                                      select new ExportRequestsListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          Status = im.Status,
                                          ExportStatus = im.ExportStatus,
                                      }).ToList();

                        return new PagedResultDto<ExportRequestsListDto>(
                            result.Count(),
                            result
                            );
                    }
                    else
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddYears(-1);
                        //1 nawm
                        var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from im in query
                                      select new ExportRequestsListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          Status = im.Status,
                                          ExportStatus = im.ExportStatus,
                                      }).ToList();

                        return new PagedResultDto<ExportRequestsListDto>(
                            result.Count(),
                            result
                            );
                    }
                }


            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }



        ///VIEW tpkh
        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllTPKHTH(DashboardInput input)
        {
            try
            {
                if (input.NumInTime == 0)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    var query = _purchasesSynthesise.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                    var result = (from ps in query
                                  select new PurchasesSynthesisListDto
                                  {
                                      Id =ps.Id,
                                      PurchasesSynthesiseId = ps.Id,
                                      PurchasesRequestStatus= ps.PurchasesRequestStatus
                                  }).ToList();

                    return new PagedResultDto<PurchasesSynthesisListDto>(result.Count(),result);
                }
                else
                {
                    if (input.NumInTime == 1)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-1);
                        // 1 thang trk
                        var query = _purchasesSynthesise.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from ps in query
                                      select new PurchasesSynthesisListDto
                                      {
                                          Id = ps.Id,
                                          PurchasesSynthesiseId = ps.Id,
                                          PurchasesRequestStatus = ps.PurchasesRequestStatus
                                      }).ToList();

                        return new PagedResultDto<PurchasesSynthesisListDto>(result.Count(), result);
                    }
                    else if (input.NumInTime == 3)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-3);
                        // 3 thang trl select
                        var query = _purchasesSynthesise.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from ps in query
                                      select new PurchasesSynthesisListDto
                                      {
                                          Id = ps.Id,
                                          PurchasesSynthesiseId = ps.Id,
                                          PurchasesRequestStatus = ps.PurchasesRequestStatus
                                      }).ToList();
                        return new PagedResultDto<PurchasesSynthesisListDto>(result.Count(), result);
                    }
                    else
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddYears(-1);
                        //1 nawm
                        var query = _purchasesSynthesise.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from ps in query
                                      select new PurchasesSynthesisListDto
                                      {
                                          Id = ps.Id,
                                          PurchasesSynthesiseId = ps.Id,
                                          PurchasesRequestStatus = ps.PurchasesRequestStatus
                                      }).ToList();
                        return new PagedResultDto<PurchasesSynthesisListDto>(result.Count(), result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ImportRequestSubListDto>> GetAllTPKHPN(DashboardInput input)
        {
            try
            {
                if (input.NumInTime == 0)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    var query = _ImportRequestSubsidiary.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                    var result = (from irs in query
                                  select new ImportRequestSubListDto
                                  {
                                      Id = irs.Id,
                                      ImportStatus = irs.ImportStatus
                                  }).ToList();

                    return new PagedResultDto<ImportRequestSubListDto>(result.Count(), result);
                }
                else
                {
                    if (input.NumInTime == 1)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-1);
                        // 1 thang trk
                        var query = _ImportRequestSubsidiary.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from irs in query
                                      select new ImportRequestSubListDto
                                      {
                                          Id = irs.Id,
                                          ImportStatus = irs.ImportStatus
                                      }).ToList();

                        return new PagedResultDto<ImportRequestSubListDto>(result.Count(), result);
                    }
                    else if (input.NumInTime == 3)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-3);
                        // 3 thang trl select
                        var query = _ImportRequestSubsidiary.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from irs in query
                                      select new ImportRequestSubListDto
                                      {
                                          Id = irs.Id,
                                          ImportStatus = irs.ImportStatus
                                      }).ToList();

                        return new PagedResultDto<ImportRequestSubListDto>(result.Count(), result);
                    }
                    else
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddYears(-1);
                        //1 nawm
                        var query = _ImportRequestSubsidiary.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from irs in query
                                      select new ImportRequestSubListDto
                                      {
                                          Id = irs.Id,
                                          ImportStatus = irs.ImportStatus
                                      }).ToList();

                        return new PagedResultDto<ImportRequestSubListDto>(result.Count(), result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ExportRequestsListDto>> GetAllTPKHPx(DashboardInput input)
        {
            try
            {
                if (input.NumInTime == 0)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.Status != Core.Enums.ExportEnums.ExportStatus.Complete);
                    var result = (from im in query
                                  select new ExportRequestsListDto
                                  {
                                      Id = im.Id,
                                      Code = im.Code,
                                      Status = im.Status,
                                  }).ToList();

                    return new PagedResultDto<ExportRequestsListDto>(result.Count(), result);
                }
                else
                {
                    if (input.NumInTime == 1)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-1);
                        // 1 thang trk
                        var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.Status != Core.Enums.ExportEnums.ExportStatus.Complete);
                        var result = (from im in query
                                      select new ExportRequestsListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          Status = im.Status,
                                      }).ToList();

                        return new PagedResultDto<ExportRequestsListDto>(result.Count(), result);
                    }
                    else if (input.NumInTime == 3)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-3);
                        // 3 thang trl select
                        var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.Status != Core.Enums.ExportEnums.ExportStatus.Complete);
                        var result = (from im in query
                                      select new ExportRequestsListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          Status = im.Status,
                                      }).ToList();

                        return new PagedResultDto<ExportRequestsListDto>(result.Count(), result);
                    }
                    else
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddYears(-1);
                        //1 nawm
                        var query = _exporTrequest.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime && x.Status != Core.Enums.ExportEnums.ExportStatus.Complete);
                        var result = (from im in query
                                      select new ExportRequestsListDto
                                      {
                                          Id = im.Id,
                                          Code = im.Code,
                                          Status = im.Status,
                                      }).ToList();

                        return new PagedResultDto<ExportRequestsListDto>(result.Count(),result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<TransferListDto>> GetAllTranfer(DashboardInput input)
        {
            try
            {
                if (input.NumInTime == 0)
                {
                    DateTime toTime = DateTime.Now;
                    DateTime fromTime = toTime.AddMonths(-3);
                    var query = _Transfer.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                    var result = (from tr in query
                                  select new TransferListDto
                                  {
                                      Id = tr.Id,
                                      TransferCode = tr.TransferCode,
                                      Status = tr.Status,
                                  }).ToList();

                    return new PagedResultDto<TransferListDto>(result.Count(), result);
                }
                else
                {
                    if (input.NumInTime == 1)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-1);
                        // 1 thang trk
                        var query = _Transfer.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from tr in query
                                      select new TransferListDto
                                      {
                                          Id = tr.Id,
                                          TransferCode = tr.TransferCode,
                                          Status = tr.Status,
                                      }).ToList();

                        return new PagedResultDto<TransferListDto>(result.Count(), result);
                    }
                    else if (input.NumInTime == 3)
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddMonths(-3);
                        // 3 thang trl select
                        var query = _Transfer.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from tr in query
                                      select new TransferListDto
                                      {
                                          Id = tr.Id,
                                          TransferCode = tr.TransferCode,
                                          Status = tr.Status,
                                      }).ToList();

                        return new PagedResultDto<TransferListDto>(result.Count(), result);
                    }
                    else
                    {
                        DateTime toTime = DateTime.Now;
                        DateTime fromTime = toTime.AddYears(-1);
                        //1 nawm
                        var query = _Transfer.GetAll().Where(x => x.CreationTime > fromTime && x.CreationTime < toTime);
                        var result = (from tr in query
                                      select new TransferListDto
                                      {
                                          Id = tr.Id,
                                          TransferCode = tr.TransferCode,
                                          Status = tr.Status,
                                      }).ToList();

                        return new PagedResultDto<TransferListDto>(result.Count(), result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
