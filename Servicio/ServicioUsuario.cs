using WebApplication6.Models;

namespace WebApplication6.Servicio
{
    public class ServicioUsuario : IUsuario
    {
        AplicationDbContext _context = null;
        public ServicioUsuario(AplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(int Id)
        {
            _context.Database.ExecuteSqlRaw($"PS_EliminarUsuario {Id}");
            var usuario = _context.Usuario.ToList();
        }

        public List<Usuario> GetUsuario(string Nombre)
        {
            throw new NotImplementedException();
        }

        public void SaveOurUpdate(Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}
