using System;
using System.Collections.Generic;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class LocationViewModel : ViewModelBase
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public float zoom { set; get; }
    }
}
