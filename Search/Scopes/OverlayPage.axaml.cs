using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SearchViewClone.ViewModels;

namespace SearchViewClone.Search.Scopes;

// The overlay page pushed on top of a tab page.
public sealed partial class OverlayPage : ContentPage, INavigationAware
{
    private readonly OverlayViewModel _viewModel;

    public OverlayPage()
    {
        _viewModel = null!;
        InitializeComponent();
    }

    public OverlayPage(OverlayViewModel viewModel) : this()
    {
        _viewModel = viewModel;
        DataContext = viewModel;

        AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
        AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Bubble);
        AddHandler(PointerCaptureLostEvent, OnPointerCaptureLost, RoutingStrategies.Bubble);
        InputBox.AddHandler(KeyDownEvent, OnInputBoxKeyDown, RoutingStrategies.Tunnel);
    }

    /// <inheritdoc />
    /// <remarks>
    ///     Runs before the view model's navigation callback pushes the carried-over text into the box; the caret
    ///     move is deferred so it runs once that text is present.
    /// </remarks>
    public Task OnNavigatedToAsync()
    {
        Dispatcher.UIThread.Post(() =>
        {
            InputBox.Focus();
            if (InputBox.Text != null)
            {
                InputBox.CaretIndex = InputBox.Text.Length;
            }
        }, DispatcherPriority.Background);

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
        ContentScrollViewer.Offset = Vector.Zero;
    }

    /// <inheritdoc />
    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        RemoveHandler(PointerPressedEvent, OnPointerPressed);
        RemoveHandler(PointerReleasedEvent, OnPointerReleased);
        RemoveHandler(PointerCaptureLostEvent, OnPointerCaptureLost);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _viewModel.IsMousePressed = true;
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _viewModel.IsMousePressed = false;
    }

    private void OnPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        _viewModel.IsMousePressed = false;
    }

    private void OnInputBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        ContentScrollViewer.Focus();
        if (InputBox.Text != null)
        {
            InputBox.CaretIndex = InputBox.Text.Length;
        }

        e.Handled = true;
    }
}
