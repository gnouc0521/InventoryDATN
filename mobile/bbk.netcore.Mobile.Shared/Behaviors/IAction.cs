using Xamarin.Forms.Internals;

namespace bbk.netcore.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}

