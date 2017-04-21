using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminUserApp.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<User> user { get; set; }
    }
}