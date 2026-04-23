using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using WigglerBySmba.Models;
using Point = System.Windows.Point;
using Rect = System.Windows.Rect;
using Vector = System.Windows.Vector;

namespace WigglerBySmba.Services;

public sealed class MouseMovementService : IDisposable
{
    private readonly System.Windows.Threading.DispatcherTimer _timer;
    private readonly Stopwatch _stopwatch = new();
    private readonly Random _random = new();

    private MovementPattern _pattern;
    private double _speed;
    private double _size;
    private double _phase;
    private Vector _centerVelocity;
    private Point _roamingCenter;
    private Point _lastTarget;
    private Point _randomTarget;
    private bool _hasLastTarget;
    private bool _isRunning;

    public MouseMovementService()
    {
        _timer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        _timer.Tick += OnTick;
    }

    public bool IsRunning => _isRunning;

    public void Start(MovementPattern pattern, double speed, double size)
    {
        _pattern = pattern;
        _speed = speed;
        _size = size;
        _phase = 0;
        _hasLastTarget = false;

        NativeMethods.GetCursorPos(out var cursor);
        _roamingCenter = new Point(cursor.X, cursor.Y);
        _lastTarget = _roamingCenter;
        _centerVelocity = CreateVelocity();
        _randomTarget = ClampToScreen(_roamingCenter);
        _isRunning = true;
        _stopwatch.Restart();
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        _stopwatch.Reset();
        _isRunning = false;
        _hasLastTarget = false;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (!_isRunning)
        {
            return;
        }

        var dt = Math.Max(0.01, _stopwatch.Elapsed.TotalSeconds);
        _stopwatch.Restart();

        UpdateRoamingCenter(dt);
        _phase += dt * (0.72 + (_speed * 0.9));

        var currentTarget = _pattern == MovementPattern.Random
            ? NextRandomPoint()
            : OffsetFromPattern(_pattern, _phase);

        if (!_hasLastTarget)
        {
            _lastTarget = currentTarget;
            _hasLastTarget = true;
            return;
        }

        var desiredDelta = currentTarget - _lastTarget;
        _lastTarget = currentTarget;

        GlideDelta(desiredDelta, dt);
    }

    private void UpdateRoamingCenter(double dt)
    {
        var bounds = GetVirtualBounds();
        var margin = _size + 42;
        var left = bounds.Left + margin;
        var top = bounds.Top + margin;
        var right = bounds.Right - margin;
        var bottom = bounds.Bottom - margin;

        _roamingCenter = new Point(
            _roamingCenter.X + (_centerVelocity.X * dt),
            _roamingCenter.Y + (_centerVelocity.Y * dt));

        if (_roamingCenter.X < left || _roamingCenter.X > right)
        {
            _centerVelocity = new Vector(-_centerVelocity.X, _centerVelocity.Y);
            _roamingCenter.X = Math.Clamp(_roamingCenter.X, left, right);
        }

        if (_roamingCenter.Y < top || _roamingCenter.Y > bottom)
        {
            _centerVelocity = new Vector(_centerVelocity.X, -_centerVelocity.Y);
            _roamingCenter.Y = Math.Clamp(_roamingCenter.Y, top, bottom);
        }
    }

    private Point OffsetFromPattern(MovementPattern pattern, double phase)
    {
        var amplitude = _size;
        return pattern switch
        {
            MovementPattern.Circle => ClampToScreen(new Point(
                _roamingCenter.X + (Math.Cos(phase) * amplitude),
                _roamingCenter.Y + (Math.Sin(phase) * amplitude))),
            MovementPattern.Square => ClampToScreen(TracePolygon(phase, new[]
            {
                new Point(-amplitude, -amplitude),
                new Point(amplitude, -amplitude),
                new Point(amplitude, amplitude),
                new Point(-amplitude, amplitude)
            })),
            MovementPattern.Triangle => ClampToScreen(TracePolygon(phase, new[]
            {
                new Point(0, -amplitude),
                new Point(amplitude, amplitude * 0.75),
                new Point(-amplitude, amplitude * 0.75)
            })),
            MovementPattern.Figure8 => ClampToScreen(new Point(
                _roamingCenter.X + (Math.Sin(phase) * amplitude),
                _roamingCenter.Y + (Math.Sin(phase * 2) * amplitude * 0.54))),
            MovementPattern.Parallelogram => ClampToScreen(TracePolygon(phase, new[]
            {
                new Point(-amplitude * 0.72, -amplitude),
                new Point(amplitude * 0.92, -amplitude),
                new Point(amplitude * 0.72, amplitude),
                new Point(-amplitude * 0.92, amplitude)
            })),
            _ => ClampToScreen(_roamingCenter)
        };
    }

    private Point TracePolygon(double phase, Point[] points)
    {
        var loops = points.Length;
        var normalized = ((phase * 0.38) % loops + loops) % loops;
        var index = (int)Math.Floor(normalized);
        var nextIndex = (index + 1) % loops;
        var progress = normalized - index;

        var start = points[index];
        var end = points[nextIndex];

        return new Point(
            _roamingCenter.X + Lerp(start.X, end.X, progress),
            _roamingCenter.Y + Lerp(start.Y, end.Y, progress));
    }

    private Point NextRandomPoint()
    {
        var cursor = GetCursorPoint();
        var toTarget = _randomTarget - cursor;

        if (toTarget.Length < 14 || IsNearScreenEdge(cursor, 30))
        {
            _randomTarget = CreateRandomTarget();
        }

        return _randomTarget;
    }

    private void GlideDelta(Vector desiredDelta, double dt)
    {
        if (desiredDelta.Length < 0.01)
        {
            return;
        }

        var cursor = GetCursorPoint();
        var idealNext = ClampToScreen(cursor + desiredDelta);
        var actualDelta = idealNext - cursor;

        if (actualDelta.Length < 0.2)
        {
            return;
        }

        var maxPixelsThisFrame = Math.Max(1.25, (6 + (_speed * 8.5)) * dt * 6);
        var distance = actualDelta.Length;
        var direction = actualDelta;
        direction.Normalize();

        var applied = direction * Math.Min(distance, maxPixelsThisFrame);
        var stepX = (int)Math.Round(applied.X);
        var stepY = (int)Math.Round(applied.Y);

        if (stepX == 0 && Math.Abs(applied.X) > 0.2)
        {
            stepX = applied.X > 0 ? 1 : -1;
        }

        if (stepY == 0 && Math.Abs(applied.Y) > 0.2)
        {
            stepY = applied.Y > 0 ? 1 : -1;
        }

        MoveCursorRelative(stepX, stepY);
    }

    private Point CreateRandomTarget()
    {
        var bounds = GetVirtualBounds();
        var margin = Math.Max(40, _size * 0.55);
        return new Point(
            bounds.Left + margin + (_random.NextDouble() * Math.Max(1, bounds.Width - (margin * 2))),
            bounds.Top + margin + (_random.NextDouble() * Math.Max(1, bounds.Height - (margin * 2))));
    }

    private bool IsNearScreenEdge(Point point, double threshold)
    {
        var bounds = GetVirtualBounds();
        return point.X <= bounds.Left + threshold
            || point.X >= bounds.Right - threshold
            || point.Y <= bounds.Top + threshold
            || point.Y >= bounds.Bottom - threshold;
    }

    private static Rect GetVirtualBounds() =>
        new(
            SystemParameters.VirtualScreenLeft,
            SystemParameters.VirtualScreenTop,
            SystemParameters.VirtualScreenWidth,
            SystemParameters.VirtualScreenHeight);

    private Point ClampToScreen(Point point)
    {
        var bounds = GetVirtualBounds();
        return new Point(
            Math.Clamp(point.X, bounds.Left + 2, bounds.Right - 2),
            Math.Clamp(point.Y, bounds.Top + 2, bounds.Bottom - 2));
    }

    private Vector CreateVelocity()
    {
        var angle = _random.NextDouble() * Math.PI * 2;
        var magnitude = 12 + (_speed * 12);
        return new Vector(Math.Cos(angle) * magnitude, Math.Sin(angle) * magnitude);
    }

    private static double Lerp(double start, double end, double progress) =>
        start + ((end - start) * progress);

    private static Point GetCursorPoint()
    {
        NativeMethods.GetCursorPos(out var cursor);
        return new Point(cursor.X, cursor.Y);
    }

    private static void MoveCursorRelative(int dx, int dy)
    {
        if (dx == 0 && dy == 0)
        {
            return;
        }

        var input = new NativeMethods.Input
        {
            Type = NativeMethods.InputMouse,
            Data = new NativeMethods.InputUnion
            {
                Mi = new NativeMethods.MouseInput
                {
                    Dx = dx,
                    Dy = dy,
                    DwFlags = NativeMethods.MouseeventfMove,
                    DwExtraInfo = NativeMethods.WigglerExtraInfo
                }
            }
        };

        NativeMethods.SendInput(1, [input], Marshal.SizeOf<NativeMethods.Input>());
    }

    public void Dispose()
    {
        Stop();
        _timer.Tick -= OnTick;
    }
}
