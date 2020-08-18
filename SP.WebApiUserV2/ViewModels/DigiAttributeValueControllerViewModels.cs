using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertDigiAttributeValueViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public int code { set; get; }

        [Required]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string value { set; get; }

        public int digiAttributeId { set; get; }
    }

    public class UpdateDigiAttributeValueViewModel
    {
        public int digiAttributeValueId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public int code { set; get; }

        [Required]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string value { set; get; }

        public int digiAttributeId { set; get; }
    }
}