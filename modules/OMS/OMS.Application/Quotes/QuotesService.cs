using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;


namespace bbk.netcore.mdl.OMS.Application.Quotes
{
    public class QuotesService : ApplicationService, IQuotesService
    {
        private readonly IRepository<Quote, long> _Quoterepository;
        private readonly IRepository<Items, long> _Itemsrepository;
        private readonly IRepository<Supplier> _supplierrepository;


        public QuotesService(IRepository<Quote, long> Quoterepository,
                             IRepository<Items, long> Itemsrepository,
                             IRepository<Supplier> supplierrepository)
        {
            _Quoterepository = Quoterepository;
            _Itemsrepository = Itemsrepository;
            _supplierrepository = supplierrepository;
        }

        public async Task<long> Create(QuoteListDto input)
        {
            try
            {

                Quote newItemId = ObjectMapper.Map<Quote>(input);
                newItemId.Quantity = input.QuantityQuote;
                var newId = await _Quoterepository.InsertAndGetIdAsync(newItemId);
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
                await _Quoterepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ItemsListDto>> GetAll(QuoteSearch input)
        {
            try
            {


                var queryQuoter = _Quoterepository.GetAll().Where(x=>x.CreatorUserId==AbpSession.UserId).Select(x => new { x.ItemId })
                                                .Distinct().ToList();

                var queryQuoterCrete = _Quoterepository.GetAll().OrderByDescending(x => x.CreationTime);

                var query1 = (from i in queryQuoter
                              let time = (from cre in queryQuoterCrete where i.ItemId == cre.ItemId select cre.CreationTime).ToList()
                              select new ItemsListDto
                              {
                                  Id = i.ItemId,
                                  CreationTime = time[0],

                              }); 

                var queryItems = _Itemsrepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Name.Contains(input.SearchTerm) || x.ItemCode.Contains(input.SearchTerm));
                var Quoterlist = (from query12 in query1
                                  join item in queryItems on query12.Id equals item.Id
                                  select new ItemsListDto
                                  {
                                      Id = item.Id,
                                      CreationTime = query12.CreationTime,
                                      Name = item.ItemCode + "-" + item.Name
                                  }).ToList();
           
                var Quotercount = Quoterlist.Count();


                return new PagedResultDto<ItemsListDto>(
                     Quotercount,
                     Quoterlist.Distinct().ToList()
                     );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<long> Update(QuoteListDto input)
        {
            Quote items = await _Quoterepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            // input.ItemCode = code() +items.ItemCode.Substring(6, items.ItemCode.Length - 6);
            input.CreatorUserId = items.CreatorUserId;
            input.CreationTime = items.CreationTime;
            ObjectMapper.Map(input, items);
            await _Quoterepository.UpdateAsync(items);
            return input.Id;
        }

        public async Task<QuoteListDto> GetAsync(EntityDto<long> itemId)
        {
            var item = _Quoterepository.Get(itemId.Id);
            QuoteListDto newItem = ObjectMapper.Map<QuoteListDto>(item);
            newItem.QuantityQuote = item.Quantity;
            return newItem;
        }

        public async Task<PagedResultDto<QuoteListDto>> GetQuotebyItemsid(QuoteSearch input)
        {
            try
            {
                List<QuoteListDto> quoteListDtos = new List<QuoteListDto>();    
                var queryQuoter = _Quoterepository.GetAll()
                                                  .Where(x=>x.CreatorUserId ==AbpSession.UserId)
                                                  .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Note.Contains(input.SearchTerm))
                                                  .Select(x => new { x.Id, x.Note, x.ItemId, x.CreationTime, x.Specifications, x.UnitId, x.QuotePrice, x.SupplierName , x.UnitName , x.Quantity})
                                                  .OrderByDescending(x => x.CreationTime).ToList();
                var queryItems = _Itemsrepository.GetAll().Where(x => x.Id == input.ItemsId);
                var Quoterlist = (from quote in queryQuoter
                                  join item in queryItems on quote.ItemId equals item.Id
                                  select new QuoteListDto
                                  {
                                      Id = quote.Id,
                                      ItemName = item.ItemCode + "-" + item.Name,
                                      CreationTime = quote.CreationTime,
                                      Specifications = quote.Specifications,
                                      UnitName = quote.UnitName,
                                      QuotePrice = quote.QuotePrice,
                                      Note = quote.Note,
                                      SupplierName = quote.SupplierName,
                                      QuantityQuote = quote.Quantity,
                                      ItemId = quote.ItemId,


                                  }).ToList();
                //var Quoterlist = ObjectMapper.Map<List<QuoteListDto>>(queryQuoter);
                var Quotercount = Quoterlist.Count();
                var qq = Quoterlist.Distinct().OrderByDescending(x=>x.CreationTime).GroupBy(x => x.SupplierName).ToList();
                foreach (var item in qq)
                {
                    quoteListDtos.Add(item.First());
                }

                return new PagedResultDto<QuoteListDto>(
                     Quotercount,
                     quoteListDtos.Distinct().ToList()
                     );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<QuoteListDto>> GetAllHistory(QuoteSearch input)
        {
            try
            {
                var queryQuoter = _Quoterepository.GetAll().Where(x=>x.CreatorUserId == AbpSession.UserId);
                var queryItems = _Itemsrepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Name.Contains(input.SearchTerm) || x.ItemCode.Contains(input.SearchTerm));
                var Quoterlist = (from quote in queryQuoter
                                  join item in queryItems on quote.ItemId equals item.Id
                                  select new QuoteListDto
                                  {
                                      ItemId = item.Id,
                                      CreationTime = quote.CreationTime,
                                      ItemName = item.ItemCode + "-" + item.Name,
                                      Id = quote.Id,
                                      SupplierName = quote.SupplierName,
                                  });
                var getCode = Quoterlist.Select(x => x.ItemName).Distinct().ToArray();
                var reusltEnd = (from i in getCode
                              //   let idset = (from que in Quoterlist where i == que.ItemName select que.ItemId).ToArray() 
                                 let IdArr = (from que in Quoterlist where i == que.ItemName select  que.CreationTime ).Distinct().ToList()
                                 select new QuoteListDto
                                 {
                                  ///   Id = idset[0],
                                     ItemName = i,
                                     //  QuoteHístoryList = IdArr
                                     QuoteDate = IdArr

                                 });
                var Quotercount = _Quoterepository.Count();


                return new PagedResultDto<QuoteListDto>(
                     Quotercount,
                     Quoterlist.Distinct().ToList()
                     );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<PagedResultDto<QuoteListDto>> GetHistoryDetail(QuoteSearch input)
        {
            try
            {
                var queryQuoter = _Quoterepository.GetAll().Where(x=>x.Id== input.Id.Value)
                                                .Distinct().ToList();
                var queryItems = _Itemsrepository.GetAll();
                var Quoterlist = (from quote in queryQuoter
                                  join item in queryItems on quote.ItemId equals item.Id
                                  select new QuoteListDto
                                  {
                                      Id = item.Id,
                                      CreationTime = quote.CreationTime,
                                      ItemName = item.ItemCode + "-" + item.Name,
                                      SupplierName =quote.SupplierName,
                                      Specifications = quote.Specifications,
                                      UnitId = quote.UnitId,
                                      UnitName = quote.UnitName,
                                      QuotePrice = quote.QuotePrice,
                                      Note = quote.Note,
                                  }).ToList();
                var Quotercount = _Quoterepository.Count();


                return new PagedResultDto<QuoteListDto>(
                     Quotercount,
                     Quoterlist.Distinct().ToList()
                     );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<QuoteListDto>> GetHisByHa(QuoteSearch input)
        {
            try
            {
                var queryItem = _Itemsrepository.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Name.Contains(input.SearchTerm) || x.ItemCode.Contains(input.SearchTerm));
                var queryQto = _Quoterepository
                                            .GetAll()
                                            .WhereIf(!string.IsNullOrEmpty(input.Year), x => x.CreationTime.Year.ToString().Equals(input.Year))
                                            .WhereIf(input.Quy.HasValue && input.Quy == 1, x => 1 <= x.CreationTime.Month && x.CreationTime.Month <= 3)
                                            .WhereIf(input.Quy.HasValue && input.Quy == 2, x => 4 <= x.CreationTime.Month && x.CreationTime.Month <= 6)
                                            .WhereIf(input.Quy.HasValue && input.Quy == 3, x => 7 <= x.CreationTime.Month && x.CreationTime.Month <= 9)
                                            .WhereIf(input.Quy.HasValue && input.Quy == 4, x => 10 <= x.CreationTime.Month && x.CreationTime.Month <= 12);

                var queryItem1 = _Itemsrepository.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Name.Contains(input.SearchTerm) || x.ItemCode.Contains(input.SearchTerm));
                queryQto = from qto in queryQto
                           join item in queryItem1 on qto.ItemId equals item.Id
                           select qto;
                var queryHis = queryQto.Select(x => new { x.ItemId, x.SupplierName }).Distinct().ToArray();

                var resultHis = (from hi in queryHis
                                 let arrTime = (from quo in queryQto where hi.ItemId == quo.ItemId && hi.SupplierName == quo.SupplierName select new TimeAndId {IdQuotes = quo.Id,TimeCre = quo.CreationTime }).ToList()
                                 select new QuoteHistory
                                 {
                                     ItemId = hi.ItemId,
                                     SupplierName = hi.SupplierName,
                                     timeAndIds = arrTime,
                                 }).ToList();

                var arrId = queryQto.Select(x => x.ItemId).Distinct().ToArray();

                var query = (from idT in arrId
                             let nameItem = (from item in queryItem where idT == item.Id select item.Name).ToArray()
                             let nameCode = (from item in queryItem where idT == item.Id select item.ItemCode).ToArray()
                             let arrHis = (from his in resultHis where idT == his.ItemId select new QuoteHistory { ItemId = his.ItemId, SupplierName = his.SupplierName,timeAndIds = his.timeAndIds}).ToList()
                           
                             select new QuoteListDto
                             {
                                 Id = idT,
                                 ItemName = nameItem[0],
                                 ItemCode = nameCode[0],
                                 QuoteHistoryList = arrHis
                             }).ToList();

                return new PagedResultDto<QuoteListDto>(query.Count(), query);


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


      
    }
}
