using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.Helper
{
    public class Email
    {

        public string TO { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SMTPHost { get; set; }
        public int ServerPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
        public bool IsBodyHTML { get; set; }
        public List<string> Attachments { get; set; }

        public Email()
        {
            this.Attachments = new List<string>();
            this.CC = string.Empty;
            this.BCC = string.Empty;

        }

        public bool Send()
        {
            try
            {

                if (string.IsNullOrEmpty(this.TO))
                {
                    throw new Exception("Email must contain at least one 'To' address.");
                }

                if (string.IsNullOrEmpty(this.From))
                {
                    throw new Exception("Email must contain a 'From' property.");
                }

                if (string.IsNullOrEmpty(this.Body))
                {
                    throw new Exception("Email must contain a 'Body' property.");
                }

                //get mail object
                MailMessage mail = new MailMessage();
                
                //Set server
                SmtpClient SmtpServer = new SmtpClient(this.SMTPHost);

                //Set From Address
                mail.From = new MailAddress(this.From); 


                //Reaplce "," with ";" in the email addresses if present
                TO=TO.Replace(",", ";");
                CC=CC.Replace(",", ";");
                BCC = BCC.Replace(",", ";");

                //Set TO Addressses
                foreach (var email in this.TO.Split(';'))
                {
                    mail.To.Add(email.ToString());
                }

                //Set CC Addressses
                if (!string.IsNullOrEmpty(CC))
                    foreach (var email in this.CC.Split(';'))
                    {
                        mail.CC.Add(email);
                    }

                //Set BCC Addressses
                if (!string.IsNullOrEmpty(BCC))
                    foreach (var email in this.BCC.Split(';'))
                    {
                        mail.Bcc.Add(email);
                    }

                //Set Subject
                if (!string.IsNullOrEmpty(Subject))
                {
                    mail.Subject = this.Subject;
                }
                else
                {
                    mail.Subject = string.Empty;
                }

                //Set Body
                mail.Body = this.Body;

                //HTML/Text setting
                mail.IsBodyHtml = IsBodyHTML;

                //set port if given
                if (this.ServerPort > 0)
                {
                    SmtpServer.Port = this.ServerPort;
                }

                //set credentials if given
                if (this.Username != null && this.Password != null)
                {
                    SmtpServer.Credentials = new System.Net.NetworkCredential(this.Username, this.Password);
                }

                //set SSL settings if given
                if (this.EnableSSL == null)
                {
                    this.EnableSSL = false;
                }
                SmtpServer.EnableSsl = this.EnableSSL;

                //Add attachments if we have them
                if (this.Attachments.Count > 0)
                {
                    foreach(string path in this.Attachments)
                    {
                        mail.Attachments.Add(new Attachment(path));
                    }
                }

                //Send
                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

    }
}
