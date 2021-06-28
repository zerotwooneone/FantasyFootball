using System;

public class PlayerHealthChangedArgs
{
    public int? Current { get; }
    public int? MAX { get; }

    private PlayerHealthChangedArgs(int? current,
        int? max)
    {
        Current = current;
        MAX = max;
    }

    public static PlayerHealthChangedArgs Factory(int? current,
        int? max)
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

        return new PlayerHealthChangedArgs(current, max);
    }
}