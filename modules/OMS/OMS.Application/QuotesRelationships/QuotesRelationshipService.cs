using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.QuotesRelationships.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace bbk.netcore.mdl.OMS.Application.QuotesRelationships
{
    public class QuotesRelationshipService : ApplicationService, IQuotesRelationshipService
    {
        private readonly IRepository<QuoteRelationship, long> _QuoteRelationshiprepository;
        private readonly IRepository<Items, long> _Itemsrepository;
        private readonly IRepository<Supplier> _supplierrepository;


        public QuotesRelationshipService(IRepository<QuoteRelationship, long> QuoteRelationshiprepository,
                             IRepository<Items, long> Itemsrepository,
                             IRepository<Supplier> supplierrepository)
        {
           _QuoteRelationshiprepository = QuoteRelationshiprepository;
            _Itemsrepository = Itemsrepository;
            _supplierrepository = supplierrepository;
        }

        public async Task<long> Create(QuotesRelationshipListDto input)
        {
            try
            {
                QuoteRelationship newItemId = ObjectMapper.Map<QuoteRelationship>(input);
                var newId = await _QuoteRelationshiprepository.InsertAndGetIdAsync(newItemId);
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
                await _QuoteRelationshiprepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
 
        //public async Task<PagedResultDto<ItemsListDto>> GetAll(QuoteSearch input)
        //{
        //    try
        //    {
        //        var queryQuoter = _QuoteRelationshiprepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Note.Contains(input.SearchTerm))
        //                                        .ToList();
        //        var queryItems = _Itemsrepository.GetAll();
        //        var Quoterlist = (from quote in queryQuoter 
        //                          join item in queryItems on quote.ItemId equals item.Id
        //                          select new ItemsListDto
        //                          {
        //                              Id = item.Id,
        //                              CreationTime = quote.CreationTime.Date,
        //                              Name = item.ItemCode + "-" + item.Name
        //                          }).ToList();
        //            //var Quoterlist = ObjectMapper.Map<List<QuoteListDto>>(queryQuoter);
        //        var Quotercount = _QuoteRelationshiprepository.Count();
  

        //        return new PagedResultDto<ItemsListDto>(
        //             Quotercount,
        //             Quoterlist.Distinct().ToList()
        //             );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}


        public async Task<long> Update(QuotesRelationshipListDto input)
        {
            QuoteRelationship items = await _QuoteRelationshiprepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            // input.ItemCode = code() +items.ItemCode.Substring(6, items.ItemCode.Length - 6);
            input.CreatorUserId = items.CreatorUserId;
            input.CreationTime = items.CreationTime;
            ObjectMapper.Map(input, items);
            await _QuoteRelationshiprepository.UpdateAsync(items);
            return input.Id;
        }

       public async Task<QuoteListDto> GetAsync(EntityDto<long> itemId)
        {
            var item = _QuoteRelationshiprepository.Get(itemId.Id);
            QuoteListDto newItem = ObjectMapper.Map<QuoteListDto>(item);
            return newItem;
        }

      
        //public async Task<PagedResultDto<QuoteListDto>> GetQuotebyItemsid(QuoteSearch input)
        //{
        //    try
        //    {
        //        var queryQuoter = _QuoteRelationshiprepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Note.Contains(input.SearchTerm))
        //                                        .ToList();
        //        var queryItems = _Itemsrepository.GetAll().Where(x=>x.Id==input.ItemsId);
        //        var Quoterlist = (from quote in queryQuoter
        //                          join item in queryItems on quote.ItemId equals item.Id
        //                          select new QuoteListDto
        //                          {
        //                              Id = quote.Id,
        //                              ItemName = item.ItemCode + "-" + item.Name,
        //                              CreationTime = quote.CreationTime ,
        //                              Specifications = quote.Specifications,
        //                              UnitName = quote.UnitId.ToString(),
        //                              QuotePrice = quote.QuotePrice,
        //                              Note = quote.Note,
        //                              SupplierName = quote.SupplierName,

        //                          }).ToList();
        //        //var Quoterlist = ObjectMapper.Map<List<QuoteListDto>>(queryQuoter);
        //        var Quotercount = Quoterlist.Count();


        //        return new PagedResultDto<QuoteListDto>(
        //             Quotercount,
        //             Quoterlist.Distinct().ToList()
        //             );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}
    }
}
