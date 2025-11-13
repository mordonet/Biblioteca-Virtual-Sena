using BibliotecaVirtual.Data;
using BibliotecaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace BibliotecaVirtual.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS DE REGISTRO (SIN CAMBIOS) ---
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            // 1. Asignamos el rol manualmente.
            usuario.Rol = "Aprendiz";

            // 2. Ahora sí, verificamos la validación.
            if (ModelState.IsValid)
            {
                var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Correo == usuario.Correo);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("Correo", "Este correo electrónico ya está registrado.");
                    return View(usuario);
                }

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(usuario);
        }

        // --- MÉTODOS DE LOGIN (ACTUALIZADOS) ---
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string password)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);

            if (usuario != null && usuario.Password == password)
            {
                // --- INICIO DE LA LÓGICA DE SESIÓN ---
                // 1. Creamos la lista de "claims"
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Correo", usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                };

                // 2. Creamos la identidad y el principal
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    // Puedes configurar propiedades adicionales aquí (ej. si la sesión es persistente)
                };

                // 3. Creamos la cookie de autenticación
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                // --- FIN DE LA LÓGICA DE SESIÓN ---

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña inválidos.");
            return View();
        }

        // --- NUEVO MÉTODO DE LOGOUT ---
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}