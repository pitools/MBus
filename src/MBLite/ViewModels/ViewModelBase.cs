using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MBLite.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _status = "Готово";

    [ObservableProperty]
    private string _title = string.Empty;

    protected virtual void OnInitialized() { }

    protected virtual Task OnInitializedAsync() => Task.CompletedTask;

}
