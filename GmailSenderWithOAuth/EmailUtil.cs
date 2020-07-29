using System.IO;
using Google.Apis.Gmail.v1.Data;
using MimeKit;

namespace GmailSenderWithOAuth
{
    class EmailUtil
    {

        /**
         * Create a MimeMessage using the parameters provided.
         *
         * @param to email address of the receiver
         * @param from email address of the sender, the mailbox account
         * @param subject subject of the email
         * @param bodyText body text of the email
         * @param isHtml the bodyText of email will be treated as text/html instead of text/plain
         * @return the MimeMessage to be used to send email
         */
        public static MimeMessage CreateEmailMessage(
            string to,
            string from,
            string subject,
            string bodyText,
            bool isHtml = false)
        {
            string bodyFormat = "plain";
            if (isHtml)
            {
                bodyFormat = "html";
            }

            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress("", from));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;
            email.Body = new TextPart(bodyFormat)
            {
                Text = bodyText
            };
            return email;
        }

        /**
         * Create a message from an email.
         *
         * @param emailContent Email to be set to raw of message
         * @return a message containing a base64url encoded email
         */
        public static Message CreateMessageWithEmail(MimeMessage emailContent)
        {
            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                emailContent.WriteTo(stream);
                bytes = stream.ToArray();
            }

            char[] padding = { '=' };
            string encodedEmail = System.Convert.ToBase64String(bytes)
                .TrimEnd(padding).Replace('+', '-').Replace('/', '_');

            Message message = new Message
            {
                Raw = encodedEmail
            };

            return message;
        }

        /**
         * Create a MimeMessage using the parameters provided.
         *
         * @param to Email address of the receiver.
         * @param from Email address of the sender, the mailbox account.
         * @param subject Subject of the email.
         * @param bodyText Body text of the email.
         * @param file Path to the file to be attached.
         * @param isHtml the bodyText of email will be treated as text/html instead of text/plain
         * @return MimeMessage to be used to send email.
         */
        public static MimeMessage CreateEmailWithAttachment(
            string to,
            string from,
            string subject,
            string bodyText,
            string filePath,
            bool isHtml)
        {
            string bodyFormat = "plain";
            if (isHtml)
            {
                bodyFormat = "html";
            }
            // create our message text, just like before (except don't set it as the message.Body)
            var body = new TextPart(bodyFormat)
            {
                Text = bodyText
            };

            // create an image attachment for the file located at path
            var attachment = new MimePart("image", "gif")
            {
                Content = new MimeContent(File.OpenRead(filePath), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(filePath)
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed")
            {
                body,
                attachment
            };

            // now set the multipart/mixed as the message body
            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress("", from));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;
            email.Body = multipart;

            return email;
        }


    }
}
