// Lets C# records / init-only setters compile on netstandard2.1 (the type ships with .NET 5+ only).
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}
