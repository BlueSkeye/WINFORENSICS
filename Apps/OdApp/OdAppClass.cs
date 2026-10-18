using Microsoft.Graph;
using Microsoft.Graph.Models.Security;
using Microsoft.Identity.Client;
using MIC = Microsoft.Identity.Client;
using OneDriveLib;
using System.Collections;
using System.Collections.Generic;
using SCG = System.Collections.Generic;
using STT = System.Threading.Tasks;
using System.Reflection.Metadata;

namespace OdApp
{
    // This class is the application used for OneDrive content processing. It requires the OneDriveLib
    // library to function. It is a console application that can be run from the command line.
    internal class OdAppClass
    {
        private const string myApplicationID = "5f22a978-4566-41d4-90fc-131dfb1d2642"; // "[EAZ] DEV - Habilitations"
        private static string[] scopes = { "User.Read" };
        private static Guid thisTenantID = Guid.Parse("43007ff5-0887-4e53-b014-4ac3ca035328");

        static async Task Main(string[] args)
        {
            object? TRASH = null;
            IPublicClientApplication appIPCA = PublicClientApplicationBuilder.Create(myApplicationID)
                // Use a global variable that has the registered tenant ID in a GUID variable.
                .WithAuthority(AzureCloudInstance.AzurePublic, thisTenantID.ToString())
                .WithRedirectUri("http://localhost") // was http://localhost - 78507667-2470-4016-8e32-c732a7fc07e3 faioled
                .Build();

            MIC.IClientApplicationBase iCAB = (MIC.IClientApplicationBase)appIPCA;
            MIC.IApplicationBase iAB = (MIC.IApplicationBase)iCAB;
            MIC.PublicClientApplication PCA = (MIC.PublicClientApplication)iAB;
            iCAB = (MIC.IClientApplicationBase)iAB;
            //try {
            //    var X = (MIC.IConfidentialClientApplication)iCAB;
            //    MIC.IConfidentialClientApplication iCCA = X;
            //    throw new Exception("TRY succeded");
            //}
            //catch (Exception e) {
                AcquireTokenInteractiveParameterBuilder ATIPB = PCA.AcquireTokenInteractive(scopes);
                STT.Task<MIC.AuthenticationResult> t_AR = ATIPB.ExecuteAsync();
                MIC.AuthenticationResult MIC_AR = await t_AR;
                // PCA.AcquireTokenInteractive(scopes).ExecuteAsync();
                throw new Exception("IN TRY ELSE");
                // result = await iPCA.AcquireTokenInteractive(scopes).ExecuteAsync();

            //}

            throw new Exception("So good so far.");

            //MIC.IConfidentialClientApplication V = ((MIC.IConfidentialClientApplication) ((MIC.IClientApplicationBase)iCAB));

            //throw new Exception("So good so far.");

            //IConfidentialClientApplication iCCA = (IConfidentialClientApplication)iPCA;
            //IClientApplicationBase iCAB = (IClientApplicationBase)iPCA;
            // AuthenticationResult result;


            //try {
            //    AcquireTokenSilentParameterBuilder ATSPB = iCAB.AcquireTokenSilent(scopes, firstOrDefaultAccount);
            //    Task<AuthenticationResult> t_AR = ATSPB.ExecuteAsync();
            //    // STT.Task<MIC.AuthenticationResult>> result = await t_AR;
            //    result = await t_AR;
            //}
            //catch {
            //    result = await iPCA.AcquireTokenInteractive(scopes).ExecuteAsync();
            //}
            //Console.WriteLine($"Successfully authenticated to {result.Account.Username}.");
        }
    }
}
