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
        private string[] scopes = new string[3]{"https://www.googleapis.com/auth/gmail.readonly", "https://www.googleapis.com/auth/gmail.modify", "https://www.googleapis.com/auth/gmail.send"};
        public GmailDataAccess()
        {
            var json = File.ReadAllText("C:\\OtherProjects\\docs\\BackendClientSecret.json");

            var cr = JsonConvert.DeserializeObject<OAuthBackendUserCred>(json);

            UserCredential credential2 =  GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets(){ClientId = cr.installed.client_id, ClientSecret = cr.installed.client_secret},
                scopes,
            "bnesiba@gmail.com",
            default).Result;

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
                email.From.Add(new MimeKit.MailboxAddress("Ai Assistant - Brandon Nesiba", "bnesiba@gmail.com"));
                email.To.Add(new MimeKit.MailboxAddress(toRecipient.Name ?? "emailRecipient", toRecipient.EmailAddress));
                if (ccRecipient != null)
                {
                    email.Cc.Add(new MimeKit.MailboxAddress(ccRecipient.Name ?? "ccRecipient", ccRecipient.EmailAddress));
                }
                email.Subject = subject;

                email.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Text)
                { 
                    Text = emailContent + "\n\nThis email was assembled and sent by AI systems. Hopefully it worked"
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
