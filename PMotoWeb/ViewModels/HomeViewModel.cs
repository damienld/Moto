using Moto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMotoWeb.ViewModels
{
    public class HomeViewModel
    {
        //public Categories SelectedCategory { get; set; }
        public List<Gp> ListGps { get; set; }
        public Gp SelectedGp { get; set; }
        public Session SelectedSession { get; set; }
    }
}