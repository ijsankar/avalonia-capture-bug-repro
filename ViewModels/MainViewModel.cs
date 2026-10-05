using SearchViewClone.Search;
using SearchViewClone.Search.Scopes;

namespace SearchViewClone.ViewModels;

// The top-level view model. It owns the two pieces of state the bug needs:
//   * SelectedTab - the selected tab index (0 or 1), bound two-way to both the tab ListBox and the
//                   CarouselPage that hosts the tab pages.
//   * Text        - shared text; the tab pages read/write it through this instance.
// Switching tabs clears the text. The text drives the overlay: empty -> non-empty pushes it,
// non-empty -> empty pops it back.
public sealed class MainViewModel(NavigationService navigationService) : ObservableBase
{
    private int _selectedTab;
    private string _text = string.Empty;

    public int SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (Set(ref _selectedTab, value))
            {
                // Switching tabs clears the shared text, so a tab's box never carries text over from the other.
                Text = string.Empty;
            }
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            var oldValue = _text;
            if (Set(ref _text, value))
            {
                if (oldValue.Length == 0 && value.Length > 0)
                {
                    navigationService.NavigateWithHierarchy<OverlayPage>();
                }
                else if (oldValue.Length > 0 && value.Length == 0)
                {
                    navigationService.GoBack();
                }
            }
        }
    }

    public Task InitializeAsync()
    {
        // A tab is selected on initialization so a tab page is shown.
        SelectedTab = 0;
        return Task.CompletedTask;
    }
}
