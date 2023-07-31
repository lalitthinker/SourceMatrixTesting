namespace IdentityApi.Models.Pagination
{
    public class PaginationModel
    {
        public string SortColumn { get; set; }

        public string SortDirection { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string SearchText { get; set; }

        public bool GetAll { get; set; }

        public string Date { get; set; }

        //public DateTime ToDate { get; set; }
    }
}
