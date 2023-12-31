//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Decent.IMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double PriceRate { get; set; }
        public int Availability { get; set; }
        public int ProductCategoryId { get; set; }
        public double Benifit { get; set; }
        public Nullable<int> SalesmanId { get; set; }
        public Nullable<int> DaySellId { get; set; }
        public Nullable<int> SellerNewProductId { get; set; }
        public int SellerId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> TemporarySellId { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual DaySell DaySell { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Salesman Salesman { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual SellerNewProduct SellerNewProduct { get; set; }
        public virtual TemporarySell TemporarySell { get; set; }
    }
}
