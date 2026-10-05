namespace SearchViewClone.Search.Scopes;

public sealed partial class PageA : ContentPage, INavigationAware
{
    private readonly NavigationService _navigationService;

    public PageA()
    {
        _navigationService = null!;
        InitializeComponent();
    }

    public PageA(PageAViewModel viewModel, NavigationService navigationService) : this()
    {
        _navigationService = navigationService;
        DataContext = viewModel;
    }

    /// <inheritdoc />
    public Task OnNavigatedToAsync()
    {
        InputBox.Focus();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        InputBox.Focus();
        // Clicking the text box opens the overlay (the top-level view model also opens it on the
        // empty->non-empty text transition — both triggers exist).
        InputBox.AddHandler(PointerPressedEvent, OnInputBoxPointerPressed, RoutingStrategies.Tunnel);
    }

    /// <inheritdoc />
    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        InputBox.RemoveHandler(PointerPressedEvent, OnInputBoxPointerPressed);
    }

    private void OnInputBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _navigationService.NavigateWithHierarchy<OverlayPage>();
    }
}
