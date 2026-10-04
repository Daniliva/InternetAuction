namespace InternetAuction.Rules;

public enum AuctionState
{
    Planned,
    Active,
    Closed,
    Cancelled,
}

/// <summary>One bid as the rules see it (no entities, no database).</summary>
/// <param name="BidderId">Who bid.</param>
/// <param name="Amount">How much.</param>
/// <param name="PlacedAt">When (UTC).</param>
public sealed record Bid(string BidderId, decimal Amount, DateTime PlacedAt);

/// <summary>Everything the bidding rules need to know about an auction at one moment.</summary>
/// <param name="AuctionId">The auction id.</param>
/// <param name="LotOwnerId">The author of the lot.</param>
/// <param name="StartingPrice">The minimum price of the lot (Lot.CostMin).</param>
/// <param name="Start">When bidding opens (UTC).</param>
/// <param name="End">When bidding closes (UTC); moves later when a bid arrives at the last moment.</param>
/// <param name="Cancelled">The auction was cancelled by its owner or an administrator.</param>
/// <param name="Bids">The bids placed so far, in any order.</param>
public sealed record AuctionSnapshot(int AuctionId, string LotOwnerId, decimal StartingPrice, DateTime Start, DateTime End, bool Cancelled, IReadOnlyList<Bid> Bids);

public enum BidRejection
{
    None,
    InvalidAmount,
    AuctionNotStarted,
    AuctionClosed,
    AuctionCancelled,
    OwnerCannotBid,
    AlreadyHighestBidder,
    BelowMinimum,
}

/// <summary>The answer of the rules. For an accepted bid, <see cref="NewEnd"/> is the (possibly extended) closing time.</summary>
public sealed record BidDecision(bool Accepted, BidRejection Reason, decimal MinimumAllowed, DateTime NewEnd)
{
    public static BidDecision Reject(BidRejection reason, decimal minimumAllowed, DateTime end) => new(false, reason, minimumAllowed, end);
}
