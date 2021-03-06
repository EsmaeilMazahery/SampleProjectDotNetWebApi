﻿using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.ViewModels
{
    public class InsertLicenseViewModel
    {

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public decimal price { set; get; }

        public int day { set; get; }

        public int count { set; get; }
    }

    public class UpdateLicenseViewModel
    {
        public int licenseId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public decimal price { set; get; }

        public int day { set; get; }

        public int count { set; get; }
    }
}
