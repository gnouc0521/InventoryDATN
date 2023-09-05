using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using bbk.netcore.mdl.OMS.Application.OrdersDetail.Export;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Orders
{
    [AbpAuthorize]
    public class OrdersDetailAppService : ApplicationService, IOrdersDetailAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<OrderDetail, long> _orderDetailRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<PurchaseAssignment> _purchaseAssignmentRepository;
        private readonly IRepository<ImportRequestDetail> _importRequestDetailRepository;
        private readonly IRepository<PurchasesRequestDetail, long> _purchasesRequestDetail;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<User, long> _user;
        private readonly IRepository<Items, long> _itemrepository;
        private readonly IRepository<UserWorkCount> _userWorkCountRepository;
        private readonly IRepository<PurchasesRequest, long> _PurchasesRequest;
        private readonly IRepository<PurchasesRequestDetail, long> _PurchasesRequestDetail;
        private readonly IExportOrder _exportOrder;
        public OrdersDetailAppService(
                                          IRepository<Order, long> orderRepository,
                                          IRepository<OrderDetail, long> orderDetailRepository,
                                          IRepository<Items, long> itemsRepository,
                                          IRepository<Supplier> supplierRepository,
                                          IRepository<PurchaseAssignment> purchaseAssignmentRepository,
                                          IRepository<ImportRequestDetail> importRequestDetailRepository,
                                          IRepository<PurchasesRequestDetail, long> purchasesRequestDetail,
                                          IRepository<Contract> contractRepository,
                                          IRepository<Items, long> itemrepository,
                                          IRepository<UserWorkCount> userWorkCountRepository,
                                          IRepository<PurchasesRequest, long> purchasesRequest,
                                          IRepository<PurchasesRequestDetail, long> PurchasesRequestDetail,
                                          IExportOrder exportOrder,
                                          IRepository<User, long> user)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _supplierRepository = supplierRepository;
            _purchaseAssignmentRepository = purchaseAssignmentRepository;
            _importRequestDetailRepository = importRequestDetailRepository;
            _purchasesRequestDetail = purchasesRequestDetail;
            _contractRepository = contractRepository;
            _itemrepository = itemrepository;
            _purchaseAssignmentRepository = purchaseAssignmentRepository;
            _userWorkCountRepository = userWorkCountRepository;
            _PurchasesRequest = purchasesRequest;
            _PurchasesRequestDetail = purchasesRequestDetail;
            _exportOrder = exportOrder;
            _user = user;
        }

        public async Task<long> Create(OrdersDetailListDto input)
        {
            try
            {

                OrderDetail newItemId = ObjectMapper.Map<OrderDetail>(input);

                var newId = await _orderDetailRepository.InsertAndGetIdAsync(newItemId);


                return input.Id;
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
                await _orderDetailRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<OrdersDetailListDto>> GetAll(OrdersDetailSearch input)
        {
            try
            {
                List<OrdersDetailListDto> orderListDtos = new List<OrdersDetailListDto>();
                var queryorder = _orderDetailRepository.GetAll().Where(x => x.OrderId == input.OrderId).Distinct().ToList();
                var item = _itemrepository.GetAll();

                var query = (from qu in queryorder
                             join it in item on qu.ItemId equals it.Id
                             select new OrdersDetailListDto
                             {
                                 Itemcode = it.Name + "-" + it.ItemCode,
                                 OrderPrice = qu.OrderPrice,
                                 Quantity = qu.Quantity,
                                 UnitName = qu.UnitName,

                             }).ToList();

                return new PagedResultDto<OrdersDetailListDto>(
                query.Count,
                query
                ); ;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<OrdersDetailListDto> GetAsync(EntityDto<long> itemId)
        {
            try
            {
                var item = _orderDetailRepository.Get(itemId.Id);
                OrdersDetailListDto newItem = ObjectMapper.Map<OrdersDetailListDto>(item);
                return newItem;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public Task<long> Update(OrdersDetailListDto input)
        {
            throw new NotImplementedException();
        }





        /// <summary>
        /// ham tao de get nhiem vu mua hang 
        /// created by : Kien
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<OrdersDetailListDto>> GetItemMission()
        {
            try
            {

                var puAss = _purchaseAssignmentRepository.GetAll();
                var order = _orderRepository.GetAll();
                var ordetail = _orderDetailRepository.GetAll();
                var purchasesDetail = _purchasesRequestDetail.GetAll();
                var sup = _supplierRepository.GetAll();
                var contrat = _contractRepository.GetAll();
                var importDetail = _importRequestDetailRepository.GetAll();
                var userwork = _userWorkCountRepository.GetAll();
                var query = (from or in order
                             join hd in contrat on or.ContractId equals hd.Id
                             join ord in ordetail on or.Id equals ord.OrderId
                             join ass in puAss on ord.ItemId equals ass.ItemId
                             join uwk in userwork on ass.Id equals uwk.PurchaseAssignmentId into t
                             from uwk in t.DefaultIfEmpty()
                             join ncc in sup on hd.SupplierId equals ncc.Id
                             where or.UserId == AbpSession.UserId
                             select new OrdersDetailListDto
                             {
                                 Id = or.Id,
                                 ContractCode = hd.Code,
                                 Note = ord.Note,
                                 OrderCode = or.Code,
                                 SupplierName = ncc.Name,
                                 UserId = or.UserId,
                                 OrderStatus = or.OrderStatus,
                                 CreationTime = or.CreationTime,
                                 purAssigmentId = ass.Id,
                                 WorkStatus = uwk.WorkStatus,

                             }).ToList();
                var CodePu = query.Select(x => x.OrderCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in query where i == z.OrderCode select z.Id).ToArray()
                               let ContractCode = (from z in query where i == z.OrderCode select z.ContractCode).Distinct().ToList()
                               let note = (from z in query where i == z.OrderCode select z.Note).Distinct().ToList()
                               let supplier = (from z in query where i == z.OrderCode select z.SupplierName).Distinct().ToList()
                               let status = (from z in query where i == z.OrderCode select z.OrderStatus).Distinct().ToList()
                               let UserId = (from z in query where i == z.OrderCode select z.UserId).Distinct().ToList()
                               let CreatimeTT = (from z in query where i == z.OrderCode select z.CreationTime).ToArray()
                               let WorkStatus = (from z in query where i == z.OrderCode select z.WorkStatus).Distinct().ToList()
                               let purAssigmentId = (from z in query where i == z.OrderCode select z.purAssigmentId).Distinct().ToList()
                               select new OrdersDetailListDto
                               {
                                   Id = idset[0],
                                   OrderCode = i,
                                   Note = note[0],
                                   UserId = UserId[0],
                                   SupplierName = supplier[0],
                                   OrderStatus = status[0],
                                   ContractCode = ContractCode[0],
                                   CreationTime = CreatimeTT[0],
                                   WorkStatus = WorkStatus[0],
                                   purAssigmentId = purAssigmentId[0],
                               }
                                );


                return new PagedResultDto<OrdersDetailListDto>(Haquery.Count(), Haquery.ToList());

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<OrdersDetailListDto>> GetYCNK()
        {
            try
            {
                var order = _orderRepository.GetAll();
                var ordetail = _orderDetailRepository.GetAll();
                var purchasesDetail = _purchasesRequestDetail.GetAll();
                var sup = _supplierRepository.GetAll();
                var contrat = _contractRepository.GetAll();
                //GetUsersInput getUsersInput = new GetUsersInput();
                //getUsersInput.MaxResultCount = 1000;
                //var querya = _userAppService.GetUsers(getUsersInput).Result.Items.ToList();

                string fullname = "";
                var user = _user.GetAll().ToList();
                foreach (var u in user)
                {
                    foreach (var item in order)
                    {
                        if (u.Id == item.UserId)
                        {
                            fullname = u.Surname + " " + u.Name;
                            break;
                        }
                    }
                }



                var query = (from or in order
                             join hd in contrat on or.ContractId equals hd.Id
                             join ord in ordetail on or.Id equals ord.OrderId
                             join ncc in sup on hd.SupplierId equals ncc.Id
                             where or.OrderStatus == PurchasesRequestEnum.OrderStatus.Approve
                             select new OrdersDetailListDto
                             {
                                 Id = or.Id,
                                 ContractCode = hd.Code,
                                 OrderCode = or.Code,
                                 SupplierName = ncc.Name,
                                 CreationTime = or.CreationTime,
                                 CreatedBy = fullname,
                                 StatusCreateYCNK = or.StatusCreateYCNK,
                             }).ToList();
                var CodePu = query.Select(x => x.OrderCode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let idset = (from z in query where i == z.OrderCode select z.Id).ToArray()
                               let ContractCode = (from z in query where i == z.OrderCode select z.ContractCode).Distinct().ToList()
                               let supplier = (from z in query where i == z.OrderCode select z.SupplierName).Distinct().ToList()
                               let CreatimeTT = (from z in query where i == z.OrderCode select z.CreationTime).ToArray()
                               let createdBy = (from z in query where i == z.OrderCode select z.CreatedBy).Distinct().ToList()
                               let StatusCreateYCNK = (from z in query where i == z.OrderCode select z.StatusCreateYCNK).Distinct().ToList()
                               select new OrdersDetailListDto
                               {
                                   Id = idset[0],
                                   OrderCode = i,
                                   SupplierName = supplier[0],
                                   ContractCode = ContractCode[0],
                                   CreationTime = CreatimeTT[0],
                                   CreatedBy = createdBy[0],
                                   StatusCreateYCNK = StatusCreateYCNK[0]
                               }
                                );


                return new PagedResultDto<OrdersDetailListDto>(Haquery.Count(), Haquery.OrderByDescending(x=>x.Id).ToList());

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<OrdersDetailListDto>> GetAllDetail(OrdersDetailSearch input)
        {
            try
            {
                List<OrdersDetailListDto> orderListDtos = new List<OrdersDetailListDto>();
                var queryorder = _orderDetailRepository.GetAll().Where(x => x.OrderId == input.OrderId).Distinct().ToList();
                var item = _itemrepository.GetAll();
                var puAss = _purchaseAssignmentRepository.GetAll();
                var pur = _PurchasesRequest.GetAll();
                var purd = _PurchasesRequestDetail.GetAll();
                var query = (from qu in queryorder
                             join it in item on qu.ItemId equals it.Id
                             join ass in puAss on qu.ItemId equals ass.ItemId
                             join p in pur on ass.PurchasesSynthesiseId equals p.PurchasesSynthesiseId
                             into t from p in t.DefaultIfEmpty()
                             join pd in purd on p.Id equals pd.PurchasesRequestId
                             into z from pd in z.DefaultIfEmpty()
                             select new OrdersDetailListDto
                             {
                                 Itemcode = it.Name + "-" + it.ItemCode,
                                 OrderPrice = qu.OrderPrice,
                                 Quantity = qu.Quantity,
                                 UnitName = qu.UnitName,
                                 DateTimeNeed = pd.TimeNeeded
                             }).ToList();

                var CodePu = query.Select(x => x.Itemcode).Distinct().ToArray();

                var Haquery = (from i in CodePu
                               let OrderPrice = (from z in query where i == z.Itemcode select z.OrderPrice).Distinct().ToList()
                               let Quantity = (from z in query where i == z.Itemcode select z.Quantity).Distinct().ToList()
                               let UnitName = (from z in query where i == z.Itemcode select z.UnitName).Distinct().ToList()
                               let DateTimeNeed = (from z in query where i == z.Itemcode select z?.DateTimeNeed).Distinct().ToList()
                               select new OrdersDetailListDto
                               {
                                   Itemcode = i,
                                   OrderPrice = OrderPrice[0],
                                   Quantity = Quantity[0],
                                   UnitName = UnitName[0],
                                   DateTimeNeed = DateTimeNeed[0]
                               }
                                ).ToList();


                return new PagedResultDto<OrdersDetailListDto>(
                Haquery.Count,
                Haquery
                ); ;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<FileDto> GetPOListDto(OrdersDetailListDto ordersDetailListDto)
        {
            try
            {
                //List<OrdersDetailListDto> result = new List<OrdersDetailListDto>();
              


                var orderDetail = _orderDetailRepository.GetAll().Where(x=>x.OrderId == ordersDetailListDto.Id);
                var queryorder = _orderRepository.GetAll().Where(x=>x.Id == ordersDetailListDto.Id);
                var items = _itemrepository.GetAll();
                var ouput = (from orderdetail in orderDetail
                             join item in items on orderdetail.ItemId equals item.Id
                             join order in queryorder on orderdetail.OrderId equals order.Id
                             select new OrdersDetailListDto
                             {
                                 OrderCode = order.Code,
                                 Quantity = orderdetail.Quantity,
                                 Itemcode = item.ItemCode,
                                 OrderPrice = orderdetail.OrderPrice,
                                 TotalPrice = orderdetail.OrderPrice * orderdetail.Quantity,
                                 ItemName = item.Name,
                                 UnitName = orderdetail.UnitName,
                                 Note = orderdetail.Note,
                                 Specifications = orderdetail.Specifications,   
                             }).ToList();

                // tổng giá trị order 
                var totalorder = (from output in ouput
                                  select output).Select(x => x.TotalPrice).Sum();


                // tên nhà cung cấp 
                var querycontract = _contractRepository.GetAll();
                var querysupplier = _supplierRepository.GetAll();

                var SupplierName = (from contract in querycontract
                                   join order in queryorder on contract.Id equals order.ContractId
                                   join supplier in querysupplier on contract.SupplierId equals supplier.Id
                                   select supplier).ToList()[0].Name;


                return await _exportOrder.ExportPOToFile(ouput, DateTime.Now, totalorder, SupplierName);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
