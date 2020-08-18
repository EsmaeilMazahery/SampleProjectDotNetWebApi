using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertDigiAttributeViewModel
    {
        [MaxLength(ConstantValidations.NameLength)]
        public int digiCode { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public bool filter { set; get; }
    }

    public class UpdateDigiAttributeViewModel
    {
        public int digiAttributeId { set; get; }
        
        [MaxLength(ConstantValidations.NameLength)]
        public int digiCode { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public bool filter { set; get; }
    }
}
