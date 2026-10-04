using InternetAuction.Rules;

namespace InternetAuction.Rules.Tests;

/// <summary>A deterministic fake: "H1:" + Base64(29+ bytes starting with 0x01 ...) so that LooksHashed accepts it.</summary>
internal sealed class FakeHasher : IPasswordHasher
{
    public int HashCalls { get; private set; }

    public string Hash(string password)
    {
        this.HashCalls++;
        var bytes = new byte[40];
        bytes[0] = 0x01;
        System.Text.Encoding.UTF8.GetBytes(password).Take(30).ToArray().CopyTo(bytes, 8);
        return Convert.ToBase64String(bytes);
    }

    public bool Verify(string hash, string password) => hash == this.Hash(password);
}

public class PasswordMigrationTests
{
    private readonly FakeHasher hasher = new();

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("1111", false)]
    [InlineData("password123", false)]
    [InlineData("not base64 !!", false)]
    [InlineData("AAAA", false)]
    public void Plain_values_are_not_hashes(string? value, bool expected) => PasswordMigration.LooksHashed(value).Should().Be(expected);

    [Fact]
    public void A_v3_identity_style_hash_is_recognised()
    {
        var bytes = new byte[61];
        bytes[0] = 0x01;
        PasswordMigration.LooksHashed(Convert.ToBase64String(bytes)).Should().BeTrue();
        bytes[0] = 0x00;
        PasswordMigration.LooksHashed(Convert.ToBase64String(bytes)).Should().BeTrue();
        bytes[0] = 0x07;
        PasswordMigration.LooksHashed(Convert.ToBase64String(bytes)).Should().BeFalse();
    }

    [Fact]
    public void A_matching_plain_text_password_succeeds_and_returns_the_hash_to_store()
    {
        var result = PasswordMigration.Check("1111", "1111", this.hasher);

        result.Success.Should().BeTrue();
        result.NewHash.Should().Be(this.hasher.Hash("1111"));
        PasswordMigration.LooksHashed(result.NewHash).Should().BeTrue();
    }

    [Fact]
    public void A_wrong_plain_text_password_fails_without_a_hash()
    {
        var result = PasswordMigration.Check("1111", "1112", this.hasher);

        result.Should().Be(new PasswordCheck(false, null));
        this.hasher.HashCalls.Should().Be(0);
    }

    [Fact]
    public void A_hashed_password_is_verified_by_the_hasher_and_not_rehashed()
    {
        var stored = this.hasher.Hash("secret1");

        PasswordMigration.Check(stored, "secret1", this.hasher).Should().Be(new PasswordCheck(true, null));
        PasswordMigration.Check(stored, "secret2", this.hasher).Should().Be(new PasswordCheck(false, null));
    }

    [Fact]
    public void The_stored_hash_itself_is_not_a_valid_password()
    {
        var stored = this.hasher.Hash("secret1");

        PasswordMigration.Check(stored, stored, this.hasher).Success.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, "x")]
    [InlineData("", "x")]
    [InlineData("1111", null)]
    [InlineData("1111", "")]
    [InlineData("", "")]
    public void Missing_values_never_succeed(string? stored, string? provided) =>
        PasswordMigration.Check(stored, provided, this.hasher).Should().Be(new PasswordCheck(false, null));

    [Fact]
    public void Comparison_is_case_sensitive() => PasswordMigration.Check("Secret", "secret", this.hasher).Success.Should().BeFalse();

    [Fact]
    public void A_null_hasher_is_rejected() => ((Action)(() => PasswordMigration.Check("a", "a", null!))).Should().Throw<ArgumentNullException>();
}

public class LoginThrottleTests
{
    private DateTime now = new(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);

    private LoginThrottle Sut(int max = 3) => new(() => this.now, max, TimeSpan.FromMinutes(15));

    [Fact]
    public void Locks_after_the_configured_number_of_failures()
    {
        var sut = this.Sut();
        sut.RegisterFailure("a@x.y");
        sut.RegisterFailure("a@x.y");

        sut.IsLocked("a@x.y").Should().BeFalse();
        sut.RegisterFailure("a@x.y");
        sut.IsLocked("a@x.y").Should().BeTrue();
    }

    [Fact]
    public void The_lock_expires_after_the_lockout_period()
    {
        var sut = this.Sut();
        for (var i = 0; i < 3; i++)
        {
            sut.RegisterFailure("a");
        }

        this.now = this.now.AddMinutes(14).AddSeconds(59);
        sut.IsLocked("a").Should().BeTrue();
        this.now = this.now.AddSeconds(1);
        sut.IsLocked("a").Should().BeFalse();
    }

    [Fact]
    public void Failures_while_locked_do_not_extend_the_lock()
    {
        var sut = this.Sut();
        for (var i = 0; i < 3; i++)
        {
            sut.RegisterFailure("a");
        }

        this.now = this.now.AddMinutes(10);
        sut.RegisterFailure("a");
        this.now = this.now.AddMinutes(5);

        sut.IsLocked("a").Should().BeFalse();
    }

    [Fact]
    public void After_a_lock_expires_counting_starts_again()
    {
        var sut = this.Sut();
        for (var i = 0; i < 3; i++)
        {
            sut.RegisterFailure("a");
        }

        this.now = this.now.AddMinutes(16);
        sut.RegisterFailure("a");
        sut.RegisterFailure("a");

        sut.IsLocked("a").Should().BeFalse();
        sut.RegisterFailure("a");
        sut.IsLocked("a").Should().BeTrue();
    }

    [Fact]
    public void Success_resets_the_counter()
    {
        var sut = this.Sut();
        sut.RegisterFailure("a");
        sut.RegisterFailure("a");
        sut.RegisterSuccess("a");
        sut.RegisterFailure("a");
        sut.RegisterFailure("a");

        sut.IsLocked("a").Should().BeFalse();
    }

    [Fact]
    public void Success_does_not_unlock_a_locked_account()
    {
        var sut = this.Sut();
        for (var i = 0; i < 3; i++)
        {
            sut.RegisterFailure("a");
        }

        sut.RegisterSuccess("a");

        sut.IsLocked("a").Should().BeTrue();
    }

    [Fact]
    public void Accounts_are_independent_and_case_insensitive()
    {
        var sut = this.Sut();
        for (var i = 0; i < 3; i++)
        {
            sut.RegisterFailure("Alice@X.y");
        }

        sut.IsLocked("alice@x.y").Should().BeTrue();
        sut.IsLocked("bob@x.y").Should().BeFalse();
        sut.IsLocked("nobody").Should().BeFalse();
    }

    [Fact]
    public void Is_thread_safe()
    {
        var sut = this.Sut(max: 100);

        Parallel.For(0, 500, _ => sut.RegisterFailure("a"));

        sut.IsLocked("a").Should().BeTrue();
    }
}
