using System;

public class PlayerHealthChangedArgs
{
    public int? Delta { get; }
    public int? Current { get; }
    public int? MAX { get; }

    private PlayerHealthChangedArgs(int? delta,
        int? current,
        int? max)
    {
        Delta = delta;
        Current = current;
        MAX = max;
    }

    public static PlayerHealthChangedArgs Factory(int? delta = null,
        int? current = null,
        int? max = null)
    {
        const int MinCurrent = 0;
        const int MaxCurrent = 400;
        if (current.HasValue &&
            (current < MinCurrent ||
             current > MaxCurrent))
        {
            throw new ArgumentException($"current must be between {MinCurrent} and {MaxCurrent} but got {current}",
                nameof(current));
        }

        if (max < MinCurrent)
        {
            throw new ArgumentException($"max cannot be below {MinCurrent}");
        }

        return new PlayerHealthChangedArgs(delta, current, max);
    }
}