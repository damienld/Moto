using PMotoWpf.Infra;
using Moto.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMotoWpf.ViewModel
{
    public class MainWindowViewModel : NotificationClass
    {
        Dal _dal;
        private Season _season;
        public EventHandler ShowMessageBox = delegate { };
        public MainWindowViewModel()
        {
            _dal = new Dal();
            SeasonCollection = _dal.getAllSeasons();
        }

        private ObservableCollection<Season> seasonCollection;
        public ObservableCollection<Season> SeasonCollection
        {
            get { return seasonCollection; }
            set
            {
                seasonCollection = value;
                OnProprtyChanged();
            }
        }
        public Season SelectedSeason
        {
            get
            {
                return _season;
            }
            set
            {
                _season = value;
                OnProprtyChanged();
            }
        }


        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddSeason, true);
            }
        }

        private void AddSeason()
        {
            try
            {
                SelectedSeason = new Season();
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }

        public RelayCommand Save
        {
            get
            {
                return new RelayCommand(SaveSeason, true);
            }
        }

        private void SaveSeason()
        {
            try
            {
                _dal.AddSeasonsIfDontExist(SelectedSeason.Year, SelectedSeason.Category);
                SeasonCollection = new ObservableCollection<Season>(_dal.getAllSeasons());
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = "Changes are saved !"
                });
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }

        }

        public RelayCommand Delete
        {
            get
            {
                return new RelayCommand(DeleteSeason, true);
            }
        }

        private void DeleteSeason()
        {
            //_dal.Delete(SelectedSeason);
        }
    }
}