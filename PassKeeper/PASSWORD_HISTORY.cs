namespace PassKeeper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PASSWORD_HISTORY
    {
        public int ID { get; set; }

        public int DATA_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string PASSWORD_HIST { get; set; }

        [Column(TypeName = "date")]
        public DateTime DATE_HIST { get; set; }

        public virtual USER_DATA USER_DATA { get; set; }
    }
}
