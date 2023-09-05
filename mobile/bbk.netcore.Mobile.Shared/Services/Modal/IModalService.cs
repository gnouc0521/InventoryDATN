using System.Threading.Tasks;
using bbk.netcore.Views;
using Xamarin.Forms;

namespace bbk.netcore.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}


