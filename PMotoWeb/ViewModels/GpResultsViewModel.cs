using Moto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace PMotoWeb.ViewModels
{
    public class GpResultsViewModel
    {
        public string GpName
        {
            get
            {
                if (SelectedSession != null)
                    return SelectedSession.Gp.Name;
                else
                    return "";
            }
        }
        public List<Session> Sessions { get; set; }
        public Session SelectedSession
        {
            get; set;
        }
        public string SelectedSessionName
        {
            get
            {
                if (SelectedSession != null)
                    return SelectedSession.SessionType.ToString();
                else
                    return "";
            }
        }
        public List<RiderSession> RiderSessions { get; set; }
        public RiderSession SelectedRiderSession
        {
            get; set;
        }
    }
}