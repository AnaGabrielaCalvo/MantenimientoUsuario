namespace WebApplication6.Models.Paginador
{
    public class DataPaginador<T>
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public string BusquedaActual { get; set; }
        public IEnumerable<T> Resultado { get; set; }
    }
}
