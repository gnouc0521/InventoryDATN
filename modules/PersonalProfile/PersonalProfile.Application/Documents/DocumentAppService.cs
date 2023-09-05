using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Categories;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.Storage.FileSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.DocumentEnum;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Documents
{
    [AbpAuthorize]
    public class DocumentAppService : netcoreAppServiceBase, IDocumentAppService
    {
        private readonly IRepository<Document, long> _docRepository;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        private readonly ICategoryAppService _categoryAppService;

        public DocumentAppService(IRepository<Document, long> docRepository,
            IFileSystemBlobProvider fileSystemBlobProvider,
            ICategoryAppService categoryAppService)
        {
            _docRepository = docRepository;
            _fileSystemBlobProvider = fileSystemBlobProvider;
            _categoryAppService = categoryAppService;
        }
        public async Task<PagedResultDto<DocListDto>> GetDocs(GetDocsInput input)
        {
            try
            {
                var query = _docRepository.GetAll()
                    //.WhereIf(input.EndIssuedDate)
                    .WhereIf(input.DocumentCategoryEnum.HasValue, u => u.DocumentCategoryEnum == input.DocumentCategoryEnum)
                    .WhereIf(input.DocumentCategoryTypeId.HasValue && input.DocumentCategoryTypeId.Value > 0, u => u.DocumentCategoryTypeId == input.DocumentCategoryTypeId)
                    .WhereIf(input.DocumentTypeEnum != null && input.DocumentTypeEnum > 0, u => u.DocumentTypeEnum == input.DocumentTypeEnum)
                    .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.DecisionNumber.Contains(input.SearchTerm) || u.Description.Contains(input.SearchTerm));

                var docCount = await query.CountAsync();

                var docs = await query.OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var docListDto = ObjectMapper.Map<List<DocListDto>>(docs);

                /* Re-fill for content */
                //await FillEnumName(docListDto);
                foreach (var d in docListDto)
                {
                    d.DocumentTypeName = d.DocumentTypeEnum.HasValue ? L(d.DocumentTypeEnum.Value.ToString()) : string.Empty;
                }

                return new PagedResultDto<DocListDto>(
                    docCount,
                    docListDto
                    );
            }
            catch (Exception ex) { throw new UserFriendlyException(ex.Message); }
        }
        #region TCCB-VEA
        public async Task<PagedResultDto<DocListDto>> GetDashboardDocs(GetDocsInput input)
        {
            try
            {
                // comment for allow search all documents
                //if (input.DocumentCategoryEnum == 0)
                //{
                //    throw new Exception("Unknow category");
                //}
                var query = _docRepository.GetAll();
                //.WhereIf(input.DocumentCategoryEnum > 0, u => u.DocumentCategoryEnum == input.DocumentCategoryEnum)
                //.WhereIf(input.DocumentTypeEnum != null && input.DocumentTypeEnum > 0, u => u.DocumentTypeEnum == input.DocumentTypeEnum)
                //.WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.DecisionNumber.Contains(input.SearchTerm) || u.Description.Contains(input.SearchTerm));

                //var docCount = await query.CountAsync();

                var docs = await query.OrderBy(input.Sorting)
                    .Take(4)
                    .ToListAsync();

                var docListDto = ObjectMapper.Map<List<DocListDto>>(docs);

                /* Re-fill for content */
                //await FillEnumName(docListDto);
                //foreach (var d in docListDto)
                //{
                //    /* d.IssueDateString = d.IssuedDate.ToString("dd/MM/yyyy"); // KHÔNG CHẠY FORMAT DATETIME TRONG DB, SERVICE, CONTROLLER */
                //    d.DocumentTypeName = d.DocumentTypeEnum.HasValue ? L(d.DocumentTypeEnum.Value.ToString()) : string.Empty;
                //}

                return new PagedResultDto<DocListDto>(
                    0,
                    docListDto
                    );
            }
            catch (Exception ex) { throw new UserFriendlyException(ex.Message); }
        }
        #endregion
        public async Task<DocListDto> GetById(long id)
        {
            try
            {
                var entity = await _docRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                {
                    throw new Exception("Not exist");
                }
                return ObjectMapper.Map<DocListDto>(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task<DocListDto> GetDocForEdit(NullableIdDto<long> input)
        {
            var output = new DocListDto
            {

            };

            if (!input.Id.HasValue)
            {
                return output;
            }
            else
            {
                try
                {
                    var doc = await GetById(input.Id.Value);
                    return ObjectMapper.Map<DocListDto>(doc);
                }
                catch
                {
                    return output;
                }
            }
        }

        public async Task<int> GetDocumentCategoryEnum(int docCategoryId)
        {
            var category = await _categoryAppService.GetCategorybyId(docCategoryId);
            var docEnum = (DocumentCategoryEnum)Enum.Parse(typeof(DocumentCategoryEnum), Enum.GetName(typeof(CategoryType), category.CategoryType));
            return (int)docEnum;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Documents_Create)]
        public async Task<long> CreateAsync(DocListDto input)
        {
            try
            {
                Document document = ObjectMapper.Map<Document>(input);
                document.TenantId = AbpSession.TenantId;
                var docCategoryType = await _categoryAppService.GetCategorybyId(document.DocumentCategoryTypeId);
                document.DocumentCategoryEnum = (DocumentCategoryEnum)Enum.Parse(typeof(DocumentCategoryEnum), Enum.GetName(typeof(CategoryType), docCategoryType.CategoryType));
                return await _docRepository.InsertAndGetIdAsync(document);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Documents_Edit)]
        public async Task UpdateAsync(DocListDto input)
        {
            try
            {
                Document document = await _docRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                var docCategoryType = await _categoryAppService.GetCategorybyId(input.DocumentCategoryTypeId);
                input.DocumentCategoryEnum = (DocumentCategoryEnum)Enum.Parse(typeof(DocumentCategoryEnum), Enum.GetName(typeof(CategoryType), docCategoryType.CategoryType));
                ObjectMapper.Map(input, document);

                await _docRepository.UpdateAsync(document);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteDoc(EntityDto<long> input)
        {
            try
            {
                var document = _docRepository.FirstOrDefault(input.Id);
                await _fileSystemBlobProvider.DeleteAsync(document.FilePath);
                await _docRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
