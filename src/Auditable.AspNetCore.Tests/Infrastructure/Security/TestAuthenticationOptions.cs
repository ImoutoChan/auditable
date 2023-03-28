using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Auditable.AspNetCore.Tests.Infrastructure
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new(new[]
        {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "abc-123"),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "dave")
        }, "test");
    }
}