using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Util;
using GoogleCloudConnector.GmailAccess.models;
using MimeKit;
using MimeKit.Encodings;
using Newtonsoft.Json;

namespace GoogleCloudConnector.GmailAccess
{
    public class GmailDataAccess
    {
        private GmailService _gmailService;
        private string ServiceAccountUserName = "orchestragmailservice@orchestra-412904.iam.gserviceaccount.com";
        private string[] scopes = new string[3]{"https://www.googleapis.com/auth/gmail.readonly", "https://www.googleapis.com/auth/gmail.modify", "https://www.googleapis.com/auth/gmail.send"};
        public GmailDataAccess()
        {
            //var json = File.ReadAllText("C:\\OtherProjects\\docs\\GmailServiceAccountCred.json");
            //ServiceAccountCred cr = JsonConvert.DeserializeObject<ServiceAccountCred>(json); 
            //string s = cr.private_key;

            //var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(cr.client_id)
            //{
            //    Scopes = scopes
            //}.FromPrivateKey(cr.private_key));

            //_gmailService = new GmailService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "OrchestraGPT"
            //});

            var json = File.ReadAllText("C:\\OtherProjects\\docs\\WebClientSecret.json");
            var cr = JsonConvert.DeserializeObject<OauthUserCred>(json);
            //var client_secret = cr.web.client_secret;
            //var client_id = cr.web.client_id;
            //var redirect_uris = cr.web.redirect_uris;


            //Oauth2Service oauthService = new Oauth2Service(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = GoogleCredential.FromFile("C:\\OtherProjects\\docs\\WebClientSecret.json"),
            //    ApplicationName = "OrchestraGPT"
            //});

            //var token = oauthService.Tokeninfo().Execute();

            //TODO: solve invalid redirect error
            UserCredential credential2 =  GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets(){ClientId = cr.web.client_id, ClientSecret = cr.web.client_secret},
                scopes,
            "bnesiba@gmail.com",
            default).Result;


            //UserCredential credential = new UserCredential(new AuthorizationCodeFlow(
            //                        new AuthorizationCodeFlow.Initializer(cr.installed.auth_uri, cr.installed.token_uri)), "bnesiba@gmail.com", token);

            //var oAuth2Client = GoogleCredential.FromFile("C:\\OtherProjects\\docs\\gmail_client_secret.json")
            //    .CreateScoped(scopes);
            //oAuth2Client.Impersonate(new ImpersonatedCredential.Initializer("bnesiba@gmail.com"));

            _gmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential2,
                ApplicationName = "OrchestraGPT"
            });
        }

        public bool SendEmail(string toAddress, string subject, string content)
        {
            bool success = false;
            var gmailMessage = CreateEmailMessage(new EmailRecipient(toAddress), null, subject, content);
            var result = _gmailService.Users.Messages.Send(gmailMessage, "bnesiba@gmail.com").Execute();
            return success;
        }

        public bool SendEmail(EmailRecipient toRecipient, string subject, string content)
        {
            bool success = false;
            var gmailMessage = CreateEmailMessage(toRecipient, null, subject, content);
            _gmailService.Users.Messages.Send(gmailMessage, "me");

            return success;
        }

        private Message? CreateEmailMessage(EmailRecipient toRecipient, EmailRecipient? ccRecipient, string subject, string emailContent)
        {
            if (MailAddress.TryCreate(toRecipient.EmailAddress ,out _))
            {
                var email = new MimeKit.MimeMessage();
                email.From.Add(new MimeKit.MailboxAddress("OrchestraGPT", "bnesiba@gmail.com"));
                email.To.Add(new MimeKit.MailboxAddress(toRecipient.Name ?? "emailRecipient", toRecipient.EmailAddress));
                if (ccRecipient != null)
                {
                    email.Cc.Add(new MimeKit.MailboxAddress(ccRecipient.Name ?? "ccRecipient", ccRecipient.EmailAddress));
                }
                email.Subject = subject;

                email.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = emailContent
                };
               
                var gmailMessage = ConvertToGmailMessage(email);
               
                return gmailMessage;
            }
            else
            {
                throw new InvalidAddressException(toRecipient.EmailAddress);
            }
        }


        private Message ConvertToGmailMessage(MimeMessage emailMessage)
        {
            //convert to gmail message
            MemoryStream stream = new MemoryStream();
            emailMessage.WriteTo(stream, new CancellationToken());
            var bytes = stream.ToArray();
            string base64EncodedEmail = Convert.ToBase64String(bytes);

            Message gmailMessage = new Message();
            gmailMessage.Raw = base64EncodedEmail;
            
            return gmailMessage;
        }


    }
}
