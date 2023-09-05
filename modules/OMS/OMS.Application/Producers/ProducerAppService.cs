using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using bbk.netcore.Storage.FileSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Producers
{
    [AbpAuthorize]
    public class ProducerAppService : ApplicationService, IProducerAppService
    {
        private readonly IRepository<Producer> _producerrepository;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        public ProducerAppService(IRepository<Producer> producerrepository, IFileSystemBlobProvider fileSystemBlobProvider)
        {
          _producerrepository= producerrepository;
            _fileSystemBlobProvider= fileSystemBlobProvider;
        }


        public async Task<PagedResultDto<ProducerListDto>> GetAll(GetProducerInput input)
        {
            try
            {
                var query = _producerrepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Name.Contains(input.SearchTerm))
                      .OrderBy(x => x.Id);
                var ProducerCount = query.Count();


                var ProducerListDto = ObjectMapper.Map<List<ProducerListDto>>(query);
                return new PagedResultDto<ProducerListDto>(
                  ProducerCount,
                  ProducerListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Create(ProducerCreateDto input)
        {
            try
            {
                string sinhma(string ma)
                {
                    string s = ma.Substring(4, ma.Length - 4);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "NSX-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "NSX-" + "0" + Convert.ToString(i);
                    else return "NSX-" + Convert.ToString(i);

                }
                string ma;
                var query = await _producerrepository.GetAll().ToListAsync();
                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _producerrepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();


                }



                input.Code = sinhma(ma.ToString());
                Producer newItemId = ObjectMapper.Map<Producer>(input);
                var newId = await _producerrepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

     

        public async Task<long> Update(ProducerListDto input)
        {
            Producer producer = await _producerrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.Code = producer.Code;
            ObjectMapper.Map(input, producer);
            await _producerrepository.UpdateAsync(producer);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _producerrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<Address> GetAddress(string filePath, string superiorId)
        {
            string file = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.DataAddress + @"\\" + filePath));
            var data = ReadJson<Address>.ConvertJsonToObject(file);
            Address result = new Address
            {
                Addresses = data.Addresses.WhereIf(!string.IsNullOrEmpty(superiorId), u => u.SuperiorId == superiorId).OrderBy(u => u.Name).ToList()
            };
            return await Task.FromResult(result);
        }
        public async Task<ProducerListDto> GetAsync(EntityDto itemId)
        {
            var item = _producerrepository.Get(itemId.Id);
            ProducerListDto newItem = ObjectMapper.Map<ProducerListDto>(item);
            return newItem;
        }

        public async Task<List<ProducerListDto>> GetProducerList()
        {
            try
            {
                var query = _producerrepository.GetAll().OrderBy(x => x.Id);

                var listproducer = ObjectMapper.Map<List<ProducerListDto>>(query);
                return listproducer;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
