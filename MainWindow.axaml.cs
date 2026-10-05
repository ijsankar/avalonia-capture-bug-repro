using Avalonia.Controls;
using SearchViewClone.Search;

namespace SearchViewClone;

public partial class MainWindow : Window
{
    public MainWindow() : this(null)
    {
    }

    public MainWindow(RootPage? rootPage)
    {
        InitializeComponent();

        if (rootPage is not null)
        {
            PageHost.Content = rootPage;
        }

        WireEnableFixToggle();
    }

    // Live toggle of Avalonia.Input.Pointer.EnableFix so the fix can be turned on/off without rebuilding.
    // That static field only exists in the source clone, so this compiles only against source; a stock
    // NuGet build disables the checkbox instead.
    private void WireEnableFixToggle()
    {
#if AVALONIA_SOURCE
        EnableFixToggle.IsChecked = Avalonia.Input.Pointer.EnableFix;
        EnableFixToggle.IsCheckedChanged += (_, _) =>
            Avalonia.Input.Pointer.EnableFix = EnableFixToggle.IsChecked == true;
#else
        EnableFixToggle.IsEnabled = false;
        EnableFixToggle.Content = "Pointer.EnableFix (source build only)";
#endif
    }
}
