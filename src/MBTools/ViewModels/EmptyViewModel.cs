using MBTools.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MBTools.ViewModels;

public class EmptyViewModel : ViewModelBase
{
    private string _emptyAreaContent = string.Empty;
    public string EmptyAreaContent
    {
        get => _emptyAreaContent;
        set => this.RaiseAndSetIfChanged(ref _emptyAreaContent, value);
    }

    public EmptyViewModel()
    {
        EmptyAreaContent = "Empty";
    }
}