using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using MBLite.Services;
using MBLite.ViewModels;
using MBLite.ViewModels.Connection;
using MBLite.Views;
using Microsoft.Extensions.Configuration;
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

        //base.OnFrameworkInitializationCompleted();

        try
        {
            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            _logger?.LogError(ex, "Framework initialization failed");
            throw;
        }
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
            builder.SetMinimumLevel(LogLevel.Debug);
            //builder.AddFile("logs/app.log"); //如果需要文件日志
        });

        // 2. Сервисы приложения (из AddCommonServices)
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IFileService, FileService>();

        // 3. Modbus сервисы
        //services.AddSingleton<IModbusService, ModbusService>();
        //services.AddSingleton<IMessageService, MessageService>();

        // 4. ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<ConnectionViewModel>();

        // 5. Дополнительные сервисы можно добавить здесь
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