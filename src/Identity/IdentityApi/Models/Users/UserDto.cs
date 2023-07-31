using System.Collections.Generic;
using IdentityApi.Models.Pagination;

namespace IdentityApi.Models.Users
{
    public class UserDto
    {
        public PagingHeader Paging { get; set; }
        public List<UserInfo> Items { get; set; }

    }

    public class UserInfo
    {
        public UserInfo()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Active { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string SuiteApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public decimal? SaleQuota { get; set; }
        public decimal? SaleCommissionRate { get; set; }
        public decimal? PurchaseQuota { get; set; }
        public decimal? PurchaseCommissionRate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public IList<string> Roles { get; set; }
        public dynamic RolesLookupValues { get; set; }
    }
}
