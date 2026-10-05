namespace SearchViewClone.ViewModels;

// The overlay view model. Carries only the members the OverlayPage code-behind touches (IsMousePressed,
// Text). Text is shared with the top-level view model so it survives the push.
public sealed class OverlayViewModel(MainViewModel mainViewModel)
    : ObservableBase, INavigationAware
{
    private bool _isMousePressed;

    public bool IsMousePressed
    {
        get => _isMousePressed;
        set => Set(ref _isMousePressed, value);
    }

    public string Text
    {
        get => mainViewModel.Text;
        set
        {
            if (mainViewModel.Text != value)
            {
                mainViewModel.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public Task OnNavigatedToAsync()
    {
        OnPropertyChanged(nameof(Text));
        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync() => Task.CompletedTask;
}
