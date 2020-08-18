using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SP.DomainLayer.Models
{
    public class Setting
    {
        [Key]
        public SettingType SettingType { get; set; }

        public string Value { get; set; }
    }
}
