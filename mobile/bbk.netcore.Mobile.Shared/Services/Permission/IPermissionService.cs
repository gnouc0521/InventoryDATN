namespace bbk.netcore.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}

