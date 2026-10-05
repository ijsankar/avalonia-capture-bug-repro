namespace SearchViewClone.Search;

// Hosts the active tab page and forwards INavigationAware notifications to it.
public sealed partial class RootPage : ContentPage
{
    private Page? _activePage;

    public RootPage()
    {
        InitializeComponent();
    }

    public RootPage(
        MainViewModel viewModel,
        NavigationService navigationService) : this()
    {
        DataContext = viewModel;
        navigationService.SetNavigationControl(NavigationPresenter);

        NavigationPresenter.PropertyChanged += OnNavigationPresenterPropertyChanged;
        AddHandler(SelectingMultiPage.SelectionChangedEvent, OnTabSelectionChanged);

        viewModel.InitializeAsync();
        navigationService.Navigate<TabsPage>();
    }

    // CurrentPage is the single signal the host updates for every navigation kind: push, pop and replace.
    private void OnNavigationPresenterPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Property != CurrentPageProperty)
        {
            return;
        }

        SetActivePage(args.GetNewValue<Page?>());
    }

    private void OnTabSelectionChanged(object? sender, PageSelectionChangedEventArgs args)
    {
        SetActivePage(args.CurrentPage);
    }

    private void SetActivePage(Page? page)
    {
        var activePage = page is SelectingMultiPage host ? host.SelectedPage : page;
        if (ReferenceEquals(activePage, _activePage))
        {
            return;
        }

        NotifyNavigatedFrom(_activePage);
        _activePage = activePage;

        // A sibling host puts the selected page into the tree in the layout pass that follows the selection.
        // A page outside the tree cannot take the keyboard focus it asks for on arrival, so we notify it once
        // the layout that realizes it has run.
        Dispatcher.UIThread.Post(() =>
        {
            if (ReferenceEquals(activePage, _activePage))
            {
                NotifyNavigatedTo(activePage);
            }
        }, DispatcherPriority.Loaded);
    }

    private static void NotifyNavigatedTo(Page? page)
    {
        if (page is INavigationAware content)
        {
            content.OnNavigatedToAsync();
        }

        if (page is { DataContext: INavigationAware contentViewModel })
        {
            contentViewModel.OnNavigatedToAsync();
        }
    }

    private static void NotifyNavigatedFrom(Page? page)
    {
        if (page is INavigationAware source)
        {
            source.OnNavigatedFromAsync();
        }

        if (page is { DataContext: INavigationAware sourceViewModel })
        {
            sourceViewModel.OnNavigatedFromAsync();
        }
    }
}
