using System.Security.Claims;

namespace GameVerse.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
            => user.FindFirst("sub")?.Value;
    }
}