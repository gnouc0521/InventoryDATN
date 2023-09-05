using Abp.Application.Services;
using bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations
{
    public interface IPropertyDeclarationAppService : IApplicationService
    {
        Task<int> Create(PropertyDeclarationDto propertyDeclarationDto);

        Task<int> Update(PropertyDeclarationDto propertyDeclarationDto);

        Task<List<PropertyDeclarationDto>> GetAll(GetAllFilter filter);

        Task<PropertyDeclarationDto> GetById(int id);

        Task DeleteById(DeletePropertyDeclarationDto deletePropertyDeclarationDto);

        Task UploadFile(UploadFileDto uploadFileDto);

        Task DeleteFile(int id);
    }
}
