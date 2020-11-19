using Moto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMotoWeb.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public bool Authentifie { get; set; }
    }
}