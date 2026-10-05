namespace SearchViewClone;

/// <summary>
///     Defines a contract that notifies a view model of navigation transitions into and out of its view.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    ///     Asynchronously handles the event that is fired after the component is navigated to.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task OnNavigatedToAsync();

    /// <summary>
    ///     Asynchronously handles the event that is fired before the component is navigated from.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task OnNavigatedFromAsync();
}
