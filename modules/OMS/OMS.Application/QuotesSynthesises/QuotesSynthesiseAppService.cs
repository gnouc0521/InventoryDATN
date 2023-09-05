using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.QuotesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto.QuotesSynthesisListDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace bbk.netcore.mdl.OMS.Application.QuotesSynthesises
{
    public class QuotesSynthesiseAppService : ApplicationService, IQuotesSynthesiseAppService
    {
        private readonly IRepository<QuoteSynthesise, long> _QuoteSynthesiseRepository;
        private readonly IRepository<QuoteRequest, long> _QuoteRequestRepository;
        private readonly IRepository<QuoteRelationship, long> _QuoteRelationshipRepository;
        private readonly IRepository<Quote, long> _QuoteRepository;
        private readonly IRepository<User, long> _user;
        private readonly IRepository<Items, long> _itemsRepository;
        private readonly IRepository<Subsidiary, long> _subsidiaryRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private IHostingEnvironment _Environment;
        private readonly ISendMailService _sendMailService;
        public QuotesSynthesiseAppService(IRepository<QuoteSynthesise, long> QuoteSynthesiseRepository,
                                          IRepository<QuoteRequest, long> QuoteRequestRepository,
                                          IRepository<QuoteRelationship, long> QuoteRelationshipRepository,
                                          IRepository<Quote, long> QuoteRepository,
                                          IRepository<Items, long> itemsRepository,
                                          IRepository<Unit> unitRepository,
                                          IRepository<Subsidiary, long> subsidiaryRepository,
                                          IRepository<Supplier> supplierRepository,
                                          IHostingEnvironment Environment,
                                          ISendMailService sendMailService,
                                          IRepository<User, long> user)
        {
            _QuoteRequestRepository = QuoteRequestRepository;
            _itemsRepository = itemsRepository;
            _unitRepository = unitRepository;
            _subsidiaryRepository = subsidiaryRepository;
            _supplierRepository = supplierRepository;
            _QuoteSynthesiseRepository = QuoteSynthesiseRepository;
            _QuoteRelationshipRepository = QuoteRelationshipRepository;
            _QuoteRepository = QuoteRepository;
            _Environment = Environment;
            _sendMailService = sendMailService;
            _user = user;
        }

        public async Task<long> Create(QuotesSynthesisListDto input)
        {
            try
            {
                var query = await _QuoteSynthesiseRepository.GetAll().ToListAsync();
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
                    if (i < 10) return "BG" + Thang + DateTime.Now.Year + "000" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "BG" + Thang + DateTime.Now.Year + "00" + Convert.ToString(i);
                    else
                    if (i >= 100 && i < 1000) return "BG" + Thang + DateTime.Now.Year + "0" + Convert.ToString(i);
                    else
                        return "BG" + Thang + DateTime.Now.Year + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)
                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _QuoteSynthesiseRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                input.Code = sinhma(ma.ToString());
                QuoteSynthesise newItemId = ObjectMapper.Map<QuoteSynthesise>(input);
                var newId = await _QuoteSynthesiseRepository.InsertAndGetIdAsync(newItemId);
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
                await _QuoteSynthesiseRepository.DeleteAsync(id);
                return id;

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<QuotesSynthesisListDto>> GetAllByCreator(QuotesSynthesisSearch input)
        {
            try
            {

                var queryQuoteSyn = _QuoteSynthesiseRepository.GetAll().Where(x => x.CreatorUserId == AbpSession.UserId);
                var queryQuoteReq = _QuoteRequestRepository.GetAll();
                                                      
                var results = (from Syn in queryQuoteSyn
                                join Req in queryQuoteReq on Syn.Id equals Req.QuotesSynthesiseId

                               select new QuotesSynthesisListDto
                               {
                                   Id = Syn.Id,
                                   Code = Syn.Code,
                                   SuppliersName = Req.SupplierName,
                                   CreationTime = Syn.CreationTime,
                                   Status = Syn.Status,
                                   
                               });
                var getCode = results.Select(x => x.Code).Distinct().ToArray();

                var reusltEnd = (from i in getCode
                                 let idset = (from que in results where i == que.Code select que.Id).ToArray()
                                 let nameArr = (from que in results where i == que.Code select que.SuppliersName).Distinct().ToList()
                                 let listCreationTime = (from que in results where i == que.Code select que.CreationTime).Distinct().ToList()
                                 let liststatus = (from que in results where i == que.Code select que.Status).Distinct().ToList()
                                 select new QuotesSynthesisListDto
                                 {
                                     Id = idset[0],
                                     Code = i,
                                     SuppliersNames = nameArr,
                                     Status = liststatus.First(),
                                     CreationTime = listCreationTime.First(),

                                 }).ToList().OrderBy(x => x.Status).ThenBy(x=>x.Id);

                return new PagedResultDto<QuotesSynthesisListDto>(
                  reusltEnd.Count(),
                  reusltEnd.ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<PagedResultDto<QuoteListDto>> Detail(QuotesSynthesisSearch input)
        {
            try
            {
                var queryQuoteSyn = _QuoteSynthesiseRepository.GetAll().Where(x => x.Id == input.Id)
                                                      .OrderBy(x => x.Id);
                var queryRequests = _QuoteRequestRepository.GetAll();
                var queryItems = _itemsRepository.GetAll();
                var result = (from qouteRequests in queryRequests
                              join Item in queryItems on qouteRequests.ItemId equals Item.Id
                               select new QuoteListDto
                               {
                                   SupplierName = qouteRequests.SupplierName,
                                   ItemId = qouteRequests.ItemId,
                                   Note = qouteRequests.Note,
                                   ItemName =Item.Name,
                                   Id = qouteRequests.QuoteId,
                                   ItemCode = Item.ItemCode,
                                   QuotePrice = qouteRequests.QuotePrice,
                                   UnitName = qouteRequests.UnitName,
                                   QuantityQuote = qouteRequests.Quantity,
                                   QuoteSynthesiseId = qouteRequests.QuotesSynthesiseId,



                               }); 
                var results = (from result1 in result
                               join Syn in queryQuoteSyn on result1.QuoteSynthesiseId equals Syn.Id
                               select new QuoteListDto
                               {
                                   Id = result1.Id,
                                   QuoteSynCode = Syn.Code,
                                   SupplierName = result1.SupplierName,
                                   ItemId = result1.ItemId,
                                   Note = result1.Note,
                                   ItemName = result1.ItemName,
                                   ItemCode = result1.ItemCode,
                                   QuotePrice = result1.QuotePrice,
                                   UnitName = result1.UnitName,
                                   QuantityQuote = result1.QuantityQuote,
                               });
                return new PagedResultDto<QuoteListDto>(
                  results.Distinct().Count(),
                  results.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<QuotesSynthesisListDto> GetAsync(EntityDto<long> itemId)
        {
            try
            {
                var item = _QuoteSynthesiseRepository.Get(itemId.Id);
                QuotesSynthesisListDto newItem = ObjectMapper.Map<QuotesSynthesisListDto>(item);
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
                var user = _user.FirstOrDefault(x => x.Id == newItem.CreatorUserId);
                newItem.CreateName = user.Surname + " " + user.Name;
                return newItem;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(QuotesSynthesisListDto input)
        {
            try
            {
                var IMP = await _QuoteSynthesiseRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                input.CreationTime = IMP.CreationTime;
                input.Code = IMP.Code;
                input.CreatorUserId = IMP.CreatorUserId;
                input.QuoteSynthesiseDate = DateTime.Now;
                ObjectMapper.Map(input, IMP);

                var webRootPath = this._Environment.WebRootPath;
                string path = "";
                path = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "sendMailCV.html";
                path = File.ReadAllText(path);
                var passlinkEmail = input.Link;
                var email = input.Email;
                var name = input.Name;
                if (IMP.CreatorUserId == AbpSession.UserId)
                {
                     email = input.Email;
                     name = input.Name;
                    path = path.Replace("{{TenNguoiNhan}}", name);
                    path = path.Replace("{{LinkFromEmail}}", passlinkEmail);
                    await _sendMailService.SendEmailCvAsync(email, "Bạn có công việc mới :", path);
                }
                else
                {
                    var user = _user.GetAll().Where(x => x.Id == IMP.CreatorUserId).FirstOrDefault();
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
                    //    if (querya.Items[i].Id == IMP.CreatorUserId)
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


                
                await _QuoteSynthesiseRepository.UpdateAsync(IMP);
                return input.Id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<QuotesSynthesisListDto>> GetAll(QuotesSynthesisSearch input)
        {
            try
            {

                var queryQuoteSyn = _QuoteSynthesiseRepository.GetAll().Where(x=>x.Status != QuoteEnum.SyntheticQuote.Draft);
                var queryQuoteRequests = _QuoteRequestRepository.GetAll();


                var results = (from Syn in queryQuoteSyn
                               join quoteRequests in queryQuoteRequests on Syn.Id equals quoteRequests.QuotesSynthesiseId
                               select new QuotesSynthesisListDto
                               {
                                   Id = Syn.Id,
                                   Code = Syn.Code,
                                   SuppliersName = quoteRequests.SupplierName,
                                   CreationTime = Syn.CreationTime,
                                   Status = Syn.Status,
                                   QuoteSynthesiseDate = Syn.QuoteSynthesiseDate,


                               });
                var getCode = results.Select(x => x.Code).Distinct().ToArray();

                var reusltEnd = (from i in getCode
                                 let idset = (from que in results where i == que.Code select que.Id).ToArray()
                                 let nameArr = (from que in results where i == que.Code select que.SuppliersName).Distinct().ToList()
                                 let status = (from que in results where i ==  que.Code select que.Status).ToArray()
                                 let creationTime = (from que in results where i ==  que.Code select que.CreationTime).ToArray()
                                 let quoteSynthesiseDate = (from que in results where i ==  que.Code select que.QuoteSynthesiseDate).ToArray()
                                 select new QuotesSynthesisListDto
                                 {
                                     Id = idset[0],
                                     Code = i,
                                     SuppliersNames = nameArr,
                                     Status = status[0],
                                     CreationTime = creationTime[0],
                                     QuoteSynthesiseDate = quoteSynthesiseDate[0],

                                 }).ToList().OrderByDescending(x => x.CreationTime).OrderBy(x => x.Status);

                return new PagedResultDto<QuotesSynthesisListDto>(
                  reusltEnd.Count(),
                  reusltEnd.ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<QuotesSynthesisListDto>> GetAllQuoteApprove(QuotesSynthesisSearch input)
        {
            try
            {

                var queryQuoteSyn = _QuoteSynthesiseRepository.GetAll().Where(x => x.Status == QuoteEnum.SyntheticQuote.Approve && x.CreatorUserId == AbpSession.UserId);
                var queryQuoteRequests = _QuoteRequestRepository.GetAll();

                //var queryQuoteRel = _QuoteRelationshipRepository.GetAll();
                //var queryQuote = _QuoteRepository.GetAll();

                var results = (from Syn in queryQuoteSyn
                               join quoteRequests in queryQuoteRequests on Syn.Id equals quoteRequests.QuotesSynthesiseId
                               //join Rel in queryQuoteRel on Syn.Id equals Rel.QuoteSynthesiseId
                               //join Quote in queryQuote on Rel.QuoteId equals Quote.Id

                               select new QuotesSynthesisListDto
                               {
                                   Id = Syn.Id,
                                   Code = Syn.Code,
                                   SuppliersName = quoteRequests.SupplierName,
                                   CreationTime = Syn.CreationTime,
                                   Status = Syn.Status,
                                   QuoteSynthesiseDate = Syn.QuoteSynthesiseDate,


                               });
                var getCode = results.Select(x => x.Code).Distinct().ToArray();

                var reusltEnd = (from i in getCode
                                 let idset = (from que in results where i == que.Code select que.Id).ToArray()
                                 let nameArr = (from que in results where i == que.Code select que.SuppliersName).Distinct().ToList()
                                 let status = (from que in results where i == que.Code select que.Status).ToArray()
                                 let Syndate = (from Syndate in results where i == Syndate.Code select Syndate.QuoteSynthesiseDate).Distinct().ToList()
                                 select new QuotesSynthesisListDto
                                 {
                                     Id = idset[0],
                                     Code = i,
                                     SuppliersNames = nameArr,
                                     Status = status[0],
                                     QuoteSynthesiseDate = Syndate[0], 
                                     //  CreationTime = creationtime,

                                 }).ToList().OrderByDescending(x => x.QuoteSynthesiseDate);

                return new PagedResultDto<QuotesSynthesisListDto>(
                  reusltEnd.Count(),
                  reusltEnd.ToList()
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<QuotesSynthesisListDto> GetQuoteApprove(int Id)
        {
            try
            {
                var queryQuoteSyn = _QuoteSynthesiseRepository.GetAll().Where(x => x.Id == Id).Select(x=>x.Id).ToList();
                var queryQuoteRequests = _QuoteRequestRepository.GetAll();
                var result = (from rela in queryQuoteSyn
                              let sup = (from quote in queryQuoteRequests where rela == quote.QuotesSynthesiseId select new SupplierList { SupplierNameId = quote.SupplierId, SupplierName = quote.SupplierName }).Distinct().ToList()
                              select new QuotesSynthesisListDto
                              {
                                  Id = Id,
                                  supplierList = sup,
                              }).ToList();

               var tolisst =  result[0].supplierList.Select(x=> new { x.SupplierNameId, x.SupplierName }).Distinct().ToList();

                var suplist = (from i in tolisst
                               select new SupplierList
                               {
                                   SupplierNameId = i.SupplierNameId,
                                   SupplierName = i.SupplierName,
                               }).Distinct().ToList();
                return new QuotesSynthesisListDto
                {
                    Id = Id,
                    supplierList = suplist,
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<long>> GetQuoteId(QuotesSynthesisListDto input)
        {
            try
            {
                var IdQuote = _QuoteRequestRepository.GetAll().Where(x => x.QuotesSynthesiseId == input.Id).Select(x=>x.Id).ToList();
                return IdQuote; 
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateStatus(QuotesSynthesisListDto input)
        {
            try
            {
                QuoteSynthesise quoteSyn = await _QuoteSynthesiseRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                quoteSyn.Status = input.Status;

                await _QuoteSynthesiseRepository.UpdateAsync(quoteSyn);
                return input.Id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
