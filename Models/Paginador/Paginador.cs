namespace WebApplication6.Models.Paginador
{
    public class Paginador<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalR { get; set; }

        public Paginador(List<T>items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            TotalR = count;
            this.AddRange(items);
        }
        public bool HasPreviosPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<Paginador<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count= await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Paginador<T>(items, count, pageIndex, pageSize);
        }
    }
}
