using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace PocketStrafe
{
    internal static class LogFileManager
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly FileAppender rootAppender = ((Hierarchy)LogManager.GetRepository())
            .Root
            .Appenders
            .OfType<RollingFileAppender>()
            .FirstOrDefault();

        private static readonly FileAppender lastAppender = ((Hierarchy)LogManager.GetRepository())
            .Root
            .Appenders
            .OfType<FileAppender>()
            .Except(((Hierarchy)LogManager.GetRepository()).Root.Appenders.OfType<RollingFileAppender>())
            .FirstOrDefault();

        private static readonly string RollingLogFilename = rootAppender != null ? rootAppender.File : string.Empty;
        private static readonly string LastLogFilename = lastAppender != null ? lastAppender.File : string.Empty;

        public static bool EmailLogFile()
        {
            log.Info("Will attempt to email file: " + RollingLogFilename);

            MailMessage mail = new MailMessage("coolfontwin.crash@gmail.com", "coolfontwin.crash@gmail.com"); // from, to
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.googlemail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("coolfontwin.crash@gmail.com", "emailpassword212");
            mail.Subject = "Crash Report for " + Environment.UserName;
            mail.Body = System.IO.File.ReadAllText(LastLogFilename);
            client.Timeout = 10000; // ms
            mail.Attachments.Add(new Attachment(RollingLogFilename));

            try
            {
                client.Send(mail);
            }
            catch (SmtpException smtpEx)
            {
                log.Error(smtpEx);
            }

            return true;
        }
    }
}