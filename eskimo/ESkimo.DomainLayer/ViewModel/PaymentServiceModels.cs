using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class PaymentServiceModel : Payment
    {
    }

    public class PaymentListViewModel : PaggingViewModel
    {
        public PaymentListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }

        

        public int? sellerId { set; get; }

        public string name { set; get; }

        public IEnumerable<Payment> list { set; get; }
    }
}
