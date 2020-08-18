using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertDigiTypeViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }
        
        [MaxLength(ConstantValidations.NameLength)]
        public int digiCode { set; get; }
    }

    public class UpdateDigiTypeViewModel
    {
        public int digiTypeId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }
        
        [MaxLength(ConstantValidations.NameLength)]
        public int digiCode { set; get; }
    }
}
