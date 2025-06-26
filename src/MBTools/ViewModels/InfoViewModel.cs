using MBTools.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MBTools.ViewModels;

public class InfoViewModel : ViewModelBase
{
    //public string TestGreeting => "TestViewModel";
    private string _infoAreaContent = string.Empty;
    public string InfoAreaContent
    {
        get => _infoAreaContent;
        set => this.RaiseAndSetIfChanged(ref _infoAreaContent, value);
    }

    public InfoViewModel()
    {
        InfoAreaContent = "InfoAreaContent";
    }
}