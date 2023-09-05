using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.TransferDetails.Dto;
using bbk.netcore.mdl.OMS.Application.Transfers.Dto;
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

namespace bbk.netcore.mdl.OMS.Application.Transfers
{
    public class TransferAppService : ApplicationService, ITransferAppService
    {
        private readonly IRepository<Transfer, long> _transferRepository;

        private readonly IRepository<TransferDetail, long> _transferDetailRepository;
        private readonly IRepository<Warehouse> _wareHouseRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly  ILogger<TransferAppService> logger;
        private IHostingEnvironment _Environment;
        private readonly ISendMailService _sendMailService;
        private readonly IRepository<User, long> _user;
        public TransferAppService(IRepository<Transfer, long> transferRepository,
                                 IRepository<TransferDetail, long> transferDetailRepository,
                                 IRepository<Warehouse> wareHouseRepository,
                                 IRepository<Supplier> supplierRepository,
                                 ILogger<TransferAppService> _logger,
                                 IHostingEnvironment Environment,
                                 ISendMailService sendMailService,
                                 IRepository<User, long> user
           )
        {
            _transferRepository = transferRepository;
            _transferDetailRepository = transferDetailRepository;
            _wareHouseRepository = wareHouseRepository;
            _supplierRepository = supplierRepository;
            logger = _logger;
            _Environment = Environment;
            _sendMailService = sendMailService;
            _user = user;
        }

        public async Task<long> Create(TransferCreateDto input)
        {
            try
            {
                var query = await _transferRepository.GetAll().ToListAsync();
                string sinhma(string ma)
                {
                    string s = ma.Substring(5, ma.Length - 5);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "YCĐC-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "YCĐC-" + "0" + Convert.ToString(i);
                    else return "YCĐC-" + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "00000000";
                }
                else
                {
                    ma = _transferRepository.GetAll().OrderByDescending(x => x.TransferCode).Select(x => x.TransferCode).ToList().First();
                }

                logger.LogInformation("input1 : ", input);
              
                input.TransferCode = sinhma(ma.ToString());
                Transfer newItemId = ObjectMapper.Map<Transfer>(input);
               // newItemId.Status = QuoteEnum.SyntheticQuote.Draft;
               
                var newId = await _transferRepository.InsertAndGetIdAsync(newItemId);
                logger.LogInformation("input2 : ", newId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _transferRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(TransferListDto input)
        {
            Transfer supplier = await _transferRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //input.TransferCode = supplier.TransferCode;
            ////input.CommentText = supplier.CommentText;
            //input.CreationTime = supplier.CreationTime;
            //input.IdWarehouseExport = supplier.IdWarehouseExport;
            //input.BrowsingTime = DateTime.Now;
            //input.TransferNote = supplier.TransferNote;
            //input.StatusImportPrice = supplier.StatusImportPrice;
            ObjectMapper.Map(input, supplier);
            await _transferRepository.UpdateAsync(supplier);
            return input.Id;
        }

        public async Task<PagedResultDto<TransferListDto>> GetAll(TransferSearch input)
        {
            try
            {
                var query = _transferRepository.GetAll().Where(x => x.CreatorUserId == AbpSession.UserId)
                                            .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.TransferCode.Contains(input.SearchTerm))
                                            
                                            .OrderByDescending(x => x.Id);
                var warehouse = _wareHouseRepository.GetAll();

                var results = (from i in query
                               join ware in warehouse on i.IdWarehouseExport equals ware.Id
                               select new TransferListDto
                               {
                                   Id = i.Id,
                                   IdWarehouseExport = ware.Id,
                                   NameWareHouseExport = ware.Name,
                                   BrowsingTime = i.BrowsingTime,
                                   CreationTime = i.CreationTime,
                                   Status = i.Status,
                                   TransferCode = i.TransferCode
                               }).OrderByDescending(x => x.CreationTime).OrderBy(x => x.Status).ToList();

                //var result = ObjectMapper.Map<List<TransferListDto>>(query);

                return new PagedResultDto<TransferListDto>(results.Count(), results);


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<TransferListDto> GetAsync(EntityDto<long> itemId)
        {
            var item = _transferRepository.Get(itemId.Id);
            TransferListDto newItem = ObjectMapper.Map<TransferListDto>(item);
            return newItem;
        }

        /// <summary>
        /// ham lấy ra những yêu cầu dc đã phê duyêt
        /// created by : Kiên
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<TransferListDto>> GetTransferImp()
        {
            try
            {
                var tranferList = _transferRepository.GetAll();
                var tranferDetailList = _transferDetailRepository.GetAll();
                var warhouseList = _wareHouseRepository.GetAll();
                var result = (from tl in tranferList
                              join wh in warhouseList on tl.IdWarehouseExport equals wh.Id
                              where tl.Status == TransferEnum.TransferStatus.Approve
                              select new TransferListDto
                              {
                                  Id = tl.Id,
                                  TransferCode = tl.TransferCode,
                                  IdWarehouseExport = tl.IdWarehouseExport,
                                  NameWareHouseExport = wh.Name,
                                  CreationTime = tl.CreationTime,
                                  Status= tl.Status,
                                  BrowsingTime = tl.BrowsingTime,
                                  StatusImportPrice = tl.StatusImportPrice,

                              }).ToList();
                var result_detail = (from r in result
                                     join tdl in tranferDetailList on r.Id equals tdl.TransferId
                                     join wh in warhouseList on tdl.IdWarehouseReceiving equals wh.Id
                                     select new TransferListDto
                                     {
                                         Id = r.Id,
                                         TransferCode = r.TransferCode,
                                         NameWareHouseExport = r.NameWareHouseExport,
                                         IdWarehouseReceiving = r.IdWarehouseReceiving,
                                         NameWareHouseReceiving = wh.Name,
                                         CreationTime = r.CreationTime,
                                         BrowsingTime = r.BrowsingTime,
                                         Status = r.Status,
                                         StatusImportPrice = r.StatusImportPrice,
                                     }).ToList();

                var CodePu = result_detail.Select(x => x.TransferCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in result_detail where i == z.TransferCode select z.Id).ToArray()
                               let idWarehouseReceiving = (from z in result_detail where i == z.TransferCode select z.IdWarehouseReceiving).Distinct().First()
                               let nameWareHouseReceiving = (from z in result_detail where i == z.TransferCode select z.NameWareHouseReceiving).Distinct().First()
                               let nameWareHouseExport = (from z in result_detail where i == z.TransferCode select z.NameWareHouseExport).Distinct().First()
                               let CreatimeTT = (from z in result_detail where i == z.TransferCode select z.CreationTime).First()
                               let BrowsingTime = (from z in result_detail where i == z.TransferCode select z.BrowsingTime).First()
                               let StatusImportPrice = (from z in result_detail where i == z.TransferCode select z.StatusImportPrice).First()
                               let status = (from z in result_detail where i == z.TransferCode select z.Status).First()
                               select new TransferListDto
                               {
                                   Id = idset[0],
                                   TransferCode = i,
                                   NameWareHouseReceiving = nameWareHouseReceiving,
                                   IdWarehouseReceiving = idWarehouseReceiving,
                                   NameWareHouseExport = nameWareHouseExport,
                                   Status= status,
                                   CreationTime = CreatimeTT,
                                   BrowsingTime = BrowsingTime,
                                   StatusImportPrice = StatusImportPrice
                               }
                                );


                return new PagedResultDto<TransferListDto>(Haquery.Count(), Haquery.Distinct().ToList());

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateExportStatus(TransferListDto input)
        {
            Transfer supplier = await _transferRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            supplier.ExportStatus = input.ExportStatus;
            await _transferRepository.UpdateAsync(supplier);
            return input.Id;
        }

        public async Task<PagedResultDto<TransferListDto>> GetTransferExp(TransferSearch input)
        {
            try
            {
                var tranferList = _transferRepository.GetAll().WhereIf(input.ExportStatus.HasValue,x=>x.ExportStatus == input.ExportStatus.Value).ToList();
                var tranferDetailList = _transferDetailRepository.GetAll();
                var warhouseList = _wareHouseRepository.GetAll();
                var result = (from tl in tranferList
                              join wh in warhouseList on tl.IdWarehouseExport equals wh.Id
                              where tl.Status == TransferEnum.TransferStatus.Approve
                              select new TransferListDto
                              {
                                  Id = tl.Id,
                                  TransferCode = tl.TransferCode,
                                  IdWarehouseExport = tl.IdWarehouseExport,
                                  NameWareHouseExport = wh.Name,
                                  CreationTime = tl.CreationTime,
                                  BrowsingTime = tl.BrowsingTime,
                                  StatusImportPrice = tl.StatusImportPrice,

                              }).ToList();
                var result_detail = (from r in result
                                     join tdl in tranferDetailList on r.Id equals tdl.TransferId
                                     join wh in warhouseList on tdl.IdWarehouseReceiving equals wh.Id
                                     select new TransferListDto
                                     {
                                         Id = r.Id,
                                         TransferCode = r.TransferCode,
                                         NameWareHouseExport = r.NameWareHouseExport,
                                         IdWarehouseReceiving = r.IdWarehouseReceiving,
                                         NameWareHouseReceiving = wh.Name,
                                         CreationTime = r.CreationTime,
                                         BrowsingTime = r.BrowsingTime,
                                         StatusImportPrice = r.StatusImportPrice,
                                     }).ToList();

                var CodePu = result_detail.Select(x => x.TransferCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in result_detail where i == z.TransferCode select z.Id).ToArray()
                               let idWarehouseReceiving = (from z in result_detail where i == z.TransferCode select z.IdWarehouseReceiving).Distinct().First()
                               let nameWareHouseReceiving = (from z in result_detail where i == z.TransferCode select z.NameWareHouseReceiving).Distinct().First()
                               let nameWareHouseExport = (from z in result_detail where i == z.TransferCode select z.NameWareHouseExport).Distinct().First()
                               let CreatimeTT = (from z in result_detail where i == z.TransferCode select z.CreationTime).First()
                               let BrowsingTime = (from z in result_detail where i == z.TransferCode select z.BrowsingTime).First()
                               let StatusImportPrice = (from z in result_detail where i == z.TransferCode select z.StatusImportPrice).First()
                               select new TransferListDto
                               {
                                   Id = idset[0],
                                   TransferCode = i,
                                   NameWareHouseReceiving = nameWareHouseReceiving,
                                   IdWarehouseReceiving = idWarehouseReceiving,
                                   NameWareHouseExport = nameWareHouseExport,
                                   CreationTime = CreatimeTT,
                                   BrowsingTime = BrowsingTime,
                                   StatusImportPrice = StatusImportPrice
                               }
                                );


                return new PagedResultDto<TransferListDto>(Haquery.Count(), Haquery.Distinct().ToList());

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateTransferStatus(TransferListDto input)
        {
            Transfer supplier = await _transferRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            supplier.Status = input.Status;
            supplier.CommentText = input.CommentText;

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

            await _transferRepository.UpdateAsync(supplier);
            return input.Id;
        }

        public async Task<PagedResultDto<TransferListDto>> GetAllApprove(TransferSearch input)
        {
            try
            {
                var query = _transferRepository.GetAll().Where(x => x.Status != TransferEnum.TransferStatus.Original)
                                            .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.TransferCode.Contains(input.SearchTerm))

                                            .OrderByDescending(x => x.Id).ToList();
                var tranferList = _transferDetailRepository.GetAll().ToList();
                var warehouse = _wareHouseRepository.GetAll();

                var transDetail = (from i in tranferList
                                   join wh in warehouse on i.IdWarehouseReceiving equals wh.Id
                                   select new TransferDetailListDto
                                   {
                                       Id = i.Id,
                                       IdWarehouseReceiving = i.IdWarehouseReceiving,
                                       WarehouseReceivingName = wh.Name,
                                       TransferId = i.TransferId
                                   }).ToList();


                var results = (from i in query
                               join ware in warehouse on i.IdWarehouseExport equals ware.Id
                               let nameWare = (from transferDt in transDetail where i.Id == transferDt.TransferId select transferDt.WarehouseReceivingName).Distinct().ToList()
                               select new TransferListDto
                               {
                                   Id = i.Id,
                                   IdWarehouseExport = ware.Id,
                                   NameWareHouseExport = ware.Name,
                                   BrowsingTime = i.BrowsingTime,
                                   CreationTime = i.CreationTime,
                                   Status = i.Status,
                                   TransferCode = i.TransferCode,
                                   ListNameWareHouseReceiving = nameWare,
                               }).OrderByDescending(x => x.CreationTime).OrderBy(x => x.Status).ToList();

                //var result = ObjectMapper.Map<List<TransferListDto>>(query);

                return new PagedResultDto<TransferListDto>(results.Count(), results);


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateTransfer(TransferListDto input)
        {
            try
            {
                Transfer supplier = await _transferRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                supplier.IdWarehouseExport = input.IdWarehouseExport;
                supplier.TransferNote = input.TransferNote;
                supplier.Status = input.Status;
                //input.TransferCode = supplier.TransferCode;
                ////input.CommentText = supplier.CommentText;
                //input.CreationTime = supplier.CreationTime;
                //input.IdWarehouseExport = supplier.IdWarehouseExport;
                //input.BrowsingTime = DateTime.Now;
                //input.TransferNote = supplier.TransferNote;
                //input.StatusImportPrice = supplier.StatusImportPrice;
                //ObjectMapper.Map(input, supplier);
                await _transferRepository.UpdateAsync(supplier);
                return input.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
