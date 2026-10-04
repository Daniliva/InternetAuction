namespace InternetAuction.Rules;

/// <summary>
/// An in-memory auction book that applies <see cref="BiddingRules"/> atomically: two bidders racing for the same price cannot both win.
/// It is the reference for what the database version must guarantee (transaction + RowVersion / unique rule) and a handy fake for tests.
/// </summary>
public sealed class AuctionBook
{
    private readonly Dictionary<int, State> auctions = new();
    private readonly object gate = new();

    /// <summary>Registers an auction (not cancelled, no bids). Throws ArgumentException when the id is already registered.</summary>
    /// <param name="auctionId">The id.</param>
    /// <param name="lotOwnerId">The lot owner.</param>
    /// <param name="startingPrice">The starting price.</param>
    /// <param name="start">Start (UTC).</param>
    /// <param name="end">End (UTC), must be after start (ArgumentException otherwise).</param>
    public void Open(int auctionId, string lotOwnerId, decimal startingPrice, DateTime start, DateTime end)
    {
        if (end <= start)
        {
            throw new ArgumentException("The end must be after the start.", nameof(end));
        }

        lock (this.gate)
        {
            if (this.auctions.ContainsKey(auctionId))
            {
                throw new ArgumentException("The auction is already registered.", nameof(auctionId));
            }

            this.auctions[auctionId] = new State(lotOwnerId, startingPrice, start, end);
        }
    }

    /// <summary>
    /// Applies the rules and, when the bid is accepted, stores it and moves the end if anti-sniping says so — all under one lock.
    /// Unknown auction: KeyNotFoundException. The stored bid has PlacedAt = now.
    /// </summary>
    /// <param name="auctionId">The auction.</param>
    /// <param name="bidderId">The bidder.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="now">Current UTC time.</param>
    /// <returns>The decision.</returns>
    public BidDecision Place(int auctionId, string bidderId, decimal amount, DateTime now)
    {
        lock (this.gate)
        {
            var state = this.Find(auctionId);
            var decision = BiddingRules.Decide(state.ToSnapshot(auctionId), bidderId, amount, now);
            if (decision.Accepted)
            {
                state.Bids.Add(new Bid(bidderId, amount, now));
                state.End = decision.NewEnd;
            }

            return decision;
        }
    }

    /// <summary>Marks the auction cancelled (idempotent). Unknown auction: KeyNotFoundException.</summary>
    /// <param name="auctionId">The auction.</param>
    public void Cancel(int auctionId)
    {
        lock (this.gate)
        {
            this.Find(auctionId).Cancelled = true;
        }
    }

    /// <summary>A copy of the auction's current state (the bids are copied too). Unknown auction: KeyNotFoundException.</summary>
    /// <param name="auctionId">The auction.</param>
    /// <returns>The snapshot.</returns>
    public AuctionSnapshot Snapshot(int auctionId)
    {
        lock (this.gate)
        {
            return this.Find(auctionId).ToSnapshot(auctionId);
        }
    }

    private State Find(int id) => this.auctions.TryGetValue(id, out var s) ? s : throw new KeyNotFoundException($"Auction {id} not found.");

    private sealed class State
    {
        public State(string owner, decimal startingPrice, DateTime start, DateTime end)
        {
            this.Owner = owner;
            this.StartingPrice = startingPrice;
            this.Start = start;
            this.End = end;
        }

        public string Owner { get; }

        public decimal StartingPrice { get; }

        public DateTime Start { get; }

        public DateTime End { get; set; }

        public bool Cancelled { get; set; }

        public List<Bid> Bids { get; } = new();

        public AuctionSnapshot ToSnapshot(int id) => new(id, this.Owner, this.StartingPrice, this.Start, this.End, this.Cancelled, this.Bids.ToList());
    }
}
