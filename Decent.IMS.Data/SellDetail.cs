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
    
    public partial class SellDetail
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string Time { get; set; }
        public string Salesman { get; set; }
        public string Customer { get; set; }
        public string CustomerPhone { get; set; }
        public string ProductName { get; set; }
        public int ProductCategoryId { get; set; }
        public double RealPriceRate { get; set; }
        public double SellPriceRate { get; set; }
        public int Amount { get; set; }
        public double TotalPrice { get; set; }
        public double Benifit { get; set; }
        public int CashMemoId { get; set; }
    
        public virtual CashMemo CashMemo { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
