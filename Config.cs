using Duende.IdentityServer.Models;

namespace Duende;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                // treat as a public PKCE client (no client secret) so Postman and native apps can use PKCE
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = 
                {
                    "https://localhost:44300/signin-oidc",
                    "https://oauth.pstmn.io/v1/callback"
                },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" },
                AllowedCorsOrigins = 
                {
                    "https://localhost:44300",
                    "https://localhost:5001",
                    "https://oauth.pstmn.io"
                }
            },
            new Client
            {
                ClientId = "mobile.app",
                ClientName = "Mobile App (Auth Code + PKCE)",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false, // public/native client
                RedirectUris = { "com.myapp://callback", "myapp://oauth2redirect" },
                PostLogoutRedirectUris = { "com.myapp://signout-callback" },
                AllowedScopes = { "openid", "profile", "scope2" },
                AllowOfflineAccess = true, // enable refresh tokens
                // optional hardening:
                RequireConsent = false,
                AccessTokenLifetime = 3600
            }
        };
}
