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
    
    public partial class PaymentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentDetail()
        {
            this.Sellers = new HashSet<Seller>();
        }
    
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string SellerName { get; set; }
        public string Phone { get; set; }
        public double TotalDue { get; set; }
        public double Payment { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seller> Sellers { get; set; }
    }
}
