using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Application.Orders.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseLocationItem.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace bbk.netcore.mdl.OMS.Application.Orders
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        private readonly IRepository<User, long> _user;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<QuoteSynthesise, long> _quoteSynthesiseRepository;
        private readonly IRepository<QuoteRequest, long> _quoteRequestRepository;
        private readonly IRepository<Items, long> _itemsRepository;
        private readonly IRepository<Assignment> _assigmentRespository;
        private readonly IRepository<Expert> _expertRepository;
        private readonly IRepository<ImportRequestSubsidiary> _ImportRequestSubsidiary;
        private readonly IRepository<ImportRequest> _ImportRequest;
        private readonly IRepository<ImportRequestDetail> _ImportRequestDetail;
        private readonly IRepository<Supplier> _supplierRepository;

        public OrderAppService(IRepository<Order, long> orderRepository,
            IRepository<OrderDetail, long> orderDetailRepository,                                          
                                          IRepository<Contract> contractRepository,
                                          IRepository<QuoteSynthesise, long> quoteSynthesiseRepository,
                                          IRepository<QuoteRequest, long> quoteRequestRepository,
                                          IRepository<Items, long> itemsRepository,
                                          IRepository<Assignment> assigmentRespository,
                                          IRepository<Expert> expertRepository,
                                          IRepository<ImportRequestSubsidiary> importRequestSubsidiary,
                                          IRepository<ImportRequest> importRequest,
                                          IRepository<ImportRequestDetail> importRequestDetail,
                                          IRepository<Supplier> supplierRepository,
                                          IRepository<User, long> user)
        {
            _orderRepository = orderRepository;
            _contractRepository = contractRepository;
            _quoteRequestRepository = quoteRequestRepository;
            _quoteSynthesiseRepository = quoteSynthesiseRepository;
            _itemsRepository = itemsRepository;
            _assigmentRespository = assigmentRespository;
            _expertRepository = expertRepository;
            _orderDetailRepository = orderDetailRepository;
            _ImportRequestSubsidiary = importRequestSubsidiary;
            _ImportRequest = importRequest;
            _ImportRequestDetail = importRequestDetail;
            _supplierRepository = supplierRepository;
            _user = user;
        }

        public async Task<long> Create(OrderListDto input)
        {
            try
            {
                var query = await _orderRepository.GetAll().ToListAsync();
                string sinhma(string ma)
                {
                    string s = ma.Substring(3, ma.Length - 3);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "PO-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "PO-" + "0" + Convert.ToString(i);
                    else return "PO-" + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "000000";
                }
                else
                {
                    ma = _orderRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                input.OrderCode = sinhma(ma.ToString());
                Order newItemId = ObjectMapper.Map<Order>(input);
                newItemId.Code = input.OrderCode;
                newItemId.OrderStatus = PurchasesRequestEnum.OrderStatus.Draft;
                var newId = await _orderRepository.InsertAndGetIdAsync(newItemId);


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
                await _orderRepository.DeleteAsync(id);
                return id;

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<OrderListDto>> GetAll(OrderDetailSearch input)
        {
            try
            {
                List<OrderListDto> orderListDtos = new List<OrderListDto>();
                var queryorder = _orderRepository.GetAll().Select(x => x.Code).Distinct().ToList();
                foreach (var item in queryorder)
                {
                    var first = _orderRepository.GetAll().Where(x => x.Code == item).First();
                    var output = ObjectMapper.Map<OrderListDto>(first);
                    output.OrderCode = first.Code;
                    orderListDtos.Add(output);
                }
                //GetUsersInput getUsersInput = new GetUsersInput();
                //getUsersInput.MaxResultCount = 1000;
                //var user = _userAppService.GetUsers(getUsersInput).Result.Items.ToList();
                var user = _user.GetAll().ToList();
                var querysupplier = _supplierRepository.GetAll();
                var querycontract = _contractRepository.GetAll().Where(x => x.Status == ContractEnum.ContractStatus.Contract);
                var ouput = (from item in user
                             join orderListDto in orderListDtos on item.Id equals orderListDto.UserId
                             join contract in querycontract on orderListDto.ContractId equals contract.Id
                             join supplier in querysupplier on contract.SupplierId equals supplier.Id
                             select new OrderListDto
                             {
                                 Id = orderListDto.Id,
                                 OrderCode = orderListDto.OrderCode,
                                 ExpertName = item.Surname + " " + item.Name,
                                 CreationTime = orderListDto.CreationTime,
                                 OrderStatus = orderListDto.OrderStatus,
                                 ContractCode = contract.Code,
                                 SupplierName = supplier.Name,
                             }).OrderByDescending(x=>x.OrderStatus).ThenByDescending(x=>x.Id).ToList();
                return new PagedResultDto<OrderListDto>(
                ouput.Count,
                ouput
                ); ;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<QuoteListDto>> GetAllDetail(OrderDetailSearch input)
        {
            try
            {
                List<QuoteListDto> orderListDtos = new List<QuoteListDto>();
                var queryorder = _orderRepository.GetAll().Where(x => x.Id == input.Id).Select(x => x.Id).Distinct().First();
                var queryorderdetail = _orderDetailRepository.GetAll().Where(x => x.OrderId == queryorder).Distinct().ToList();
                var queryItems = _itemsRepository.GetAll();
                var output1 = (from orderdetail in queryorderdetail
                               join item in queryItems on orderdetail.ItemId equals item.Id
                               select new QuoteListDto
                               {
                                   QuantityQuote = orderdetail.Quantity,
                                   ItemName = item.ItemCode + "-" + item.Name,
                                   UnitName = orderdetail.UnitName,
                                   QuotePrice = orderdetail.OrderPrice,
                                   Note = orderdetail.Note,

                               }).ToList();
                return new PagedResultDto<QuoteListDto>(
                output1.Count,
                output1
                ); ;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task<bool> CheckContract(OrderDetailSearch input)
        {
            bool draft = true;
            var querycontract = _contractRepository.GetAll().Where(x => x.Status == ContractEnum.ContractStatus.Contract && x.Id == input.Id).ToList();
            var queryorder = _orderRepository.GetAll().Where(x => x.ContractId == input.Id);
            var queryquoteSyn = _quoteSynthesiseRepository.GetAll();
            var queryItems = _itemsRepository.GetAll();
            var assigment = _assigmentRespository.GetAll();
            var expert = _expertRepository.GetAll();
            var queryquoteReq = _quoteRequestRepository.GetAll().Where(x => x.QuotesSynthesiseId == querycontract[0].QuoteSynId && x.SupplierId == querycontract[0].SupplierId);
            var queryOrderDetail = _orderDetailRepository.GetAll();
            var orderOfContract = (from order in queryorder
                                   join orderdetail in queryOrderDetail on order.Id equals orderdetail.OrderId
                                   select orderdetail).ToList();
            if (orderOfContract.Count() > 0)
            {
                var results = (from line in orderOfContract
                               group line by line.ItemId into g
                               select new QuoteListDto
                               {
                                   ItemId = g.Last().ItemId,
                                   QuantityQuote = g.Sum(pc => (long)pc.Quantity),
                               }).ToList();
                var output = (from contract in querycontract
                              join quoteSyn in queryquoteSyn on contract.QuoteSynId equals quoteSyn.Id
                              join quoteReq in queryquoteReq on quoteSyn.Id equals quoteReq.QuotesSynthesiseId
                              join item in queryItems on quoteReq.ItemId equals item.Id

                              select new QuoteListDto
                              {
                                  ItemName = item.ItemCode + "-" + item.Name,
                                  UnitName = quoteReq.UnitName,
                                  QuantityQuote = quoteReq.Quantity,
                                  QuotePrice = quoteReq.QuotePrice,
                                  //    NameStaff = exp.Name,
                                  //    UserId = exp.UserId,
                                  Id = quoteReq.Id,
                                  ItemId = item.Id,
                                  ContractId = contract.Id,
                              }).ToList();


                var end = (from outputs in output
                           join result in results on outputs.ItemId equals result.ItemId into g
                           from subpet in g.DefaultIfEmpty()
                           select new QuoteListDto
                           {
                               ItemName = outputs.ItemName,
                               UnitName = outputs.UnitName,
                               QuotePrice = outputs.QuotePrice,
                               Id = outputs.Id,
                               ContractId = outputs.Id,
                               QuantityQuote = outputs.QuantityQuote - subpet?.QuantityQuote ?? outputs.QuantityQuote,
                           }).Where(x => x.QuantityQuote > 0).ToList();

                if (end.Count() > 0)
                {
                    draft = false;
                }
            }
            else
            {
                draft = false;
            }
            return draft;

        }

        public async Task<PagedResultDto<QuoteListDto>> GetAssignment(OrderDetailSearch input)
        {
            try
            {
                var querycontract = _contractRepository.GetAll().Where(x => x.Status == ContractEnum.ContractStatus.Contract && x.Id == input.Id).ToList(); ;
                var queryorder = _orderRepository.GetAll().Where(x => x.ContractId == input.Id);
                var queryquoteSyn = _quoteSynthesiseRepository.GetAll();
                var queryItems = _itemsRepository.GetAll();
                var assigment = _assigmentRespository.GetAll();
                var expert = _expertRepository.GetAll();
                var queryquoteReq = _quoteRequestRepository.GetAll().Where(x=>x.QuotesSynthesiseId == querycontract[0].QuoteSynId && x.SupplierId == querycontract[0].SupplierId);
                var queryOrderDetail = _orderDetailRepository.GetAll();

                // tìm các order đã được tạo từ hợp đồng trước đó
                var orderOfContract = (from order in queryorder
                                       join orderdetail in queryOrderDetail on order.Id equals orderdetail.OrderId
                                       select orderdetail).ToList();
                // nếu có order 
                if (orderOfContract.Count() > 0)
                {


                    var results = (from line in orderOfContract
                                   group line by line.ItemId into g
                                   select new QuoteListDto
                                   {
                                       ItemId = g.Last().ItemId,
                                       QuantityQuote = g.Sum(pc => (long)pc.Quantity),
                                   }).ToList();
                    var output = (from contract in querycontract
                                  join quoteSyn in queryquoteSyn on contract.QuoteSynId equals quoteSyn.Id
                                  join quoteReq in queryquoteReq on quoteSyn.Id equals quoteReq.QuotesSynthesiseId
                                  join item in queryItems on quoteReq.ItemId equals item.Id

                                  select new QuoteListDto
                                  {
                                      ItemName = item.ItemCode + "-" + item.Name,
                                      UnitName = quoteReq.UnitName,
                                      QuantityQuote = quoteReq.Quantity,
                                      QuotePrice = quoteReq.QuotePrice,
                                      //    NameStaff = exp.Name,
                                      //    UserId = exp.UserId,
                                      Id = quoteReq.Id,
                                      ItemId = item.Id,
                                      ContractId = contract.Id,
                                      Specifications = quoteReq.Specifications
                                  }).ToList();


                    var end = (from outputs in output
                               join result in results on outputs.ItemId equals result.ItemId into g
                               from subpet in g.DefaultIfEmpty()
                               select new QuoteListDto
                               {
                                   ItemName = outputs.ItemName,
                                   UnitName = outputs.UnitName,
                                   QuotePrice = outputs.QuotePrice,
                                   Id = outputs.Id,
                                   ContractId = outputs.Id,
                                   QuantityQuote = outputs.QuantityQuote - subpet?.QuantityQuote ?? outputs.QuantityQuote,
                                   ItemId = outputs.ItemId,
                                   Specifications = outputs.Specifications
                               }).Where(x => x.QuantityQuote > 0).Distinct().ToList();

                    return new PagedResultDto<QuoteListDto>(
                        end.Count(),
                        end
                        );

                }
                else
                {


                    var output = (from contract in querycontract
                                  join quoteSyn in queryquoteSyn on contract.QuoteSynId equals quoteSyn.Id
                                  join quoteReq in queryquoteReq on  quoteSyn.Id  equals quoteReq.QuotesSynthesiseId 
                                  join item in queryItems on quoteReq.ItemId equals item.Id
                                  select new QuoteListDto
                                  {
                                      ItemName = item.ItemCode + "-" + item.Name,
                                      UnitName = quoteReq.UnitName,
                                      QuantityQuote = quoteReq.Quantity,
                                      QuotePrice = quoteReq.QuotePrice,
                                      //NameStaff = exp.Name,
                                     // UserId = exp.UserId,
                                      Id = quoteReq.Id,
                                      ItemId = item.Id,
                                      ContractId = contract.Id,
                                      Specifications = quoteReq.Specifications,
                                  }).Distinct().ToList();


                    return new PagedResultDto<QuoteListDto>(
                        output.Count(),
                        output
                        );
                }

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<OrderListDto> GetAsync(EntityDto<long> itemId)
        {
            try
            {
                var item = _orderRepository.Get(itemId.Id);
                OrderListDto newItem = ObjectMapper.Map<OrderListDto>(item);
                newItem.OrderCode = item.Code;
                return newItem;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(OrderListDto input)
        {
            Order items = await _orderRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.OrderCode = items.Code;
            input.ContractId = items.ContractId;
            input.UserId = items.UserId;
            input.QuoteId = items.QuoteId;
            input.CreatorUserId = items.CreatorUserId;
            input.CreationTime = items.CreationTime;
            ObjectMapper.Map(input, items);
            await _orderRepository.UpdateAsync(items);
            return input.Id;
        }
        public async Task<long> UpdateCreate(OrderListDto input)
        {
            try
            {
                var query = await _orderRepository.GetAll().Where(x => x.Code != null).ToListAsync();
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "000000";
                }
                else
                {
                    ma = _orderRepository.GetAll().Where(x => x.Code != null).OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }
                if (ma == null)
                {
                    ma = "000000";
                }

                input.OrderCode = sinhma(ma.ToString());

                var querywithcontractid = await _orderRepository.GetAll().Where(x => x.ContractId == input.ContractId).ToListAsync();
                var querycontractuserid = querywithcontractid.Select(x => x.UserId).Distinct().ToList();

                foreach (var order in querycontractuserid)
                {
                    foreach (var item in querywithcontractid)
                    {

                        if (item.UserId == order)
                        {
                            item.Code = input.OrderCode;
                            var newId = await _orderRepository.UpdateAsync(item);
                        }
                    }
                    //ma = _orderRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                    input.OrderCode = sinhma(input.OrderCode.ToString());
                }
                string sinhma(string ma)
                {
                    string s = ma.Substring(3, ma.Length - 3);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "PO-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "PO-" + "0" + Convert.ToString(i);
                    else return "PO-" + Convert.ToString(i);

                }
                return input.Id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        ///kien them


        public async Task<PagedResultDto<QuoteListDto>> GetAllDetailSL(OrderDetailSearch input)
        {
            try
            {
                List<QuoteListDto> orderListDtos = new List<QuoteListDto>();
                var queryorder = _orderRepository.GetAll().Where(x => x.Id == input.Id).Select(x => x.Id).Distinct().First();
                var queryorderdetail = _orderDetailRepository.GetAll().Where(x => x.OrderId == queryorder).Distinct().ToList();
                var queryItems = _itemsRepository.GetAll();
                var imp = _ImportRequest.GetAll();
                var impd = _ImportRequestDetail.GetAll();
                var impSd = _ImportRequestSubsidiary.GetAll();
                var output1 = (from orderdetail in queryorderdetail
                               join item in queryItems on orderdetail.ItemId equals item.Id
                               join isd in impSd on orderdetail.OrderId equals isd.OrderId
                               join id in imp on isd.Id equals id.ImportRequestSubsidiaryId
                               join imd in impd on orderdetail.ItemId equals imd.ItemId
                               where imd.ItemId == orderdetail.ItemId
                               select new QuoteListDto
                               {
                                   QuantityQuote = orderdetail.Quantity,
                                   ItemName = item.ItemCode + "-" + item.Name,
                                   UnitName = orderdetail.UnitName,
                                   QuotePrice = orderdetail.OrderPrice,
                                   Note = orderdetail.Note,
                                   QuantityHT = imd.QuantityHT

                               }).ToList();
                var CodePu = output1.Select(x => x.ItemName).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let QuantityQuote = (from z in output1 where i == z.ItemCode select z.QuantityQuote).Distinct().ToList()
                               let UnitName = (from z in output1 where i == z.ItemCode select z.UnitName).Distinct().ToList()
                               let QuotePrice = (from z in output1 where i == z.ItemCode select z.QuotePrice).Distinct().ToList()
                               let Note = (from z in output1 where i == z.ItemCode select z.Note).Distinct().ToList()
                               let QuantityHT = (from z in output1 where i == z.ItemCode select z.QuantityHT).Distinct().ToList()
                               select new QuoteListDto
                               {
                                   ItemName = i,
                                   QuantityQuote = QuantityQuote[0],
                                   UnitName = UnitName[0],
                                   QuotePrice = QuotePrice[0],
                                   Note = Note[0],
                                   QuantityHT = QuantityHT[0]
                               }
                                ).ToList();

                return new PagedResultDto<QuoteListDto>(
                Haquery.Count,
                Haquery
                ); ;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task<long> UpdateStatusSL(OrderListDto input)
        {

            Order items = await _orderRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            items.OrderStatus = PurchasesRequestEnum.OrderStatus.Done;

            await _orderRepository.UpdateAsync(items);
            return items.Id;
        }

        public async Task<long> UpdateStatus(OrderListDto input)
        {

            Order items = await _orderRepository.FirstOrDefaultAsync(x => x.Id == input.OrderId);

            items.StatusCreateYCNK = true;

            await _orderRepository.UpdateAsync(items);
            return items.Id;
        }
    }
}
