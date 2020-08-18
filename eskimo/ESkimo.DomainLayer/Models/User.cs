
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;

namespace ESkimo.DomainLayer.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string username { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { get; set; }
        
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string family { get; set; }
        
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { get; set; }
        
        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [MaxLength(ConstantValidations.PasswordLength)]
        public string newPassword { get; set; }
        
        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }
    
        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        public bool enable { get; set; } = true;
        
        public DateTime registerDate { set; get; } = DateTime.Now;

        public virtual IList<Rel_RoleUser> roles { get; set; }
        public virtual IList<PocketPost> pocketPosts { get; set; }
        public virtual IList<PocketPost> pocketPostSenders { get; set; }
        public virtual IList<BlogComment> blogComments { get; set; }
        public virtual IList<BlogPost> blogPosts { get; set; }
    }
}
