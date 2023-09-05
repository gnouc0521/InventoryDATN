using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.InventoryTickets.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.InventoryTickets
{
    [AbpAuthorize]
    public class InventoryTicketService : ApplicationService, IInventoryTicketService
    {
        private readonly IRepository<InventoryTicket> _inventoryTicket;
        private readonly IRepository<User, long> _user;
        public InventoryTicketService(IRepository<InventoryTicket> inventoryTicket, IRepository<User, long> user)
        {
            _inventoryTicket = inventoryTicket;
            _user = user;
        }

        public async Task<PagedResultDto<InventoryTicketListDto>> GetAll(GetInventoryTicketInput input)
        {
            try
            {
                var query = _inventoryTicket
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Code.Contains(input.SearchTerm))
                      .WhereIf(!string.IsNullOrEmpty(input.ResquestDate), u => u.CreationTime == DateTime.Parse(input.ResquestDate))
                      .WhereIf(input.WarehouseId != 0, u => u.WarehouseId == input.WarehouseId)
                      .WhereIf(input.CreatorById != 0, u => u.CreatorUserId == input.CreatorById)
                      .OrderBy(x => x.Id);
                var user = _user.GetAll().ToList();
                var results = (from imp in query
                               join u in user on imp.CreatorUserId equals u.Id
                               select new InventoryTicketListDto
                               {
                                   Id = imp.Id,
                                   Code= imp.Code,
                                   CreatedBy = u.Surname + " " + u.Name,
                                   CreationTime= imp.CreationTime,
                                   
                                 
                               }).OrderByDescending(x => x.Id);

                return new PagedResultDto<InventoryTicketListDto>(
                  results.Distinct().Count(),
                  results.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Create(InventoryTicketCreateDto input)
        {
            try
            {
                var query = await _inventoryTicket.GetAll().ToListAsync();
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
                   
                    if (i < 10) return "KK" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "KK" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "KK" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
                    else
                        return "KK" + Thang + DateTime.Now.Year + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _inventoryTicket.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                input.Code = sinhma(ma.ToString());
                InventoryTicket newItemId = ObjectMapper.Map<InventoryTicket>(input);
                var newId = await _inventoryTicket.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        public async Task<long> Update(InventoryTicketListDto input)
        {
            InventoryTicket IVTK = await _inventoryTicket.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.CreationTime = IVTK.CreationTime;
            input.Code = IVTK.Code;
            input.CreatorUserId = IVTK.CreatorUserId;
            ObjectMapper.Map(input, IVTK);
            await _inventoryTicket.UpdateAsync(IVTK);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _inventoryTicket.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<InventoryTicketListDto> GetAsync(EntityDto itemId)
        {
            var item = _inventoryTicket.Get(itemId.Id);
            InventoryTicketListDto newItem = ObjectMapper.Map<InventoryTicketListDto>(item);
            return newItem;
        }
    }
}
