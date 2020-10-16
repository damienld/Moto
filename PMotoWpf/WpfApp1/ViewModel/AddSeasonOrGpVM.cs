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
    public class AddSeasonOrGpVM : NotificationClass
    {
        public EventHandler ShowMessageBox = delegate { };
        private Season _season;
        private Gp _gp;
        private Session _session;
        private Dal _dal = MainWindowVM._dal;
        public AddSeasonOrGpVM()
        {
            SeasonCollection = _dal.getAllSeasons();
        }

        private ObservableCollection<Season> seasonCollection;
        public ObservableCollection<Season> SeasonCollection
        {
            get { return seasonCollection; }
            set
            {
                seasonCollection = value;
                NotifyPropertyChanged();
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
                //if (_season != null)
                //    SelectedGp = _season.GPs.FirstOrDefault();
                NotifyPropertyChanged();
            }
        }
        public Gp SelectedGp
        {
            get
            {
                return _gp;
            }
            set
            {
                _gp = value;
                NotifyPropertyChanged();
            }
        }
        public Session SelectedSession
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
                NotifyPropertyChanged();
            }
        }

        #region SeasonCommands
        public RelayCommand AddSeasonCmd
        {
            get
            {
                return new RelayCommand(AddSeason, canAddSeason());
            }
        }
        private bool canAddSeason()
        {
            return true;
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

        public RelayCommand SaveSeasonCmd
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

        public RelayCommand DeleteSeasonCmd
        {
            get
            {
                return new RelayCommand(DeleteSeason, canDeleteSeason());
            }
        }

        private void DeleteSeason()
        {
            //_dal.Delete(SelectedSeason);
        }
        private bool canDeleteSeason()
        {
            return false;
        }
        #endregion

        #region GpCommands
        public RelayCommand AddGpCmd
        {
            get
            {
                return new RelayCommand(AddGp, canAddGp());
            }
        }
        private bool canAddGp()
        {
            return true;
        }
        private void AddGp()
        {
            try
            {
                SelectedGp = new Gp();
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }

        public RelayCommand SaveGpCmd
        {
            get
            {
                return new RelayCommand(SaveGp, true);
            }
        }
        private void ResetForm()
        {
            SelectedGp = new Gp() { };
        }
        private void SaveGp()
        {
            try
            {
                Gp gp= _dal.AddGp(SelectedSeason.Year, SelectedSeason.Category, SelectedGp.GpIdInSeason
                    , SelectedGp.Date, SelectedGp.UrlWeather, SelectedGp.Name, SelectedGp.Note);
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = $"Gp Changes are saved ! {gp}"
                });
                ResetForm();
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }

        }

        public RelayCommand DeleteGpCmd
        {
            get
            {
                return new RelayCommand(DeleteGp, canDeleteGp());
            }
        }

        private void DeleteGp()
        {
            //_dal.Delete(SelectedGp);
        }
        private bool canDeleteGp()
        {
            return false;
        }
        #endregion
    }
}