//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class DichVu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DichVu()
        {
            this.CT_SDDichVu = new HashSet<CT_SDDichVu>();
        }
    
        public int MaDV { get; set; }
        public string TenDV { get; set; }
        public decimal Gia { get; set; }
        public Nullable<int> MaLoaiDV { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_SDDichVu> CT_SDDichVu { get; set; }
        public virtual LoaiDV LoaiDV { get; set; }
    }
}