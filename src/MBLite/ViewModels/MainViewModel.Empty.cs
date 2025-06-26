using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MBLite.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private string _emptyMessage = string.Empty;
    public string EmptyMessage
    {
        get => _emptyMessage;
        set => SetProperty(ref _emptyMessage, value);
    }
}