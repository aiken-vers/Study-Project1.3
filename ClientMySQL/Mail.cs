using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace ClientMySQL
{
    class Mail
    {
        static public void SendText()
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("ibassender@gmail.com", "IBASER");
            // кому отправляем
            MailAddress to = new MailAddress("dalexator@gmail.com");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Тестовое письмо";
            // текст письма
            m.Body = "<h2><b>Письмо-тест работы smtp-клиента</b></h2><p>Какой-то текст</p>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            smtp_Send(m);
            Console.WriteLine(m.Body);
        }
        static public void Sendpicture(string file)
        {
            string filePath = file;
            string htmlBody = "<html><body><h1>Picture</h1><br><img src=\"cid:filename\"></body></html>";
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(filePath, MediaTypeNames.Image.Jpeg);
            inline.ContentId = Guid.NewGuid().ToString();
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage();


            Attachment att = new Attachment(filePath);
            att.ContentDisposition.Inline = true;

            mail.From = new MailAddress("ibassender@gmail.com", "IBASER"); ;
            mail.To.Add(new MailAddress("dalexator@gmail.com"));
            mail.Subject = "Client: IBASER Has Sent You A Screenshot";
            mail.Body = String.Format(
                       "<h3>Client: IBASER Has Sent You A Screenshot</h3>");
            mail.IsBodyHtml = true;
            mail.Attachments.Add(att);
            smtp_Send(mail);
            Console.WriteLine("Picture sent");
        }
        static public void SendDoc(string path)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("ibassender@gmail.com", "X-Банк");
            // кому отправляем
            MailAddress to = new MailAddress("dalexator@gmail.com");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Выписка лицевого счёта";
            m.IsBodyHtml = false;
            m.Body = String.Format(
                       "Выписка лицевого счёта в Х-БАНК за {0}", DateTime.Today.ToString("MM/dd/yyyy"));

            //WordDocument doc = new WordDocument(path);
            //doc.Visible = false;
            var attachmentFilename = path;

            if(attachmentFilename != null)
            {
                Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                disposition.FileName = Path.GetFileName(attachmentFilename);
                disposition.Size = new FileInfo(attachmentFilename).Length;
                disposition.DispositionType = DispositionTypeNames.Attachment;
                m.Attachments.Add(attachment);
            }            
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            smtp_Send(m);
        }
        static public void smtp_Send(MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new System.Net.NetworkCredential("ibassender@gmail.com", "xzO_nho23011583ber!a");

            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
