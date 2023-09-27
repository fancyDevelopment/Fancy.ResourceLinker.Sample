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

                string keyJson = "{\"kid\":\"WglOa16AV2NFd5qElhLndEOujv8doEoNZeTMQCowRPc\",\"kty\":\"RSA\",\"alg\":\"RS256\",\"use\":\"sig\",\"n\":\"v5Q7fKMOTloPg4l-Ukw0rNU9h8Glmj7sCoDzvsdV0J5WhsPtSa0fFVP1sPG0wl8BwHj3l4xM4bUiWUFgodIgMpOIq5hbLXwH7FKoVhHYsB5Nem1SmT9fsYu0avez8RdQ4a7VRVq7xIFNg6FIuxQo1LcaLZClTkNRQ8aS1a9NDvcValsO0uH6N2ue93rzmlaXEgMwWZh0V4y2TzTKTIYMhBUVL5yDwHGFBsPtzqQogxU_HhSzR1jvA3CkCk4cONrIueYr83ZdlzU8vmhUV_S2CWAmo4lHhwJfL6koMbiWkDdJIakeqrdVrqdlFztR_NYT0qqGjhQRPU3RmFcW3Kf8Qw\",\"e\":\"AQAB\",\"x5c\":[\"MIICuTCCAaECBgGK1hvmlzANBgkqhkiG9w0BAQsFADAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwHhcNMjMwOTI3MTAwNjA3WhcNMzMwOTI3MTAwNzQ3WjAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC/lDt8ow5OWg+DiX5STDSs1T2HwaWaPuwKgPO+x1XQnlaGw+1JrR8VU/Ww8bTCXwHAePeXjEzhtSJZQWCh0iAyk4irmFstfAfsUqhWEdiwHk16bVKZP1+xi7Rq97PxF1DhrtVFWrvEgU2DoUi7FCjUtxotkKVOQ1FDxpLVr00O9xVqWw7S4fo3a573evOaVpcSAzBZmHRXjLZPNMpMhgyEFRUvnIPAcYUGw+3OpCiDFT8eFLNHWO8DcKQKThw42si55ivzdl2XNTy+aFRX9LYJYCajiUeHAl8vqSgxuJaQN0khqR6qt1Wup2UXO1H81hPSqoaOFBE9TdGYVxbcp/xDAgMBAAEwDQYJKoZIhvcNAQELBQADggEBALRZsTiBySjDJU0L6RmmUKxTy5e97PLybzF2VKH6CLwp79F4h6RWie1sMLoaF9JpmzrMFGqp3PRRoRhADheGyfcpt6Atzo60xc3d1YB5FEyEhe7MsChi/w+zCljW0wCFPfoQOrPfz1yXuB02Fwd14a5I95SPQW9yWO+uMzAmhrilMcDjriOntkVwp5S6si0wziTlx/jg5CXTVClZjuso+D7xmaeqg9jkRROVgzATSzmpZ5bg23HzM2DaWVBX8hqAHAiW4YFADOBO85pjkzEBvfK9/e+XmmoJbe30OsXF7fMlvZ51RCC1es37bWvFrm55Ta59qlSNJS3hSK3oGuzwF1M=\"],\"x5t\":\"MWAEZlcvJLFElYMQzhLBdizmNyQ\",\"x5t#S256\":\"je61Vmebxu-0iTqqgPW5qmpy9EPeH3IwXwEexYTJkk0\"}";

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