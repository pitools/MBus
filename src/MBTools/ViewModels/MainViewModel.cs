using MBTools.Data.Repository;
using MBTools.Helpers;
using MBTools.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MBTools.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Properties

    public static string Greeting => "Welcome to Avalonia!";
    public static string TestGreeting => "Test greeting from MainViewModel";
    //public string LiveTime => "Status bar";

    private string _liveTime;

    public string LiveTime
    {
        get => _liveTime;
        set => this.RaiseAndSetIfChanged(ref _liveTime, value);
    }

    #endregion Properties

    #region Collection

    public ObservableCollection<Node> Nodes { get; }

    private ObservableCollection<Node> _selectedNodes = [];

    public ObservableCollection<Node> SelectedNodes
    {
        get => _selectedNodes;
        set => this.RaiseAndSetIfChanged(ref _selectedNodes, value);
    }

    private List<Register> _registers = [];

    public List<Register> Registers
    {
        get => _registers;
        set => this.RaiseAndSetIfChanged(ref _registers, value);
    }

    private ObservableCollection<Person> _people;

    public ObservableCollection<Person> People
    {
        get => _people;
        set => this.RaiseAndSetIfChanged(ref _people, value);
    }

    #endregion Collection

    private string _description = string.Empty;

    public string Description
    {
        get => _description;
        //set => this.RaiseAndSetIfChanged(ref _description, value)
        set
        {
            this.RaiseAndSetIfChanged(ref _description, value);
            LiveTime = value;
            GreetingFromService = value;
        }
    }

    #region ViewLocator

    private readonly ViewModelBase EmptyView = new EmptyViewModel();

    // The default is the first page
    private ViewModelBase _emptyArea;

    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public ViewModelBase EmptyArea
    {
        get { return _emptyArea; }
        private set { this.RaiseAndSetIfChanged(ref _emptyArea, value); }
    }

    private readonly ViewModelBase TestView = new TestViewModel();

    // The default is the first page
    private ViewModelBase _testArea;

    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public ViewModelBase TestArea
    {
        get { return _testArea; }
        private set { this.RaiseAndSetIfChanged(ref _testArea, value); }
    }

    private readonly ViewModelBase InfoView = new InfoViewModel();

    // The default is the first page
    private ViewModelBase _infoArea;

    /// <summary>
    /// Gets the current page. The property is read-only
    /// </summary>
    public ViewModelBase InfoArea
    {
        get { return _infoArea; }
        private set { this.RaiseAndSetIfChanged(ref _infoArea, value); }
    }

    #endregion ViewLocator

    private readonly IUnitOfWork? _unitOfWork;

    public string _greetingFromService = string.Empty;

    public string GreetingFromService
    {
        get => _greetingFromService;
        set => this.RaiseAndSetIfChanged(ref _greetingFromService, value);
    }

    //private IBusinessService _businessService;

    //public IBusinessService BusinessService
    //{
    //    get => _businessService;
    //    set => this.RaiseAndSetIfChanged(ref _businessService, value);
    //}

    public MainViewModel()
    {
        _liveTime = "Default";
        _people = [];
        People = new ObservableCollection<Person>(
            new List<Person>
            {
                new Person("Neil", "Armstrong", false),
                new Person("Buzz", "Lightyear", true),
                new Person("James", "Kirk", true)
            }
        );

        Nodes = new ObservableCollection<Node>
        {
            new Node("Device_1", new ObservableCollection<Node>
            {
                new Node("Params", new ObservableCollection<Node>
                {
                    new Node("Rating current"), new Node("Rating voltage"), new Node("Zebra")
                }),
                new Node("Diagrams", new ObservableCollection<Node>
                {
                    new Node("Functional"), new Node("Decrete input"), new Node("Descrete output")
                }),
            }),
            new Node("Device_2", new ObservableCollection<Node>
            {
                new Node("Params", new ObservableCollection<Node>
                {
                    new Node("Rating current"), new Node("Rating voltage"), new Node("Zebra")
                }),
                new Node("Diagrams", new ObservableCollection<Node>
                {
                    new Node("Functional"), new Node("Decrete input"), new Node("Descrete output")
                }),
            })
        };

        // Set current page to first on start up
        _emptyArea = EmptyView;
        _testArea = TestView;
        _infoArea = InfoView;
    }

    public MainViewModel(IUnitOfWork unitOfWork) : this()
    {
        _unitOfWork = unitOfWork;

        //GreetingFromService = unitOfWork.GetGreeting();
        // seeding
        IRepo<Test> repo = _unitOfWork.GetRepository<Test>(hasCustomRepository: false);
        if (repo.Count() == 0)
        {

            Stopwatch swFull = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                repo.Insert(
                    new Test
                    {
                        Name = "Param" + i.ToString(),
                        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                        DeltaTime = ((double)swFull.ElapsedTicks/10000).ToString(),
                        //Device = _unitOfWork.GetRepository<Device>(hasCustomRepository: false).Insert(new Device { Manufacturer = "Acrel", Model = "ARD3M" })
                    });
                swFull.Restart();
            }
            _unitOfWork.SaveChanges();
        }
        IRepo<Register> repoRegister = _unitOfWork.GetRepository<Register>(hasCustomRepository: false);
        if (repoRegister.Count() == 0)
        {
            for (int i = 0; i < 1; i++)
            {
                repoRegister.Insert(
                    new Register
                    {
                        Address = 0,
                        Description = "Ток фазы L1",
                        Min = 0,
                        Max = 15000,
                        Default = 1000,
                        Unit = "А",
                        Scale = 1.2,
                        Access = "W",
                        Type = RegisterType.WORD,
                        Category = "Monitor"
                    });
                repoRegister.Insert(
                    new Register
                    {
                        Address = 1,
                        Description = "Ток фазы L2",
                        Min = 0,
                        Max = 15000,
                        Default = 1000,
                        Unit = "А",
                        Scale = 1.2,
                        Access = "W",
                        Type = RegisterType.WORD,
                        Category = "Monitor"
                    });
                repoRegister.Insert(
                    new Register
                    {
                        Address = 2,
                        Description = "Ток фазы L3",
                        Min = 0,
                        Max = 15000,
                        Default = 1000,
                        Unit = "А",
                        Scale = 1.2,
                        Access = "W",
                        Type = RegisterType.WORD,
                        Category = "Monitor"
                    });
            }
            _unitOfWork.SaveChanges();
        }
        //Registers = repoRegister.GetAll().ToList();
        var RegisterOne = repoRegister.GetFirstOrDefault(predicate: r => r.Address == 0, disableTracking: false);
        RegisterOne.Min = 555;
        Registers.Add(RegisterOne);
        repoRegister.Update(RegisterOne);

        RegisterOne = repoRegister.GetFirstOrDefault(predicate: r => r.Address == 1, disableTracking: false);
        RegisterOne.Min = 666;
        Registers.Add(RegisterOne);
        repoRegister.Update(RegisterOne);

        RegisterOne = repoRegister.GetFirstOrDefault(predicate: r => r.Address == 2, disableTracking: false);
        RegisterOne.Min = 123;
        Registers.Add(RegisterOne);
        repoRegister.Update(RegisterOne);
        _unitOfWork.SaveChanges();
    }
}

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsFictitious { get; set; }

    public Person(string firstName, string lastName, bool isFictitious)
    {
        FirstName = firstName;
        LastName = lastName;
        IsFictitious = isFictitious;
    }
}