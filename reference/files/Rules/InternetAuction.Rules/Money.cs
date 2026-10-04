namespace InternetAuction.Rules;

public static class Money
{
    /// <summary>The largest amount a bid may have.</summary>
    public const decimal MaxBid = 1_000_000_000m;

    /// <summary>True for a positive amount with at most two decimal places (cents) that does not exceed <see cref="MaxBid"/>.</summary>
    /// <param name="amount">The amount.</param>
    /// <returns>Whether it is a valid money amount for a bid.</returns>
    public static bool IsValidBidAmount(decimal amount) =>
        amount > 0 && amount <= MaxBid && decimal.Round(amount, 2) == amount;

    /// <summary>
    /// The smallest allowed increase over the current price: 1 % of it, but never less than 1.00 and always rounded UP to whole cents.
    /// Examples: 50.00 → 1.00; 100.00 → 1.00; 250.00 → 2.50; 333.33 → 3.34.
    /// </summary>
    /// <param name="current">The current highest price (not negative).</param>
    /// <returns>The minimum step.</returns>
    public static decimal MinimumStep(decimal current)
    {
        var onePercent = Math.Ceiling(current * 0.01m * 100m) / 100m;
        return Math.Max(1.00m, onePercent);
    }
}
