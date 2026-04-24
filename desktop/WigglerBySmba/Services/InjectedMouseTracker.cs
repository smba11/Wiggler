using Point = System.Windows.Point;

namespace WigglerBySmba.Services;

internal static class InjectedMouseTracker
{
    private static readonly object Sync = new();
    private static Point _lastPoint;
    private static DateTime _lastUtc = DateTime.MinValue;

    public static void Record(Point point)
    {
        lock (Sync)
        {
            _lastPoint = point;
            _lastUtc = DateTime.UtcNow;
        }
    }

    public static bool Matches(int x, int y)
    {
        lock (Sync)
        {
            if (DateTime.UtcNow - _lastUtc > TimeSpan.FromMilliseconds(120))
            {
                return false;
            }

            return Math.Abs(_lastPoint.X - x) <= 3 && Math.Abs(_lastPoint.Y - y) <= 3;
        }
    }
}
