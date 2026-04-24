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
        _phase = 0;

        NativeMethods.GetCursorPos(out var cursor);
        _roamingCenter = new Point(cursor.X, cursor.Y);
        _centerVelocity = CreateVelocity();
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

        UpdateRoamingCenter(dt);
        _phase += dt * Math.Max(0.9, _speed * 1.8);

        var target = _pattern == MovementPattern.Random
            ? NextRandomPoint()
            : PointAlongPattern(_pattern, _phase);

        MoveTowardTarget(target, dt);
    }

    private void UpdateRoamingCenter(double dt)
    {
        var bounds = GetVirtualBounds();
        var margin = _size + 80;
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

    private Point PointAlongPattern(MovementPattern pattern, double phase)
    {
        var amplitude = _size;
        return pattern switch
        {
            MovementPattern.Circle => ClampToScreen(new Point(
                _roamingCenter.X + (Math.Cos(phase) * amplitude),
                _roamingCenter.Y + (Math.Sin(phase) * amplitude))),
            MovementPattern.Square => ClampToScreen(TracePolygon(phase * 0.38, new[]
            {
                new Point(-amplitude, -amplitude),
                new Point(amplitude, -amplitude),
                new Point(amplitude, amplitude),
                new Point(-amplitude, amplitude)
            })),
            MovementPattern.Triangle => ClampToScreen(TracePolygon(phase * 0.44, new[]
            {
                new Point(0, -amplitude),
                new Point(amplitude, amplitude * 0.76),
                new Point(-amplitude, amplitude * 0.76)
            })),
            MovementPattern.Figure8 => ClampToScreen(new Point(
                _roamingCenter.X + (Math.Sin(phase) * amplitude),
                _roamingCenter.Y + (Math.Sin(phase * 2) * amplitude * 0.58))),
            MovementPattern.Parallelogram => ClampToScreen(TracePolygon(phase * 0.36, new[]
            {
                new Point(-amplitude * 0.72, -amplitude),
                new Point(amplitude * 0.92, -amplitude),
                new Point(amplitude * 0.72, amplitude),
                new Point(-amplitude * 0.92, amplitude)
            })),
            _ => ClampToScreen(_roamingCenter)
        };
    }

    private Point TracePolygon(double progress, Point[] points)
    {
        var loops = points.Length;
        var normalized = ((progress % loops) + loops) % loops;
        var index = (int)Math.Floor(normalized);
        var nextIndex = (index + 1) % loops;
        var edgeProgress = normalized - index;

        var start = points[index];
        var end = points[nextIndex];

        return new Point(
            _roamingCenter.X + Lerp(start.X, end.X, edgeProgress),
            _roamingCenter.Y + Lerp(start.Y, end.Y, edgeProgress));
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
        MoveCursorAbsolute(nextPoint);
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
        var magnitude = 34 + (_speed * 22);
        return new Vector(Math.Cos(angle) * magnitude, Math.Sin(angle) * magnitude);
    }

    private static double Lerp(double start, double end, double progress) =>
        start + ((end - start) * progress);

    private static Point GetCursorPoint()
    {
        NativeMethods.GetCursorPos(out var cursor);
        return new Point(cursor.X, cursor.Y);
    }

    private static void MoveCursorAbsolute(Point target)
    {
        var bounds = GetVirtualBounds();
        var normalizedX = NormalizeAbsoluteCoordinate(target.X, bounds.Left, bounds.Width);
        var normalizedY = NormalizeAbsoluteCoordinate(target.Y, bounds.Top, bounds.Height);

        var input = new NativeMethods.Input
        {
            Type = NativeMethods.InputMouse,
            Data = new NativeMethods.InputUnion
            {
                Mi = new NativeMethods.MouseInput
                {
                    Dx = normalizedX,
                    Dy = normalizedY,
                    DwFlags = NativeMethods.MouseeventfMove | NativeMethods.MouseeventfAbsolute | NativeMethods.MouseeventfVirtualdesk,
                    DwExtraInfo = NativeMethods.WigglerExtraInfo
                }
            }
        };

        NativeMethods.SendInput(1, [input], Marshal.SizeOf<NativeMethods.Input>());
    }

    private static int NormalizeAbsoluteCoordinate(double value, double min, double length)
    {
        var relative = length <= 1 ? 0 : (value - min) / (length - 1);
        return (int)Math.Round(Math.Clamp(relative, 0, 1) * 65535.0);
    }

    public void Dispose()
    {
        Stop();
        _timer.Tick -= OnTick;
    }
}
