using MongoDB.Driver;
using PinguRock.App_Start;
using PinguRock.Models;
using System.Net.Mail;
using System;
using System.Web.Mvc;

namespace PinguRock.Controllers
{
    //Controlador para la autenticación de usuarios
    public class AccountController : Controller
    {
        private readonly IMongoCollection<Usuario> _usuariosCollection;
        public AccountController()
        {
            // Inicializa el contexto de MongoDB y la colección de usuarios
            MongoDBContext dbContext = new MongoDBContext();
            _usuariosCollection = dbContext.database.GetCollection<Usuario>("Usuario");
        }

        // Acción de Login [GET]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // Acción de Login [POST]
        [HttpPost]
        public ActionResult Login(string nombreUsuario, string contrasena)
        {
            // Busca un usuario que coincida con el nombre de usuario y la contraseña proporcionados
            var usuario = _usuariosCollection
                .Find(u => u.NombreUsuario == nombreUsuario && u.Contrasena == contrasena)
                .FirstOrDefault();

            if (usuario != null)
            {
                // Si el usuario es encontrado, se inicia sesión
                Session["UsuarioLogueado"] = usuario.NombreUsuario;
                return RedirectToAction("Index", "Proveedor"); // Redirige a una página principal después del login exitoso
            }
            else
            {
                // Si no se encuentra el usuario, muestra un mensaje de error
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }

        // Acción de Logout
        [HttpGet]
        public ActionResult Logout()
        {
            // Cierra la sesión
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // Acción de Registro [GET]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // Acción de Registro [POST]
        [HttpPost]
        public ActionResult Register(string email, string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
            {
                ViewBag.Error = "Debe ingresar en todos los campos.";
                return View();
            }

            // Verifica si el nombre de usuario o el correo electronico ya existen
            var usuarioExistente = _usuariosCollection
                .Find(u => u.NombreUsuario == nombreUsuario || u.Email == email)
                .FirstOrDefault();

            if (usuarioExistente != null)
            {
                ViewBag.Error = "El nombre de usuario o el correo electrónico ya están registrados.";
                return View();
            }

            // Crea un nuevo objeto de Usuario y lo guarda en la base de datos
            var nuevoUsuario = new Usuario
            {
                Email = email,
                NombreUsuario = nombreUsuario,
                Contrasena = contrasena
            };
            _usuariosCollection.InsertOne(nuevoUsuario);

            // Redirige al Login después del registro
            return RedirectToAction("Login", "Account");
        }

        // Acción para solicitar el restablecimiento de contraseña
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult ForgotPassword(string email)
        {
            var usuario = _usuariosCollection.Find(u => u.Email == email).FirstOrDefault();
            if (usuario != null)
            {
                // Genera un token para restablecer la contraseña
                usuario.ResetPasswordToken = Guid.NewGuid().ToString();
                usuario.ResetPasswordExpiration = DateTime.Now.AddHours(1); // Expira en 1 hora

                // Actualiza el usuario en la base de datos
                var filter = Builders<Usuario>.Filter.Eq(u => u.Email, email);
                _usuariosCollection.ReplaceOne(filter, usuario);

                // Enlace para restablecer la contraseña
                string resetLink = Url.Action("ResetPassword", "Account", new { token = usuario.ResetPasswordToken }, protocol: Request.Url.Scheme);

                // Crea el mensaje de correo
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("loropianagymse@gmail.com"); // direccion configurada en SMTP
                    mailMessage.To.Add(email);
                    mailMessage.Subject = "Restablecer Contraseña";
                    mailMessage.Body = $"Haga clic en el siguiente enlace para restablecer su contraseña: <a href='{resetLink}'>Restablecer Contraseña</a>";
                    mailMessage.IsBodyHtml = true; // Permite el HTML en el cuerpo del correo

                    try
                    {
                        // Envío del correo electrónico
                        using (SmtpClient smtpClient = new SmtpClient())
                        {
                            smtpClient.Send(mailMessage);
                            ViewBag.Message = "Se ha enviado un correo con el enlace para restablecer tu contraseña.";
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = $"Error al enviar el correo: {ex.Message}";
                    }
                }
            }
            else
            {
                ViewBag.Error = "No se encontró ninguna cuenta con ese correo.";
            }

            return View();
        }


        // Acción para manejar el token de restablecimiento de contraseña
        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            var usuario = _usuariosCollection.Find(u => u.ResetPasswordToken == token && u.ResetPasswordExpiration > DateTime.Now).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "El enlace para restablecer la contraseña es inválido o ha expirado.";
                return View("Error");
            }

            return View(new ResetPasswordViewModel { Token = token });
        }

        // Acción para cambiar la contraseña
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = _usuariosCollection
                .Find(u => u.ResetPasswordToken == model.Token && u.ResetPasswordExpiration > DateTime.Now)
                .FirstOrDefault();

            if (usuario != null)
            {
                usuario.Contrasena = model.NewPassword;
                usuario.ResetPasswordToken = null;
                usuario.ResetPasswordExpiration = null;

                // Actualiza el usuario en la base de datos
                var filter = Builders<Usuario>.Filter.Eq(u => u.Id, usuario.Id);
                _usuariosCollection.ReplaceOne(filter, usuario);

                ViewBag.Message = "Tu contraseña ha sido restablecida exitosamente.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Error = "El token es inválido o ha expirado.";
                return View("Error");
            }
        }
    }
}
