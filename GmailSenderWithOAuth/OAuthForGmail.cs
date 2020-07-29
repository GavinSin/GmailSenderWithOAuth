using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

namespace GmailSenderWithOAuth
{
    class OAuthForGmail
    {
        public static string CredPath { get; set; }

        /// <summary>
        /// ** Installed Application only ** 
        /// This method requests Authentication from a user using Oauth2.  
        /// </summary>
        /// <param name="clientSecretJson">Path to the client secret json file from Google Developers console.</param>
        /// <param name="userName">Identifying string for the user who is being authentcated.</param>
        /// <param name="scopes">Array of Google scopes</param>
        /// <returns>GmailService used to make requests against the Gmail API</returns>
        public static GmailService GetGmailService(string clientSecretJson, string userName, string[] scopes)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");

                var cred = GetUserCredential(clientSecretJson, userName, scopes);
                return GetService(cred);

            }
            catch (Exception ex)
            {
                throw new Exception("Get Gmail service failed.", ex);
            }
        }

        /// <summary>
        /// ** Installed Aplication only ** 
        /// This method requests Authentication from a user using Oauth2.  
        /// Credentials are stored in System.Environment.SpecialFolder.CommonDocuments
        /// Documentation https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="clientSecretJson">Path to the client secret json file from Google Developers console.</param>
        /// <param name="userName">Identifying string for the user who is being authentcated.</param>
        /// <param name="scopes">Array of Google scopes</param>
        /// <returns>authencated UserCredential</returns>
        private static UserCredential GetUserCredential(string clientSecretJson, string userName, string[] scopes)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");

                // These are the scopes of permissions you need. It is best to request only what you need and not all of them               
                using (var stream = new FileStream(clientSecretJson, FileMode.Open, FileAccess.Read))
                {
                    // string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments);
                    string refreshTokenPath = Path.Combine(CredPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                    // Requesting Authentication or loading previously stored authentication for userName
                    var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                                                                             scopes,
                                                                             userName,
                                                                             CancellationToken.None,
                                                                             new FileDataStore(refreshTokenPath, true)).Result;

                    credential.GetAccessTokenForRequestAsync();
                    return credential;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get user credentials failed.", ex);
            }
        }

        /// <summary>
        /// This method get a valid service
        /// </summary>
        /// <param name="credential">Authecated user credentail</param>
        /// <returns>GmailService used to make requests against the Gmail API</returns>
        private static GmailService GetService(UserCredential credential)
        {
            try
            {
                if (credential == null)
                    throw new ArgumentNullException("credential");

                // Create Gmail API service.
                return new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Gmail Oauth2 Authentication Sample"
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Get Gmail service failed.", ex);
            }
        }
    }
}
