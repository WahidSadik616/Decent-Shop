﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DecentDbEntities : DbContext
    {
        public DecentDbEntities()
            : base("name=DecentDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CashMemo> CashMemoes { get; set; }
        public virtual DbSet<CompanyList> CompanyLists { get; set; }
        public virtual DbSet<CostDetail> CostDetails { get; set; }
        public virtual DbSet<CustomerDaySell> CustomerDaySells { get; set; }
        public virtual DbSet<CustomerPaymentDetail> CustomerPaymentDetails { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DayCost> DayCosts { get; set; }
        public virtual DbSet<DayIncome> DayIncomes { get; set; }
        public virtual DbSet<DayPayment> DayPayments { get; set; }
        public virtual DbSet<DaySell> DaySells { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SalesmanPayment> SalesmanPayments { get; set; }
        public virtual DbSet<Salesman> Salesmen { get; set; }
        public virtual DbSet<SellDetail> SellDetails { get; set; }
        public virtual DbSet<SellerNewProduct> SellerNewProducts { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<TemporarySell> TemporarySells { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
    }
}