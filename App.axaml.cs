using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace SearchViewClone;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var provider = BuildServiceProvider();
            var rootPage = provider.GetRequiredService<RootPage>();
            desktop.MainWindow = new MainWindow(rootPage);
        }

        base.OnFrameworkInitializationCompleted();
    }

    // The navigation service, the shared top-level view model, the tab/overlay view models, and every page by
    // concrete type so the navigation service can resolve them and TabsPage's constructor can take the two
    // tab pages.
    private static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<NavigationService>();
        services.AddSingleton<MainViewModel>();

        services.AddTransient<PageAViewModel>();
        services.AddTransient<PageBViewModel>();
        services.AddTransient<OverlayViewModel>();

        services.AddTransient<PageA>();
        services.AddTransient<PageB>();
        services.AddTransient<OverlayPage>();
        services.AddTransient<TabsPage>();
        services.AddTransient<RootPage>();

        return services.BuildServiceProvider();
    }
}
