using System.Drawing;

namespace Alaveri.Configuration;

public interface IStoredWindowState<TWindow> where TWindow : class
{
    PointF Position { get; set; }
    PointF RestoredPosition { get; set; }
    SizeF RestoredSize { get; set; }
    SizeF Size { get; set; }
    GenericWindowState State { get; set; }

    void RestoreWindowState(TWindow window);
    void StoreWindowState(TWindow window);
}