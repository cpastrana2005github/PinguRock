using MongoDB.Driver;
using PinguRock.App_Start;
using PinguRock.Models;
using System.Web.Mvc;

namespace PinguRock.Controllers
{
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
    }
}
