using System.Threading.Tasks;
using bbk.netcore.ApiClient.Models;

namespace bbk.netcore.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }
        Task LoginUserAsync();
    }
}


