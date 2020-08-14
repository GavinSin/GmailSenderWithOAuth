## GmailSenderWithOAuth
As far as I know, there are 2 ways to send an automated email:

 1. Sending using a simple powershell script with plain **username** and **password**. SMTP server of Google and Outlook will automatically blocked the script from sending. Thus, a workaround is to enable a **Less Secure App** setting, to allow script to execute the email. This will makes the email less secure overall and is not recommended.
 2. Another way is using **OAuth2.0**, which is what this library is using. This library will attempt to get **OAuth token** and **refresh token** with credentials. After that, every email send can be authenticated by just using OAuth token, and a refresh token that has **no expiry date** to retrieve new OAuth token if needed.

> This library can sent an Email on behalf of a Gmail user via OAuth2.0. To send Email on behalf of an Outlook user via OAuth2.0, you can visit link [here](https://docs.microsoft.com/en-us/outlook/rest/get-started#use-oauth2-to-authenticate).

The source code of this library can be found [here](./GmailSenderWithOAuth).

#### Create an Email
In the library, there are currently two Email templates. The different is that one contains a verification link, while the other do not.

To create a verification email:
```csharp
EmailTemplate.CreateEmailTemplateHTML(
	"Title of the Email",
	"Username",
	"This is the main content of an email",
	"VERIFICATION_LINK")
```

To create a normal email:
```csharp
EmailTemplate.CreateEmailTemplateHTML(
	"Title of the Email",
	"Username",
	"This is the main content of an email")
```

Both functions will return an HTML format of the email that is ready to be sent.

#### Send Email
This library will look for a file named "credentials.json" that is used to send an email, like the code shown below:
```csharp
GmailSender gmailSender = new GmailSender
{
    BaseCredDir = "path/to/directory_where_credential_is",
    EmailContent = EmailTemplate.GetEmailTemplate(
     "Title of the Email",
     "Username",
     "This is the main content of an email"),
    EmailSubject = "Subject of an Email",
    Receiver = "receiver@example.com",
    Sender = "sender@gmail.com",
    IsHTML = true
};
gmailSender.SendGmail();
```

> Written with [StackEdit](https://stackedit.io/).
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTI0MzgyODA4OF19
-->