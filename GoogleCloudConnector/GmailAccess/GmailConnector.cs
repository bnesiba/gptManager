using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudConnector.GmailAccess
{
    //TODO: probably should define an interface here so testing is possible in the future
    public class GmailConnector
    {
        private GmailDataAccess _gmailAccess;
        public GmailConnector(GmailDataAccess gmailAccess)
        {
            _gmailAccess = gmailAccess;
        }

        public void SendEmail(string toAddress, string subject, string content)
        {
            _gmailAccess.SendEmail(toAddress, subject, content);
        }
    }
}
