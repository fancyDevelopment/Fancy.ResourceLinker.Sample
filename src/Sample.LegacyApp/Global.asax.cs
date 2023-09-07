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

                string keyJson = "{\"kid\":\"Ue98_07R1XWiIVudY-lDJtLb3iAjwEnmwLXgrjV8ZcE\",\"kty\":\"RSA\",\"alg\":\"RS256\",\"use\":\"sig\",\"n\":\"rlMbLbKX4ezoRUfiPm2dWEpdxS7hyCFeVYYf_6Q_5DJXxXW0X6twcP7rUKkTpEQujj2VD9rnBnOjGlhfSeL6XmFlyLwJeAF0nNLI7llWkUfJkAuRdvjL56JoP3DeqRudxee8zuVWzk0a_uNUh4-MD7qulJCRPmsRrExO_N5KIRaZdthIkEOOIVkthWfsD3zzoLPintKNvbEOuGaFbw64ejrpADaIv_ltLclaOJYmNGIMBquouNv6CyD4T63wlBR4QOAipGhUgPn8SKOT7R-ZkKyVyEJV91WjElYm2CqFv6pHBeIfRtmYy9nRCXtDcaZ6Zib5W-MGUAmFV6NtANldbw\",\"e\":\"AQAB\",\"x5c\":[\"MIICuTCCAaECBgGKbBbGtjANBgkqhkiG9w0BAQsFADAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwHhcNMjMwOTA2MjAwMDQ2WhcNMzMwOTA2MjAwMjI2WjAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCuUxstspfh7OhFR+I+bZ1YSl3FLuHIIV5Vhh//pD/kMlfFdbRfq3Bw/utQqROkRC6OPZUP2ucGc6MaWF9J4vpeYWXIvAl4AXSc0sjuWVaRR8mQC5F2+Mvnomg/cN6pG53F57zO5VbOTRr+41SHj4wPuq6UkJE+axGsTE783kohFpl22EiQQ44hWS2FZ+wPfPOgs+Ke0o29sQ64ZoVvDrh6OukANoi/+W0tyVo4liY0YgwGq6i42/oLIPhPrfCUFHhA4CKkaFSA+fxIo5PtH5mQrJXIQlX3VaMSVibYKoW/qkcF4h9G2ZjL2dEJe0NxpnpmJvlb4wZQCYVXo20A2V1vAgMBAAEwDQYJKoZIhvcNAQELBQADggEBABZL1q+KyWlmphT7u5+Rad+/G8Wbrl3fmlN/gKoeetLPbyxrab0cmSMg8JEU1RyVU9ifvEtBj9e5xjmx9Pe4nBb4bxNo3oO2nMGc40tO14H5Y2EOulLxWJUEsBQvHhMi04RW8ZoGLFbCSNB9AO3qjT4uJ/d+aXM3csrcMujm2Z4ty1hkiw3d0hzy4oinN1tYZ4gsGRn/+Oj26L7wLgXJSHQRNyZVWfc+XMnTNHTIZETij3A6k1YM6DSeX2HK3b7TBBWy89TmgupCsMzEpmKGUVobM3fGae0gJglPB6ybJ+1z2PqVAsa33VIgsVveqk4B9Hy/bUmRErfYLdSyk98ckrw=\"],\"x5t\":\"byZlQF0XX2Dwd0xfGhHPmPe_aKY\",\"x5t#S256\":\"Mx1P58YJnZXNcVn1xNEb8e8K9sO11QDyUST-e1NHIfA\"}";

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