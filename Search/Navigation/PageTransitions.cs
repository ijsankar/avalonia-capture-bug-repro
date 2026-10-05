using Avalonia.Animation;
using Avalonia.Animation.Easings;

namespace SearchViewClone.Search.Navigation;

/// <summary>
///     Page transitions for the page hosts of the application.
/// </summary>
public static class PageTransitions
{
    private static readonly TimeSpan Duration = TimeSpan.FromMilliseconds(300);

    /// <summary>
    ///     Slides the incoming page in from the edge the host moves away from.
    /// </summary>
    /// <remarks>
    ///     A <c>CarouselPage</c> derives the direction from the selection change, so a page selected further down
    ///     the list enters from the right and a page selected further up enters from the left.
    /// </remarks>
    public static IPageTransition Slide { get; } = new PageSlide(Duration, PageSlide.SlideAxis.Horizontal)
    {
        SlideInEasing = new SplineEasing(0.16, 1, 0.3),
        SlideOutEasing = new SplineEasing(0.2, 0.85, 0.3)
    };
}
