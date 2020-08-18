using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertBlogCategoryViewModel
    {
        [MaxLength(ConstantValidations.TitleLength)]
        public string title { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public int? parentId { set; get; }

        public bool enable { set; get; }
    }

    public class UpdateBlogCategoryViewModel
    {
        public int blogCategoryId { set; get; }

        [MaxLength(ConstantValidations.TitleLength)]
        public string title { set; get; }

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public int? parentId { set; get; }

        public bool enable { set; get; }
    }
}
