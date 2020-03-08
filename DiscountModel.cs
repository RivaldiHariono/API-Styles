using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loyalty.WebAPI.Models
{
    public class CreateDiscountRequest
    {
        public string DiscountName { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StoreCategory { get; set; }
        public string Mall { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string Tenant { get; set; }
    }

    public class CreateDiscountResponse : ResponseBase
    {
        public string DiscountID { get; set; }
    }

    public class UpdateDiscountRequest
    {
        public string DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StoreCategory { get; set; }
        public string Mall { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string Tenant { get; set; }
    }

    public class UpdateDiscountResponse : ResponseBase
    {
        
    }

    public class ListDiscount
    {
        public string DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StoreCategoryID { get; set; }
        public string StoreCategoryName { get; set; }
        public string MallID { get; set; }
        public string MallName { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string TenantID { get; set; }
        public string TenantName { get; set; }
    }

    public class ListDiscountResponse : ResponseBase
    {
        public List<ListDiscount> ListDiscount { get; set; }
    }

    public class ListDiscountRequest{
        public string StoreCategory { get; set; }
        public string Mall { get; set; }
    }

    public class DetailDiscount
    {
        public string DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StoreCategoryID { get; set; }
        public string StoreCategoryName { get; set; }
        public string MallID { get; set; }
        public string MallName { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public string TenantID { get; set; }
        public string TenantName { get; set; }
    }

    public class DetailDiscountRequest
    {
        public string DiscountID { get; set; }
    }

    public class DetailDiscountResponse : ResponseBase
    {
        public DetailDiscount DetailDiscount { get; set; }
    }


}