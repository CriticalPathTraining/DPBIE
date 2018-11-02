﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Owin;
using WorkspaceManager.Models;
using Microsoft.IdentityModel.Tokens;
using SystemClaims = System.Security.Claims;

namespace WorkspaceManager {

  public partial class Startup {

    private static string resourceUriPowerBi = "https://analysis.windows.net/powerbi/api";
    private static string aadInstance = "https://login.microsoftonline.com/";
    private static string commonAuthority = aadInstance + "common/";
  
    private static string clientId = ConfigurationManager.AppSettings["client-id"];
    private static string appKey = ConfigurationManager.AppSettings["client-secret"];
    private static string replyUrl = ConfigurationManager.AppSettings["reply-url"];
    

    public void ConfigureAuth(IAppBuilder app) {
      ApplicationDbContext db = new ApplicationDbContext();

      app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

      app.UseCookieAuthentication(new CookieAuthenticationOptions());

      app.UseOpenIdConnectAuthentication(
          new OpenIdConnectAuthenticationOptions {

            ClientId = clientId,
            Authority = commonAuthority,
            TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = false },
            PostLogoutRedirectUri = replyUrl,

            Notifications = new OpenIdConnectAuthenticationNotifications() {            
              AuthorizationCodeReceived = (context) => {
                // get tenant ID
                string tenantID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                // create URL for tenant-specific authority 
                string tenantAuthority = aadInstance + tenantID + "/";
                var code = context.Code;
                ClientCredential credential = new ClientCredential(clientId, appKey);
                string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                AuthenticationContext authContext = new AuthenticationContext(tenantAuthority, new ADALTokenCache(signedInUserID));

                AuthenticationResult result = 
                    authContext.AcquireTokenByAuthorizationCodeAsync(
                      code,
                      new Uri(replyUrl), 
                      credential, 
                      resourceUriPowerBi).Result;

                return Task.FromResult(0);
              }
            }
          });
    }
  }
}