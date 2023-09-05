using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Sample.LegacyApp
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

                string keyJson = "{\r\n  \"kid\": \"C_xKvI3FmI64vt6-eO3QYzJ2et8UmVVqftdR1gvZgI8\",\r\n  \"kty\": \"RSA\",\r\n  \"alg\": \"RS256\",\r\n  \"use\": \"sig\",\r\n  \"n\": \"nE8_hIB6q3i5lG8qETqs95GSG4A5kh59HK6va6fLHSdM8WmnfgIVl-gHXWBCCn1J5Iq-7Ii0AFRh-z85_qHqSusu4LY2R3t5iqPJMCy6Au1S2FzyY0gNpJyfvmBlqLpBOWvoGbnbk-NwLISyL3YdvFxOh9QHCh5-JA6yiBTZGZwnvjVav8i_3yvnCkCZ8XfghQslAU2Exj_H4zw5GmXFSn-JXA5Coo7I0TuVtZrZBjuoe97T8WTfAcnIef0V-h-M387AIBtGexFvmljRi7cD33Q_T3oOgWwl16Kv7tuyuVoNi_NscL6tNO108dsv_dl-MgbMM76hhSAUxhZXZamfJw\",\r\n  \"e\": \"AQAB\",\r\n  \"x5c\": [\r\n    \"MIICnzCCAYcCBgGGxlyO/DANBgkqhkiG9w0BAQsFADATMREwDwYDVQQDDAhGbGlnaHQ0MjAeFw0yMzAzMDkxMjMxNDBaFw0zMzAzMDkxMjMzMjBaMBMxETAPBgNVBAMMCEZsaWdodDQyMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnE8/hIB6q3i5lG8qETqs95GSG4A5kh59HK6va6fLHSdM8WmnfgIVl+gHXWBCCn1J5Iq+7Ii0AFRh+z85/qHqSusu4LY2R3t5iqPJMCy6Au1S2FzyY0gNpJyfvmBlqLpBOWvoGbnbk+NwLISyL3YdvFxOh9QHCh5+JA6yiBTZGZwnvjVav8i/3yvnCkCZ8XfghQslAU2Exj/H4zw5GmXFSn+JXA5Coo7I0TuVtZrZBjuoe97T8WTfAcnIef0V+h+M387AIBtGexFvmljRi7cD33Q/T3oOgWwl16Kv7tuyuVoNi/NscL6tNO108dsv/dl+MgbMM76hhSAUxhZXZamfJwIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQAvyRv8E9/pzExopaARWoOguL2wkmJsrd1LHb2Oeg3d6QEd4hJHydTyya+kHvw6ZtzUkpgIHE8Fn5kvuO+7O9hIZHouCQfeZqI5siTAj/VnyOAN2G/FT7m1VS7sGWG+ghxSnJSp2s7ssnwhd9gWcfU03fEMR8trMUUBxzoPZh02HEBnn6ZoXgrLPwnWYtOjRXdJlxL2imkMnPpl0+J11q+qj0LXoTBNTC5P5Hca4pN8himI4sfUFyjn2PIRTkVUq2y5+UwDDmViAh67/7Uic1JWXqUYqcSrQUtrn3BRxjhKW+qjHywhK2/+qywp/opuy/3FXFqCDnAIIoiOpQ+BO34D\"\r\n  ],\r\n  \"x5t\": \"_7WN8rRqXsbWrqODDhqwsMDZf4Y\",\r\n  \"x5t#S256\": \"ES5zEMsy6h6Us_Ot0GA7raTnNhveOIPr_VU5JsDBzXE\"\r\n}";

                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "flight42-flightmanagement",
                    ValidIssuer = "http://localhost:8080/auth/realms/Flight42",
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