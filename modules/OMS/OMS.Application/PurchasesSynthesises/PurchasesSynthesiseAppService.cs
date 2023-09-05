using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Export;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
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

namespace bbk.netcore.mdl.OMS.Application.PurchasesSynthesises
{
    public class PurchasesSynthesiseAppService : ApplicationService, IPurchasesSynthesiseAppService
    {
        private readonly IRepository<PurchasesSynthesise, long> _purchasesSynthesiseRepository;
        private readonly IRepository<User,long> _user;
        private readonly IRepository<PurchasesRequest, long> _purchasesRequestRepository;
        private readonly IRepository<PurchasesRequestDetail, long> _purchasesRequestDetailRepository;
        private readonly IRepository<Items, long> _itemsRepository;
        private readonly IRepository<Subsidiary, long> _subsidiaryRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<Assignment> _assigmentRespository;
        private readonly IRepository<PurchaseAssignment> _purchaseAssignmentRepository;
        private readonly IRepository<Expert> _expertRepository;
        private readonly ILogger<SendMailService> logger;
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        private readonly IRepository<Quote, long> _quoteRepository;
        private readonly IRepository<QuoteRequest, long> _quoteRequest;
        private readonly IRepository<QuoteSynthesise, long> _quoteSynthesise;
        private IHostingEnvironment _Environment;
        private readonly ISendMailService _sendMailService;
      
        private readonly IExportPurchaseAssignment _iExportPurchaseAssignment;
        public PurchasesSynthesiseAppService(IRepository<PurchasesSynthesise, long> purchasesSynthesiseRepository,
                                          IRepository<User, long> user,
                                          IRepository<PurchasesRequest, long> purchasesRequestRepository,
                                          IRepository<PurchasesRequestDetail, long> purchasesRequestDetailRepository,
                                          IRepository<Items, long> itemsRepository,
                                          IRepository<Unit> unitRepository,
                                          IRepository<Subsidiary, long> subsidiaryRepository,
                                          IRepository<Supplier> supplierRepository,
                                          IRepository<Assignment> assigmentRespository,
                                          IRepository<PurchaseAssignment> purchaseAssignmentRepository,
                                          IRepository<Expert> expertRepository,
                                          ILogger<SendMailService> _logger,
                                          IRepository<Order, long> orderRepository,
                                          IRepository<Quote, long> quoteRepository,
                                          IRepository<OrderDetail, long> orderDetailRepository,
                                          IRepository<QuoteRequest, long> quoteRequest,
                                          IRepository<QuoteSynthesise, long> quoteSynthesise,
                                          IHostingEnvironment Environment,
                                          ISendMailService sendMailService,
                                          IExportPurchaseAssignment iExportPurchaseAssignment)
        {
            _purchasesSynthesiseRepository = purchasesSynthesiseRepository;
            _purchasesRequestDetailRepository = purchasesRequestDetailRepository;
            _purchasesRequestRepository = purchasesRequestRepository;
            _itemsRepository = itemsRepository;
            _unitRepository = unitRepository;
            _subsidiaryRepository = subsidiaryRepository;
            _supplierRepository = supplierRepository;
            _assigmentRespository = assigmentRespository;
            _purchaseAssignmentRepository = purchaseAssignmentRepository;
            _expertRepository = expertRepository;
            logger = _logger;
            _orderRepository = orderRepository;
            _quoteRepository = quoteRepository;
            _orderDetailRepository = orderDetailRepository;
            _quoteRequest = quoteRequest;
            _quoteSynthesise = quoteSynthesise;
            _Environment = Environment;
            _sendMailService = sendMailService;
            _iExportPurchaseAssignment = iExportPurchaseAssignment;
            _user = user;
        }

        public async Task<long> Create(PurchasesSynthesisListDto input)
        {
            try
            {
                var query = await _purchasesSynthesiseRepository.GetAll().ToListAsync();
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
                    if (i < 10) return "TH" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "TH" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "TH" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
                    else
                        return "TH" + Thang + DateTime.Now.Year + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)
                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _purchasesSynthesiseRepository.GetAll().OrderByDescending(x => x.PurchasesSynthesiseCode).Select(x => x.PurchasesSynthesiseCode).ToList().First();
                }

                input.PurchasesSynthesiseCode = sinhma(ma.ToString());
                PurchasesSynthesise newItemId = ObjectMapper.Map<PurchasesSynthesise>(input);
                newItemId.StatusAssignment = false;
                var newId = await _purchasesSynthesiseRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<long>> Delete(long id)
        {
            try
            {
                await _purchasesSynthesiseRepository.DeleteAsync(id);
                var query = _purchasesRequestRepository.GetAll().Where(x => x.PurchasesSynthesiseId.Equals(id)).Select(x => x.Id).ToList();
                return query;

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetAll(PurchasesSynthesisSearch input)
        {
            try
            {

                var query = _purchasesSynthesiseRepository
                      .GetAll()
                      .Where(x => x.StatusAssignment == false)
                      .OrderBy(x => x.Id);
                var queryReq = _purchasesRequestRepository.GetAll();
                var querySub = _subsidiaryRepository.GetAll();
                var results = (from req in queryReq
                               join imp in query on req.PurchasesSynthesiseId equals imp.Id
                               join sub in querySub on req.SubsidiaryCompanyId equals sub.Id

                               select new PurchasesSynthesisListDto
                               {
                                   Id = imp.Id,
                                   CreationTime = imp.CreationTime,
                                   DeleterUserId = imp.CreatorUserId.Value,
                                   DeletionTime = imp.DeletionTime.Value,
                                   PurchasesSynthesiseCode = imp.PurchasesSynthesiseCode,
                                   SubsidiariesName = sub.NameCompany,
                                   PurchasesRequestStatus = imp.PurchasesRequestStatus,
                                   CreatorUserId = imp.CreatorUserId.Value,
                               });

                var CodePu = results.Select(x => x.PurchasesSynthesiseCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in results where i == z.PurchasesSynthesiseCode select z.Id).ToArray()
                               let nameArr = (from z in results where i == z.PurchasesSynthesiseCode select z.SubsidiariesName).Distinct().ToList()
                               let CreatimeTT = (from z in results where i == z.PurchasesSynthesiseCode select z.CreationTime).ToArray()
                               let PurchasesRequestStatus = (from z in results where i == z.PurchasesSynthesiseCode select z.PurchasesRequestStatus).ToArray()
                               let CreatorUserId = (from z in results where i == z.PurchasesSynthesiseCode select z.CreatorUserId).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   Id = idset[0],
                                   PurchasesSynthesiseCode = i,
                                   subsidiaries = nameArr,
                                   CreationTime = CreatimeTT[0],
                                   PurchasesRequestStatus = PurchasesRequestStatus[0],
                                   CreatorUserId = CreatorUserId[0]
                               }
                                );




                return new PagedResultDto<PurchasesSynthesisListDto>(
                  Haquery.Count(),
                  Haquery.OrderByDescending(x => x.Id).ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllItems(PurchasesSynthesisSearch input)
        {
            try
            {

                var querypurchasesSyn = _purchasesSynthesiseRepository.GetAll()
                                                                      .Where(x => x.Id.Equals(input.Id))
                                                                      .OrderBy(x => x.Id);
                var querypurchasesReq = _purchasesRequestRepository
                   .GetAll()
                   .OrderBy(x => x.Id);
                var querypurchasesDet = _purchasesRequestDetailRepository
                   .GetAll()
                   .OrderBy(x => x.Id);
                var querySubsidiary = _subsidiaryRepository.GetAll().ToList();
                var querySupplier = _supplierRepository.GetAll().ToList();
                var queryUnit = _unitRepository.GetAll().ToList();
                var queryItems = _itemsRepository.GetAll().ToList();

                var results = (from Req in querypurchasesReq
                               join Syn in querypurchasesSyn on Req.PurchasesSynthesiseId equals Syn.Id
                               join Det in querypurchasesDet on Req.Id equals Det.PurchasesRequestId
                               select new PurchasesSynthesisListDto
                               {
                                   Id = Syn.Id,
                                   //ItemsName = item.ItemCode + "/ " + item.Name,
                                   //SubsidiariesName = Sub.NameCompany,
                                   //UnitName = unit.Name,
                                   QuantityItems = Det.Quantity,
                                   Purpose = Det.Note,
                                   Note = Det.Note,
                                   DateTimeNeed = Det.TimeNeeded,
                                   SupplierId = Det.SupplierId,
                                   UnitId = Det.UnitId,
                                   SubsidiariesId = Req.SubsidiaryCompanyId,
                                   ItemsId = Det.ItemId,
                                   CreatorUserId = Syn.CreatorUserId,
                                   PurchasesDetailId = Det.Id,

                               }).ToList();
                results = (from result in results
                           join Sup in querySupplier on result.SupplierId equals Sup.Id
                           join unit in queryUnit on result.UnitId equals unit.Id
                           join Sub in querySubsidiary on result.SubsidiariesId equals Sub.Id
                           join Item in queryItems on result.ItemsId equals Item.Id

                           select new PurchasesSynthesisListDto
                           {
                               Id = result.Id,
                               ItemsName = Item.ItemCode + "/ " + Item.Name,
                               SubsidiariesName = Sub.NameCompany,
                               UnitName = unit.Name,
                               QuantityItems = result.QuantityItems,
                               Purpose = result.Note,
                               Note = result.Note,
                               DateTimeNeed = result.DateTimeNeed,
                               SupplierName = Sup.Name,
                               CreatorUserId = result.CreatorUserId,
                               PurchasesDetailId = result.PurchasesDetailId,

                           }).ToList();





                return new PagedResultDto<PurchasesSynthesisListDto>(
                  results.Distinct().Count(),
                  results.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PurchasesSynthesisListDto> GetAsync(EntityDto<long> itemId)
        {
            try
            {
                var item = _purchasesSynthesiseRepository.Get(itemId.Id);
                PurchasesSynthesisListDto newItem = ObjectMapper.Map<PurchasesSynthesisListDto>(item);
                var user = _user.FirstOrDefault(x=>x.Id == newItem.CreatorUserId);
                newItem.CreateName = user.Surname + " " + user.Name;
                //GetUsersInput getUsersInput = new GetUsersInput();
                //getUsersInput.MaxResultCount = 1000;
                //var querya = _userAppService.GetUsers(getUsersInput).Result;
                //for (int i = 0; i < querya.TotalCount; i++)
                //{
                //    if (querya.Items[i].Id == newItem.CreatorUserId)
                //    {
                //        newItem.CreateName = querya.Items[i].Surname + " " + querya.Items[i].Name;
                //        break;
                //    }
                //}
                return newItem;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(PurchasesSynthesisListDto input)
        {
            try
            {
                var IMP = await _purchasesSynthesiseRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                input.CreationTime = IMP.CreationTime;
                input.PurchasesSynthesiseCode = IMP.PurchasesSynthesiseCode;
                input.CreatorUserId = IMP.CreatorUserId;
                ObjectMapper.Map(input, IMP);
                await _purchasesSynthesiseRepository.UpdateAsync(IMP);
                return input.Id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }









        /// <summary>
        /// kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllItemByExpert(PurchasesSynthesisSearch input)
        {
            try
            {
                var purchasesSynthesise = _purchasesSynthesiseRepository.GetAll().Where(x => x.Id == input.Id);
                var purchasesRequestDetail = _purchasesRequestDetailRepository.GetAll();
                var assigment = _assigmentRespository.GetAll();
                var expert = _expertRepository.GetAll();
                var purchasesRequest = _purchasesRequestRepository.GetAll();

                var query = (from pucAss in purchasesSynthesise
                             join puc in purchasesRequest on pucAss.Id equals puc.PurchasesSynthesiseId
                             join pucDt in purchasesRequestDetail on puc.Id equals pucDt.PurchasesRequestId
                             join assi in assigment on pucDt.ItemId equals assi.ItemId into t
                             from assi in t.DefaultIfEmpty()
                             join exp in expert on assi.UserId equals exp.UserId into i
                             from exp in i.DefaultIfEmpty()

                             select new PurchasesSynthesisListDto
                             {
                                 Id = pucAss.Id,
                                 ItemsId = pucDt.ItemId,
                                 QuantityItems = pucDt.Quantity,
                                 Note = pucDt.Note,
                                 DateTimeNeed = pucDt.TimeNeeded,
                                 SupplierId = pucDt.SupplierId,
                                 UnitId = pucDt.UnitId,
                                 SubsidiariesId = puc.SubsidiaryCompanyId,
                                 NameStaff = exp.Name,
                                 UserId = assi.UserId,
                                 PurchasesSynthesiseCode = pucAss.PurchasesSynthesiseCode
                             }).ToList();


                var items = _itemsRepository.GetAll();
                var supplier = _supplierRepository.GetAll();
                var units = _unitRepository.GetAll();


                var result = (from res in query
                              join item in items on res.ItemsId equals item.Id
                              join sup in supplier on res.SupplierId equals sup.Id
                              join unit in units on res.UnitId equals unit.Id
                              select new PurchasesSynthesisListDto
                              {
                                  Id = res.Id,
                                  ItemsId = res.ItemsId,
                                  QuantityItems = res.QuantityItems,
                                  Note = res.Note,
                                  DateTimeNeed = res.DateTimeNeed,
                                  SupplierId = res.SupplierId,
                                  UnitId = res.UnitId,
                                  SubsidiariesId = res.SubsidiariesId,
                                  NameStaff = res.NameStaff,
                                  UserId = res.UserId,
                                  ItemsName = item.Name,
                                  Itemcode = item.ItemCode,
                                  SupplierName = sup.Name,
                                  UnitName = unit.Name,
                                  PurchasesSynthesiseCode = res.PurchasesSynthesiseCode
                              });

                var CodePu = result.Select(x => x.Itemcode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let Id = (from z in result where i == z.Itemcode select z.Id).ToArray()
                               let ItemsId = (from z in result where i == z.Itemcode select z.ItemsId).ToArray()
                               let QuantityItems = (from z in result where i == z.Itemcode select z.QuantityItems).ToArray()
                               let Note = (from z in result where i == z.Itemcode select z.Note).ToArray()
                               let DateTimeNeed = (from z in result where i == z.Itemcode select z.DateTimeNeed).ToArray()
                               let SupplierId = (from z in result where i == z.Itemcode select z.SupplierId).ToArray()
                               let UnitId = (from z in result where i == z.Itemcode select z.UnitId).ToArray()
                               let ItemsName = (from z in result where i == z.Itemcode select z.ItemsName).ToArray()
                               let Itemcode = (from z in result where i == z.Itemcode select z.Itemcode).ToArray()
                               let SupplierName = (from z in result where i == z.Itemcode select z.SupplierName).ToArray()
                               let UnitName = (from z in result where i == z.Itemcode select z.UnitName).ToArray()
                               let NameStaff = (from z in result where i == z.Itemcode select z.NameStaff).ToArray()
                               let PurchasesSynthesiseCode = (from z in result where i == z.Itemcode select z.PurchasesSynthesiseCode).ToArray()
                               let UserId = (from z in result where i == z.Itemcode select z.UserId).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   Id = Id[0],
                                   ItemsId = ItemsId[0],
                                   QuantityItems = QuantityItems[0],
                                   Note = Note[0],
                                   DateTimeNeed = DateTimeNeed[0],
                                   SupplierId = SupplierId[0],
                                   UserId = UserId[0],
                                   UnitId = UnitId[0],
                                   ItemsName = ItemsName[0],
                                   Itemcode = i,
                                   SupplierName = SupplierName[0],
                                   UnitName = UnitName[0],
                                   NameStaff = NameStaff[0],
                                   PurchasesSynthesiseCode = PurchasesSynthesiseCode[0],
                               }).ToList();

                return new PagedResultDto<PurchasesSynthesisListDto>(Haquery.Count(), Haquery.ToList());

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        /// <summary>
        /// code kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllAssignment(PurchasesSynthesisSearch input)
        {
            try
            {

                var query = _purchasesSynthesiseRepository
                      .GetAll()
                      .Where(x => x.StatusAssignment == true)
                      .OrderBy(x => x.Id);
                var queryReq = _purchasesRequestRepository.GetAll();
                var querySub = _subsidiaryRepository.GetAll();
                var queryPurD = _purchasesRequestDetailRepository.GetAll();
                var results = (from imp in query
                               join req in queryReq on imp.Id equals req.PurchasesSynthesiseId
                               join sub in querySub on req.SubsidiaryCompanyId equals sub.Id
                               //join prd in queryPurD on req.Id equals prd.PurchasesRequestId 
                               select new PurchasesSynthesisListDto
                               {
                                   Id = imp.Id,
                                   CreationTime = imp.CreationTime,
                                   PurchasesSynthesiseCode = imp.PurchasesSynthesiseCode,
                                   SubsidiariesName = sub.NameCompany,
                                   DateAssignment = req.LastModificationTime,
                                   StatusAssignment = imp.StatusAssignment
                               });
                var CodePu = results.Select(x => x.PurchasesSynthesiseCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in results where i == z.PurchasesSynthesiseCode select z.Id).ToArray()
                               let nameArr = (from z in results where i == z.PurchasesSynthesiseCode select z.SubsidiariesName).Distinct().ToList()
                               let CreatimeTT = (from z in results where i == z.PurchasesSynthesiseCode select z.CreationTime).ToArray()
                               let DateAssignmentTT = (from z in results where i == z.PurchasesSynthesiseCode select z.DateAssignment).ToArray()
                               let StatusAssignment = (from z in results where i == z.PurchasesSynthesiseCode select z.StatusAssignment).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   Id = idset[0],
                                   PurchasesSynthesiseCode = i,
                                   subsidiaries = nameArr,
                                   CreationTime = CreatimeTT[0],
                                   DateAssignment = DateAssignmentTT[0],
                                   StatusAssignment = StatusAssignment[0]
                               }
                                );
                return new PagedResultDto<PurchasesSynthesisListDto>(
                  Haquery.Count(),
                  Haquery.OrderByDescending(x => x.Id).ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemByStaff()
        {
            try
            {
                var purchasesSynthesise = _purchasesSynthesiseRepository.GetAll().Where(x => x.StatusAssignment == true);
                var puAss = _purchaseAssignmentRepository.GetAll();

                var query = (from pucAss in purchasesSynthesise
                             join ass in puAss on pucAss.Id equals ass.PurchasesSynthesiseId
                             where ass.UserId == AbpSession.UserId
                             select new PurchasesSynthesisListDto
                             {
                                 GetPriceStatus = ass.GetPriceStatus,
                                 PurchasesSynthesiseCode = pucAss.PurchasesSynthesiseCode,
                                 PurchasesSynthesiseId = pucAss.Id,
                                 CreatorUserId = pucAss.CreatorUserId,
                             }).ToList();

                var CodePu = query.Select(x => x.PurchasesSynthesiseId).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in query where i == z.PurchasesSynthesiseId select z.PurchasesSynthesiseCode).ToArray()
                               let GetPriceStatus = (from z in query where i == z.PurchasesSynthesiseId select z.GetPriceStatus).ToArray()
                               let CreatorUserId = (from z in query where i == z.PurchasesSynthesiseId select z.CreatorUserId).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   PurchasesSynthesiseId = i,
                                   PurchasesSynthesiseCode = idset[0],
                                   GetPriceStatus = GetPriceStatus[0],
                                   CreatorUserId = CreatorUserId[0]
                               }).OrderByDescending(x=>x.PurchasesSynthesiseId).ThenByDescending(x=>x.GetPriceStatus).ToList();
               
                return new PagedResultDto<PurchasesSynthesisListDto>(Haquery.Count(), Haquery);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetViewCV(PurchasesSynthesisSearch input)
        {
            try
            {
                var purchasesSynthesise = _purchasesSynthesiseRepository.GetAll();
                var purchasesRequestDetail = _purchasesRequestDetailRepository.GetAll();
                var purchasesRequest = _purchasesRequestRepository.GetAll().Where(x => x.PurchasesSynthesiseId == input.Id);
                var puAss = _purchaseAssignmentRepository.GetAll().Where(x => x.PurchasesSynthesiseId == input.Id);
                var quote = _quoteRepository.GetAll();
                var items = _itemsRepository.GetAll();
                var supplier = _supplierRepository.GetAll();
                var units = _unitRepository.GetAll();
                var qus = _quoteSynthesise.GetAll().Where(x => x.Status == QuoteEnum.SyntheticQuote.Approve);
                var quR = _quoteRequest.GetAll();

                var query = (from pucAss in purchasesSynthesise
                             join ass in puAss on pucAss.Id equals ass.PurchasesSynthesiseId
                             join pr in purchasesRequest on ass.PurchasesSynthesiseId equals pr.PurchasesSynthesiseId
                             join prd in purchasesRequestDetail on pr.Id equals prd.PurchasesRequestId
                             join qu in quR on ass.ItemId equals qu.ItemId into t
                             from qu in t.DefaultIfEmpty()
                             where ass.UserId == AbpSession.UserId && ass.ItemId == prd.ItemId
                             select new PurchasesSynthesisListDto
                             {
                                 Id = pucAss.Id,
                                 ItemsId = ass.ItemId,
                                 QuantityItems = prd.Quantity,
                                 UnitId = prd.UnitId,
                                 SupplierId = qu.SupplierId,
                                 DateTimeNeed = prd.TimeNeeded,
                                 Note = prd.Note,
                                 UserId = ass.UserId,
                                 Price = (decimal?)qu.QuotePrice ?? 0,
                                 PurchasesSynthesiseId = pucAss.Id,
                                 GetPriceStatus = ass.GetPriceStatus,
                             }).ToList();

                var result = (from res in query
                              join item in items on res.ItemsId equals item.Id
                              join sup in supplier on res.SupplierId equals sup.Id
                              join unit in units on res.UnitId equals unit.Id
                              select new PurchasesSynthesisListDto
                              {
                                  Id = res.Id,
                                  ItemsId = res.ItemsId,
                                  QuantityItems = res.QuantityItems,
                                  Note = res.Note,
                                  DateTimeNeed = res.DateTimeNeed,
                                  SupplierId = res.SupplierId,
                                  UnitId = res.UnitId,
                                  ItemsName = item.Name,
                                  Itemcode = item.ItemCode,
                                  SupplierName = sup.Name,
                                  UnitName = unit.Name,
                                  Price = res.Price,
                                  PurchasesSynthesiseId = res.PurchasesSynthesiseId,
                                  GetPriceStatus = res.GetPriceStatus
                              }).ToList();


                var CodePu = result.Select(x => x.Itemcode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let Id = (from z in result where i == z.Itemcode select z.Id).ToArray()
                               let ItemsId = (from z in result where i == z.Itemcode select z.ItemsId).ToArray()
                               let QuantityItems = (from z in result where i == z.Itemcode select z.QuantityItems).ToArray()
                               let Note = (from z in result where i == z.Itemcode select z.Note).ToArray()
                               let DateTimeNeed = (from z in result where i == z.Itemcode select z.DateTimeNeed).ToArray()
                               let SupplierId = (from z in result where i == z.Itemcode select z.SupplierId).ToArray()
                               let UnitId = (from z in result where i == z.Itemcode select z.UnitId).ToArray()
                               let ItemsName = (from z in result where i == z.Itemcode select z.ItemsName).ToArray()
                               let Itemcode = (from z in result where i == z.Itemcode select z.Itemcode).ToArray()
                               let SupplierName = (from z in result where i == z.Itemcode select z.SupplierName).Distinct().ToList()
                               let UnitName = (from z in result where i == z.Itemcode select z.UnitName).ToArray()
                               let Price = (from z in result where i == z.Itemcode select z.Price).ToArray()
                               let PurchasesSynthesiseId = (from z in result where i == z.Itemcode select z.PurchasesSynthesiseId).ToArray()
                               let GetPriceStatus = (from z in result where i == z.Itemcode select z.GetPriceStatus).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   Id = Id[0],
                                   ItemsId = ItemsId[0],
                                   QuantityItems = QuantityItems[0],
                                   Note = Note[0],
                                   DateTimeNeed = DateTimeNeed[0],
                                   SupplierId = SupplierId[0],
                                   UnitId = UnitId[0],
                                   ItemsName = ItemsName[0],
                                   Itemcode = Itemcode[0],
                                   Supplier = SupplierName,
                                   UnitName = UnitName[0],
                                   Price = Price[0],
                                   PurchasesSynthesiseId = PurchasesSynthesiseId[0],
                                   GetPriceStatus = GetPriceStatus[0]
                               }).ToList();

                return new PagedResultDto<PurchasesSynthesisListDto>(Haquery.Count(), Haquery);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllDone(PurchasesSynthesisSearch input)
        {
            try
            {
                var query = _purchasesSynthesiseRepository.GetAll().Where(x => x.StatusAssignment == false && x.PurchasesRequestStatus == PurchasesRequestEnum.YCNK.Approve);
                var queryReq = _purchasesRequestRepository.GetAll();
                var querySub = _subsidiaryRepository.GetAll();
                var results = (from req in queryReq
                               join imp in query on req.PurchasesSynthesiseId equals imp.Id
                               join sub in querySub on req.SubsidiaryCompanyId equals sub.Id

                               select new PurchasesSynthesisListDto
                               {
                                   Id = imp.Id,
                                   CreationTime = imp.CreationTime,
                                   DeleterUserId = imp.CreatorUserId.Value,
                                   DeletionTime = imp.DeletionTime.Value,
                                   PurchasesSynthesiseCode = imp.PurchasesSynthesiseCode,
                                   SubsidiariesName = sub.NameCompany,
                                   PurchasesRequestStatus = imp.PurchasesRequestStatus,
                                   CreatorUserId = imp.CreatorUserId.Value,
                                   StatusAssignment = imp.StatusAssignment
                               });

                var CodePu = results.Select(x => x.PurchasesSynthesiseCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in results where i == z.PurchasesSynthesiseCode select z.Id).ToArray()
                               let nameArr = (from z in results where i == z.PurchasesSynthesiseCode select z.SubsidiariesName).Distinct().ToList()
                               let CreatimeTT = (from z in results where i == z.PurchasesSynthesiseCode select z.CreationTime).ToArray()
                               let PurchasesRequestStatus = (from z in results where i == z.PurchasesSynthesiseCode select z.PurchasesRequestStatus).ToArray()
                               let CreatorUserId = (from z in results where i == z.PurchasesSynthesiseCode select z.CreatorUserId).ToArray()
                               let StatusAssignment = (from z in results where i == z.PurchasesSynthesiseCode select z.StatusAssignment).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   Id = idset[0],
                                   PurchasesSynthesiseCode = i,
                                   subsidiaries = nameArr,
                                   CreationTime = CreatimeTT[0],
                                   PurchasesRequestStatus = PurchasesRequestStatus[0],
                                   CreatorUserId = CreatorUserId[0],
                                   StatusAssignment = StatusAssignment[0]
                               });
                return new PagedResultDto<PurchasesSynthesisListDto>(
                  Haquery.Count(),
                  Haquery.OrderByDescending(x => x.Id).ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateStatus(PurchasesSynthesisListDto input)
        {
            PurchasesSynthesise items = await _purchasesSynthesiseRepository.FirstOrDefaultAsync(x => x.Id == input.PurchasesSynthesiseId);
            items.PurchasesRequestStatus = input.PurchasesRequestStatus;
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

                var user = _user.GetAll().Where(x=>x.Id == items.CreatorUserId).FirstOrDefault();
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

            await _purchasesSynthesiseRepository.UpdateAsync(items);


            return items.Id;
        }

        public async Task<long> UpdateSyn(PurchasesSynthesisListDto input)
        {
            try
            {
                PurchasesSynthesise IMP = await _purchasesSynthesiseRepository.FirstOrDefaultAsync(x => x.Id == input.PurchasesSynthesiseId);
                IMP.StatusAssignment = true;
                await _purchasesSynthesiseRepository.UpdateAsync(IMP);
                return input.Id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetAllConfirm(PurchasesSynthesisSearch input)
        {
            try
            {

                var query = _purchasesSynthesiseRepository
                      .GetAll()
                      .Where(x => x.StatusAssignment == false && x.PurchasesRequestStatus >= PurchasesRequestEnum.YCNK.Original)
                      .OrderBy(x => x.Id);
                var queryReq = _purchasesRequestRepository.GetAll();
                var querySub = _subsidiaryRepository.GetAll();
                var results = (from req in queryReq
                               join imp in query on req.PurchasesSynthesiseId equals imp.Id
                               join sub in querySub on req.SubsidiaryCompanyId equals sub.Id

                               select new PurchasesSynthesisListDto
                               {
                                   Id = imp.Id,
                                   CreationTime = imp.CreationTime,
                                   DeleterUserId = imp.CreatorUserId.Value,
                                   DeletionTime = imp.DeletionTime.Value,
                                   PurchasesSynthesiseCode = imp.PurchasesSynthesiseCode,
                                   SubsidiariesName = sub.NameCompany,
                                   PurchasesRequestStatus = imp.PurchasesRequestStatus,
                                   CreatorUserId = imp.CreatorUserId.Value,
                               });

                var CodePu = results.Select(x => x.PurchasesSynthesiseCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in results where i == z.PurchasesSynthesiseCode select z.Id).ToArray()
                               let nameArr = (from z in results where i == z.PurchasesSynthesiseCode select z.SubsidiariesName).Distinct().ToList()
                               let CreatimeTT = (from z in results where i == z.PurchasesSynthesiseCode select z.CreationTime).ToArray()
                               let PurchasesRequestStatus = (from z in results where i == z.PurchasesSynthesiseCode select z.PurchasesRequestStatus).ToArray()
                               let CreatorUserId = (from z in results where i == z.PurchasesSynthesiseCode select z.CreatorUserId).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   Id = idset[0],
                                   PurchasesSynthesiseCode = i,
                                   subsidiaries = nameArr,
                                   CreationTime = CreatimeTT[0],
                                   PurchasesRequestStatus = PurchasesRequestStatus[0],
                                   CreatorUserId = CreatorUserId[0]
                               }
                                );




                return new PagedResultDto<PurchasesSynthesisListDto>(
                  Haquery.Count(),
                  Haquery.OrderByDescending(x => x.Id).ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="input"> input.id_ IdpurchasesSynthesId</param>
        /// <param name="input"> UserId Current</param>
        /// <returns>the list of the row is a partition</returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemsAssignments(PurchasesSynthesisSearch input)
        {
            try
            {
                var queryPuschasesAss = _purchaseAssignmentRepository.GetAll().Where(x => x.PurchasesSynthesiseId == input.Id && x.UserId == AbpSession.UserId);
                var queryPuschasesSyn = _purchasesSynthesiseRepository.GetAll();
                var queryPuschasesDeatil = _purchasesRequestDetailRepository.GetAll();
                var queryPuschasesRequests = _purchasesRequestRepository.GetAll();
                var queryItems = _itemsRepository.GetAll();
                var queryunit = _unitRepository.GetAll();
                var querySupplier = _supplierRepository.GetAll();

                var result = (from puschasesAss in queryPuschasesAss
                              join puschasesSyn in queryPuschasesSyn on puschasesAss.PurchasesSynthesiseId equals puschasesSyn.Id
                              join puschasesRequests in queryPuschasesRequests on puschasesSyn.Id equals puschasesRequests.PurchasesSynthesiseId
                              join puschasesDetail in queryPuschasesDeatil on puschasesRequests.Id equals puschasesDetail.PurchasesRequestId
                              join item in queryItems on puschasesDetail.ItemId equals item.Id
                              join unit in queryunit on puschasesDetail.UnitId equals unit.Id
                              join supplier in querySupplier on puschasesDetail.SupplierId equals supplier.Id

                              select new PurchasesSynthesisListDto
                              {
                                  Id = puschasesSyn.Id,
                                  ItemsName = item.ItemCode + "-" + item.Name,
                                  ItemsId = item.Id,
                                  SupplierName = supplier.Name,
                                  UnitName = unit.Name,
                                  Quantity = puschasesDetail.Quantity,
                                  // DateTimeNeed = puschasesDetail.TimeNeeded,
                                  Note = puschasesDetail.Note,
                                  CreationTime = puschasesDetail.CreationTime,
                              }).ToList();

                var results = (from p in result
                               group p by p.ItemsId into g
                               select new PurchasesSynthesisListDto
                               {
                                   // Id = g.Key,
                                   ItemsName = g.First().ItemsName,
                                   ItemsId = g.First().ItemsId,
                                   SupplierName = g.First().SubsidiariesName,
                                   UnitName = g.First().UnitName,
                                   Quantity = g.Sum(x => x.Quantity),
                                   Note = g.First().Note,
                               }).ToList();


                return new PagedResultDto<PurchasesSynthesisListDto>(results.Count(), results.Distinct().ToList());
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<FileDto> GetPurchaseAssignmentListDto(PurchasesSynthesisSearch input)
        {
            try
            {
                var queryPuschasesAss = _purchaseAssignmentRepository.GetAll().Where(x => x.PurchasesSynthesiseId == input.Id && x.UserId == AbpSession.UserId);
                var queryPuschasesSyn = _purchasesSynthesiseRepository.GetAll();
                var queryPuschasesDeatil = _purchasesRequestDetailRepository.GetAll();
                var queryPuschasesRequests = _purchasesRequestRepository.GetAll();
                var queryItems = _itemsRepository.GetAll().Where(x=>input.listItems.ToArray().Contains(x.Id));
                var queryunit = _unitRepository.GetAll();
                var querySupplier = _supplierRepository.GetAll();

                var result = (from puschasesAss in queryPuschasesAss
                              join puschasesSyn in queryPuschasesSyn on puschasesAss.PurchasesSynthesiseId equals puschasesSyn.Id
                              join puschasesRequests in queryPuschasesRequests on puschasesSyn.Id equals puschasesRequests.PurchasesSynthesiseId
                              join puschasesDetail in queryPuschasesDeatil on puschasesRequests.Id equals puschasesDetail.PurchasesRequestId
                              join item in queryItems on puschasesDetail.ItemId equals item.Id
                              join unit in queryunit on puschasesDetail.UnitId equals unit.Id
                              join supplier in querySupplier on puschasesDetail.SupplierId equals supplier.Id

                              select new PurchasesSynthesisListDto
                              {
                                  Id = puschasesSyn.Id,
                                  ItemsName = item.ItemCode + "-" + item.Name,
                                  Itemcode = item.ItemCode,
                                  ItemsId = item.Id,
                                  SupplierName = supplier.Name,
                                  UnitName = unit.Name,
                                  Quantity = puschasesDetail.Quantity,
                                  // DateTimeNeed = puschasesDetail.TimeNeeded,
                                  Note = puschasesDetail.Note,
                                  CreationTime = puschasesDetail.CreationTime,
                              }).ToList();
                var results = (from p in result
                               group p by p.ItemsId into g
                               select new PurchasesSynthesisListDto
                               {
                                   // Id = g.Key,
                                   ItemsName = g.First().ItemsName,
                                   Itemcode = g.First().Itemcode,
                                   ItemsId = g.First().ItemsId,
                                   SupplierName = g.First().SubsidiariesName,
                                   UnitName = g.First().UnitName,
                                   Quantity = g.Sum(x => x.Quantity),
                                   Note = g.First().Note,
                               }).ToList();

           

                var OutputSupplier = await _supplierRepository.GetAsync(input.SupplierId);
                var SupplierName = OutputSupplier.Name;


                return await _iExportPurchaseAssignment.ExportPOToFile(results, DateTime.Now, SupplierName);

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<PurchasesSynthesisListDto>> GetItemByStaffD()
        {
            try
            {
                var purchasesSynthesise = _purchasesSynthesiseRepository.GetAll().Where(x => x.StatusAssignment == true);
                var puAss = _purchaseAssignmentRepository.GetAll();

                var query = (from pucAss in purchasesSynthesise
                             join ass in puAss on pucAss.Id equals ass.PurchasesSynthesiseId
                             where ass.UserId == AbpSession.UserId
                             select new PurchasesSynthesisListDto
                             {
                                 GetPriceStatus = ass.GetPriceStatus,
                                 PurchasesSynthesiseCode = pucAss.PurchasesSynthesiseCode,
                                 PurchasesSynthesiseId = pucAss.Id,
                                 CreatorUserId = pucAss.CreatorUserId,
                             }).ToList();

                var CodePu = query.Select(x => x.PurchasesSynthesiseId).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in query where i == z.PurchasesSynthesiseId select z.PurchasesSynthesiseCode).ToArray()
                               let GetPriceStatus = (from z in query where i == z.PurchasesSynthesiseId select z.GetPriceStatus).ToArray()
                               let CreatorUserId = (from z in query where i == z.PurchasesSynthesiseId select z.CreatorUserId).ToArray()
                               select new PurchasesSynthesisListDto
                               {
                                   PurchasesSynthesiseId = i,
                                   PurchasesSynthesiseCode = idset[0],
                                   GetPriceStatus = GetPriceStatus[0],
                                   CreatorUserId = CreatorUserId[0]
                               }).OrderByDescending(x => x.PurchasesSynthesiseId).ThenByDescending(x => x.GetPriceStatus).Take(5).ToList();

                return new PagedResultDto<PurchasesSynthesisListDto>(Haquery.Count(), Haquery);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
