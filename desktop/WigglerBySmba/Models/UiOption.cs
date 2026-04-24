namespace WigglerBySmba.Models;

public sealed record UiOption<T>(T Value, string Label)
{
    public override string ToString() => Label;
}
