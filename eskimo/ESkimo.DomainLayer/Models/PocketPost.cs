using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class PocketPost
    {
        [Key]
        public int pocketPostId { set; get; }

        public DateTime registerDateTime { set; get; } 
        public DateTime sendDateTime { set; get; } 

        public decimal amount { set; get; }

        public int count { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public int userSenderId { set; get; }
        public virtual User userSender { set; get; }

        public int userId { set; get; }
        public virtual User user { set; get; }

        public virtual IList<Factor> factors { get; set; }
    }
}
