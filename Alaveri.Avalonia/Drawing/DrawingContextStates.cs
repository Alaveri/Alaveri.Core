using static Avalonia.Media.DrawingContext;

namespace Alaveri.Avalonia.Drawing;

public class DrawingContextStates : IDisposable
{
    public IList<PushedState> States { get; } = [];

    public void PushState(PushedState state)
    {
        States.Add(state);
    }

    public PushedState? PeekState()
    {
        if (States.Count == 0)
            return null;
        return States.Last();
    }

    public void PopState()
    {
        var state = PopAndPreserve();
        state?.Dispose();
    }

    public PushedState? PopAndPreserve()
    {
        if (States.Count == 0)
            return null;
        var state = States.Last();
        States.Remove(state);
        return state;
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