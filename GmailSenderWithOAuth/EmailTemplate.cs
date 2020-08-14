namespace GmailSenderWithOAuth
{
    public class EmailTemplate
    {
        private static string _Template = @"<!DOCTYPE html><html><head><style>.item1{grid-area: header;padding: 0px;}.item2{grid-area: main;}.item3{grid-area: footer;}.grid-container{display: grid; grid-template-areas: 'header header header header header header''title title title title title title''salutation salutation salutation salutation salutation salutation' 'main main main main main main' 'footer footer footer footer footer footer'; grid-gap: 5px; background-color: rgba(255, 255, 255, 0.8); padding: 5px;}.grid-container > div{text-align: left; padding: 10px 0; font-size: 14px;}.title{font-size: 26px;color: #4a7eb0;grid-area: title;font-weight: 900;font-stretch: condensed;padding: 1px 0px;}.salutation{font-size: 18px;grid-area: salutation;margin: 0px;}.button{background-color: #4a7eb0; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer;}.underlineBlk{background-color: #999999;padding: 1px 0px;width: 50px;}</style></head><body><div class=""grid-container""> <div class=""item1""><a href=""https://www.e-speed.com.sg/"" target=3D""_blank"" style=""text-align: center;margin: 25%;""><img src=""https://www.e-speed.com.sg/hostinza/wp-content/uploads/2020/01/logo-hitam.png"" alt=""ESpeed logo"" title=""ESpeed logo"" width=""135""></a> </div><div class=""title""> <h1>{0}</h1> <div class=""underlineBlk""></div></div><p class=""salutation"">Hi {1},</p><div class=""item2""><p style=""font-size: 15px;"">{2}</p><a href=""{3}"" class=""button"">Verify Email</a><p style=""font-size: 15px;"">If you need help visit the <a href=""https://www.e-speed.com.sg/"" target=3D""_blank"">Help</a> page or <a href=""https://www.e-speed.com.sg/"" target=3D""_blank"">contact us</a>.</p></div><div class=""item3""><img src=""https://www.e-speed.com.sg/hostinza/wp-content/uploads/2020/01/logo-hitam.png"" alt=""ESpeed logo"" title=""ESpeed logo"" width=""109""><p style=""font-size: 13px;"">You are receiving this email because you have sign up for the multifactor authentication.</p></div></div></body></html>";

        public static string CreateEmailTemplateHTML(string title, string name, string mainContent)
        {
            string _TemplateWithoutButton = _Template.Replace("inline-block", "none");
            string _TemplateWithTitle = _TemplateWithoutButton.Replace("{0}", title);
            string _TemplateWithName = _TemplateWithTitle.Replace("{1}", name);
            string _TemplateWithContent = _TemplateWithName.Replace("{2}", mainContent);

            return _TemplateWithContent;
        }

        public static string CreateEmailTemplateHTML(string title, string name, string mainContent, string verificationLink)
        {
            string _TemplateWithTitle = _Template.Replace("{0}", title);
            string _TemplateWithName = _TemplateWithTitle.Replace("{1}", name);
            string _TemplateWithContent = _TemplateWithName.Replace("{2}", mainContent);
            string _TemplateWithButton = _TemplateWithContent.Replace("{3}", verificationLink);

            return _TemplateWithButton;
        }
    }
}
