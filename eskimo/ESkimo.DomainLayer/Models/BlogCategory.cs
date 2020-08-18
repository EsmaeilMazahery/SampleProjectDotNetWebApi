using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class BlogCategory
    {
        public int blogCategoryId { set; get; }

        [MaxLength(ConstantValidations.TitleLength)]
        public string title { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public int? parentId { set; get; }
        public virtual BlogCategory parent { set; get; }

        public bool enable { set; get; }

        public virtual IList<BlogCategory> children { set; get; }

        public virtual IList<BlogPost> blogPosts { set; get; }
    }
}
