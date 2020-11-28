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
        public static readonly IDal _dal = new Dal("name = cnnMotoDb");

        public EventHandler ShowMessageBox = delegate { };

        public MainWindowVM()
        {

        }
    }
}
