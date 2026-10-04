using System.Security.Cryptography;
using System.Text;

namespace InternetAuction.Rules;

/// <summary>Hashes and checks passwords. The real implementation wraps ASP.NET Identity's PasswordHasher; tests use a fake.</summary>
public interface IPasswordHasher
{
    /// <summary>Creates a salted hash that <see cref="Verify"/> accepts and <see cref="PasswordMigration.LooksHashed"/> recognises.</summary>
    string Hash(string password);

    bool Verify(string hash, string password);
}

/// <summary>The outcome of checking a login password against what the database holds.</summary>
/// <param name="Success">The password is right.</param>
/// <param name="NewHash">When not null, the caller must store it instead of the old value (the old value was plain text).</param>
public sealed record PasswordCheck(bool Success, string? NewHash);

/// <summary>
/// Moves a database whose "PasswordHash" column holds PLAIN TEXT to real hashes without forcing everybody to reset the password:
/// each user is upgraded the first time they log in successfully.
/// </summary>
public static class PasswordMigration
{
    /// <summary>
    /// True when the value looks like an ASP.NET Identity hash: Base64 that decodes to at least 29 bytes starting with the format marker
    /// 0x00 (v2) or 0x01 (v3). Plain passwords (short, not Base64, or without the marker) are not hashes; null, empty and whitespace are not hashes either.
    /// </summary>
    /// <param name="stored">The value of the PasswordHash column.</param>
    /// <returns>Whether it is already hashed.</returns>
    public static bool LooksHashed(string? stored)
    {
        throw new NotImplementedException("TODO");
    }

    /// <summary>
    /// Checks a login password.
    /// - null or empty stored value, or an empty provided password: failure, no hash;
    /// - stored value looks hashed: success is <c>hasher.Verify</c>; no new hash;
    /// - otherwise the stored value is the old plain text: compare in constant time (so timing does not reveal a prefix) and, on a match,
    ///   return success together with <c>hasher.Hash(provided)</c> to store; on a mismatch a plain failure.
    /// A user must never be able to log in with the stored HASH as the password.
    /// </summary>
    /// <param name="stored">What the database holds.</param>
    /// <param name="provided">What the user typed.</param>
    /// <param name="hasher">The hasher.</param>
    /// <returns>The check result.</returns>
    public static PasswordCheck Check(string? stored, string? provided, IPasswordHasher hasher)
    {
        throw new NotImplementedException("TODO");
    }

    private static bool ConstantTimeEquals(string a, string b)
    {
        using var sha = SHA256.Create();
        var x = sha.ComputeHash(Encoding.UTF8.GetBytes(a));
        var y = sha.ComputeHash(Encoding.UTF8.GetBytes(b));
        var diff = 0;
        for (var i = 0; i < x.Length; i++)
        {
            diff |= x[i] ^ y[i];
        }

        return diff == 0;
    }
}
