
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.Models
{
    public class Propertise
    {
        [Key]
        [Display(Name = "کلید")]
        public PropertiseKey propertiseKey { set; get; }

        [Display(Name = "نام")]
        public string name { set; get; }

        [Display(Name = "توضیحات")]
        public string value { set; get; }

        [Display(Name = "نوع")]
        public PropertiseType propertiseType { set; get; }


        public virtual IList<PropertiseOption> propertiseOptions { get; set; } = new List<PropertiseOption>();
    }
}
