namespace InternetAuction.Rules;

public static class BiddingRules
{
    /// <summary>A bid placed less than this before the end moves the end to "bid time + this" (anti-sniping).</summary>
    public static readonly TimeSpan AntiSnipingWindow = TimeSpan.FromMinutes(2);

    /// <summary>The highest amount so far, or null when nobody has bid.</summary>
    /// <param name="auction">The auction.</param>
    /// <returns>The current price or null.</returns>
    public static decimal? CurrentPrice(AuctionSnapshot auction)
    {
        throw new NotImplementedException("TODO");
    }

    /// <summary>
    /// The smallest amount that would be accepted now: the starting price while there are no bids,
    /// otherwise the current price + <see cref="Money.MinimumStep"/>.
    /// </summary>
    /// <param name="auction">The auction.</param>
    /// <returns>The minimum accepted bid.</returns>
    public static decimal MinimumNextBid(AuctionSnapshot auction)
    {
        throw new NotImplementedException("TODO");
    }

    /// <summary>
    /// Decides whether <paramref name="bid"/> may be placed. Checks, in this order (the first failure is the reason):
    /// 1. amount valid (<see cref="Money.IsValidBidAmount"/>) → InvalidAmount;
    /// 2. auction cancelled → AuctionCancelled; 3. before Start → AuctionNotStarted; 4. at or after End → AuctionClosed;
    /// 5. the bidder is the lot owner → OwnerCannotBid;
    /// 6. the bidder already holds the highest bid → AlreadyHighestBidder;
    /// 7. amount below <see cref="MinimumNextBid"/> → BelowMinimum (the decision carries the minimum).
    /// For an accepted bid NewEnd = End, or now + <see cref="AntiSnipingWindow"/> when End − now is shorter than the window (never earlier than End).
    /// The time used is <paramref name="now"/>, not bid.PlacedAt.
    /// </summary>
    /// <param name="auction">The auction as it is now.</param>
    /// <param name="bidderId">Who wants to bid.</param>
    /// <param name="amount">How much.</param>
    /// <param name="now">Current UTC time.</param>
    /// <returns>The decision.</returns>
    public static BidDecision Decide(AuctionSnapshot auction, string bidderId, decimal amount, DateTime now)
    {
        throw new NotImplementedException("TODO");
    }
}
