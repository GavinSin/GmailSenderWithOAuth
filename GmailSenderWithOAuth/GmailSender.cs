using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using MimeKit;
using System;
using System.IO;

namespace GmailSenderWithOAuth
{
    public class GmailSender
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public string BaseCredDir { get; set; }
        public bool IsHTML { get; set; } = false;

        public void SendGmail()
        {
            // Null Check
            if (Sender is null) { throw new ArgumentNullException(nameof(Sender)); }
            if (Receiver is null) { throw new ArgumentNullException(nameof(Receiver)); }
            if (EmailSubject is null) { throw new ArgumentNullException(nameof(EmailSubject)); }
            if (EmailContent is null) { throw new ArgumentNullException(nameof(EmailContent)); }
            if (BaseCredDir is null) { throw new ArgumentNullException(nameof(BaseCredDir)); }

            // Set Credential Path to store credential.json and refresh token
            OAuthForGmail.CredPath = BaseCredDir;

            // Initialize Gmail Service with credentials
            GmailService gmailService = OAuthForGmail.GetGmailService(
                Path.Combine(OAuthForGmail.CredPath, "credentials.json"),
                Sender,
                new string[] { GmailService.Scope.MailGoogleCom });

            // Create email
            MimeMessage emailContent = EmailUtil.CreateEmailMessage(
                Receiver,
                Sender,
                EmailSubject,
                EmailContent,
                IsHTML);

            // Convert email to raw base64 message
            Message message = EmailUtil.CreateMessageWithEmail(emailContent);

            // Create request and send the email
            UsersResource.MessagesResource.SendRequest request = gmailService.Users.Messages.Send(message, Sender);
            request.Execute();

        }
    }

}
