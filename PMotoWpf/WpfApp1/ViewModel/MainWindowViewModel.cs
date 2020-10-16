using Moto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMotoWpf.ViewModel
{
    public class MainWindowViewModel
    {
        public static readonly Dal _dal = new Dal();

        public EventHandler ShowMessageBox = delegate { };

        public MainWindowViewModel()
        {

        }
    }
}
