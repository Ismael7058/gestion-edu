using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Educacion.Desktop.ViewModels;
using Educacion.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using Educacion.Desktop.Data;
using Educacion.Desktop.Services;

namespace Educacion.Desktop;

public partial class App : Application
{
    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance for the application.
    /// </summary>
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Configure services
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            // Get the MainWindowViewModel from the DI container
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private void ConfigureServices(IServiceCollection services)
    {
        // DbContext
        services.AddDbContext<EducacionDbContext>();

        // Services
        services.AddSingleton<ImageService>();
        services.AddTransient(typeof(IDataService<>), typeof(DataService<>));

        // ViewModels
        services.AddSingleton<ViewModels.Student.StudentListViewModel>();
        services.AddSingleton<ViewModels.Teacher.TeacherListViewModel>();
        services.AddSingleton<ViewModels.Subject.SubjectListViewModel>();
        services.AddSingleton<ViewModels.Enrollment.EnrollmentListViewModel>();
        services.AddSingleton<DashboardViewModel>();
        
        services.AddSingleton<MainWindowViewModel>();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}