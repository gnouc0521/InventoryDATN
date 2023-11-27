using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Dashboard.Dto;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Dashboard
{
  public class DashboardService : ApplicationService, IDashboardService
    {

        private readonly IRepository<ImportRequest> _imporTrequest;
        private readonly IRepository<ExportRequest, long> _exporTrequest;
        private readonly IRepository<ImportRequestSubsidiary> _ImportRequestSubsidiary;
        public DashboardService(
            IRepository<ImportRequest> imporTrequest,
            IRepository<ExportRequest, long> exporTrequest,
           IRepository<ImportRequestSubsidiary> ImportRequestSubsidiary)

        {
            _imporTrequest = imporTrequest;
            _exporTrequest = exporTrequest;
            _ImportRequestSubsidiary = ImportRequestSubsidiary;
        }




        /// <summary>
        /// load bieu do chuyen vien mua hang
        /// created by : Kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>


        ///kiem them tam de tao du lieu view giam doc 



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


    }
}
