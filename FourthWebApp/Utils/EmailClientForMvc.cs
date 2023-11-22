using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using System.Text;
using SendGrid.Helpers.Mail;
using System.IO;
using Attachment = System.Net.Mail.Attachment;

namespace FourthWebApp.Utils
{
    public class EmailClientForMvc
    {
        //public static string SendEmail(string toAddresses, string emailBody, string subject, EmailSettingView emailSettingView)
        //{



        //    //emailBody = emailBody.Replace("'/Account", "'http://www.nonstopwork.com/Account").Replace("'/Images", "'http://www.nonstopwork.com/Images");


        //    //emailBody = "===== WRITE YOUR REPLY ABOVE THIS LINE =====" + System.Environment.NewLine + emailBody;
        //    //return SendEmailOutlook(toAddresses, emailBody, subject);


        //    MailMessage mail = new MailMessage(emailSettingView.FromEmail, toAddresses);
        //    mail.From = new MailAddress(emailSettingView.FromEmail, emailSettingView.Name);

        //    //set the content
        //    mail.Subject = subject;
        //    mail.Body = emailBody;
        //    mail.IsBodyHtml = true;

        //    SmtpClient smtp = new SmtpClient(emailSettingView.Server, int.Parse(emailSettingView.Port));
        //    //smtp.Credentials = new NetworkCredential("notificationsspmlive@gmail.com", "a123456!");
        //    smtp.Credentials = new NetworkCredential(emailSettingView.Server, emailSettingView.FromPassword);
        //    //smtp.EnableSsl = true;

        //    try
        //    {
        //        smtp.Send(mail);
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public static string SendEmail(string toAddresses, string emailBody, string subject)
        //{


        //    //emailBody = emailBody.Replace("'/Account", "'http://www.nonstopwork.com/Account").Replace("'/Images", "'http://www.nonstopwork.com/Images");


        //    //emailBody = "===== WRITE YOUR REPLY ABOVE THIS LINE =====" + System.Environment.NewLine + emailBody;
        //    //return SendEmailOutlook(toAddresses, emailBody, subject);


        //    //MailMessage mail = new MailMessage("no-reply@ledger365.com", toAddresses);
        //    //mail.From = new MailAddress("no-reply@ledger365.com", "Tilvin");

        //    MailMessage mail = new MailMessage("info@tilvin.com", toAddresses);
        //    mail.From = new MailAddress("info@tilvin.com", "Tilvin");

        //    //set the content
        //    mail.Subject = subject;
        //    mail.Body = emailBody;
        //    mail.IsBodyHtml = true;

        //    SmtpClient smtp = new SmtpClient("smtp.mandrillapp.com", 587);
        //    //smtp.Credentials = new NetworkCredential("notificationsspmlive@gmail.com", "a123456!");
        //    smtp.Credentials = new NetworkCredential("info@tilvin.com", "LCbAOMsnxGs7E0u6azhT1w");
        //    //smtp.EnableSsl = true;

        //    try
        //    {
        //        smtp.Send(mail);
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public static string SendInvoiceEmailWithPdf(string toAddresses, string emailBody, string subject, string allcc, string allbcc, string path, int invoiceType)
        //{

        //    var emails = toAddresses.Split(',');
        //    var ccs = allcc.Split(',');
        //    var bccs = allbcc.Split(',');

        //    MailMessage mail = new MailMessage();

        //    foreach (var email in emails)
        //    {
        //        mail.To.Add(email);
        //    }

        //    if (allcc != "")
        //    {
        //        foreach (var cc in ccs)
        //        {
        //            mail.CC.Add(cc);
        //        }
        //    }

        //    if (allbcc != "")
        //    {
        //        foreach (var bcc in bccs)
        //        {
        //            mail.Bcc.Add(bcc);
        //        }
        //    }


        //    Attachment attachmentFile;
        //    attachmentFile = new Attachment(path);
        //    mail.Attachments.Add(attachmentFile);


        //    //emailBody = emailBody.Replace("'/Account", "'http://www.nonstopwork.com/Account").Replace("'/Images", "'http://www.nonstopwork.com/Images");


        //    //emailBody = "===== WRITE YOUR REPLY ABOVE THIS LINE =====" + System.Environment.NewLine + emailBody;
        //    //return SendEmailOutlook(toAddresses, emailBody, subject);

        //    string invoiceTypeText = "";

        //    if (invoiceType == 1 || invoiceType == 2)
        //    {
        //        invoiceTypeText = "Invoice";
        //    }

        //    if (invoiceType == 3)
        //    {
        //        invoiceTypeText = "Estimate";
        //    }

        //    mail.From = new MailAddress("info@tilvin.com", invoiceTypeText + " via Tilvin");

        //    //set the content
        //    mail.Subject = subject;
        //    mail.Body = emailBody;
        //    mail.IsBodyHtml = true;

        //    try
        //    {
        //        SendEmailSendGrid(toAddresses, mail.Body, mail.Subject, "info@tilvin.com", "Tilvin", path);
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public static async Task<string> SendEmailSendGrid(string toAddresses, string emailBody, string subject, string sendingEmail, string sendingName, string path = null, string fileName = null)
        //{
        //    var apiKey = "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";
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress(sendingEmail, sendingName);

        //    System.Net.ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol;
        //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        //    //var to = new EmailAddress(toAddresses);

        //    var tos = new List<EmailAddress>
        //    {
        //        new EmailAddress(toAddresses),
        //        //new EmailAddress("test2@example.com", "Example User2"),
               
        //    };

        //    //var to = new EmailAddress("arijit.chatterjee@egosimarketing.com");
        //    var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", emailBody);
        //    //msg.AddCc("belen@shnpartners.com");

        //    if (!string.IsNullOrEmpty(path))
        //    {
        //        //Attachment attachment;
        //        //attachment = new Attachment(path);                
        //        //msg.AddAttachmentAsync(path, attachment);

        //        using (var fileStream = File.OpenRead(path))
        //        {
        //            await msg.AddAttachmentAsync(fileName, fileStream);
        //            //msg.AddAttachment("a.pdf", fileStream.ToString());
        //            var response = await client.SendEmailAsync(msg);
        //            return "Ok";
        //            //return response.StatusCode.ToString();
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var response = client.SendEmailAsync(msg).Result;
        //            return response.StatusCode.ToString();
        //        }
        //        catch (Exception ex)
        //        {

        //            return ex.Message;
        //        }
        //    }


        //}
    }
}