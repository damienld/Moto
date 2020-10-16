using Moto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMotoWpf.ViewModel
{
    public class MainWindowVM
    {
        public static readonly Dal _dal = new Dal("MotoGp");

        public EventHandler ShowMessageBox = delegate { };

        public MainWindowVM()
        {

        }
    }
}
