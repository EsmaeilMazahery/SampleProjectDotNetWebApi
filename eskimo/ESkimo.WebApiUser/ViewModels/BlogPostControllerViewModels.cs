using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertBlogPostViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.TitleLength)]
        public string title { set; get; }

        public int userId { set; get; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { set; get; }

        public string content { set; get; }

        public DateTime publishDate { set; get; } = DateTime.Now;
        public DateTime registerDateTime { set; get; } = DateTime.Now;

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string url { set; get; }

        public bool enable { set; get; } = true;

        [Required]
        public bool enableComment { set; get; }

        public int? blogCategoryId { set; get; }
    }

    public class UpdateBlogPostViewModel
    {
        public int blogPostId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.TitleLength)]
        public string title { set; get; }

        public int userId { set; get; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { set; get; }

        public string content { set; get; }

        public DateTime publishDate { set; get; } = DateTime.Now;
        public DateTime registerDateTime { set; get; } = DateTime.Now;

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string url { set; get; }

        public bool enable { set; get; } = true;

        [Required]
        public bool enableComment { set; get; }

        public int? blogCategoryId { set; get; }
    }
}
