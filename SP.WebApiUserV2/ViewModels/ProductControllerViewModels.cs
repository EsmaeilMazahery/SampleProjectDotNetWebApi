using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertProductViewModel
    {
        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        public int dkpCode { set; get; }

        public decimal basePrice { set; get; }

        public decimal minPromotion { set; get; }

        public int brandId { set; get; }
        
        public int categoryId { set; get; }
        
        public int digiTypeId { set; get; }
        
        public virtual List<int> selectedColors { get; set; }
        public virtual List<int> selectedDigiAttributeValues { get; set; }
    }

    public class UpdateProductViewModel
    {
        [Key]
        public int productId { get; set; }

        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        public int dkpCode { set; get; }

        public decimal basePrice { set; get; }

        public decimal minPromotion { set; get; }

        public int brandId { set; get; }

        public int categoryId { set; get; }

        public int digiTypeId { set; get; }

        public virtual List<int> selectedColors { get; set; }
        public virtual List<int> selectedDigiAttributeValues { get; set; }
    }
}
