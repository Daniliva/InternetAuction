namespace InternetAuction.Rules;

public static class AuctionLifecycle
{
    /// <summary>
    /// The state at <paramref name="now"/>: Cancelled when the snapshot is cancelled (whatever the time);
    /// Planned before Start; Active from Start (inclusive) to End (exclusive); Closed from End on.
    /// </summary>
    /// <param name="auction">The auction.</param>
    /// <param name="now">Current UTC time.</param>
    /// <returns>The state.</returns>
    public static AuctionState StateAt(AuctionSnapshot auction, DateTime now)
    {
        throw new NotImplementedException("TODO");
    }

    /// <summary>The highest bid; on equal amounts the one placed first wins. Null when there are no bids.</summary>
    /// <param name="bids">The bids.</param>
    /// <returns>The winning bid or null.</returns>
    public static Bid? Winner(IEnumerable<Bid> bids)
    {
        throw new NotImplementedException("TODO");
    }

    /// <summary>
    /// True when the background job should mark the auction as closed now: it is not cancelled and End has been reached.
    /// (A cancelled auction is never "closed by time".)
    /// </summary>
    /// <param name="auction">The auction.</param>
    /// <param name="now">Current UTC time.</param>
    /// <returns>Whether to close it.</returns>
    public static bool ShouldClose(AuctionSnapshot auction, DateTime now)
    {
        throw new NotImplementedException("TODO");
    }
}
