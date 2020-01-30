

namespace LostAndFound.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubCategory()
        {
            this.finds = new HashSet<Find>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
    
        public virtual HeadCategory headCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Find> finds { get; set; }
    }
}
