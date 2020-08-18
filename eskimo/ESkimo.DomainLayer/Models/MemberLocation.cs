using ESkimo.Infrastructure.Constants;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace ESkimo.DomainLayer.Models
{
    public class MemberLocation
    {
        public int memberLocationId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string phone { set; get; }

        [MaxLength(ConstantValidations.MobileLength)]
        public string postalCode { set; get; }

        public IPoint location { get; set; }
        public float zoom { set; get; }

        public int memberId { set; get; }
        public virtual Member member { set; get; }

        public int areaId { set; get; }
        public virtual Area area { set; get; }
        public virtual IList<Factor> factors { get; set; }
    }
}
