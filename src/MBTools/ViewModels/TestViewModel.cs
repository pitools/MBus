using MBTools.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MBTools.ViewModels;

public class TestViewModel : ViewModelBase
{
    //public string TestGreeting => "TestViewModel";
    private string _testAreaContent = string.Empty;
    public string TestAreaContent
    {
        get => _testAreaContent;
        set => this.RaiseAndSetIfChanged(ref _testAreaContent, value);
    }

    public TestViewModel()
    {
        TestAreaContent = "TestAreaContent";
    }
}