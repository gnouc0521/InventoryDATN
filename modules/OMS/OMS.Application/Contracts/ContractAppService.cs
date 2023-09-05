using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.Contracts.Export;
using bbk.netcore.mdl.OMS.Application.Quotes.Dto;
using bbk.netcore.mdl.OMS.Application.QuotesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Contracts
{
    [AbpAuthorize]
    public class ContractAppService : ApplicationService, IContractAppService
    {
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<Supplier> _supplierrepository;
        private readonly IRepository<Quote, long> _quoterepository;
        private readonly IRepository<QuoteRelationship, long> _quoteRelarepository;
        private readonly IRepository<QuoteSynthesise, long> _quoteSynrepository;
        private readonly IRepository<QuoteRequest, long> _quoteRequestrepository;
        private readonly IRepository<Items, long> _itemrepository;
        private readonly IExportContract _contractOrder;
        private readonly IRepository<User, long> _user;

        private IHostingEnvironment _Environment;
        private readonly ISendMailService _sendMailService;

        public ContractAppService(IRepository<Contract> contractRepository,
            IRepository<Supplier> supplierrepository,
            IRepository<Quote, long> quoterepository, 
            IRepository<QuoteRelationship, long> quoteRelarepository,
            IRepository<QuoteSynthesise, long> quoteSynrepository,
            IRepository<QuoteRequest, long> quoteRequestrepository,
            IExportContract contractOrder,
            IRepository<Items, long> itemrepository,
            IHostingEnvironment Environment,
            ISendMailService sendMailService,
            IRepository<User, long> user
           )
        {
            _contractRepository = contractRepository;
            _supplierrepository = supplierrepository;
            _quoterepository = quoterepository;
            _quoteRelarepository = quoteRelarepository;
            _itemrepository = itemrepository;
            _quoteRequestrepository = quoteRequestrepository;
            _quoteSynrepository = quoteSynrepository;
            _contractOrder = contractOrder;
            _Environment= Environment;
            _sendMailService= sendMailService;
            _user = user;
        }



        public async Task<int> Create(ContractCreateDto input)
        {
            try
            {

                //var query = _contractRepository.GetAll().ToList();
                //string sinhma(string ma)
                //{
                //    string s = ma.Substring(5, ma.Length - 5);

                //    int i = int.Parse(s);
                //    i++;
                //    if (i < 10) return "HDMB-" + "00" + Convert.ToString(i);
                //    else
                //    if (i >= 10 && i < 100) return "HDMB-" + "0" + Convert.ToString(i);
                //    else return "HDMB-" + Convert.ToString(i);

                //}
                //string ma;

                //var count = query.Count;
                //if (count == 0)
                //{
                //    ma = "0000000000";
                //}
                //else
                //{
                //    ma = _contractRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                //}

                //input.Code = sinhma(ma.ToString());
                //Contract newItemId = ObjectMapper.Map<Contract>(input);
                //var newId = _contractRepository.InsertAndGetId(newItemId);
                //return newId;

                var query = _contractRepository.GetAll().ToList();
               
                string maCode;

                var count = query.Count;
                if (count == 0)
                {
                    maCode = "HDMB-001";
                }
                else
                {
                    var ma = _contractRepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();

                    string s = ma.Substring(5, ma.Length - 5);
                    int i = int.Parse(s) + input.Number;

                    if (i < 10) { maCode = "HDMB-" + "00" + Convert.ToString(i); }
                    else
                    {
                        if (i >= 10 && i < 100) { maCode = "HDMB-" + "0" + Convert.ToString(i); }
                        else {maCode = "HDMB-" + Convert.ToString(i); }
                    }
                 
                }

                input.Code = maCode;
                Contract newItemId = ObjectMapper.Map<Contract>(input);
                var newId = _contractRepository.InsertAndGetId(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ContractListDto>> GetAll(ContractSearch input)
        {
            try
            {
                var queryQuoteRequests = _quoteRequestrepository.GetAll();

                var resul = (from quo in queryQuoteRequests
                             select new QuoteRequestsListDto
                             {
                                 Id = quo.Id,
                                 QuotesSynthesiseId = quo.QuotesSynthesiseId,
                                 UnitName = quo.UnitName,
                                 QuantityQuote = quo.Quantity,
                                 QuotePrice = quo.QuotePrice,
                                 Specifications = quo.Specifications,
                                 SupplierId = quo.SupplierId,
                                 SupplierName = quo.SupplierName,
                                 TotalNumber = quo.Quantity * quo.QuotePrice,
                             }).ToList();

                var querySup = _supplierrepository.GetAll();
                var query = _contractRepository.GetAll().Where(x => x.CreatorUserId == AbpSession.UserId)
                    .WhereIf(input.status.HasValue, x => x.Status == input.status)
                    .WhereIf(input.ExportStatus.HasValue, x => x.ExportStatus == input.ExportStatus)
                    .OrderByDescending(x => x.Id);
                var queryQuoSyn = _quoteSynrepository.GetAll();

                var result = (from contract in query
                              join sup in querySup on contract.SupplierId equals sup.Id
                              join quoSyn in queryQuoSyn on contract.QuoteSynId equals quoSyn.Id
                              select new ContractListDto
                              {
                                  Id = contract.Id,
                                  QuoteSynCode = quoSyn.Code,
                                  QuoteSynId = quoSyn.Id,
                                  Code = contract.Code,
                                  SupplierId = contract.SupplierId,
                                  SupplierName = sup.Name,
                                  Status = contract.Status,
                                  CreationTime = contract.CreationTime,
                                  LastModificationTime = contract.LastModificationTime.Value,
                              }).OrderBy(x=>x.Status).ThenByDescending(x=>x.Id).ToList();

                return new PagedResultDto<ContractListDto>(
                    result.Count(),
                    result
                    );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ContractListDto> GetAsync(EntityDto itemId)
        {
            try
            {
                var item = _contractRepository.Get(itemId.Id);
                ContractListDto newItem = ObjectMapper.Map<ContractListDto>(item);
                return newItem;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

        public async Task<int> UpdateCodeSame(int idQuo)
        {
            try
            {
                var query = _contractRepository.GetAll().Where(x => x.QuoteSynId == idQuo).ToList();
                var codeOld = _contractRepository.GetAll().Where(x => x.QuoteSynId == idQuo).Select(x => x.Code).FirstOrDefault();
                for (int i = 1; i < query.Count(); i++)
                {
                    var codeUpdate = "";
                    //string s = query[i].Code.Substring(5, query[i].Code.Length - 5);
                    string s = codeOld.Substring(5, query[i].Code.Length - 5);
                    var code = int.Parse(s) + i;
                    if (code < 10)
                    {
                        codeUpdate = "HDMB-" + "00" + code;
                    }
                    else
                    if (code >= 10 && code < 100)
                    {
                        codeUpdate = "HDMB-" + "0" + code;
                    }
                    else
                    {
                        codeUpdate = "HDMB-" + code;
                    };

                    //Contract newValue = new Contract
                    //{
                    //    Id = query[i].Id,
                    //    QuoteSynId = idQuo,
                    //    SupplierId = query[i].SupplierId,
                    //    CreationTime = query[i].CreationTime,
                    //    CreatorUserId = query[i].CreatorUserId,
                    //    Code = codeUpdate,
                    //};

                    Contract queryNew = _contractRepository.FirstOrDefault(x => x.Id == query[i].Id);

                    queryNew.QuoteSynId = idQuo;
                    queryNew.SupplierId = query[i].SupplierId;
                    queryNew.Code = codeUpdate;

                    await _contractRepository.UpdateAsync(queryNew);
                }

                return idQuo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// <summary>
        /// Lay het san pham trong HOP DONg theo NCC
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<QuoteListDto>> AllItemInContact(ContractSearch input)
        {
            try
            {

                var queryQuoteRequests = _quoteRequestrepository.GetAll().Where(x => x.QuotesSynthesiseId == input.quoSyn && x.SupplierId == input.Supper);
                //var query1 = _quoteRelarepository.GetAll().Where(x => x.QuoteSynthesiseId == input.quoSyn);
                //var query2 = _quoterepository.GetAll().Where(x => x.SupplierId == input.Supper);
                var query3 = _itemrepository.GetAll();

                var resul = (from quo in queryQuoteRequests
                             join item in query3 on quo.ItemId equals item.Id
                             select new QuoteListDto
                             {
                                 ItemId = item.Id,
                                 ItemName = item.Name,
                                 UnitName = quo.UnitName,
                                 QuantityQuote = quo.Quantity,
                                 QuotePrice = quo.QuotePrice,
                                 Specifications = quo.Specifications,
                                 SupplierId = quo.SupplierId,
                                 SupplierName = quo.SupplierName,
                                 TotalNumber = quo.Quantity * quo.QuotePrice,
                             }).ToList();

                return new PagedResultDto<QuoteListDto>(
                    resul.Count(), resul);
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

        public async Task<long> UpdateContractStatus(ContractListDto input)
        {
            Contract supplier = await _contractRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.ExportStatus = supplier.ExportStatus;
            input.Code = supplier.Code;
            input.SupplierId = supplier.SupplierId;
            input.QuoteSynId = supplier.QuoteSynId;
            input.CreationTime = supplier.CreationTime;
            input.CreatorUserId = supplier.CreatorUserId;
            input.MouthNumber = supplier.MouthNumber;
            input.Price = supplier.Price;
            input.Indemnify = supplier.Indemnify;
            input.SellerDate = supplier.SellerDate;
            input.BankName = supplier.BankName;
            input.Stk = supplier.Stk;
            input.PositionA = supplier.PositionA;
            input.PositionB = supplier.PositionB;
            input.RepresentA = supplier.RepresentA;
            input.RepresentB = supplier.RepresentB;

            var webRootPath = this._Environment.WebRootPath;
            string path = "";
            path = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "sendMailCV.html";
            path = File.ReadAllText(path);
            var passlinkEmail = input.Link;
            var email = input.Email;
            var name = input.Name;
            if (supplier.CreatorUserId == AbpSession.UserId)
            {
                email = input.Email;
                name = input.Name;
                path = path.Replace("{{TenNguoiNhan}}", name);
                path = path.Replace("{{LinkFromEmail}}", passlinkEmail);
                await _sendMailService.SendEmailCvAsync(email, "Bạn có công việc mới :", path);
            } else if(supplier.Status == ContractEnum.ContractStatus.Original)
            {
                email = input.Email;
                name = input.Name;
                path = path.Replace("{{TenNguoiNhan}}", name);
                path = path.Replace("{{LinkFromEmail}}", passlinkEmail);
                await _sendMailService.SendEmailCvAsync(email, "Bạn có công việc mới :", path);
            }
            else
            {
                var user = _user.GetAll().Where(x => x.Id == supplier.CreatorUserId).FirstOrDefault();
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
                //    if (querya.Items[i].Id == supplier.CreatorUserId)
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


            ObjectMapper.Map(input, supplier);
            await _contractRepository.UpdateAsync(supplier);
            return input.Id;
        }

        public async Task<long> UpdateContract(ContractListDto input)
        {
            Contract supplier = await _contractRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.ExportStatus = supplier.ExportStatus;
            input.Code = supplier.Code;
            input.SupplierId = supplier.SupplierId;
            input.QuoteSynId = supplier.QuoteSynId;
            input.CreationTime = supplier.CreationTime;
            input.CreatorUserId = supplier.CreatorUserId;
            //input.MouthNumber = supplier.MouthNumber;
            //input.Price = supplier.Price;
            //input.Indemnify = supplier.Indemnify;
            //input.SellerDate = supplier.SellerDate;
            //input.BankName = supplier.BankName;
            //input.Stk = supplier.Stk;

            ObjectMapper.Map(input, supplier);
            await _contractRepository.UpdateAsync(supplier);
            return input.Id;
        }

        public async Task<PagedResultDto<ContractListDto>> GetAllInApprove(ContractSearch input)
        {
            try
            {
                if (input.status == ContractEnum.ContractStatus.Original)
                {
                    var query = _contractRepository.GetAll()
                    .WhereIf(input.status.HasValue, x => x.Status != ContractEnum.ContractStatus.Draft && x.Status != ContractEnum.ContractStatus.Approve)
                    .WhereIf(input.ExportStatus.HasValue, x => x.ExportStatus == input.ExportStatus)
                    .OrderByDescending(x => x.Id);

                    var queryQuoteRequests = _quoteRequestrepository.GetAll();

                    var resul = (from quo in queryQuoteRequests
                                 select new QuoteRequestsListDto
                                 {
                                     Id = quo.Id,
                                     QuotesSynthesiseId = quo.QuotesSynthesiseId,
                                     UnitName = quo.UnitName,
                                     QuantityQuote = quo.Quantity,
                                     QuotePrice = quo.QuotePrice,
                                     Specifications = quo.Specifications,
                                     SupplierId = quo.SupplierId,
                                     SupplierName = quo.SupplierName,
                                     TotalNumber = quo.Quantity * quo.QuotePrice,
                                 }).ToList();

                    var querySup = _supplierrepository.GetAll();

                    var queryQuoSyn = _quoteSynrepository.GetAll();

                    var result = (from contract in query
                                  join sup in querySup on contract.SupplierId equals sup.Id
                                  join quoSyn in queryQuoSyn on contract.QuoteSynId equals quoSyn.Id
                                  select new ContractListDto
                                  {
                                      Id = contract.Id,
                                      QuoteSynCode = quoSyn.Code,
                                      QuoteSynId = quoSyn.Id,
                                      Code = contract.Code,
                                      SupplierId = contract.SupplierId,
                                      SupplierName = sup.Name,
                                      Status = contract.Status,
                                      CreationTime = contract.CreationTime,
                                      LastModificationTime = contract.LastModificationTime.Value,
                                  }).ToList();

                    return new PagedResultDto<ContractListDto>(
                        result.Count(),
                        result
                        );
                }
                else {
                    var query = _contractRepository.GetAll()
                    .WhereIf(input.status.HasValue, x => x.Status != ContractEnum.ContractStatus.Draft && x.Status != ContractEnum.ContractStatus.Original && x.Status != ContractEnum.ContractStatus.RejectManager)
                    .WhereIf(input.ExportStatus.HasValue, x => x.ExportStatus == input.ExportStatus)
                    .OrderByDescending(x => x.Id);

                    var queryQuoteRequests = _quoteRequestrepository.GetAll();

                    var resul = (from quo in queryQuoteRequests
                                 select new QuoteRequestsListDto
                                 {
                                     Id = quo.Id,
                                     QuotesSynthesiseId = quo.QuotesSynthesiseId,
                                     UnitName = quo.UnitName,
                                     QuantityQuote = quo.Quantity,
                                     QuotePrice = quo.QuotePrice,
                                     Specifications = quo.Specifications,
                                     SupplierId = quo.SupplierId,
                                     SupplierName = quo.SupplierName,
                                     TotalNumber = quo.Quantity * quo.QuotePrice,
                                 }).ToList();

                    var querySup = _supplierrepository.GetAll();

                    var queryQuoSyn = _quoteSynrepository.GetAll();

                    var result = (from contract in query
                                  join sup in querySup on contract.SupplierId equals sup.Id
                                  join quoSyn in queryQuoSyn on contract.QuoteSynId equals quoSyn.Id
                                  select new ContractListDto
                                  {
                                      Id = contract.Id,
                                      QuoteSynCode = quoSyn.Code,
                                      QuoteSynId = quoSyn.Id,
                                      Code = contract.Code,
                                      SupplierId = contract.SupplierId,
                                      SupplierName = sup.Name,
                                      Status = contract.Status,
                                      CreationTime = contract.CreationTime,
                                      LastModificationTime = contract.LastModificationTime.Value,
                                  }).ToList();


                    return new PagedResultDto<ContractListDto>(
                        result.Count(),
                        result
                        );
                }

                
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateExportStatus(ContractListDto input)
        {
            try
            {
                Contract contract = await _contractRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                contract.ExportStatus = input.ExportStatus;
               
                await _contractRepository.UpdateAsync(contract);
                return input.Id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            }
        public async Task<decimal> TotalNumber(ContractSearch input)
        {
            try
            {

                var queryQuoteRequests = _quoteRequestrepository.GetAll().Where(x => x.QuotesSynthesiseId == input.quoSyn && x.SupplierId == input.Supper).ToList();
                //var query1 = _quoteRelarepository.GetAll().Where(x => x.QuoteSynthesiseId == input.quoSyn);
                //var query2 = _quoterepository.GetAll().Where(x => x.SupplierId == input.Supper);
               

                var resul = (from quo in queryQuoteRequests
                             select new QuoteListDto
                             {
                                 QuantityQuote = quo.Quantity,
                                 QuotePrice = quo.QuotePrice,
                                 Specifications = quo.Specifications,
                                 SupplierId = quo.SupplierId,
                                 SupplierName = quo.SupplierName,
                                 TotalNumber = quo.Quantity * quo.QuotePrice,
                             }).Select(x => x.TotalNumber).Sum();

                return resul;
            }

            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

        public async Task<string> CreateCode(int Id)
        {
            try
            {
                var query = _contractRepository.GetAll().ToList();
                string sinhma(string ma)
                {
                    string s = ma.Substring(5, ma.Length - 5);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "HDMB-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "HDMB-" + "0" + Convert.ToString(i);
                    else return "HDMB-" + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _contractRepository.GetAll().Where(x => x.Code != null).OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                Contract con = _contractRepository.FirstOrDefault(x => x.Id == Id);
                con.Code = sinhma(ma.ToString());
                await _contractRepository.UpdateAsync(con);
                return con.Code;

            }
            catch (Exception ex) 
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<FileDto> GetPOListDto(int idContract)
        {
            try
            {
                //Lay ra hop dong
                var contractdto = _contractRepository.Get(idContract);
                var resultContract = ObjectMapper.Map<ContractListDto>(contractdto);

                //Lay ra nha cung cap
                var idsuper = contractdto.SupplierId;

                var supperdto = _supplierrepository.Get((int)idsuper);
                var resultSup = ObjectMapper.Map<SupplierListDto>(supperdto);


                //Lay ra san pham trong kho
                var idquo = contractdto.QuoteSynId;

                var queryQuoteRequests = _quoteRequestrepository.GetAll().Where(x => x.QuotesSynthesiseId == idquo && x.SupplierId == idsuper);
               
                var query3 = _itemrepository.GetAll();

                var resul = (from quo in queryQuoteRequests
                             join item in query3 on quo.ItemId equals item.Id
                             select new QuoteListDto
                             {
                                 ItemId = item.Id,
                                 ItemName = item.Name,
                                 UnitName = quo.UnitName,
                                 QuantityQuote = quo.Quantity,
                                 QuotePrice = quo.QuotePrice,
                                 Specifications = quo.Specifications,
                                 SupplierId = quo.SupplierId,
                                 SupplierName = quo.SupplierName,
                                 TotalNumber = quo.Quantity * quo.QuotePrice,
                             }).ToList();

                //Lay ra tong
                var queryQuoteSum = _quoteRequestrepository.GetAll().Where(x => x.QuotesSynthesiseId == idquo && x.SupplierId == idsuper).ToList();

                var resulSum = (from quo in queryQuoteRequests
                             select new QuoteListDto
                             {
                                 QuantityQuote = quo.Quantity,
                                 QuotePrice = quo.QuotePrice,
                                 Specifications = quo.Specifications,
                                 SupplierId = quo.SupplierId,
                                 SupplierName = quo.SupplierName,
                                 TotalNumber = quo.Quantity * quo.QuotePrice,
                             }).Select(x => x.TotalNumber).Sum();

                return await _contractOrder.ExportPOToFile(resultContract, resulSum, resultSup, resul);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
