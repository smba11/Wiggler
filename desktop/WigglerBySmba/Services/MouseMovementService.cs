using System.Diagnostics;
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
    private double _cycleProgress;
    private Vector _centerVelocity;
    private Point _roamingCenter;
    private Point _nextRoamingCenter;
    private Point _randomTarget;
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
        _cycleProgress = 0;

        NativeMethods.GetCursorPos(out var cursor);
        var startPoint = new Point(cursor.X, cursor.Y);
        _roamingCenter = DeriveCenterFromCursor(pattern, startPoint);
        _centerVelocity = CreateVelocity();
        _nextRoamingCenter = CreateNextCenter(_roamingCenter);
        _randomTarget = CreateRandomTarget();
        _isRunning = true;
        _stopwatch.Restart();
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        _stopwatch.Reset();
        _isRunning = false;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (!_isRunning)
        {
            return;
        }

        var dt = Math.Clamp(_stopwatch.Elapsed.TotalSeconds, 0.01, 0.08);
        _stopwatch.Restart();

        if (_pattern == MovementPattern.Random)
        {
            MoveTowardTarget(NextRandomPoint(), dt);
            return;
        }

        _cycleProgress += dt * GetLoopsPerSecond(_pattern);
        while (_cycleProgress >= 1)
        {
            _cycleProgress -= 1;
            _roamingCenter = _nextRoamingCenter;
            _nextRoamingCenter = CreateNextCenter(_roamingCenter);
        }

        MoveCursor(PointAlongPattern(_pattern, _cycleProgress));
    }

    private Point CreateNextCenter(Point currentCenter)
    {
        var bounds = GetVirtualBounds();
        var margin = _size + 80;
        var left = bounds.Left + margin;
        var top = bounds.Top + margin;
        var right = bounds.Right - margin;
        var bottom = bounds.Bottom - margin;
        var travel = 7 + (_speed * 4);

        var nextCenter = new Point(
            currentCenter.X + (_centerVelocity.X * travel),
            currentCenter.Y + (_centerVelocity.Y * travel));

        if (nextCenter.X < left || nextCenter.X > right)
        {
            _centerVelocity = new Vector(-_centerVelocity.X, _centerVelocity.Y);
            nextCenter.X = Math.Clamp(nextCenter.X, left, right);
        }

        if (nextCenter.Y < top || nextCenter.Y > bottom)
        {
            _centerVelocity = new Vector(_centerVelocity.X, -_centerVelocity.Y);
            nextCenter.Y = Math.Clamp(nextCenter.Y, top, bottom);
        }

        return nextCenter;
    }

    private Point PointAlongPattern(MovementPattern pattern, double progress)
    {
        var amplitude = _size;
        var center = GetDisplayCenter(progress);
        return pattern switch
        {
            MovementPattern.Circle => ClampToScreen(new Point(
                center.X + (Math.Cos(progress * Math.PI * 2) * amplitude),
                center.Y + (Math.Sin(progress * Math.PI * 2) * amplitude))),
            MovementPattern.Square => ClampToScreen(TracePolygon(progress, new[]
            {
                new Point(-amplitude, -amplitude),
                new Point(amplitude, -amplitude),
                new Point(amplitude, amplitude),
                new Point(-amplitude, amplitude)
            }, center)),
            MovementPattern.Triangle => ClampToScreen(TracePolygon(progress, new[]
            {
                new Point(0, -amplitude),
                new Point(amplitude, amplitude * 0.76),
                new Point(-amplitude, amplitude * 0.76)
            }, center)),
            MovementPattern.Figure8 => ClampToScreen(new Point(
                center.X + (Math.Sin(progress * Math.PI * 2) * amplitude),
                center.Y + (Math.Sin(progress * Math.PI * 4) * amplitude * 0.58))),
            MovementPattern.Parallelogram => ClampToScreen(TracePolygon(progress, new[]
            {
                new Point(-amplitude * 0.72, -amplitude),
                new Point(amplitude * 0.92, -amplitude),
                new Point(amplitude * 0.72, amplitude),
                new Point(-amplitude * 0.92, amplitude)
            }, center)),
            _ => ClampToScreen(center)
        };
    }

    private Point TracePolygon(double progress, Point[] points, Point center)
    {
        var loops = points.Length;
        var normalized = (((progress % 1) + 1) % 1) * loops;
        var index = (int)Math.Floor(normalized);
        var nextIndex = (index + 1) % loops;
        var edgeProgress = normalized - index;

        var start = points[index];
        var end = points[nextIndex];

        return new Point(
            center.X + Lerp(start.X, end.X, edgeProgress),
            center.Y + Lerp(start.Y, end.Y, edgeProgress));
    }

    private Point NextRandomPoint()
    {
        var cursor = GetCursorPoint();
        var toTarget = _randomTarget - cursor;

        if (toTarget.Length < Math.Max(18, _size * 0.18) || IsNearScreenEdge(cursor, 42))
        {
            _randomTarget = CreateRandomTarget();
        }

        return _randomTarget;
    }

    private void MoveTowardTarget(Point target, double dt)
    {
        var cursor = GetCursorPoint();
        var delta = target - cursor;
        var distance = delta.Length;

        if (distance < 0.6)
        {
            return;
        }

        delta.Normalize();
        var maxPixelsThisFrame = Math.Max(2.4, (110 + (_speed * 155)) * dt);
        var applied = delta * Math.Min(distance, maxPixelsThisFrame);
        var nextPoint = ClampToScreen(cursor + applied);
        MoveCursor(nextPoint);
    }

    private Point CreateRandomTarget()
    {
        var bounds = GetVirtualBounds();
        var margin = Math.Max(48, _size * 0.6);
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
        return new Vector(Math.Cos(angle), Math.Sin(angle));
    }

    private Point DeriveCenterFromCursor(MovementPattern pattern, Point cursor)
    {
        var offset = pattern switch
        {
            MovementPattern.Circle => new Vector(_size, 0),
            MovementPattern.Square => new Vector(-_size, -_size),
            MovementPattern.Triangle => new Vector(0, -_size),
            MovementPattern.Figure8 => new Vector(0, 0),
            MovementPattern.Parallelogram => new Vector(-_size * 0.72, -_size),
            _ => new Vector(0, 0)
        };

        return ClampCenter(cursor - offset);
    }

    private double GetLoopsPerSecond(MovementPattern pattern)
    {
        var baseLoops = pattern switch
        {
            MovementPattern.Circle => 0.18,
            MovementPattern.Square => 0.16,
            MovementPattern.Triangle => 0.17,
            MovementPattern.Figure8 => 0.14,
            MovementPattern.Parallelogram => 0.15,
            _ => 0.16
        };

        return baseLoops * Math.Max(0.75, _speed * 0.95);
    }

    private Point ClampCenter(Point point)
    {
        var bounds = GetVirtualBounds();
        var margin = _size + 80;
        return new Point(
            Math.Clamp(point.X, bounds.Left + margin, bounds.Right - margin),
            Math.Clamp(point.Y, bounds.Top + margin, bounds.Bottom - margin));
    }

    private static double Lerp(double start, double end, double progress) =>
        start + ((end - start) * progress);

    private Point GetDisplayCenter(double progress)
    {
        const double transitionStart = 0.9;
        if (progress <= transitionStart)
        {
            return _roamingCenter;
        }

        var blend = (progress - transitionStart) / (1 - transitionStart);
        var easedBlend = blend * blend * (3 - (2 * blend));
        return new Point(
            Lerp(_roamingCenter.X, _nextRoamingCenter.X, easedBlend),
            Lerp(_roamingCenter.Y, _nextRoamingCenter.Y, easedBlend));
    }

    private static Point GetCursorPoint()
    {
        NativeMethods.GetCursorPos(out var cursor);
        return new Point(cursor.X, cursor.Y);
    }

    private static void MoveCursor(Point target)
    {
        InjectedMouseTracker.Record(target);
        NativeMethods.SetCursorPos((int)Math.Round(target.X), (int)Math.Round(target.Y));
    }

    public void Dispose()
    {
        Stop();
        _timer.Tick -= OnTick;
    }
}
