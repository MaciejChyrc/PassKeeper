namespace PassKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_DATA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER_DATA()
        {
            PASSWORD_HISTORY = new HashSet<PASSWORD_HISTORY>();
        }

        public int ID { get; set; }

        public int APP_USER_ID { get; set; }

        [StringLength(30)]
        public string SERV_NAME { get; set; }

        [Required]
        [StringLength(32)]
        public string SERV_PASSWORD { get; set; }

        [StringLength(255)]
        public string COMMENT { get; set; }

        public virtual APP_USER APP_USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PASSWORD_HISTORY> PASSWORD_HISTORY { get; set; }
    }
}
