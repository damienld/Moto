using Moto.Data;
using PMotoWpf.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMotoWpf.ViewModel
{
    public class AddGpSessionVM : NotificationClass
    {
        public EventHandler ShowMessageBox = delegate { };
        private Dal _dal = MainWindowVM._dal;

        private Categories selectedCategory;
        public Categories SelectedCategory { 
            get => selectedCategory; 
            set
            { 
                selectedCategory = value;
                ListGpsForCategory = new ObservableCollection<Gp>(_dal.getAllGp(selectedCategory));
                if (ListGpsForCategory.Count > 0)
                    SelectedGp = ListGpsForCategory.First();
                OnProprtyChanged();
            }
        }

        private ObservableCollection<Gp> _listGpsForCategory;
        public ObservableCollection<Gp> ListGpsForCategory
        {
            get { return _listGpsForCategory; }
            set
            {
                _listGpsForCategory = value;
                OnProprtyChanged();
            }
        }
        
        public AddGpSessionVM()
        {
            ListGpsForCategory = new ObservableCollection<Gp>();
        }

        private Gp _selectedGp;
        public Gp SelectedGp
        {
            get { return _selectedGp; }
            set
            {
                _selectedGp = value;
                OnProprtyChanged();
            }
        }

        private string textToProcess;

        public string TextToProcess
        {
            get { return textToProcess; }
            set 
            { 
                textToProcess = value;
                OnProprtyChanged();
            }
        }

        private ObservableCollection<RiderSession> listRiderSessions;

        public ObservableCollection<RiderSession> ListRiderSessions
        {
            get { return listRiderSessions; }
            set { listRiderSessions = value; OnProprtyChanged(); }
        }

        public RelayCommand ProcessTextCmd
        {
            get
            {
                return new RelayCommand(ProcessText, canProcessText());
            }
        }
        private bool canProcessText()
        {
            return true;
            /*if (TextToProcess != null)
                return TextToProcess.Trim() != "";
            else
                return false;*/
        }
        private void ProcessText()
        {
            try
            {
                ListRiderSessions = new ObservableCollection<RiderSession>
                    (Session.ReadAnalysisPdf(TextToProcess));
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }
    }
}
