using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTechShop.Model.Models
{
    [Table("ApplicationRoleGroups")]
    public class ApplicationRoleGroup
    {
        [Key]
        [Column(Order = 1)]
        public int GroupId { get; set; }

        [Column(Order = 2)]
        [StringLength(128)]
        [Key]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole ApplicationRole { set; get; }

        [ForeignKey("GroupId")]
        public virtual ApplicationGroup ApplicationGroup { set; get; }
    }
}
