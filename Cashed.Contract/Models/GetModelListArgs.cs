namespace Logic.Cashed.Contract.Models
{
    public class GetModelListArgs
    {
        public bool IncludeDeleted { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
    }
}