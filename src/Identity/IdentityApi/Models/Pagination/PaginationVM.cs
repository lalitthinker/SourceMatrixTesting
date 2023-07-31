using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;

namespace IdentityApi.Models.Pagination
{
    public class PaginationVM
    {

        public long Id { get; set; }
        public string StoreId = "b3c5adfd-4e0c-4cfa-a110-19eaa79e420b";
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchText { get; set; }
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
        //public DateTime FromDate = DateTime.UtcNow.AddDays(-15);
        //public DateTime ToDate = DateTime.UtcNow;
        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        public bool GetAll
        {
            get { return PageSize == 0 ? true : false; }
            set { }
        }
        public string SortedBy { get; set; }
    }
    public class PagingHeader
    {
        public PagingHeader(
           int totalItems, int pageNumber, int pageSize, int totalPages, string search, string filters, string sort, string sortOrder, int? active)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            Search = search;
            Filters = filters;
            Sort = sort;
            SortOrder = sortOrder;
            Active = active;
        }

        public string Sort { get; }
        public string SortOrder { get; }
        public string Search { get; }
        public string Filters { get; }
        public int TotalItems { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int? Active { get; }

        public string ToJson() => JsonConvert.SerializeObject(this,
                                    new JsonSerializerSettings
                                    {
                                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                                    });

    }
}
