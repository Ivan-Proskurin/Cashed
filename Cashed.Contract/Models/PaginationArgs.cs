namespace Cashed.Logic.Contract.Models
{
    public class PaginationArgs
    {
        public bool IncludeDeleted { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
    }
}