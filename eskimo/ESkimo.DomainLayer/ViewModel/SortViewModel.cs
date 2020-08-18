using ESkimo.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class SortViewModel : ViewModelBase
    {
        [Required]
        public int id { set; get; }

        [Required]
        public bool isUp { set; get; }
    }

}
