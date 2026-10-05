namespace SearchViewClone.ViewModels;

// A tab view model. Its text box reflects and drives the shared text on the top-level view model.
public sealed class PageBViewModel(MainViewModel mainViewModel)
    : ObservableBase, INavigationAware
{
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
