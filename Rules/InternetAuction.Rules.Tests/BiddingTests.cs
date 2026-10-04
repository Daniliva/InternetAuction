using InternetAuction.Rules;

namespace InternetAuction.Rules.Tests;

public class MoneyTests
{
    [Theory]
    [InlineData("0.01", true)]
    [InlineData("100", true)]
    [InlineData("19.99", true)]
    [InlineData("1000000000", true)]
    [InlineData("0", false)]
    [InlineData("-5", false)]
    [InlineData("10.001", false)]
    [InlineData("1000000000.01", false)]
    public void Valid_bid_amounts(string amount, bool valid) => Money.IsValidBidAmount(decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture)).Should().Be(valid);

    [Theory]
    [InlineData("0", "1.00")]
    [InlineData("50", "1.00")]
    [InlineData("100", "1.00")]
    [InlineData("101", "1.01")]
    [InlineData("250", "2.50")]
    [InlineData("333.33", "3.34")]
    [InlineData("1000", "10.00")]
    [InlineData("12345.67", "123.46")]
    public void Minimum_step_is_one_percent_rounded_up_but_at_least_one(string current, string step)
    {
        var inv = System.Globalization.CultureInfo.InvariantCulture;

        Money.MinimumStep(decimal.Parse(current, inv)).Should().Be(decimal.Parse(step, inv));
    }
}

public class AuctionLifecycleTests
{
    private static readonly DateTime Start = new(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);
    private static AuctionSnapshot A(bool cancelled = false, params Bid[] bids) => new(1, "owner", 100m, Start, Start.AddDays(1), cancelled, bids);

    [Theory]
    [InlineData(-1, AuctionState.Planned)]
    [InlineData(0, AuctionState.Active)]
    [InlineData(60 * 23, AuctionState.Active)]
    [InlineData(60 * 24, AuctionState.Closed)]
    [InlineData(60 * 48, AuctionState.Closed)]
    public void State_follows_the_clock(int minutesFromStart, AuctionState expected) =>
        AuctionLifecycle.StateAt(A(), Start.AddMinutes(minutesFromStart)).Should().Be(expected);

    [Theory]
    [InlineData(-100)]
    [InlineData(10)]
    [InlineData(5000)]
    public void Cancelled_is_cancelled_at_any_time(int minutes) => AuctionLifecycle.StateAt(A(cancelled: true), Start.AddMinutes(minutes)).Should().Be(AuctionState.Cancelled);

    [Fact]
    public void Winner_is_the_highest_bid_and_the_earliest_on_a_tie()
    {
        var bids = new[]
        {
            new Bid("a", 100m, Start.AddMinutes(1)),
            new Bid("b", 150m, Start.AddMinutes(5)),
            new Bid("c", 150m, Start.AddMinutes(3)),
            new Bid("d", 120m, Start.AddMinutes(2)),
        };

        AuctionLifecycle.Winner(bids)!.BidderId.Should().Be("c");
        AuctionLifecycle.Winner(bids.Reverse()).Should().Be(AuctionLifecycle.Winner(bids));
    }

    [Fact]
    public void Winner_of_nothing_is_null() => AuctionLifecycle.Winner(Array.Empty<Bid>()).Should().BeNull();

    [Theory]
    [InlineData(10, false)]
    [InlineData(60 * 24, true)]
    public void ShouldClose_only_when_the_end_is_reached(int minutes, bool expected) => AuctionLifecycle.ShouldClose(A(), Start.AddMinutes(minutes)).Should().Be(expected);

    [Fact]
    public void A_cancelled_auction_is_never_closed_by_time() => AuctionLifecycle.ShouldClose(A(cancelled: true), Start.AddDays(5)).Should().BeFalse();

    [Fact]
    public void Null_arguments_are_rejected()
    {
        ((Action)(() => AuctionLifecycle.StateAt(null!, Start))).Should().Throw<ArgumentNullException>();
        ((Action)(() => AuctionLifecycle.Winner(null!))).Should().Throw<ArgumentNullException>();
    }
}

public class BiddingRulesTests
{
    private static readonly DateTime Start = new(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime Mid = Start.AddHours(6);

    private static AuctionSnapshot Auction(decimal starting = 100m, bool cancelled = false, DateTime? end = null, params Bid[] bids) =>
        new(1, "owner", starting, Start, end ?? Start.AddDays(1), cancelled, bids);

    private static Bid B(string who, decimal amount, int minute = 1) => new(who, amount, Start.AddMinutes(minute));

    [Fact]
    public void First_bid_must_reach_the_starting_price()
    {
        BiddingRules.Decide(Auction(), "alice", 99.99m, Mid).Reason.Should().Be(BidRejection.BelowMinimum);
        BiddingRules.Decide(Auction(), "alice", 100m, Mid).Accepted.Should().BeTrue();
    }

    [Fact]
    public void Later_bids_must_beat_the_leader_by_the_minimum_step()
    {
        var auction = Auction(bids: B("alice", 200m));

        BiddingRules.MinimumNextBid(auction).Should().Be(202m);
        BiddingRules.Decide(auction, "bob", 201.99m, Mid).Reason.Should().Be(BidRejection.BelowMinimum);
        BiddingRules.Decide(auction, "bob", 202m, Mid).Accepted.Should().BeTrue();
    }

    [Fact]
    public void A_rejection_for_a_low_bid_tells_the_minimum()
    {
        var decision = BiddingRules.Decide(Auction(bids: B("alice", 200m)), "bob", 150m, Mid);

        decision.MinimumAllowed.Should().Be(202m);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("100.005")]
    [InlineData("2000000000")]
    public void Invalid_amounts_are_rejected_before_anything_else(string amount) =>
        BiddingRules.Decide(Auction(cancelled: true), "alice", decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture), Mid).Reason.Should().Be(BidRejection.InvalidAmount);

    [Fact]
    public void Time_and_state_checks()
    {
        BiddingRules.Decide(Auction(), "alice", 100m, Start.AddMinutes(-1)).Reason.Should().Be(BidRejection.AuctionNotStarted);
        BiddingRules.Decide(Auction(), "alice", 100m, Start.AddDays(1)).Reason.Should().Be(BidRejection.AuctionClosed);
        BiddingRules.Decide(Auction(cancelled: true), "alice", 100m, Mid).Reason.Should().Be(BidRejection.AuctionCancelled);
        BiddingRules.Decide(Auction(), "alice", 100m, Start).Accepted.Should().BeTrue("the start is inclusive");
    }

    [Fact]
    public void The_lot_owner_cannot_bid() => BiddingRules.Decide(Auction(), "owner", 500m, Mid).Reason.Should().Be(BidRejection.OwnerCannotBid);

    [Fact]
    public void The_current_leader_cannot_outbid_themselves()
    {
        var auction = Auction(bids: B("alice", 200m));

        BiddingRules.Decide(auction, "alice", 300m, Mid).Reason.Should().Be(BidRejection.AlreadyHighestBidder);
        BiddingRules.Decide(auction, "bob", 300m, Mid).Accepted.Should().BeTrue();
    }

    [Fact]
    public void Someone_who_was_outbid_may_bid_again()
    {
        var auction = Auction(bids: new[] { B("alice", 200m, 1), B("bob", 250m, 2) });

        BiddingRules.Decide(auction, "alice", 300m, Mid).Accepted.Should().BeTrue();
    }

    [Fact]
    public void Rules_use_the_given_time_not_the_time_in_the_bid_list()
    {
        var auction = Auction(bids: B("alice", 200m, 99999));

        BiddingRules.Decide(auction, "bob", 300m, Mid).Accepted.Should().BeTrue();
    }

    [Fact]
    public void Checks_run_in_the_documented_order()
    {
        // an owner bidding on a closed auction is told the auction is closed, not that owners cannot bid
        BiddingRules.Decide(Auction(), "owner", 500m, Start.AddDays(2)).Reason.Should().Be(BidRejection.AuctionClosed);
        // a leader bidding too little is told they are already the leader
        BiddingRules.Decide(Auction(bids: B("alice", 200m)), "alice", 1m, Mid).Reason.Should().Be(BidRejection.AlreadyHighestBidder);
    }

    [Fact]
    public void A_bid_in_the_last_two_minutes_extends_the_end()
    {
        var end = Start.AddDays(1);
        var now = end.AddSeconds(-30);

        var decision = BiddingRules.Decide(Auction(end: end), "alice", 100m, now);

        decision.NewEnd.Should().Be(now.AddMinutes(2));
    }

    [Theory]
    [InlineData(-300)]
    [InlineData(-120)]
    public void An_earlier_bid_does_not_move_the_end(int secondsBeforeEnd)
    {
        var end = Start.AddDays(1);

        BiddingRules.Decide(Auction(end: end), "alice", 100m, end.AddSeconds(secondsBeforeEnd)).NewEnd.Should().Be(end);
    }

    [Fact]
    public void A_rejected_bid_never_moves_the_end()
    {
        var end = Start.AddDays(1);

        BiddingRules.Decide(Auction(end: end), "owner", 100m, end.AddSeconds(-10)).NewEnd.Should().Be(end);
    }

    [Fact]
    public void Current_price_is_null_without_bids_and_the_highest_otherwise()
    {
        BiddingRules.CurrentPrice(Auction()).Should().BeNull();
        BiddingRules.CurrentPrice(Auction(bids: new[] { B("a", 120m), B("b", 180m) })).Should().Be(180m);
    }

    [Fact]
    public void Null_auction_is_rejected() => ((Action)(() => BiddingRules.Decide(null!, "a", 1m, Mid))).Should().Throw<ArgumentNullException>();
}

public class AuctionBookTests
{
    private static readonly DateTime Start = new(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime Mid = Start.AddHours(6);

    private static AuctionBook Book(decimal starting = 100m)
    {
        var book = new AuctionBook();
        book.Open(1, "owner", starting, Start, Start.AddDays(1));
        return book;
    }

    [Fact]
    public void Accepted_bids_are_stored_with_the_time_of_the_decision()
    {
        var book = Book();

        book.Place(1, "alice", 100m, Mid).Accepted.Should().BeTrue();

        book.Snapshot(1).Bids.Should().ContainSingle().Which.Should().Be(new Bid("alice", 100m, Mid));
    }

    [Fact]
    public void Rejected_bids_are_not_stored()
    {
        var book = Book();

        book.Place(1, "owner", 100m, Mid);
        book.Place(1, "alice", 5m, Mid);

        book.Snapshot(1).Bids.Should().BeEmpty();
    }

    [Fact]
    public void Anti_sniping_moves_the_stored_end()
    {
        var book = Book();
        var now = Start.AddDays(1).AddSeconds(-20);

        book.Place(1, "alice", 100m, now);

        book.Snapshot(1).End.Should().Be(now.AddMinutes(2));
        book.Place(1, "bob", 200m, now.AddMinutes(1)).Accepted.Should().BeTrue("the auction is still open after the extension");
    }

    [Fact]
    public void Of_many_racing_identical_bids_exactly_one_wins()
    {
        var book = Book();

        var results = new System.Collections.Concurrent.ConcurrentBag<bool>();
        Parallel.For(0, 200, i => results.Add(book.Place(1, "bidder" + i, 100m, Mid).Accepted));

        results.Count(r => r).Should().Be(1);
        book.Snapshot(1).Bids.Should().HaveCount(1);
    }

    [Fact]
    public void Concurrent_ascending_bids_keep_the_book_consistent()
    {
        var book = Book();

        Parallel.For(0, 300, i => book.Place(1, "bidder" + (i % 7), 100m + i, Mid));

        var bids = book.Snapshot(1).Bids.OrderBy(b => b.Amount).ToList();
        for (var i = 1; i < bids.Count; i++)
        {
            bids[i].Amount.Should().BeGreaterThanOrEqualTo(bids[i - 1].Amount + Money.MinimumStep(bids[i - 1].Amount));
        }
    }

    [Fact]
    public void Cancel_stops_bidding_and_is_idempotent()
    {
        var book = Book();

        book.Cancel(1);
        book.Cancel(1);

        book.Place(1, "alice", 100m, Mid).Reason.Should().Be(BidRejection.AuctionCancelled);
    }

    [Fact]
    public void Snapshots_are_copies()
    {
        var book = Book();
        book.Place(1, "alice", 100m, Mid);

        var bids = (List<Bid>)book.Snapshot(1).Bids;
        bids.Clear();

        book.Snapshot(1).Bids.Should().HaveCount(1);
    }

    [Fact]
    public void Unknown_and_duplicate_auctions_and_bad_dates()
    {
        var book = Book();

        ((Action)(() => book.Place(9, "a", 1m, Mid))).Should().Throw<KeyNotFoundException>();
        ((Action)(() => book.Cancel(9))).Should().Throw<KeyNotFoundException>();
        ((Action)(() => book.Snapshot(9))).Should().Throw<KeyNotFoundException>();
        ((Action)(() => book.Open(1, "o", 1m, Start, Start.AddDays(1)))).Should().Throw<ArgumentException>();
        ((Action)(() => book.Open(2, "o", 1m, Start, Start))).Should().Throw<ArgumentException>();
    }
}
