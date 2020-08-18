using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public partial class Setting
    {
        [Key]
        public SettingType SettingType { get; set; }

        public string Value { get; set; }
    }
}
