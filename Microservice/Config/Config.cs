using IdentityServer4.Models;

namespace Microservice.Config
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { new ApiScope("LibraryAPI.read"), new ApiScope("LibraryAPI.write"), };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("LibraryAPI")
                {
                    Scopes = new List<string> { "LibraryAPI.read", "LibraryAPI.write" },
                    ApiSecrets = new List<Secret> { new Secret("LibrarySecret".Sha256()) },
                    UserClaims = new List<string> { "role" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "library.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("@1q2w3e4r!!".Sha256()) },
                    AllowedScopes = { "LibraryAPI.read", "LibraryAPI.write" }
                },
                // schoolclient client using code flow + pkce
                new Client
                {
                    ClientId = "libraryapplication",
                    ClientSecrets = { new Secret("@1q2w3e4r!!".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:7116/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7116/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7116/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "LibrarylAPI.read" },
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                },
            };
    }
}
