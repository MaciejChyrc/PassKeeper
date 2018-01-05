namespace PassKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class APP_USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APP_USER()
        {
            USER_DATA = new HashSet<USER_DATA>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string EMAIL { get; set; }

        [Required]
        [StringLength(32)]
        public string PASSWORD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_DATA> USER_DATA { get; set; }
    }
}
