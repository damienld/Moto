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
        public AddGpSessionVM()
        {
            ListSessionsForGp = new ObservableCollection<Session>();
        }

        public EventHandler ShowMessageBox = delegate { };
        private IDal _dal = MainWindowVM._dal;

        private bool isWet;
        public bool IsWet
        {
            get { return isWet; }
            set { isWet = value; NotifyPropertyChanged(); }
        }
        private SessionType sessionType;
        public SessionType SessionType
        {
            get { return sessionType; }
            set { sessionType = value; NotifyPropertyChanged(); }
        }
        private string note;
        public string Note
        {
            get { return note; }
            set { note = value; NotifyPropertyChanged(); }
        }


        private Categories selectedCategory;
        public Categories SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                ListGpsForCategory = new ObservableCollection<Gp>(_dal.getAllGp(selectedCategory));
                if (ListGpsForCategory.Count > 0)
                    SelectedGp = ListGpsForCategory.First();
                NotifyPropertyChanged();
            }
        }
        private ObservableCollection<Gp> _listGpsForCategory = new ObservableCollection<Gp>();
        public ObservableCollection<Gp> ListGpsForCategory
        {
            get { return _listGpsForCategory; }
            set
            {
                _listGpsForCategory = value;
                NotifyPropertyChanged();
            }
        }
        private Gp _selectedGp;
        public Gp SelectedGp
        {
            get { return _selectedGp; }
            set
            {
                if (value != null)
                {
                    _selectedGp = value;
                    ListSessionsForGp = new ObservableCollection<Session>(_dal.GetGpSessions(_selectedGp));
                    if (ListSessionsForGp.Count > 0)
                        SelectedSession = ListSessionsForGp.First();
                    NotifyPropertyChanged();
                }
            }
        }
        private ObservableCollection<Session> _listSessionsForGp = new ObservableCollection<Session>();
        public ObservableCollection<Session> ListSessionsForGp
        {
            get { return _listSessionsForGp; }
            set
            {
                _listSessionsForGp = value;
                NotifyPropertyChanged();
            }
        }
        private Session selectedSession;
        public Session SelectedSession
        {
            get { return selectedSession; }
            set
            {
                if (value != null)
                {
                    selectedSession = value;
                    ListRiderSessions = new ObservableCollection<RiderSession>(selectedSession.RiderSessions);
                    Note = selectedSession.Note;
                    IsWet = selectedSession.IsWet;
                    GroundTemp = selectedSession.GroundTemperature;
                    SessionType = selectedSession.SessionType;
                    NotifyPropertyChanged();
                }
            }
        }

        private void ResetForm()
        {
            TextToProcess = "";
            GroundTemp = null;
            IsWet = false;
            Note = "";
        }
        
        private int? groundTemp;

        public int? GroundTemp
        {
            get { return groundTemp; }
            set { groundTemp = value; NotifyPropertyChanged(); }
        }

        private string textToProcess;

        public string TextToProcess
        {
            get { return textToProcess; }
            set 
            { 
                textToProcess = value;
                NotifyPropertyChanged();
            }
        }

        private RiderSession selectedRiderSession;

        public RiderSession SelectedRiderSession
        {
            get { return selectedRiderSession; }
            set { selectedRiderSession = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<RiderSession> listRiderSessions;

        public ObservableCollection<RiderSession> ListRiderSessions
        {
            get { return listRiderSessions; }
            set { listRiderSessions = value; NotifyPropertyChanged(); }
        }

        public RelayCommand ProcessTextCmd
        {
            get
            {
                return new RelayCommand(x=>ProcessText());
            }
        }
        private bool canProcessText()
        {
            return true;
            if (TextToProcess != null)
                return TextToProcess.Trim() != "";
            else
                return false;
        }
        private void ProcessText()
        {
            try
            {
                ListRiderSessions = new ObservableCollection<RiderSession>
                    (Session.ReadAnalysisPdf(TextToProcess));
                if (TextToProcess.ToLower().Contains("Free Practice Nr. 1".ToLower()))
                    SessionType = SessionType.FP1;
                else if (TextToProcess.ToLower().Contains("Free Practice Nr. 2".ToLower()))
                    SessionType = SessionType.FP2;
                else if (TextToProcess.ToLower().Contains("Free Practice Nr. 3".ToLower()))
                    SessionType = SessionType.FP3;
                else if (TextToProcess.ToLower().Contains("Free Practice Nr. 4".ToLower()))
                    SessionType = SessionType.FP4;
                else if (TextToProcess.ToLower().Contains("Qualifying Nr. 1".ToLower()))
                    SessionType = SessionType.Q1;
                else if (TextToProcess.ToLower().Contains("Qualifying Nr. 2".ToLower()))
                    SessionType = SessionType.Q2;
                else if (TextToProcess.ToLower().Contains("Warm Up".ToLower()))
                    SessionType = SessionType.WUP;
                else if (TextToProcess.ToLower().Contains("Race 2".ToLower()))
                    SessionType = SessionType.Race2;
                else if (TextToProcess.ToLower().Contains("Race Part 2".ToLower()))
                    SessionType = SessionType.Race2;
                else if (TextToProcess.ToLower().Contains("Race".ToLower()))
                    SessionType = SessionType.Race;
                Note = "";
                isWet = false;
                GroundTemp = null;
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }

        public RelayCommand SaveSessionCmd
        {
            get
            {
                return new RelayCommand(x=>SaveSession(), x=>canSaveSession());
            }
        }
        private bool canSaveSession()
        {
            return true;// ListRiderSessions != null && ListRiderSessions.Count > 0;
        }
        private void SaveSession()
        {
            try
            {
                if (SelectedGp == null)
                {
                    ShowMessageBox(this, new MessageEventArgs()
                    {
                        Message = "Please select a GrandPrix"
                    }); ;
                    return;
                }
                Session session = new Session() { Gp = SelectedGp, IsWet = IsWet, Note = Note
                    , Date = SelectedGp.Date, GroundTemperature = GroundTemp, SessionType = SessionType
                };
                _dal.AddOrReplaceSession(SelectedGp, session);
                SelectedGp.NotifyPropertyChanged();
                _dal.SetListRiderSession(session, ListRiderSessions.ToList());
                ResetForm();
                Note = $"<{SessionType} saved>";
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }
        #region DELETE SESSION
        public RelayCommand DeleteSessionCmd
        {
            get
            {
                return new RelayCommand(x => DeleteSession(), x => canDeleteSession());
            }
        }
        private bool canDeleteSession()
        {
            return true;// ListRiderSessions != null && ListRiderSessions.Count > 0;
        }
        private void DeleteSession()
        {
            try
            {
                if (SelectedGp == null || SelectedSession == null)
                {
                    ShowMessageBox(this, new MessageEventArgs()
                    {
                        Message = "Please select a GrandPrix"
                    }); ;
                    return;
                }
                _dal.RemoveSession(SelectedSession);
                SelectedGp.NotifyPropertyChanged();
                ResetForm();
                Note = $"<{SessionType} removed>";
            }
            catch (Exception ex)
            {
                ShowMessageBox(this, new MessageEventArgs()
                {
                    Message = ex.Message
                });
            }
        }
        #endregion
    }
}
