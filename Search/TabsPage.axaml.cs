using Avalonia;
using Avalonia.Controls;
using Avalonia.Reactive;
using SearchViewClone.Search.Scopes;
using SearchViewClone.ViewModels;

namespace SearchViewClone.Search;

// Hosts the tab pages as siblings, selected by SelectingMultiPage.SelectedIndex. Switching one for another
// is a selection, not a stack navigation. Selecting a tab while the overlay is up drops the overlay.
public sealed partial class TabsPage : CarouselPage
{
    public TabsPage()
    {
        InitializeComponent();
    }

    public TabsPage(MainViewModel viewModel, PageA pageA, PageB pageB) : this()
    {
        DataContext = viewModel;
        Pages = [pageA, pageB];

        SelectionChanged += OnSelectionChanged;
    }

    private void OnSelectionChanged(object? sender, PageSelectionChangedEventArgs args)
    {
        if (Navigation is NavigationPage host)
        {
            DropPagesAboveTabs(host);
        }
    }

    private async void DropPagesAboveTabs(NavigationPage host)
    {
        try
        {
            while (host.IsNavigating)
            {
                await WaitForIdleAsync(host);
            }

            if (host.CanGoBack && host.NavigationStack.Contains(this))
            {
                await host.PopToPageAsync(this, null);
            }
        }
        catch
        {
            //ignored
        }
    }

    private static async Task WaitForIdleAsync(NavigationPage host)
    {
        var idle = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        using var subscription = host.GetObservable(NavigationPage.IsNavigatingProperty)
            .Subscribe(new AnonymousObserver<bool>(isNavigating =>
            {
                if (!isNavigating)
                {
                    idle.TrySetResult();
                }
            }));

        await idle.Task;
    }
}
