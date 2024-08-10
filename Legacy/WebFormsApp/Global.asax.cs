using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace WebFormsApp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Request.Headers.AllKeys.Contains("Authorization"))
            {
                string token = Request.Headers.Get("Authorization").Substring("Bearer ".Length);

                string keyJson = "{\"kid\":\"vnXJY9myn-fjE5JViPrK6Qk2ROdmZjKUPrM4hFSfNbA\",\"kty\":\"RSA\",\"alg\":\"RS256\",\"use\":\"sig\",\"n\":\"z7L0x2a3VPx1cYzKn0aeWjDGv5KzwZYLVVr7A_Buw7Rkt52D13bVFnJVjZmmPqLVXmwvr_Zqzm8UdihiRnTRnLTpUZhy4mRcFkZY3tyLOoeeOlhGuqHImaUAkge-il8y0GGiBlFNmvZDxtmPyBdDk4wW9c1NPi11AUsQPg7I0qRLVFQouVDTaIOEdPSPAiBNmuzgUmHDecb9uF3E6rRe02FW5omxMEE4NS-6Z_8SbNhkEDKICsuwqEUJFjGNkbOD-HfZkbzHeB7CrTjguxdC_5SaCxlZ3X06iCuZYCgakUjpOgsHiYa47HqIVOLJKyKT6s5ERarM5AhenWD8vB30CQ\",\"e\":\"AQAB\",\"x5c\":[\"MIICuTCCAaECBgGPVD6XjjANBgkqhkiG9w0BAQsFADAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwHhcNMjQwNTA3MTgwNzE5WhcNMzQwNTA3MTgwODU5WjAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDPsvTHZrdU/HVxjMqfRp5aMMa/krPBlgtVWvsD8G7DtGS3nYPXdtUWclWNmaY+otVebC+v9mrObxR2KGJGdNGctOlRmHLiZFwWRlje3Is6h546WEa6ociZpQCSB76KXzLQYaIGUU2a9kPG2Y/IF0OTjBb1zU0+LXUBSxA+DsjSpEtUVCi5UNNog4R09I8CIE2a7OBSYcN5xv24XcTqtF7TYVbmibEwQTg1L7pn/xJs2GQQMogKy7CoRQkWMY2Rs4P4d9mRvMd4HsKtOOC7F0L/lJoLGVndfTqIK5lgKBqRSOk6CweJhrjseohU4skrIpPqzkRFqszkCF6dYPy8HfQJAgMBAAEwDQYJKoZIhvcNAQELBQADggEBACmn1ZhbdSDNdt6u+Xjsd50tQa5gfwauKVNriHt8e6fFRPOaxBgsQmcpcwB2Pz1QkZDpIgkANJQmGF046o8qy2ryunK9sg7R+MERVJI7txzxSEn97XqcRozce+/atL5cM648JeeVoHG9ouk5P5xs1W53gUOJi0T+yqA6yXaas9TbM8cqRnNPIQNTDaNnMjL3mRFZd59A87QD7q1G9b0B0ejOiEyB8XEjJqlRgDF/F7LCr3ReZS5hi54+rldi24XkLxX9jvFvim4cibKft+66CTrSPV/Tn8KlzE1NvGlo3hZzObKGemFPt2J204MmmO3jA8OZq10z6SD7sU+39OG9zxs=\"],\"x5t\":\"QWFNr4Mzb-cqEFfqt_ORxFHwFY4\",\"x5t#S256\":\"-JlqRrAt_kqRJlKjoGIecqq1NI88R6bTzPoEbQ78oTw\"}";

                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "legacyapp",
                    ValidIssuer = "http://localhost:8080/realms/ResourceLinker-Sample",
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new JsonWebKey(keyJson),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                if (principal != null)
                {
                    string currentUserName = principal.Claims.Single(c => c.Type == "preferred_username").Value;
                    var ticket = new FormsAuthenticationTicket(currentUserName, false, 10);
                    HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
                }
            }

        }
    }
}