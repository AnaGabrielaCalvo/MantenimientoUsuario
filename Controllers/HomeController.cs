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
        public async Task<IActionResult> ManteUsu(string buscar)
        {
            //ViewBag.NameSortParm = string.IsNullOrEmpty(buscar) ? "Nombre_desc" : "";
            //ViewBag.DateSortParm = buscar == "FechaNacimiento" ? "Fecha_desc" : "Date";

            //var usua = from s in _context.Usuario select s;
            //switch (buscar)
            //{
            //    case "Nombre_Desc":
            //        usua = usua.OrderByDescending(s => s.Nombre);
            //        break;

            //    case "Date":
            //      usua= usua.OrderByDescending(s => s.FechaNacimiento);
            //        break;
            //    case "Fecha_Desc":
            //        usua = usua.OrderByDescending(s => s.FechaNacimiento);
            //        break;
            //    default:
            //        usua = usua.OrderBy(s => s.Id);
            //        break;
            //}
            //return View(usua.ToList());

            List<Usuario> usuarioList = null;

            if (!String.IsNullOrEmpty(buscar))
            {
                usuarioList = _context.Usuario.Where(e => e.Nombre.Contains(buscar)).ToList();

            }
            else
            {
                usuarioList = await _context.Usuario.ToListAsync();
            }
            

            return View(usuarioList);
            //int _TotalRegistros = 0;
            //int _TotalPaginas = 0;



            //    // Recuperamos el 'DbSet' completo
            //    _Usuario = _context.Usuario.ToList();
            //    // Filtramos el resultado por el 'texto de búqueda'
            //    if (!string.IsNullOrEmpty(buscar))
            //    {
            //        foreach (var item in buscar.Split(new char[] { ' ' },
            //                 StringSplitOptions.RemoveEmptyEntries))
            //        {
            //            _Usuario = _Usuario.Where(x => x.Nombre.Contains(item) ||
            //                                          x.Contrasena.Contains(item) ||
            //                                          x.Usu.Contains(item))
            //                                          .ToList();
            //        }
            //    }
            //else
            //{
            //    // Número total de registros de la tabla Customers
            //    _TotalRegistros = _Usuario.Count();
            //    // Obtenemos la 'página de registros' de la tabla Customers
            //    _Usuario = _Usuario.OrderBy(x => x.Nombre)
            //                                     .Skip((pagina - 1) * _RegistrosPorPagina)
            //                                     .Take(_RegistrosPorPagina)
            //                                     .ToList();
            //    // Número total de páginas de la tabla Customers
            //    _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);

            //    // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
            //    _PaginadorUsuario = new DataPaginador<Usuario>()
            //    {
            //        RegistrosPorPagina = _RegistrosPorPagina,
            //        TotalRegistros = _TotalRegistros,
            //        TotalPaginas = _TotalPaginas,
            //        PaginaActual = pagina,
            //        BusquedaActual = buscar,
            //        Resultado = _Usuario
            //    };
            //}






            //List<Usuario> usuarioList = null;
            //if (search != null)
            //{
            //    usuarioList =  _context.Usuario.Include(e=>e.Nombre.Contains(search)).ToList();
            //}
            //else
            //{
            //    usuarioList = await _context.Usuario.ToListAsync();
            //}

            //string hots = Request.Scheme + "://" + Request.Host.Value;
            //object[] resultado = new Paginador<Usuario>().paginador(usuarioList, pagina, registro, "Usuario", "Home", "ManteUsu", hots);

            //DataPaginador<Usuario> usus = new DataPaginador<Usuario>
            //{
            //    Lista = (List<Usuario>) resultado[2],
            //    Pagi_info = (string)resultado[0],
            //    Pagi_navegacion = (string)resultado[1]

            //};

            ////return View(await _context.Usuario.ToListAsync());
            //return View(_PaginadorUsuario);
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