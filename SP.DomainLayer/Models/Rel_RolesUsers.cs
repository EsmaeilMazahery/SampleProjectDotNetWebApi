using SP.Infrastructure.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace SP.DomainLayer.Models
{
    public class Rel_RolesUsers
    {
        [Key]
        public int userId { get; set; }
        public virtual User user { get; set; }

        [Key]
        public RolesKey roleId { get; set; }
        public virtual Role role { get; set; }
    }
}