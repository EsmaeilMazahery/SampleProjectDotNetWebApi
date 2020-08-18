using ESkimo.Infrastructure.Constants;
using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Area
    {
        public int areaId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        public IPoint location { get; set; }

        public float zoom { set; get; }

        public string sendDaies { set; get; }

        public virtual IList<MemberLocation> memberLocations { get; set; }

        public virtual IList<AreaPrice> prices { get; set; }
    }

   

}
