using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using MBLite.Services;
using MBLite.ViewModels;
using MBLite.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MBLite;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;
    private ILogger<App>? _logger;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Создаем и конфигурируем сервисы
        _serviceProvider = ConfigureServices().BuildServiceProvider();

        // Получаем логгер
        _logger = _serviceProvider.GetService<ILogger<App>>();

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();

        SetupApplicationLifetime(mainViewModel);

        // Подписываемся на событие выхода приложения
        if (ApplicationLifetime is IControlledApplicationLifetime controlledLifetime)
        {
            controlledLifetime.Exit += OnApplicationExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Конфигурация сервисов приложения
    /// </summary>
    private static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        // Добавляем логирование
        services.AddLogging(builder =>
        {
            builder.AddDebug(); // Логи в Output window
            builder.AddConsole(); // Логи в консоль
            //builder.AddFile("logs/app.log"); //如果需要文件日志
        });

        services.AddCommonServices();
        // Здесь можно добавить дополнительные сервисы
        // services.AddTransient<IMyService, MyService>();
        return services;
    }

    /// <summary>
    /// Настройка жизненного цикла приложения в зависимости от платформы
    /// </summary>
    private void SetupApplicationLifetime(MainViewModel mainViewModel)
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = CreateMainWindow(mainViewModel);
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                break;

            case ISingleViewApplicationLifetime singleView:
                singleView.MainView = CreateMainView(mainViewModel);
                break;

            case null:
                // Режим дизайнера или другие сценарии
                SetupDesignTimeDataContext(mainViewModel);
                _logger?.LogWarning("ApplicationLifetime is null - running in design mode");
                break;

            default:
                //throw new NotSupportedException(
                //    $"Unsupported application lifetime: {ApplicationLifetime?.GetType().Name}");
                // Для неизвестных типов просто логируем и продолжаем работу
                _logger?.LogWarning("Unsupported application lifetime: {LifetimeType}", ApplicationLifetime.GetType().Name);
                SetupDesignTimeDataContext(mainViewModel);
                break;
        }
    }

    /// <summary>
    /// Настройка контекста данных для режима дизайнера
    /// </summary>
    private void SetupDesignTimeDataContext(MainViewModel mainViewModel)
    {
        // В режиме дизайнера мы не можем создать полноценное окно,
        // но можем установить контекст данных для предпросмотра
        if (Design.IsDesignMode)
        {
            // Устанавливаем DataContext для предпросмотра в дизайнере
            if (Current?.ApplicationLifetime == null)
            {
                // Этот код выполняется только в дизайнере
            }
        }
    }

    /// <summary>
    /// Создание главного окна для десктопных приложений
    /// </summary>
    private static Window CreateMainWindow(MainViewModel mainViewModel) => new MainWindow
    {
        DataContext = mainViewModel
    };

    /// <summary>
    /// Создание главного представления для одностраничных приложений
    /// </summary>
    private static Control CreateMainView(MainViewModel mainViewModel) => new MainView
    {
        DataContext = mainViewModel
    };

    /// <summary>
    /// Обработчик выхода из приложения
    /// </summary>
    private void OnApplicationExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        // Освобождаем ресурсы
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}

//public partial class App : Application
//{
//    public override void Initialize()
//    {
//        AvaloniaXamlLoader.Load(this);
//    }

//    public override void OnFrameworkInitializationCompleted()
//    {
//        // Register all the services needed for the application to run
//        var collection = new ServiceCollection();
//        collection.AddCommonServices();

//        // Creates a ServiceProvider containing services from the provided IServiceCollection
//        var services = collection.BuildServiceProvider();

//        var vm = services.GetRequiredService<MainViewModel>();

//        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
//        {
//            desktop.MainWindow = new MainWindow
//            {
//                //DataContext = new MainViewModel()
//                DataContext = vm
//            };
//        }
//        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
//        {
//            singleViewPlatform.MainView = new MainView
//            {
//                //DataContext = new MainViewModel()
//                DataContext = vm
//            };
//        }

//        base.OnFrameworkInitializationCompleted();
//    }
//}
// Line below is needed to remove Avalonia data validation.
// Without this line you will get duplicate validations from both Avalonia and CT
//BindingPlugins.DataValidators.RemoveAt(0);


