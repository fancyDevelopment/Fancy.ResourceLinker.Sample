using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

                string keyJson = "{\"kid\":\"ZDqn-X3hIOwDiB-zthb7Q0b1XU9pmiPY5X3CLJ_RJJw\",\"kty\":\"RSA\",\"alg\":\"RS256\",\"use\":\"sig\",\"n\":\"30rEEnKiZwKqFptVmwBvw9uG16DNedCAbxO3yIqkoekqcNSCUJj4ebGH76_PT5vgvcSdegLYu5rfk26_9chG-LA17Fbu7Y2jAFvT3U6nbFaZs1fxxW_lq5nYTICBCTz9MNV7CUSfwEql2Ij_gFhkodlkkZfu2DNDF5S0QHixFFyEjcfLYy19JSPWzo6L3WQTifQefaycdmTVG0OC6J2tef1KKXowv7Q31lD7frhe6MtsnrNwMadoWCH8FE3J-888bypVrcjZ2X7shXC2axplp_tFxldwF4Dp8sIY1nhNMY-deCjjgyC4wSESNalpVmAYHyR4FEbY7qR78gs1NwICIQ\",\"e\":\"AQAB\",\"x5c\":[\"MIICuTCCAaECBgGRUn/K8TANBgkqhkiG9w0BAQsFADAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwHhcNMjQwODE0MjAwNDUyWhcNMzQwODE0MjAwNjMyWjAgMR4wHAYDVQQDDBVSZXNvdXJjZUxpbmtlci1TYW1wbGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDfSsQScqJnAqoWm1WbAG/D24bXoM150IBvE7fIiqSh6Spw1IJQmPh5sYfvr89Pm+C9xJ16Ati7mt+Tbr/1yEb4sDXsVu7tjaMAW9PdTqdsVpmzV/HFb+WrmdhMgIEJPP0w1XsJRJ/ASqXYiP+AWGSh2WSRl+7YM0MXlLRAeLEUXISNx8tjLX0lI9bOjovdZBOJ9B59rJx2ZNUbQ4Lona15/UopejC/tDfWUPt+uF7oy2yes3Axp2hYIfwUTcn7zzxvKlWtyNnZfuyFcLZrGmWn+0XGV3AXgOnywhjWeE0xj514KOODILjBIRI1qWlWYBgfJHgURtjupHvyCzU3AgIhAgMBAAEwDQYJKoZIhvcNAQELBQADggEBAMMav20zv4x0GOGU3wtRArSJpEGZc0u3tDBwUzJd97THrNvqJa2gRh8k03agawF8FMyaoK7nJN7h2OWXDFxC0hdlCP+76tASKvbOe7NuYcnBwMxv7R9bZJmYIgTd8kKnXSy9PkQdAoAh02bXHMIjiznbOEpqHn7Yf8Ppt9D7t+SHh/O07kI1y7bdbF0T14dV11VZtk13uH8aIYK9kSibPWQ8UbVRCMCUzlqaZx9Pvdez1tYZhlQl86dt/j4aKclg54DI6g5QhoVWu9NtVKs/jT0Lccf/9bqKwtaauPkjyE9HFNnB4+wni37R4ec/zIclx+bNlqxAhOnQkqLtaH8hU+A=\"],\"x5t\":\"0G61tVmbWnRBb8gI4NVve9L_E8Y\",\"x5t#S256\":\"rh-Kn6kIvFK-f94h4KFG5odqS60CywNoC8nInZOyxLE\"}";

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
                    string currentUserName = principal.Claims.Single(c => c.Type == ClaimTypes.Email).Value;
                    var ticket = new FormsAuthenticationTicket(currentUserName, false, 10);
                    HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
                }
            }

        }
    }
}