using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PinguRock.Controllers
{
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendChart(IEnumerable<HttpPostedFileBase> images, string email)
        {
            if (string.IsNullOrWhiteSpace(email) || images == null || !images.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Correo electrónico o imágenes no proporcionados.");
            }

            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "luisangelrh98@gmail.com";
            string smtpPassword = "jysu jdzx vsru sami";

            using (var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = "Diagramas MongoDB",
                Body = "Aquí están los diagramas solicitados.",
                IsBodyHtml = true
            })
            {
                mailMessage.To.Add(email);

                foreach (var image in images)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await image.InputStream.CopyToAsync(stream);
                            stream.Position = 0;
                            mailMessage.Attachments.Add(new Attachment(stream, Path.GetFileName(image.FileName)));
                        }
                    }
                }

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = true
                })
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }

            return Content("Correo enviado con éxito.");
        }
    }
}
