using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseCards.Dto;
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

namespace bbk.netcore.mdl.OMS.Application.WarehouseCards
{
  public class WarehouseCardAppService : ApplicationService, IWarehouseCardAppService
  {
    private readonly IRepository<WarehouseCard, long> _WarehouseCardRepository;
    private readonly IRepository<WarehouseCardDetail, long> _WarehouseCardDetailRepository;
    private readonly IRepository<User, long> _user;
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IRepository<Unit> _UnitRepository;
    private readonly IRepository<Items,long> _ItemsRepository;
    public WarehouseCardAppService(
        IRepository<WarehouseCard, long> WarehouseCardRepository,
        IRepository<WarehouseCardDetail, long> WarehouseCardDetailRepository,
        IRepository<Warehouse> warehouseRepository,
       IRepository<User, long> user,
       IRepository<Unit> unitRepository,
       IRepository<Items, long> itemsRepository)
    {
      _WarehouseCardRepository = WarehouseCardRepository;
      _WarehouseCardDetailRepository = WarehouseCardDetailRepository;
      _warehouseRepository = warehouseRepository;
      _user = user;
      _UnitRepository = unitRepository;
      _ItemsRepository = itemsRepository;
    }

    public async Task<long> Create(WarehouseCardCreate input)
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

          if (i < 10) return "TK" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
          else
          if (i >= 10 && i < 100) return "TK" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
          else
          if (i >= 100 && i < 1000) return "TK" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
          else
            return "TK" + Thang + DateTime.Now.Year + Convert.ToString(i);

        }
        string ma;
        string Requirement;
        var query = await _WarehouseCardRepository.GetAll().ToListAsync();

        var count = query.Count;
        if (count == 0)
        {
          ma = "0000000000";
        }
        else
        {
          ma = _WarehouseCardRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
        }
      
        input.Code = sinhma(ma.ToString());
        WarehouseCard newItemId = ObjectMapper.Map<WarehouseCard>(input);
        var newId = await _WarehouseCardRepository.InsertAndGetIdAsync(newItemId);
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
        await _WarehouseCardRepository.DeleteAsync(id);
        return id;
      }
      catch (Exception ex)
      {

        throw new UserFriendlyException(ex.Message);
      }
    }

    public async Task<PagedResultDto<WarehouseCardListDto>> GetAll(WarehouseCardSearch input)
    {
      try
      {
        var query = _WarehouseCardRepository.GetAll()
                                             .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Code.ToLower().Contains(input.SearchTerm.ToLower()))
                                             .WhereIf(input.WarehouseDestinationId != null, x => x.WarehouseDestinationId == input.WarehouseDestinationId)
                                             .ToList();
        var queryware = _warehouseRepository.GetAll();
        var querydetail = _WarehouseCardDetailRepository.GetAll();
        var queryItems = _ItemsRepository.GetAll();
        var queryUnits = _UnitRepository.GetAll();

        var user = _user.GetAll().ToList();
        var output = (from q in query
                      join u in user on q.CreatorUserId equals u.Id
                      join item in queryItems on q.ItemsId equals item.Id
                      join unit in queryUnits on q.UnitId equals unit.Id
                      join WarehouseCardDetail in querydetail on q.Id equals WarehouseCardDetail.WarehouseId
                      join ware in queryware on q.WarehouseDestinationId equals ware.Id into gj
                      from subpet in gj.DefaultIfEmpty()
                      select new WarehouseCardListDto
                      {
                        Id = q.Id,
                        Code = q.Code,
                        CreationTime = q.CreationTime,
                        CreatorUserId = q.CreatorUserId,
                        WarehouseDestinationId = q.WarehouseDestinationId,
                        ItemsName = item.Name,
                        ItemsId = item.Id,
                        UnitId = q.UnitId,
                        UnitName = unit.Name,
                        WarehouseCardDetail = WarehouseCardDetail

                      }).ToList();
        var itemscount = query.Count();
        var itemslist = ObjectMapper.Map<List<WarehouseCardListDto>>(output);
        return new PagedResultDto<WarehouseCardListDto>(
             itemscount,
             itemslist.ToList()
             );
      }
      catch (Exception ex)
      {

        throw new UserFriendlyException(ex.Message);
      };
    }


    public async Task<WarehouseCardListDto> GetAsync(EntityDto<long> itemId)
    {
      try
      {
        var item = _WarehouseCardRepository.Get(itemId.Id);
        WarehouseCardListDto newItem = ObjectMapper.Map<WarehouseCardListDto>(item);
        return newItem;
      }
      catch (Exception ex)
      {

        throw new UserFriendlyException(ex.Message);
      }
    }

  
    public async Task<long> Update(WarehouseCardCreate input)
    {
      WarehouseCard items = await _WarehouseCardRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

      input.CreationTime = items.CreationTime;
      input.Code = items.Code;
      input.CreatorUserId = items.CreatorUserId;
      ObjectMapper.Map(input, items);
      await _WarehouseCardRepository.UpdateAsync(items);
      return items.Id;
    }
   
  }
}
