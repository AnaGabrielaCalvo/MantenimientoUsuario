using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication6.Models;
using WebApplication6.Servicio;
using WebApplication6.Models.Paginador;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        private readonly AplicationDbContext _context;
        IUsuario _usuario;
        private readonly ILogger<HomeController> _logger;
        private readonly int _RegistrosPorPagina = 10;
        private List<Usuario> _Usuario;
        private DataPaginador<Usuario> _PaginadorUsuario;
        public HomeController(AplicationDbContext contexto, IUsuario usuario)
        {
           _context = contexto;
            _usuario = usuario;
        }

        [HttpGet]
        public async Task<IActionResult> ManteUsu(string orden, string buscar, string filtro, int ? page)
        {
            ViewData["Nombre"] = string.IsNullOrEmpty(orden) ? "Nombre_desc" : "";
            ViewData["Usuario"] = orden == "Usuario" ? "Usu_desc" : "";
            ViewData["Id"] = string.IsNullOrEmpty(orden) ? "Id_desc" : "";
            ViewData["Correo"] = string.IsNullOrEmpty(orden) ? "Correo_desc" : "";
            ViewData["Fecha"] = string.IsNullOrEmpty(orden) ? "Fecha_desc" : "";
            ViewData["Direccion"] = string.IsNullOrEmpty(orden) ? "Direccion_desc" : "";
            ViewData["Telefono"] = string.IsNullOrEmpty(orden) ? "Telefono_desc" : "";
            if (buscar != null)
            {
                page = 1;
            }
            else
            {
                buscar = filtro;
            }
            ViewData["Filtro"] = buscar;
            ViewData["OrdenActual"] = orden;

            var usua = from s in _context.Usuario select s;
            
            //int buscarId = Convert.ToInt32(buscar);

            if (!String.IsNullOrEmpty(buscar))
            {
                //DateTime buscarValor = Convert.ToDateTime(buscar);
                //string buscar2 = Convert.ToString(buscar);
                usua = usua.Where(s => s.Nombre.Contains(buscar) || s.Usu.Contains(buscar) || s.Correo.Contains(buscar) || s.telefono.Equals(buscar) ||
                //s.FechaNacimiento.Day.Equals( fecha.Day ) && s.FechaNacimiento.Month.Equals(fecha.Month) && s.FechaNacimiento.Year.Equals(fecha.Year) ||
                //s.Id == Int32.Parse(buscar)
                //|| s.Direccion.Contains(buscar)
                
                
                s.Id.ToString()==buscar || Convert.ToString(s.FechaNacimiento).Equals(buscar)
                ); 
            }

            switch (buscar)
            {
                case "Nombre_Desc":
                    usua = usua.OrderByDescending(s => s.Nombre);
                    break;

               
                case "Usu_desc":
                    usua = usua.OrderByDescending(s => s.Usu);
                    break;
                case "Id_desc":
                    usua = usua.OrderByDescending(s => s.Id);
                    break;
                case "Correo_desc":
                    usua = usua.OrderByDescending(s => s.Correo);
                    break;
                case "Fecha_desc":
                    usua = usua.OrderByDescending(s => s.FechaNacimiento);
                    break;
                case "Direccion_desc":
                    usua = usua.OrderByDescending(s => s.Direccion);
                    break;
                case "Telefono_desc":
                    usua = usua.OrderByDescending(s => s.telefono);
                    break;
                default:
                    usua = usua.OrderBy(s => s.Id);
                    break;
            }
            int pageSize = 8;
            return View(await  Paginador<Usuario>.CreateAsync(usua.AsNoTracking(), page??1, pageSize));

            
        }

        public IActionResult Crear()
        {
            return View();
        }

        public int validarContrasena(Usuario usuario)
        {
            if (!usuario.Contrasena.Equals(usuario.CContrasena))
            {
                return 0;
            }
            return 1;
        }

        public int validarExContrasena(Usuario usuario)
        {
            List<Usuario> lista = new List<Usuario>();
            lista = _context.Usuario.ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                Usuario usua = lista[i];
                if (usua.Contrasena.Equals(usuario.Contrasena))
                {
                    return 1;
                }
            }
            return 0;
        }

        public int validarCorreo(Usuario usuario)
        {
            List<Usuario> lista = new List<Usuario>();
            lista =  _context.Usuario.ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                Usuario usua = lista[i];
                if (usua.Correo.Equals(usuario.Correo))
                {
                    return 1;
                }
            }
            return 0;
        }

        public int validarTelefono(Usuario usuario)
        {
            List<Usuario> lista = new List<Usuario>();
            lista = _context.Usuario.ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                Usuario usua = lista[i];
                if (usua.telefono.Equals(usuario.telefono))
                {
                    return 1;
                }
            }
            return 0;
        }

        public int validarDominio(Usuario usuario)
        {
            string dominio = "grupobabel.com";
            if (usuario.Correo.Contains(dominio))
            {
                return 1;
            }
            return 0;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (validarContrasena(usuario) != 0)
                {
                    if (validarExContrasena(usuario) != 0)
                    {
                        TempData["Mensaje"] = "La contraseña ya existe";

                    }
                    else
                    {
                        if (validarCorreo(usuario) == 0)
                        {
                            if (validarTelefono(usuario) != 0)
                            {
                                TempData["Mensaje"] = "El teléfono ya existe";
                            }
                            else
                            {
                                if (validarDominio(usuario) == 0)
                                {
                                    TempData["Mensaje"] = "El dominio del correo debe ser @grupobabel.com";
                                }
                                else
                                {
                                    _context.Usuario.Add(usuario);
                                    await _context.SaveChangesAsync();
                                    TempData["Mensaje"] = "La transacción se realizó exitosamente";
                                    return RedirectToAction("ManteUsu");
                                }
                            }
                        }
                        else
                        {
                            TempData["Mensaje"] = "El correo ya existe";                            
                        }
                    }
                }
                else
                {
                    TempData["Mensaje"] = "La contraseña y la confirmación de la contraseña no coincide";
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Editar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (usuario.Contrasena.Equals(usuario.CContrasena))
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "La transacción se realizó exitosamente";
                    return RedirectToAction("ManteUsu");
                }
                TempData["Mensaje"] = "La contraseña y confirmación de contraseña no coinciden";
            }
            else
            {
                TempData["Mensaje"] = "La transacción no se realizó";
                return RedirectToAction("ManteUsu");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Borrar(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Borrar(Usuario usuario)
        {

            if (usuario == null)
            {
                return NotFound();
            }
            _usuario.Delete(usuario.Id);
            //_context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "La transacción se realizó exitosamente";
            return RedirectToAction("ManteUsu");
        }

        //public List<Usuario> getUsuario(string valor)
        //{
        //    List<Usuario> usuarioList = new List<Usuario>();
        //    usuarioList = _context.Usuario.ToList();

        //    foreach (Usuario item in usuarioList)
        //    {
        //        if (usuarioList.Contains(valor))
        //        {

        //        }
        //    }
        //    ;
        //}

        [HttpGet]
        public IActionResult Get(string valor)
        {
            try
            {
                List<Usuario> listaUsu = new List<Usuario>();
                if(valor == null)
                {
                    listaUsu = _context.Usuario.ToList();
                }
                else
                {
                    listaUsu = _context.Usuario.Where(x=>x.Nombre.ToLower().IndexOf(valor.ToLower()) > -1).ToList();
                }
                return Ok(listaUsu);

            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}