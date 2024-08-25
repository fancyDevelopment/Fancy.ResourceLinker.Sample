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

                string keyJson = "{\"kid\":\"n-5IiGHbaq4jeg_FyaAC3kKLe1Gxixx7sJUq_w1wPxw\",\"kty\":\"RSA\",\"alg\":\"RS256\",\"use\":\"sig\",\"n\":\"uhAweRS5k4k8aC7O_aCxlsoDlOprUFm_nuAPeIb_LXU_3XcYqiXDvVcIj8XATd92WkpTAqJgh7uhuey7WSvf_BgxqJL7LsjrzioMe3LN81vSPoqgvrY9b-8drZq5mTvjKCS9P4aCYt4Xpzk4ZovNoZW3J1qBMTjremYAatkOO4QmO2xtd0W9TPrupO9ZTIvsk6KD1GH5lS8c8tbSm8E3KB0nUE2IeNKbLRHYiuXFeaXIUfa5F8EHXTlBLPUjQnRehS5LkyMfhGA0uRXUYUVItD9ytuWmODlgfVfkitj3wioC6bXd0Ht5aOXNXnSqXT9wHm6ZiSufGqUddNbINVH4nw\",\"e\":\"AQAB\",\"x5c\":[\"MIICuTCCAaECBgGRiajMsTANBgkqhkiG9w0BAQsFADAgMR4wHAYDVQQDDBVyZXNvdXJjZWxpbmtlci5zYW1wbGUwHhcNMjQwODI1MTMwODQ2WhcNMzQwODI1MTMxMDI2WjAgMR4wHAYDVQQDDBVyZXNvdXJjZWxpbmtlci5zYW1wbGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC6EDB5FLmTiTxoLs79oLGWygOU6mtQWb+e4A94hv8tdT/ddxiqJcO9VwiPxcBN33ZaSlMComCHu6G57LtZK9/8GDGokvsuyOvOKgx7cs3zW9I+iqC+tj1v7x2tmrmZO+MoJL0/hoJi3henOThmi82hlbcnWoExOOt6ZgBq2Q47hCY7bG13Rb1M+u6k71lMi+yTooPUYfmVLxzy1tKbwTcoHSdQTYh40pstEdiK5cV5pchR9rkXwQddOUEs9SNCdF6FLkuTIx+EYDS5FdRhRUi0P3K25aY4OWB9V+SK2PfCKgLptd3Qe3lo5c1edKpdP3AebpmJK58apR101sg1UfifAgMBAAEwDQYJKoZIhvcNAQELBQADggEBAEt4hC3F9XQVOqUz1F9J4ORl+f4SZZYCygIhEebzYbOT9pBMJiKZvd+0zbLHbBPU5K6h/w/LpefoqcJHUb3kZt4SRCXmZmqlzdfAuZe9kP0dSmKoF1WMkgZonVgL/wxNkh5pDgOWY4Mmt/yV5tSvUR8VWqGAeyek/0eI67arwEiuLwxK9EI9MLv8orxOODLojKXeqfrstY7GlceQjzbmGbopiRYmqWnHIxRhSLbStbVmpPCmjj6c6KWY1Octh0sUp75IEULgaLbmh/KbRcVPB3UeyMbzuRoeU1JmGv/4RNf/jmmOaEcpBrYLr1rBKImvNd+kjVXPFOmtlwSISJ+exao=\"],\"x5t\":\"O7ZqZ4xB38kQLRYyvhAgm4V7aIc\",\"x5t#S256\":\"XGVeX4XzonG56U3VNeUbjNm3VmylHeHx9bjHj1Hk6mY\"}";

                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "webformsapp",
                    ValidIssuer = "http://localhost:8080/realms/resourcelinker.sample",
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