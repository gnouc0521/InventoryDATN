using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.DayOffs.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.DayOffs
{
    [AbpAuthorize]
    public class DayOffAppService : ApplicationService, IDayOffAppService
    {
        private readonly IRepository<DayOff> _dayoffrepository;

        public DayOffAppService(IRepository<DayOff> dayoffrepository)
        {
            _dayoffrepository = dayoffrepository;
        }

        public async Task<DayOffListDto> GetAsync(EntityDto itemId)
        {
            var item = _dayoffrepository.Get(itemId.Id);
            DayOffListDto newItem = ObjectMapper.Map<DayOffListDto>(item);
            return newItem;
        }
        public async Task<int> Create(DayOffCreateDto input)
        {
            try
            {
                //DayOff newItemId = ObjectMapper.Map<DayOff>(input);
                //var newId = await _dayoffrepository.InsertAndGetIdAsync(newItemId);
                //return newId;
                DateTime ngaybatdau = DateTime.ParseExact(input.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime ngayketthuc = DateTime.ParseExact(input.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                TimeSpan TongNgayNghi = ngayketthuc - ngaybatdau;

                DayOff newDayOff = new DayOff();
                newDayOff.Title = input.Title;
                newDayOff.StartDate = ngaybatdau;
                newDayOff.EndDate = ngayketthuc;
                newDayOff.SumDayOff = TongNgayNghi.Days + 1;
                newDayOff.TypeDayOff = input.TypeDayOff;
                newDayOff.Order = input.Order;

                var newId = await _dayoffrepository.InsertAndGetIdAsync(newDayOff);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<int> Update(DayOffEditDto input)
        {
            DayOff dayoff = await _dayoffrepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            DateTime ngaybatdau = DateTime.ParseExact(input.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime ngayketthuc = DateTime.ParseExact(input.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            TimeSpan TongNgayNghi = ngayketthuc - ngaybatdau;

            dayoff.Title = input.Title;
            dayoff.StartDate = ngaybatdau;
            dayoff.EndDate = ngayketthuc;
            dayoff.SumDayOff = TongNgayNghi.Days + 1;
            dayoff.TypeDayOff = input.TypeDayOff;
            dayoff.Order = input.Order;

            //ObjectMapper.Map(input, dayoff);
            await _dayoffrepository.UpdateAsync(dayoff);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _dayoffrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<DayOffListDto>> GetAll(GetDayOffInput input)
        {
            try
            {
                var query = _dayoffrepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Title.Contains(input.SearchTerm))
                      .OrderByDescending(x=>x.Id);
                var worksCount = query.Count();


                var WorkListDto = ObjectMapper.Map<List<DayOffListDto>>(query);
                return new PagedResultDto<DayOffListDto>(
                  worksCount,
                  WorkListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public object  GetAllDate(DateTime startDate)
        {
            var query = _dayoffrepository.GetAll().Where(x=>x.StartDate <= startDate && x.EndDate >= startDate).ToList();

            var output = (from db in query
                          select new 
                          {
                              Title = db.Title,
                          }).ToList();

            return output;
        }

        public async Task<bool> CheckDayOff(DateTime startDate, DateTime endDate)
        {
            //_dayOffRepository.GetAll();

            // Check start out of start&end of DateOff : begin loop dayoff
            // 1. startDate is not inside  DateOff
            // 2. endDate is not inside  DateOff
            // 3. startDate & endDate ko chua DayOff
            return false;
        }

        public async Task<List<string>> GetAllDayOffsIf()
        {
            var query = _dayoffrepository.GetAll().ToList();
            List<string> daylist = new List<string>();

            foreach (var item in query)
            {
                DateTime ngaybatdau = Convert.ToDateTime(item.StartDate);
                DateTime ngayketthuc = Convert.ToDateTime(item.EndDate);
                TimeSpan TongNgayNghi = ngayketthuc - ngaybatdau;
                for (int i = 0; i <= TongNgayNghi.Days; i++)
                {
                    string lopsp = ngaybatdau.AddDays(i).ToString("dd-MM-yyyy");
                    daylist.Add(lopsp);
                }
            }
            

            return daylist;
        }

        public async Task<List<string>> GetAllDayOffsIfById(int id)
        {
            var query = _dayoffrepository.GetAll().Where(x => x.Id != id).ToList();
            List<string> daylist = new List<string>();

            foreach (var item in query)
            {
                DateTime ngaybatdau = Convert.ToDateTime(item.StartDate);
                DateTime ngayketthuc = Convert.ToDateTime(item.EndDate);
                TimeSpan TongNgayNghi = ngayketthuc - ngaybatdau;
                for (int i = 0; i <= TongNgayNghi.Days; i++)
                {
                    string lopsp = ngaybatdau.AddDays(i).ToString("dd-MM-yyyy");
                    daylist.Add(lopsp);
                }
            }


            return daylist;
        }
    }
}
