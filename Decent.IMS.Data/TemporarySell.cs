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
    
    public partial class TemporarySell
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TemporarySell()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int ID { get; set; }
        public int Count { get; set; }
        public System.DateTime Date { get; set; }
        public string Name { get; set; }
        public int ProductCategoryId { get; set; }
        public double RealPriceRate { get; set; }
        public double PriceRate { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double Benifit { get; set; }
        public int ProductId { get; set; }
    
        public virtual ProductCategory ProductCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
