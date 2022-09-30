using WebApplication6.Models;

namespace WebApplication6.Servicio
{
    public interface IUsuario
    {
        List<Usuario> GetUsuario(string Nombre);
        void SaveOurUpdate(Usuario usuario);
        void Delete(int Id);
        void FiltrarNombre(string Nombre);
    }
}
