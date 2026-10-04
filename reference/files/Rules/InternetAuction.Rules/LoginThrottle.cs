namespace InternetAuction.Rules;

/// <summary>Locks an account after repeated failed logins (the login page used to allow unlimited guessing).</summary>
public sealed class LoginThrottle
{
    private readonly Func<DateTime> clock;
    private readonly int maxFailures;
    private readonly TimeSpan lockout;
    private readonly Dictionary<string, Entry> entries = new(StringComparer.OrdinalIgnoreCase);
    private readonly object gate = new();

    public LoginThrottle(Func<DateTime> clock, int maxFailures = 5, TimeSpan? lockout = null)
    {
        this.clock = clock;
        this.maxFailures = maxFailures;
        this.lockout = lockout ?? TimeSpan.FromMinutes(15);
    }

    /// <summary>True while the account is locked (it was locked less than the lockout period ago). Keys compare case-insensitively. Unknown keys are not locked.</summary>
    /// <param name="key">Usually the e-mail or user name.</param>
    /// <returns>Whether login attempts must be refused right now.</returns>
    public bool IsLocked(string key)
    {
        lock (this.gate)
        {
            return this.entries.TryGetValue(key, out var e) && e.LockedUntil is DateTime until && this.clock() < until;
        }
    }

    /// <summary>
    /// Records a failed login. After <c>maxFailures</c> failures in a row the account is locked for the lockout period starting now.
    /// A failure while locked changes nothing (the lock does not extend). When the lock has expired the counter starts again from 1.
    /// </summary>
    /// <param name="key">The account key.</param>
    public void RegisterFailure(string key)
    {
        lock (this.gate)
        {
            var now = this.clock();
            if (!this.entries.TryGetValue(key, out var e))
            {
                e = new Entry();
                this.entries[key] = e;
            }

            if (e.LockedUntil is DateTime until)
            {
                if (now < until)
                {
                    return;
                }

                e.LockedUntil = null;
                e.Failures = 0;
            }

            e.Failures++;
            if (e.Failures >= this.maxFailures)
            {
                e.LockedUntil = now + this.lockout;
            }
        }
    }

    /// <summary>A successful login clears the failure counter (and any expired lock). It does not clear an active lock: a locked account cannot log in at all.</summary>
    /// <param name="key">The account key.</param>
    public void RegisterSuccess(string key)
    {
        lock (this.gate)
        {
            if (this.entries.TryGetValue(key, out var e) && !(e.LockedUntil is DateTime until && this.clock() < until))
            {
                _ = this.entries.Remove(key);
            }
        }
    }

    private sealed class Entry
    {
        public int Failures { get; set; }

        public DateTime? LockedUntil { get; set; }
    }
}
