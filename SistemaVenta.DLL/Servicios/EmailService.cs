using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using SistemaVenta.DLL.Servicios.Contrato;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SistemaVenta.Models;

namespace SistemaVenta.DLL.Servicios
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        private IWebHostEnvironment environment;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDTO request)
        {


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(request.Para));
            email.Subject = request.Asunto;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Contenido
                
            };
            //using var smtp = new SmtpClient();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(_config.GetSection("Email:UserName").Value,
               _config.GetSection("Email:PassWord").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
        /*
        public void SendEmailAdjunto(EmailPathDTO emailPath)
        {


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(emailPath.Para));
            email.Subject = emailPath.Asunto;
            var builder=new BodyBuilder();
            builder.HtmlBody= emailPath.Contenido;
            email.Body=builder.ToMessageBody();

            byte[] fileByte;
            if (System.IO.File.Exists(emailPath.PathAdjunto))
            {
                FileStream file = new FileStream(emailPath.PathAdjunto, FileMode.Open, FileAccess.Read);
                using(var ms=new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileByte = ms.ToArray();
                }
                builder.Attachments.Add("attachment.pdf", fileByte, ContentType.Parse("application/octet-stream"));
            }
           /// email.Attachment = request.PathAdjunto;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailPath.Contenido

            };
            using var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(_config.GetSection("Email:UserName").Value,
               _config.GetSection("Email:PassWord").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
        */
        public void SendEmailAdjunto(EmailPathDTO emailPath)
        {


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(emailPath.Para));
            email.Subject = emailPath.Asunto;
            var builder = new BodyBuilder();
            builder.HtmlBody = emailPath.Contenido;
            email.Body = builder.ToMessageBody();

            byte[] fileByte;
            if (System.IO.File.Exists(emailPath.PathAdjunto))
            {
                FileStream file = new FileStream(emailPath.PathAdjunto, FileMode.Open, FileAccess.Read);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileByte = ms.ToArray();
                }
                builder.Attachments.Add("attachment.pdf", fileByte, ContentType.Parse("application/octet-stream"));
            }
            /// email.Attachment = request.PathAdjunto;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailPath.Contenido

            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(_config.GetSection("Email:UserName").Value,
               _config.GetSection("Email:PassWord").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
        /*
        public void SendEmailAdjunto1(EmailPathDTO emailPath)
        {


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(emailPath.Para));
            email.Subject = emailPath.Asunto;
            var builder = new BodyBuilder();
            builder.HtmlBody = emailPath.Contenido;
            email.Body = builder.ToMessageBody();

            byte[] fileByte;




            var fileEncontrado = await _fileDataRepositorio.Obtener(u => u.IdFile == id);
            var wwwPath = this.environment.WebRootPath;
            if (fileEncontrado.Path == null)
            {
                return Ok("fallido, ruta no encontrada");
            }
            var path = fileEncontrado.Path;

            //getting file from inmemory obj
            //var file = fileDB?.Where(n => n.Id == id).FirstOrDefault();
            //getting file from DB

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);

            return File(memory, contentType, fileName);




            if (System.IO.File.Exists(emailPath.PathAdjunto))
            {
                FileStream file = new FileStream(emailPath.PathAdjunto, FileMode.Open, FileAccess.Read);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileByte = ms.ToArray();
                }
                builder.Attachments.Add("attachment.pdf", fileByte, ContentType.Parse("application/octet-stream"));
            }
            /// email.Attachment = request.PathAdjunto;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailPath.Contenido

            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(_config.GetSection("Email:UserName").Value,
               _config.GetSection("Email:PassWord").Value);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
        */

        /*
          [HttpDelete("Download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileEncontrado = await _fileDataRepositorio.Obtener(u => u.IdFile == id);
            var wwwPath = this.environment.WebRootPath;
            if (fileEncontrado.Path == null)
            {
                return Ok("fallido, ruta no encontrada");
            }
            var path = fileEncontrado.Path;
            
            //getting file from inmemory obj
            //var file = fileDB?.Where(n => n.Id == id).FirstOrDefault();
            //getting file from DB

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);

            return File(memory, contentType, fileName);
        }*/
    }
}
