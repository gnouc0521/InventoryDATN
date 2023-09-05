using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<PagedResultDto<DocListDto>> GetDocs(GetDocsInput input);

        Task<DocListDto> GetById(long id);

        Task<DocListDto> GetDocForEdit(NullableIdDto<long> input);

        Task<long> CreateAsync(DocListDto input);

        Task UpdateAsync(DocListDto input);

        Task DeleteDoc(EntityDto<long> input);

        Task<int> GetDocumentCategoryEnum(int docCategoryId);
    }
}
