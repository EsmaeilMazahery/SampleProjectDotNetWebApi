using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Comment
    {
        public int commentId { set; get; }

        public DateTime dateTime { set; get; }

        [Required]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string text { set; get; }

        public int productId { set; get; }
        public virtual Product product { set; get; }

        public int memberId { set; get; }
        public virtual Member member { set; get; }

        //1 : confirm
        //0 : no confirm
        //null : not check in settings
        public bool? confirm { set; get; } = null;
    }
}
