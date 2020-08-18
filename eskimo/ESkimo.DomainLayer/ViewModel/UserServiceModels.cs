using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.ViewModel
{
    public class UserServiceModel : User
    {
        public List<RolesKey> selectedRoles { set; get; } = new List<RolesKey>();
    }

    public class UserListViewModel : PaggingViewModel
    {
        public UserListViewModel()
        {
            sort = "name";
            sortDirection = SortDirection.ASC;
        }
        
        public string name { set; get; }
        public string family { set; get; }

        public IEnumerable<User> list { set; get; }
    }
}
