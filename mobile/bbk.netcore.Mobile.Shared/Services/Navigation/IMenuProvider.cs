using System.Collections.Generic;
using MvvmHelpers;
using bbk.netcore.Models.NavigationMenu;

namespace bbk.netcore.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}

