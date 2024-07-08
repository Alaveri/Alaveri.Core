using Avalonia.Media;

namespace Alaveri.Avalonia.Drawing;

public class DrawingContextStates : IDisposable
{
    public IList<DrawingContext.PushedState> States { get; } = [];

    public void PushState(DrawingContext.PushedState state)
    {
        States.Add(state);
    }

    public void PopState()
    {
        if (States.Count == 0)
            return;
        var state = States.Last();
        States.Remove(state);
        state.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {

            while (States.Count > 0)
                PopState();
            States.Clear();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}