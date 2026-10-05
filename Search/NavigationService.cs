using Microsoft.Extensions.DependencyInjection;

namespace SearchViewClone.Search;

// Drives the NavigationPage: sets the root page and pushes/pops an overlay on top of it.
public sealed class NavigationService(IServiceProvider services)
{
    private NavigationPage? _navigationPage;

    public void SetNavigationControl(NavigationPage navigationPage) => _navigationPage = navigationPage;

    // Replaces the stack with a freshly resolved page as its root.
    public void Navigate<TPage>() where TPage : Page
    {
        var page = services.GetRequiredService<TPage>();
        _navigationPage?.ReplaceAsync(page);
    }

    // Pushes a freshly resolved page onto the stack with NO transition (instant). The instant push is what
    // triggers the bug: the outgoing page detaches in the same beat as the capture-lost notification.
    public void NavigateWithHierarchy<TPage>() where TPage : Page
    {
        var page = services.GetRequiredService<TPage>();
        _navigationPage?.PushAsync(page, null);
    }

    // Pops the top page off the stack when there is one to pop.
    public void GoBack()
    {
        if (_navigationPage is { CanGoBack: true })
        {
            _navigationPage.PopAsync();
        }
    }
}
