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
    
    public partial class CashMemo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CashMemo()
        {
            this.SellDetails = new HashSet<SellDetail>();
        }
    
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string Time { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Salesman { get; set; }
        public string Phone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellDetail> SellDetails { get; set; }
    }
}
