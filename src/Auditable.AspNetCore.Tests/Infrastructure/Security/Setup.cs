using System;
using Microsoft.AspNetCore.Authentication;

namespace Auditable.AspNetCore.Tests.Infrastructure
{
    public static class Setup
    {
        public static AuthenticationBuilder AddTestAuth(
            this AuthenticationBuilder builder,
            Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(DisabledAuthValues.Scheme,
                DisabledAuthValues.Authority, configureOptions);
        }
    }
}