using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudConnector.GmailAccess.models
{
    public class EmailRecipient 
    {
        public string? Name { get; set; }
        public required string EmailAddress { get; set; }

        [SetsRequiredMembers]
        public EmailRecipient(string emailAddress, string? name = null)
        {
            EmailAddress = emailAddress;
            Name = name;
        }
    }

    internal class ServiceAccountCred
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
        public string universe_domain { get; set; }
    }

    internal class OauthUserCred
    {
        public OauthCred web { get; set; }
    }

    internal class OauthCred
    {
        public string client_id { get; set; }
        public string project_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_secret { get; set; }
        public string[] redirect_uris { get; set; }
    }

    public class InvalidAddressException: Exception
    {
        public InvalidAddressException(string invalidAddress) : base(
            $"The email address {invalidAddress} is not a valid email address")
        {
        }
    }
}
