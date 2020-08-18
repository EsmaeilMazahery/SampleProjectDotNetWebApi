using ESkimo.Infrastructure.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ESkimo.DomainLayer.Models
{
    public class Rel_RoleUser
    {
        [Key]
        public int userId { get; set; }
        public virtual User user { get; set; }

        [Key]
        public RolesKey roleId { get; set; }
        public virtual Role role { get; set; }
    }
}