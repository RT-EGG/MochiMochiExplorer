using System.Windows;

namespace Utility.Wpf
{
    public static class FrameworkElementGeometory
    {
        public static Rect GetRectangleOnScreen(this FrameworkElement inElement)
            => new Rect(
                inElement.PointToScreen(new Point(0.0, 0.0)),
                new Size(inElement.ActualWidth, inElement.ActualHeight)
            );
    }
}
